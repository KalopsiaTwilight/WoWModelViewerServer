using ModelViewer.Core.CM2;
using ModelViewer.Core.Components;
using ModelViewer.Core.Providers;
using SereniaBLPLib;
using SixLabors.ImageSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using WoWFileFormats.Interfaces;
using WoWFileFormats.M2;
using WoWFileFormats.WMO;

namespace Extractor
{
    record ExtractData
    {
        public uint FileId { get; set; }
        public string Type { get; set; } = string.Empty;
    }

    internal class ExtractComponent
    {
        private IFileDataProvider _fileDataProvider;
        private IDBCDStorageProvider _dbcdStorageProvider;
        private IMessageWriter messages;
        private JsonSerializerOptions _jsonOptions;
        private string _outputPath = string.Empty;

        const string texturePath = "modelviewer/textures/";
        const string characterMetadataPath = "modelviewer/metadata/character/";
        const string characterCustomizationMetadataPath = "modelviewer/metadata/charactercustomization/";
        const string itemMetadataPath = "modelviewer/metadata/item/";
        const string itemVisualMetadataPath = "modelviewer/metadata/itemvisual/";
        const string liquidTypePath = "modelviewer/metadata/liquidtype/";
        const string liquidObjectPath = "modelviewer/metadata/liquidobject/";
        const string bonePath = "modelviewer/bone/";
        const string modelPath = "modelviewer/models/";
        const string textureVariationMetadataPath = "modelviewer/metadata/texturevariations/";

        public ExtractComponent(IFileDataProvider fileDataProvider, IDBCDStorageProvider dbcdStorageProvider, string outputPath, IMessageWriter outputWriter)
        {
            _fileDataProvider = fileDataProvider;
            _outputPath = outputPath;
            _dbcdStorageProvider = dbcdStorageProvider;
            messages = outputWriter;
            _jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public void Initialize()
        {
            string[] paths = [
                texturePath, characterMetadataPath, characterCustomizationMetadataPath, itemMetadataPath, itemVisualMetadataPath, bonePath, modelPath,
                liquidTypePath, liquidObjectPath, textureVariationMetadataPath
            ];
            foreach(var path in paths)
            {
                var fullPath = Path.Combine(_outputPath, path);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
            }
        }

        public void ExtractLiquidTypes() {
            var liquidComponent = new LiquidMetadataComponent(_dbcdStorageProvider);
            var availableIds = _dbcdStorageProvider["LiquidType"].Keys;

            messages.WriteLine("Extracting liquid type metadata...");
            foreach(var id in availableIds) {
                var metadata = liquidComponent.GetLiquidTypeMetadata(id);
                if (metadata == null)
                {
                    messages.WriteLine($"Unable to create metadata for LiquidType {id}, skipping.");
                    continue;
                }

                var outputPath = Path.Combine(_outputPath, liquidTypePath, $"{id}.json");
                if (File.Exists(outputPath))
                {
                    messages.WriteLine($"Skipping LiquidType {id}, already processed.");
                    continue;
                }

                var json = JsonSerializer.Serialize(metadata, _jsonOptions);
                File.WriteAllText(outputPath, json);

                foreach (var texture in metadata.Textures)
                {
                    if (texture.FileDataId > 0)
                    {
                        ExtractTexture((uint)texture.FileDataId);
                    }
                }

                messages.WriteLine($"Liquid type {id} sucessfully written to output folder.");
            }
        }

        public void ExtractTextureVariations(uint fileId)
        {
            var metadataComponent = new TextureVariationsMetadataComponent(_dbcdStorageProvider);

            messages.WriteLine($"Extracting texture variations for file {fileId}...");
            var outputPath = Path.Combine(_outputPath, textureVariationMetadataPath, $"{fileId}.json");


            if (File.Exists(outputPath))
            {
                messages.WriteLine($"Skipping texture variation for {fileId}, already processed.");
                return;
            }

            var metadata = metadataComponent.GetTextureVariationsForModel((int)fileId);

            if (metadata == null)
            {
                messages.WriteLine($"No texture variation for {fileId} available.");
                return;
            }

            var json = JsonSerializer.Serialize(metadata, _jsonOptions);
            File.WriteAllText(outputPath, json);


            foreach (var variation in metadata.TextureVariations)
            {
                foreach(var textureId in variation.TextureIds)
                {
                    ExtractTexture((uint)textureId);
                }
            }

            messages.WriteLine($"Texture variations for {fileId} sucessfully written to output folder.");
        }

        public void ExtractWmo(uint fileId)
        {
            var outputPath = Path.Combine(_outputPath, modelPath, $"{fileId}.cwmo");
            if (File.Exists(outputPath))
            {
                messages.WriteLine($"Skipping M2 file {fileId}, already processed.");
                return;
            }
            if (!_fileDataProvider.FileIdExists(fileId)) {
                messages.WriteLine("Unable to find WMO file with fileId: " + fileId);
                return;
            }
            var fileData = _fileDataProvider.GetFileById(fileId);
            var reader = new WMOFileReader(fileId, fileData);
            var wmo = reader.ReadWMORootFile();
            if (wmo == null)
            {
                messages.WriteLine($"WMO file {fileId} was not a valid root WMO file.");
                return;
            }
            wmo.LoadGroupFiles(_fileDataProvider);
            var cwmo = CWMOConverter.Convert(wmo);
            using var outputStream = File.OpenWrite(outputPath);
            using var writer = new CWMOWriter(outputStream);
            writer.Write(cwmo);
            messages.WriteLine($"WMO file {fileId} succesfully writen to output folder.");

            wmo.MaterialList.ForEach((material) =>
            {
                var textures = new List<uint>();
                if (material.Texture1 != 0)
                {
                    textures.Add(material.Texture1);
                }
                if (material.Texture2 != 0)
                {
                    textures.Add(material.Texture2);
                }
                if (material.Texture3 != 0)
                {
                    textures.Add(material.Texture3);
                }
                foreach (var texture in textures)
                {
                    ExtractTexture(texture);
                }
            });
        }

        public void ExtractM2(uint fileId)
        {
            var outputPath = Path.Combine(this._outputPath, modelPath, $"{fileId}.cm2");
            if (File.Exists(outputPath))
            {
                messages.WriteLine($"Skipping M2 file {fileId}, already processed.");
                return;
            }
            if (!_fileDataProvider.FileIdExists(fileId))
            {
                messages.WriteLine("Unable to find M2 file with fileId: " + fileId);
                return;
            }
            var fileData = _fileDataProvider.GetFileById(fileId);

            using var m2Reader = new M2FileReader(fileId, fileData);

            var m2File = m2Reader.ReadM2File();
            if (m2File == null)
            {
                messages.WriteLine($"M2 file {fileId} was not a valid M2 file, perhaps encrypted.");
                return;
            }

            m2File.LoadSkins(_fileDataProvider);
            m2File.LoadSkeleton(_fileDataProvider);
            m2File.LoadAnims(_fileDataProvider);
            var cm2File = CM2Converter.Convert(m2File);
            using var outputStream = File.OpenWrite(outputPath);
            using var cm2Writer = new CM2Writer(outputStream);
            cm2Writer.Write(cm2File);
            messages.WriteLine($"M2 file {fileId} succesfully writen to output folder.");

            foreach (var texture in m2File.Textures)
            {
                ExtractTexture(texture.FileId);
            }
        }

        public void ExtractTexture(uint fileId)
        {
            var outputPath = Path.Combine(this._outputPath, texturePath, $"{fileId}.webp");
            if (File.Exists(outputPath))
            {
                messages.WriteLine($"Skipping BLP file {fileId}, already processed.");
                return;
            }
            if (!_fileDataProvider.FileIdExists(fileId))
            {
                messages.WriteLine("Unable to find BLP file with fileId: " + fileId);
                return;
            }

            var fileData = _fileDataProvider.GetFileById(fileId);
            var blp = new BlpFile(fileData);

            var img = blp.GetImage(0);

            img.SaveAsWebp(outputPath);

            messages.WriteLine($"BLP file {fileId} succesfully writen to output folder.");
        }
    }
}

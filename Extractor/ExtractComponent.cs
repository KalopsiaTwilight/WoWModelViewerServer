using ModelViewer.Core.CM2;
using SereniaBLPLib;
using SixLabors.ImageSharp;
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
        private IFileDataProvider fileDataProvider;
        private IMessageWriter messages;
        private string outputPath = string.Empty;

        const string texturePath = "modelviewer/textures/";
        const string characterMetadataPath = "modelviewer/metadata/character/";
        const string characterCustomizationMetadataPath = "modelviewer/metadata/charactercustomization/";
        const string itemMetadataPath = "modelviewer/metadata/item/";
        const string itemVisualMetadataPath = "modelviewer/metadata/itemvisual/";
        const string bonePath = "modelviewer/bone/";
        const string modelPath = "modelviewer/models/";

        public ExtractComponent(IFileDataProvider fileDataProvider, string outputPath, IMessageWriter outputWriter)
        {
            this.fileDataProvider = fileDataProvider;
            this.outputPath = outputPath;
            messages = outputWriter;
        }

        public void Initialize()
        {
            string[] paths = [texturePath, characterMetadataPath, characterCustomizationMetadataPath, itemMetadataPath, itemVisualMetadataPath, bonePath, modelPath];
            foreach(var path in paths)
            {
                var fullPath = Path.Combine(outputPath, path);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
            }
        }

        public void ExtractWmo(uint fileId)
        {
            var outputPath = Path.Combine(this.outputPath, modelPath, $"{fileId}.cwmo");
            if (File.Exists(outputPath))
            {
                messages.WriteLine($"Skipping M2 file {fileId}, already processed.");
                return;
            }
            if (!fileDataProvider.FileIdExists(fileId)) {
                messages.WriteLine("Unable to find WMO file with fileId: " + fileId);
                return;
            }
            var fileData = fileDataProvider.GetFileById(fileId);
            var reader = new WMOFileReader(fileId, fileData);
            var wmo = reader.ReadWMORootFile();
            if (wmo == null)
            {
                messages.WriteLine($"WMO file {fileId} was not a valid root WMO file.");
                return;
            }
            wmo.LoadGroupFiles(fileDataProvider);
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
            var outputPath = Path.Combine(this.outputPath, modelPath, $"{fileId}.cm2");
            if (File.Exists(outputPath))
            {
                messages.WriteLine($"Skipping M2 file {fileId}, already processed.");
                return;
            }
            if (!fileDataProvider.FileIdExists(fileId))
            {
                messages.WriteLine("Unable to find M2 file with fileId: " + fileId);
                return;
            }
            var fileData = fileDataProvider.GetFileById(fileId);

            using var m2Reader = new M2FileReader(fileId, fileData);

            var m2File = m2Reader.ReadM2File();
            if (m2File == null)
            {
                messages.WriteLine($"M2 file {fileId} was not a valid M2 file, perhaps encrypted.");
                return;
            }

            m2File.LoadSkins(fileDataProvider);
            m2File.LoadSkeleton(fileDataProvider);
            m2File.LoadAnims(fileDataProvider);
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
            var outputPath = Path.Combine(this.outputPath, texturePath, $"{fileId}.webp");
            if (File.Exists(outputPath))
            {
                messages.WriteLine($"Skipping BLP file {fileId}, already processed.");
                return;
            }
            if (!fileDataProvider.FileIdExists(fileId))
            {
                messages.WriteLine("Unable to find BLP file with fileId: " + fileId);
                return;
            }

            var fileData = fileDataProvider.GetFileById(fileId);
            var blp = new BlpFile(fileData);

            var img = blp.GetImage(0);

            img.SaveAsWebp(outputPath);

            messages.WriteLine($"BLP file {fileId} succesfully writen to output folder.");
        }
    }
}

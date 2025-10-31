using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using ModelViewer.Core.CM2;
using ModelViewer.Core.Components;
using ModelViewer.Core.Models;
using SereniaBLPLib;
using Server.Infrastructure;
using SixLabors.ImageSharp;
using System.IO;
using System.Text.Json;
using WoWFileFormats.Interfaces;
using WoWFileFormats.M2;
using WoWFileFormats.WMO;

namespace Server.Controllers
{
    [ApiController]
    public class ModelViewerController: ControllerBase
    {
        private readonly IFileDataProvider _fileDataProvider;
        private readonly ItemMetadataComponent _itemMetadataComponent;
        private readonly CharacterMetadataComponent _charMetadataComponent;
        private readonly ItemVisualComponent _itemVisualComponent;
        private readonly LiquidMetadataComponent _liquidMetadataComponent;
        private readonly TextureVariationsMetadataComponent _textureVariationsMetadataComponent;
        private readonly JsonSerializerOptions _jsonOptions;
        private string? _fileStoragePath;

        public ModelViewerController(
            IFileDataProvider fileDataProvider, ItemMetadataComponent itemMetadataComponent, CharacterMetadataComponent charMetadataComponent,
            ItemVisualComponent itemVisualComponent, LiquidMetadataComponent liquidMetadataComponent,
            TextureVariationsMetadataComponent textureVariationsMetadataComponent, IConfiguration config
        )
        {
            _fileDataProvider = fileDataProvider;
            _itemMetadataComponent = itemMetadataComponent;
            _charMetadataComponent = charMetadataComponent;
            _itemVisualComponent = itemVisualComponent;
            _liquidMetadataComponent = liquidMetadataComponent;
            _textureVariationsMetadataComponent = textureVariationsMetadataComponent;

            _fileStoragePath = config["OutputPath"];
            _jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        [HttpGet]
        [Route("/modelviewer/metadata/character/{modelId}.json")]
        public ActionResult GetCharacterMetadata(int modelId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/metadata/character/{modelId}.json");
                if (System.IO.File.Exists(cachePath))
                {
                    var cachedResult = JsonSerializer.Deserialize<CharacterMetadata>(System.IO.File.ReadAllText(cachePath), _jsonOptions);
                    return Ok(cachedResult);
                }
            }

            var metadata = _charMetadataComponent.GetMetadataForCharacter(modelId);
            if (metadata == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                var json = JsonSerializer.Serialize(metadata, _jsonOptions);
                System.IO.File.WriteAllText(cachePath, json);
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/metadata/item/{displayId}.json")]
        public ActionResult GetItemMetadata(int displayId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/metadata/item/{displayId}.json");
                if (System.IO.File.Exists(cachePath))
                {
                    var cachedResult = JsonSerializer.Deserialize<ItemMetadata>(System.IO.File.ReadAllText(cachePath), _jsonOptions);
                    return Ok(cachedResult);
                }
            }

            var metadata = _itemMetadataComponent.GetMetadataForDisplayId(displayId);
            if (metadata == null)
            {
                return NotFound();
            }


            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                var json = JsonSerializer.Serialize(metadata, _jsonOptions);
                System.IO.File.WriteAllText(cachePath, json);
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/metadata/itemvisual/{visualId}.json")]
        public ActionResult GetItemVisualMetadata(int visualId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/metadata/itemvisual/{visualId}.json");
                if (System.IO.File.Exists(cachePath))
                {
                    var cachedResult = JsonSerializer.Deserialize<ItemVisualMetadata>(System.IO.File.ReadAllText(cachePath), _jsonOptions);
                    return Ok(cachedResult);
                }
            }

            var metadata = _itemVisualComponent.GetItemVisualMetadata(visualId);
            if (metadata == null) {
                return NotFound();
            }


            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                var json = JsonSerializer.Serialize(metadata, _jsonOptions);
                System.IO.File.WriteAllText(cachePath, json);
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/metadata/liquidtype/{liquidTypeId}.json")]
        public ActionResult GetLiquidTypeMetadata(int liquidTypeId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/metadata/liquidtype/{liquidTypeId}.json");
                if (System.IO.File.Exists(cachePath))
                {
                    var cachedResult = JsonSerializer.Deserialize<LiquidTypeMetadata>(System.IO.File.ReadAllText(cachePath), _jsonOptions);
                    return Ok(cachedResult);
                }
            }

            var metadata = _liquidMetadataComponent.GetLiquidTypeMetadata(liquidTypeId);
            if (metadata == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                var json = JsonSerializer.Serialize(metadata, _jsonOptions);
                System.IO.File.WriteAllText(cachePath, json);
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/metadata/liquidobject/{liquidObjectId}.json")]
        public ActionResult GetLiquidObjectMetadata(int liquidObjectId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/metadata/liquidobject/{liquidObjectId}.json");
                if (System.IO.File.Exists(cachePath))
                {
                    var cachedResult = JsonSerializer.Deserialize<LiquidObjectMetadata>(System.IO.File.ReadAllText(cachePath), _jsonOptions);
                    return Ok(cachedResult);
                }
            }

            var metadata = _liquidMetadataComponent.GetLiquidObjectMetadata(liquidObjectId);
            if (metadata == null)
            {
                return NotFound();
            }


            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                var json = JsonSerializer.Serialize(metadata, _jsonOptions);
                System.IO.File.WriteAllText(cachePath, json);
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/metadata/texturevariations/{modelId}.json")]
        public ActionResult GetTextureVariationsMetadata(int modelId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/metadata/texturevariations/{modelId}.json");
                if (System.IO.File.Exists(cachePath))
                {
                    var cachedResult = JsonSerializer.Deserialize<TextureVariationsMetadata>(System.IO.File.ReadAllText(cachePath), _jsonOptions);
                    return Ok(cachedResult);
                }
            }

            var metadata = _textureVariationsMetadataComponent.GetTextureVariationsForModel(modelId);
            if (metadata == null)
            {
                return NotFound();
            }


            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                var json = JsonSerializer.Serialize(metadata, _jsonOptions);
                System.IO.File.WriteAllText(cachePath, json);
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/textures/{fileId}.webp")]
        public async Task<ActionResult> GetTextureFile(uint fileId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/textures/{fileId}.webp");
                if (System.IO.File.Exists(cachePath))
                {
                    return File(System.IO.File.OpenRead(cachePath), "image/webp");
                }
            }


            if (!_fileDataProvider.FileIdExists(fileId))
            {
                return NotFound();
            }
            var fileData = _fileDataProvider.GetFileById(fileId);
            var blp = new BlpFile(fileData);

            var img = blp.GetImage(0);
            Stream stream;
            if (string.IsNullOrEmpty(cachePath))
            {
                stream = new MemoryStream();
            } else
            {
                stream = System.IO.File.OpenWrite(cachePath);
            }

            await img.SaveAsWebpAsync(stream);
            stream.Position = 0;

            return File(stream, "image/webp");
        }

        [HttpGet]
        [AllowSynchronousIO]
        [Route("/modelviewer/bone/{fileId}.cbone")]
        public async Task GetBONEFile(uint fileId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/bone/{fileId}.cbone");
                if (System.IO.File.Exists(cachePath))
                {
                    Response.Headers.ContentType = "application/octet-stream";
                    await System.IO.File.OpenRead(cachePath).CopyToAsync(Response.Body);
                    await Response.CompleteAsync();
                    return;
                }
            }


            if (!_fileDataProvider.FileIdExists(fileId))
            {
                Response.StatusCode = 404;
                await Response.CompleteAsync();
                return;
            }

            var fileData = _fileDataProvider.GetFileById(fileId);

            using var reader = new BONEFileReader(fileData);
            var boneFile = reader.ReadBONEFile();
            var cm2BoneFile = CM2Converter.Convert(boneFile);

            CM2Writer cm2Writer;

            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                cm2Writer = new CM2Writer(System.IO.File.OpenWrite(cachePath));
                cm2Writer.Write(cm2BoneFile);
                Response.Headers.ContentType = "application/octet-stream";
                await System.IO.File.OpenRead(cachePath).CopyToAsync(Response.Body);
                await Response.CompleteAsync();
                return;
            }

            cm2Writer = new CM2Writer(Response.Body);
            Response.Headers.ContentType = "application/octet-stream";
            cm2Writer.Write(cm2BoneFile);
            await Response.CompleteAsync();
            return;
        }

        [HttpGet]
        [AllowSynchronousIO]
        [Route("/modelviewer/models/{fileId}.cm2")]
        public async Task GetCM2File(uint fileId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/models/{fileId}.cm2");
                if (System.IO.File.Exists(cachePath))
                {
                    Response.Headers.ContentType = "application/octet-stream";
                    await System.IO.File.OpenRead(cachePath).CopyToAsync(Response.Body);
                    await Response.CompleteAsync();
                    return;
                }
            }

            if (!_fileDataProvider.FileIdExists(fileId))
            {
                Response.StatusCode = 404;
                await Response.CompleteAsync();
                return;
            }
            var fileData = _fileDataProvider.GetFileById(fileId);

            using var m2Reader = new M2FileReader(fileId, fileData);

            var m2File = m2Reader.ReadM2File(); 
            if (m2File == null)
            {
                Response.StatusCode = 404;
                await Response.CompleteAsync();
                return;
            }
            m2File.LoadSkins(_fileDataProvider);
            m2File.LoadSkeleton(_fileDataProvider);
            m2File.LoadAnims(_fileDataProvider);
            var cm2File = CM2Converter.Convert(m2File);

            CM2Writer cm2Writer;

            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                cm2Writer = new CM2Writer(System.IO.File.OpenWrite(cachePath));
                cm2Writer.Write(cm2File);

                Response.Headers.ContentType = "application/octet-stream";
                await System.IO.File.OpenRead(cachePath).CopyToAsync(Response.Body);
                await Response.CompleteAsync();
                return;
            }

            cm2Writer = new CM2Writer(Response.Body);
            Response.Headers.ContentType = "application/octet-stream";
            cm2Writer.Write(cm2File);
            await Response.CompleteAsync();
        }


        [HttpGet]
        [AllowSynchronousIO]
        [Route("/modelviewer/models/{fileId}.cwmo")]
        public async Task GetCWMOFile(uint fileId)
        {
            string cachePath = string.Empty;
            if (!string.IsNullOrEmpty(_fileStoragePath))
            {
                cachePath = Path.Combine(_fileStoragePath, $"modelviewer/models/{fileId}.cwmo");
                if (System.IO.File.Exists(cachePath))
                {
                    Response.Headers.ContentType = "application/octet-stream";
                    await System.IO.File.OpenRead(cachePath).CopyToAsync(Response.Body);
                    await Response.CompleteAsync();
                    return;
                }
            }

            if (!_fileDataProvider.FileIdExists(fileId))
            {
                Response.StatusCode = 404;
                await Response.CompleteAsync();
                return;
            }
            var fileData = _fileDataProvider.GetFileById(fileId);

            using var wmoReader = new WMOFileReader(fileId, fileData);

            var wmoFile = wmoReader.ReadWMORootFile();
            if (wmoFile == null)
            {
                Response.StatusCode = 404;
                await Response.CompleteAsync();
                return;
            }

            wmoFile.LoadGroupFiles(_fileDataProvider);
            var cwmoFile = CWMOConverter.Convert(wmoFile);


            CWMOWriter cwmoWriter;

            if (!string.IsNullOrEmpty(cachePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(cachePath);
                }

                cwmoWriter = new CWMOWriter(System.IO.File.OpenWrite(cachePath));
                cwmoWriter.Write(cwmoFile);
                Response.Headers.ContentType = "application/octet-stream";
                await System.IO.File.OpenRead(cachePath).CopyToAsync(Response.Body);
                await Response.CompleteAsync();
                return;
            }

            cwmoWriter = new CWMOWriter(Response.Body);
            Response.Headers.ContentType = "application/octet-stream";
            cwmoWriter.Write(cwmoFile);
            await Response.CompleteAsync();
        }
    }
}

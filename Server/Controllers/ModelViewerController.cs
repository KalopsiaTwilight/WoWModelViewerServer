﻿using Microsoft.AspNetCore.Mvc;
using ModelViewer.Core.Components;
using SereniaBLPLib;
using Server.CM2;
using Server.Infrastructure;
using SixLabors.ImageSharp;
using WoWFileFormats.Interfaces;
using WoWFileFormats.M2;

namespace Server.Controllers
{
    [ApiController]
    public class ModelViewerController: ControllerBase
    {
        private readonly IFileDataProvider _fileDataProvider;
        private readonly ItemMetadataComponent _itemMetadataComponent;
        private readonly CharacterMetadataComponent _charMetadataComponent;
        private readonly CharacterCustomizationMetadataComponent _charCustomizationMetadataComponent;
        private readonly ItemVisualComponent _itemVisualComponent;

        public ModelViewerController(
            IFileDataProvider fileDataProvider, ItemMetadataComponent itemMetadataComponent, CharacterMetadataComponent charMetadataComponent,
            CharacterCustomizationMetadataComponent charCustomizationMetadataComponent, ItemVisualComponent itemVisualComponent
        )
        {
            _fileDataProvider = fileDataProvider;
            _itemMetadataComponent = itemMetadataComponent;
            _charMetadataComponent = charMetadataComponent;
            _charCustomizationMetadataComponent = charCustomizationMetadataComponent;
            _itemVisualComponent = itemVisualComponent;
        }

        [HttpGet]
        [Route("/modelviewer/metadata/character/{modelId}.json")]
        public ActionResult GetCharacterMetadata(int modelId)
        {
            var metadata = _charMetadataComponent.GetMetadataForCharacter(modelId);
            if (metadata == null)
            {
                return NotFound();
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/metadata/charactercustomization/{modelId}.json")]
        public ActionResult GetCharacterCustomizationMetadata(int modelId)
        {
            var metadata = _charCustomizationMetadataComponent.GetCharacterCustomizationMetadataForCharacter(modelId);
            if (metadata == null)
            {
                return NotFound();
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/metadata/item/{displayId}.json")]
        public ActionResult GetItemMetadata(int displayId)
        {
            var metadata = _itemMetadataComponent.GetMetadataForDisplayId(displayId);
            if (metadata == null)
            {
                return NotFound();
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/metadata/itemvisual/{visualId}.json")]
        public ActionResult GetItemVisualMetadata(int visualId)
        {
            var metadata = _itemVisualComponent.GetItemVisualMetadata(visualId);
            if (metadata == null) {
                return NotFound();
            }
            return Ok(metadata);
        }

        [HttpGet]
        [Route("/modelviewer/textures/{fileId}.webp")]
        public async Task<ActionResult> GetTextureFile(uint fileId)
        {
            if (!_fileDataProvider.FileIdExists(fileId))
            {
                return NotFound();
            }
            var fileData = _fileDataProvider.GetFileById(fileId);
            var blp = new BlpFile(fileData);

            var img = blp.GetImage(0);
            var memoryStream = new MemoryStream();
            await img.SaveAsWebpAsync(memoryStream);
            memoryStream.Position = 0;
            return File(memoryStream, "image/webp");
        }

        [HttpGet]
        [AllowSynchronousIO]
        [Route("/modelviewer/bone/{fileId}.cbone")]
        public async Task GetBONEFile(uint fileId)
        {
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

            var cm2Writer = new CM2Writer(Response.Body);
            Response.Headers.ContentType = "application/octet-stream";
            cm2Writer.Write(cm2BoneFile);
            await Response.CompleteAsync();
        }

        [HttpGet]
        [AllowSynchronousIO]
        [Route("/modelviewer/models/{fileId}.cm2")]
        public async Task GetCM2File(uint fileId)
        {
            if (!_fileDataProvider.FileIdExists(fileId))
            {
                Response.StatusCode = 404;
                await Response.CompleteAsync();
                return;
            }
            var fileData = _fileDataProvider.GetFileById(fileId);

            using var m2Reader = new M2FileReader(fileId, fileData);

            var m2File = m2Reader.ReadM2File();
            m2File.LoadSkins(_fileDataProvider);
            m2File.LoadSkeleton(_fileDataProvider);
            m2File.LoadAnims(_fileDataProvider);
            var cm2File = CM2Converter.Convert(m2File);

            var cm2Writer = new CM2Writer(Response.Body);
            Response.Headers.ContentType = "application/octet-stream";
            cm2Writer.Write(cm2File);
            await Response.CompleteAsync();
        }
    }
}

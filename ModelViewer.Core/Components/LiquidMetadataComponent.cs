using ModelViewer.Core.CM2;
using ModelViewer.Core.Models;
using ModelViewer.Core.Providers;
using ModelViewer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewer.Core.Components
{
    public class LiquidMetadataComponent: IComponent
    {
        private readonly IDBCDStorageProvider _dbcdStorageProvider;

        public LiquidMetadataComponent(IDBCDStorageProvider storageProvider)
        {
            _dbcdStorageProvider = storageProvider;
        }

        public LiquidTypeMetadata? GetLiquidTypeMetadata(int liquidTypeId)
        {
            if (!_dbcdStorageProvider["LiquidType"].TryGetValue(liquidTypeId, out var liquidTypeInfo))
            {
                return null;
            }

            var textures = _dbcdStorageProvider["LiquidTypeXTexture"]
                .HavingColumnVal("LiquidTypeID", liquidTypeId)
                .Select(x => new LiquidTypeTexture()
                {
                    Id = x.ID,
                    FileDataId = x.Field<int>("FileDataID"),
                    OrderIndex = x.Field<int>("OrderIndex"),
                    Type = x.Field<int>("Type")
                })
                .OrderBy(x => x.OrderIndex)
                .ToList();
            return new LiquidTypeMetadata()
            {
                Id = liquidTypeInfo.ID,
                Name = liquidTypeInfo.Field<string>("Name"),
                Color0 = liquidTypeInfo.Field<int[]>("Color")[0],
                Color1 = liquidTypeInfo.Field<int[]>("Color")[1],
                Flags = liquidTypeInfo.Field<int>("Flags"),
                Float0 = liquidTypeInfo.Field<float[]>("Float")[0],
                Float1 = liquidTypeInfo.Field<float[]>("Float")[1],
                MaterialId = liquidTypeInfo.Field<int>("MaterialID"),
                NamedTextures = liquidTypeInfo.Field<string[]>("Texture"),
                Textures = textures
            };
        }

        public LiquidObjectMetadata? GetLiquidObjectMetadata(int liquidObjectId)
        {
            if (!_dbcdStorageProvider["LiquidType"].TryGetValue(liquidObjectId, out var liquidObjectInfo))
            {
                return null;
            }

            return new LiquidObjectMetadata()
            {
                Id = liquidObjectInfo.ID,
                FlowDirection = liquidObjectInfo.Field<int>("FlowDirection"),
                FlowSpeed = liquidObjectInfo.Field<float>("FlowSpeed"),
                LiquidTypeId = liquidObjectInfo.Field<int>("LiquidTypeID"),
                Reflection = liquidObjectInfo.Field<bool>("Reflection")
            };
        }
    }
}

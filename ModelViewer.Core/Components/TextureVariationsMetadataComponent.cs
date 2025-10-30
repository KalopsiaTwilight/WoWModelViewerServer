using ModelViewer.Core.Models;
using ModelViewer.Core.Providers;
using ModelViewer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewer.Core.Components
{
    public class TextureIntArrayEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[]? x, int[]? y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x == null || y == null) return false;

            return x.SequenceEqual(y);
        }

        public int GetHashCode([DisallowNull] int[] obj)
        {
            if (obj == null) 
                return 0;
            // Item info
            if (obj.Length == 1)
            {
                return obj[0];
            }

            var hashCode = 0;
            for(var i = 0; i < obj.Length; i++)
            {
                hashCode ^= i * 1237182 ^ obj[i];
            }
            return hashCode;
        }
    }

    public class TextureVariationsMetadataComponent: IComponent
    {
        private readonly IDBCDStorageProvider _dbcdStorageProvider;
        public TextureVariationsMetadataComponent(IDBCDStorageProvider storageProvider)
        {
            _dbcdStorageProvider = storageProvider;
        }

        public TextureVariationsMetadata? GetTextureVariationsForModel(int modelId)
        {
            if (!_dbcdStorageProvider["ModelFileData"].TryGetValue(modelId, out var modelFileDataRow))
            {
                return null;
            }

            var modelResourceId = modelFileDataRow.Field<int>("ModelResourcesID");
            var idis = _dbcdStorageProvider["ItemDisplayInfo"].Where(x => x.Field<int[]>("ModelResourcesID").Contains(modelResourceId));

            var textureVariations = new List<TextureVariation>();
            foreach (var idi in idis)
            {
                var modelResources = idi.Field<int[]>("ModelResourcesID");
                var modelMaterialResources = idi.Field<int[]>("ModelMaterialResourcesID");
                int modelMaterialResourceId = modelResources[0] == modelResourceId ? modelMaterialResources[0] : modelMaterialResources[1];

                var textureDataRow = _dbcdStorageProvider["TextureFileData"].FirstOrDefault(x => x.Field<int>("MaterialResourcesID") == modelMaterialResourceId);
                if (textureDataRow != null)
                {
                    textureVariations.Add(new TextureVariation() { TextureIds = [textureDataRow.ID], DisplayType = DisplayType.Item, DisplayId = idi.ID });
                }
            }

            var creatureModels = _dbcdStorageProvider["CreatureModelData"].Where(x => x.Field<int>("FileDataID") == modelId);
            foreach(var model in creatureModels)
            {
                var creatureDisplayInfos = _dbcdStorageProvider["CreatureDisplayInfo"].Where(x => x.Field<int>("ModelID") == model.ID);
                foreach(var displayInfo in creatureDisplayInfos)
                {
                    textureVariations.Add(new TextureVariation { DisplayId = displayInfo.ID, DisplayType = DisplayType.Creature, TextureIds = displayInfo.Field<int[]>("TextureVariationFileDataID") });
                }
            }


            return new TextureVariationsMetadata() { TextureVariations = textureVariations.DistinctBy(x => x.TextureIds, new TextureIntArrayEqualityComparer()).ToList() };
        }
    }
}

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
    public class CreatureTextureEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[]? x, int[]? y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x == null || y == null) return false;

            return x.SequenceEqual(y);
        }

        public int GetHashCode([DisallowNull] int[] obj)
        {
            return 12348 ^ obj[1] + 23829 ^ obj[0] + 1235910 ^ obj[2];
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

            var idiTextures = new List<int>();
            foreach(var idi in idis)
            {
                var modelResources = idi.Field<int[]>("ModelResourcesID");
                var modelMaterialResources = idi.Field<int[]>("ModelMaterialResourcesID");
                int modelMaterialResourceId = modelResources[0] == modelResourceId ? modelMaterialResources[0] : modelMaterialResources[1];

                var textureDataRow = _dbcdStorageProvider["TextureFileData"].FirstOrDefault(x => x.Field<int>("MaterialResourcesID") == modelMaterialResourceId);
                if (textureDataRow != null)
                {
                    idiTextures.Add(textureDataRow.ID);
                }
            }

            var textureVariations = idiTextures.Distinct().Select(x => new TextureVariation { TextureIds = [x] }).ToList();

            var creatureModels = _dbcdStorageProvider["CreatureModelData"].Where(x => x.Field<int>("FileDataID") == modelId);
            var creatureVariations = new List<int[]>();
            foreach(var model in creatureModels)
            {
                var creatureDisplayInfos = _dbcdStorageProvider["CreatureDisplayInfo"].Where(x => x.Field<int>("ModelID") == model.ID);
                foreach(var displayInfo in creatureDisplayInfos)
                {
                    creatureVariations.Add(displayInfo.Field<int[]>("TextureVariationFileDataID"));
                }
            }


            textureVariations.AddRange(
                creatureVariations.Distinct(new CreatureTextureEqualityComparer()).Select(x => new TextureVariation { TextureIds = x })
            );

            return new TextureVariationsMetadata() { TextureVariations = textureVariations };
        }
    }
}

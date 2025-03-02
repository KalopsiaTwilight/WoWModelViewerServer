using DBCD;
using ModelViewer.Core.Models;
using ModelViewer.Core.Providers;
using ModelViewer.Core.Utils;

namespace ModelViewer.Core.Components
{
    public class ItemMetadataComponent : IComponent
    {
        private readonly IDBCDStorageProvider _dbcdStorageProvider;

        public ItemMetadataComponent(IDBCDStorageProvider storageProvider)
        {
            _dbcdStorageProvider = storageProvider;
        }


        public ItemMetadata? GetMetadataForDisplayId(int displayId)
        {
            if (!_dbcdStorageProvider["ItemDisplayInfo"].TryGetValue(displayId, out var displayInfo))
            {
                return null;
            }

            var itemAppearance = _dbcdStorageProvider["ItemAppearance"]
                .FirstOrDefault(x => x.Field<int>("ItemDisplayInfoID") == displayId);
            if (itemAppearance == null)
            {
                return null;
            }

            var itemModAppearance = _dbcdStorageProvider["ItemModifiedAppearance"]
                .FirstOrDefault((x) => x.Field<int>("ItemAppearanceID") == itemAppearance.ID);
            if (itemModAppearance == null)
            {
                return null;
            }

            var item = _dbcdStorageProvider["Item"][itemModAppearance.Field<int>("ItemID")];
            return GetMetadata(item, displayInfo);
        }


        public ItemMetadata? GetModelMetadataForItem(int itemId, int? appearanceMod = null)
        {
            if (!_dbcdStorageProvider["Item"].TryGetValue(itemId, out var item))
            {
                return null;
            }
            var itemModAppearance = _dbcdStorageProvider["ItemModifiedAppearance"]
                .FirstOrDefault((x) => x.Field<int>("ItemID") == itemId && (
                    appearanceMod == null || x.Field<int>("ItemAppearanceModifierID") == appearanceMod
                ));
            if (itemModAppearance == null)
            {
                return null;
            }
            var itemAppearance = _dbcdStorageProvider["ItemAppearance"][itemModAppearance.Field<int>("ItemAppearanceID")];
            var displayInfo = _dbcdStorageProvider["ItemDisplayInfo"][itemAppearance.Field<int>("ItemDisplayInfoID")];
            return GetMetadata(item, displayInfo);
        }

        private ItemMetadata GetMetadata(DBCDRow item, DBCDRow displayInfo)
        {
            var itemSlot = item.Field<int>("SubclassID");
            var modelData = new ItemMetadata()
            {
                Flags = displayInfo.Field<int>("Flags"),
                InventoryType = item.Field<int>("InventoryType"),
                ClassId = item.Field<int>("ClassID"),
                SubclassId = item.Field<int>("SubclassID"),
                GeosetGroup = displayInfo.Field<int[]>("GeosetGroup"),
                AttachmentGeosetGroup = displayInfo.Field<int[]>("AttachmentGeosetGroup"),
                ItemVisual = displayInfo.Field<int>("ItemVisual")
            };

            var textureSections = _dbcdStorageProvider["ItemDisplayInfoMaterialRes"]
                .HavingColumnVal("ItemDisplayInfoID", displayInfo.ID)
                .ToList();
            foreach (var section in textureSections)
            {
                var sectionId = section.Field<int>("ComponentSection");
                var matResId = section.Field<int>("MaterialResourcesID");

                modelData.ComponentSections.Add(new ComponentSectionData()
                {
                    Section = sectionId,
                    Textures = GetTextureFiles(matResId)
                });
            }

            var modelResourceID0 = displayInfo.Field<int[]>("ModelResourcesID")[0];
            if (modelResourceID0 != 0)
            {
                modelData.Component1 = new ItemComponentData
                {
                    ModelFiles = GetModelFiles(modelResourceID0)
                };
            }

            var modelMaterialResourceID0 = displayInfo.Field<int[]>("ModelMaterialResourcesID")[0];
            if (modelMaterialResourceID0 != 0)
            {
                modelData.Component1 = modelData.Component1 ?? new ItemComponentData();
                modelData.Component1.TextureFiles = GetTextureFiles(modelMaterialResourceID0);
            }

            var modelResourceID1 = displayInfo.Field<int[]>("ModelResourcesID")[1];
            if (modelResourceID1 != 0)
            {
                modelData.Component1 = new ItemComponentData
                {
                    ModelFiles = GetModelFiles(modelResourceID1)
                };
            }

            var modelMaterialResourceID1 = displayInfo.Field<int[]>("ModelMaterialResourcesID")[1];
            if (modelMaterialResourceID1 != 0)
            {
                modelData.Component2 = modelData.Component2 ?? new ItemComponentData();
                modelData.Component2.TextureFiles = GetTextureFiles(modelMaterialResourceID0);
            }

            var particleColorId = displayInfo.Field<int>("ParticleColorID");
            if (particleColorId != 0)
            {
                var particleColorData = _dbcdStorageProvider["ParticleColor"][particleColorId];
                modelData.ParticleColor = new ItemParticleColorOverrideData()
                {
                    Id = particleColorData.ID,
                    Start = particleColorData.FieldAs<uint[]>("Start"),
                    Mid = particleColorData.FieldAs<uint[]>("MID"),
                    End = particleColorData.FieldAs<uint[]>("End"),
                };
            }

            var helmetGeoVis0 = displayInfo.Field<int[]>("HelmetGeosetVis")[0];
            if (helmetGeoVis0 != 0)
            {
                modelData.HideGeoset1 = _dbcdStorageProvider["HelmetGeosetData"]
                    .Where(x => x.Field<int>("HelmetGeosetVisDataID") == helmetGeoVis0)
                    .Select(x => new ItemHideGeosetData()
                    {
                        RaceId = x.Field<int>("RaceID"),
                        GeosetGroup = x.Field<int>("HideGeosetGroup"),
                        RaceBitSelection = x.Field<int>("RaceBitSelection"),
                    }).ToList();
            }

            var helmetGeoVis1 = displayInfo.Field<int[]>("HelmetGeosetVis")[1];
            if (helmetGeoVis1 != 0)
            {
                modelData.HideGeoset2 = _dbcdStorageProvider["HelmetGeosetData"]
                    .Where(x => x.Field<int>("HelmetGeosetVisDataID") == helmetGeoVis1)
                    .Select(x => new ItemHideGeosetData()
                    {
                        RaceId = x.Field<int>("RaceID"),
                        GeosetGroup = x.Field<int>("HideGeosetGroup"),
                        RaceBitSelection = x.Field<int>("RaceBitSelection"),
                    }).ToList();
            }

            return modelData;
        }
        private List<TextureFileData> GetTextureFiles(int modelResourceId)
        {
            var result = new List<TextureFileData>();
            var textures = _dbcdStorageProvider["TextureFileData"]
                .Where(x => x.Field<int>("MaterialResourcesID") == modelResourceId);
            foreach (var texture in textures)
            {
                _dbcdStorageProvider["ComponentTextureFileData"].TryGetValue(texture.ID, out var componentTextureFileData);
                result.Add(new TextureFileData()
                {
                    FileDataId = (uint)texture.ID,
                    GenderId = componentTextureFileData?.Field<int>("GenderIndex") ?? 3,
                    ClassId = componentTextureFileData?.Field<int>("ClassID") ?? 0,
                    RaceId = componentTextureFileData?.Field<int>("RaceID") ?? 0,
                    UsageType = texture.Field<int>("UsageType"),
                });
            }
            return result;
        }

        private List<ModelFileData> GetModelFiles(int modelResourceId)
        {
            var result = new List<ModelFileData>();

            var modelInfo = _dbcdStorageProvider["ModelFileData"]
                   .Where(x => x.Field<int>("ModelResourcesID") == modelResourceId);
            foreach (var model in modelInfo)
            {
                _dbcdStorageProvider["ComponentModelFileData"].TryGetValue(model.ID, out var componentModelData);
                result.Add(new ModelFileData()
                {
                    FileDataId = (uint)model.ID,
                    GenderId = componentModelData?.Field<int>("GenderIndex") ?? 2,
                    ClassId = componentModelData?.Field<int>("ClassID") ?? 0,
                    RaceId = componentModelData?.Field<int>("RaceID") ?? 0,
                    PositionIndex = componentModelData?.Field<int>("PositionIndex") ?? -1,
                });
            }
            return result;
        }
    }
}

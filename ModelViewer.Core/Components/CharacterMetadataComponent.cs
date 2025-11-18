using ModelViewer.Core.Models;
using ModelViewer.Core.Providers;
using ModelViewer.Core.Utils;

namespace ModelViewer.Core.Components
{
    public class CharacterMetadataComponent : IComponent
    {
        private readonly IDBCDStorageProvider _dbcdStorageProvider;

        public CharacterMetadataComponent(IDBCDStorageProvider storageProvider)
        {
            _dbcdStorageProvider = storageProvider;
        }

        public CharacterMetadata? GetMetadataForCharacter(int characterModelId)
        {
            // Load DBC Data
            if (!_dbcdStorageProvider["ChrModel"].TryGetValue(characterModelId, out var chrModel))
            {
                return null;
            }
            if (!_dbcdStorageProvider["CreatureDisplayInfo"].TryGetValue(chrModel.Field<int>("DisplayID"), out var creatureDisplayInfo))
            {
                return null;
            }
            _dbcdStorageProvider["CreatureModelData"].TryGetValue(creatureDisplayInfo.Field<int>("ModelID"), out var creatureModelData);
            if (creatureModelData == null)
            {
                return null;
            }
            _dbcdStorageProvider["CharComponentTextureLayouts"].TryGetValue(chrModel.Field<int>("CharComponentTextureLayoutID"), out var charComponentTextureLayouts);
            if (charComponentTextureLayouts == null)
            {
                return null;
            }

            var characterCustomizationData = new CharacterCustomizationMetadata();
            characterCustomizationData.ModelMaterials = _dbcdStorageProvider["ChrModelMaterial"]
                .HavingColumnVal("CharComponentTextureLayoutsID", charComponentTextureLayouts.ID)
                .Select(x => new CharacterCustomizationMaterialsData()
                {
                    TextureType = x.Field<int>("TextureType"),
                    Width = x.Field<int>("Width"),
                    Height = x.Field<int>("Height"),
                    Flags = x.Field<int>("Flags"),
                }).ToList();


            var materialResourceIds = new List<int>();
            var options = _dbcdStorageProvider["ChrCustomizationOption"]
                .HavingColumnVal("ChrModelID", chrModel.ID)
                .OrderBy(x => x.Field<int>("OrderIndex"));
            foreach (var opt in options)
            {
                var optionData = new CharacterCustomizationOptionData()
                {
                    Id = opt.ID,
                    Name = opt.Field<string>("Name_lang"),
                    OrderIndex = opt.Field<int>("OrderIndex"),
                };
                var choiceRows = _dbcdStorageProvider["ChrCustomizationChoice"]
                    .HavingColumnVal("ChrCustomizationOptionID", opt.ID);
                foreach (var choice in choiceRows)
                {
                    var choiceData = new CharacterCustomizationChoiceData()
                    {
                        Id = choice.ID,
                        Name = choice.Field<string>("Name_lang"),
                        OrderIndex = choice.Field<int>("OrderIndex")
                    };
                    var elemRows = _dbcdStorageProvider["ChrCustomizationElement"]
                        .HavingColumnVal("ChrCustomizationChoiceID", choice.ID);
                    foreach (var elem in elemRows)
                    {
                        CharacterCustomizationBoneSetData? boneSet = null;
                        CharacterCustomizationGeosetData? geoset = null;
                        CharacterCustomizationMaterialData? material = null;
                        CharacterCustomizationSkinnedModelData? skinnedModel = null;
                        CharacterCustomizationtItemGeoModifyData? itemGeoModify = null;
                        uint condModelFileDataId = 0;

                        if (elem.Field<int>("ChrCustomizationBoneSetID") != 0)
                        {
                            var boneSetData = _dbcdStorageProvider["ChrCustomizationBoneSet"][elem.Field<int>("ChrCustomizationBoneSetID")];
                            boneSet = new()
                            {
                                BoneFileDataId = boneSetData.Field<int>("BoneFileDataID"),
                                ModelFileDataId = boneSetData.Field<int>("ModelFileDataID")
                            };
                        }

                        if (elem.Field<int>("ChrCustomizationGeosetID") != 0)
                        {
                            var geosetData = _dbcdStorageProvider["ChrCustomizationGeoset"][elem.Field<int>("ChrCustomizationGeosetID")];
                            geoset = new()
                            {
                                GeosetType = geosetData.Field<int>("GeosetType"),
                                GeosetId = geosetData.Field<int>("GeosetID"),
                                Modifier = geosetData.Field<int>("Modifier"),
                            };
                        }

                        if (elem.Field<int>("ChrCustomizationMaterialID") != 0)
                        {
                            var materialData = _dbcdStorageProvider["ChrCustomizationMaterial"][elem.Field<int>("ChrCustomizationMaterialID")];

                            var matResId = materialData.Field<int>("MaterialResourcesID");
                            var textureData = _dbcdStorageProvider["TextureFileData"]
                                .HavingColumnVal("MaterialResourcesID", matResId);
                            material = new()
                            {
                                ChrModelTextureTargetId = materialData.Field<int>("ChrModelTextureTargetID"),
                                TextureFiles = textureData == null ? [] : textureData.Select((x) =>
                                {
                                    _dbcdStorageProvider["ComponentTextureFileData"].TryGetValue(x.ID, out var compData);
                                    return new TextureFileData()
                                    {
                                        FileDataId = (uint)x.ID,
                                        GenderId = compData?.Field<int>("GenderIndex") ?? 3,
                                        ClassId = compData?.Field<int>("ClassID") ?? 0,
                                        RaceId = compData?.Field<int>("RaceID") ?? 0,
                                        UsageType = x.Field<int>("UsageType"),
                                    };
                                }).ToList()
                            };
                        }

                        if (elem.Field<int>("ChrCustomizationSkinnedModelID") != 0)
                        {
                            var skinnedModelData = _dbcdStorageProvider["ChrCustomizationSkinnedModel"][elem.Field<int>("ChrCustomizationSkinnedModelID")];
                            skinnedModel = new()
                            {
                                CollectionsFileDataId = skinnedModelData.Field<int>("CollectionsFileDataID"),
                                GeosetType = skinnedModelData.Field<int>("GeosetType"),
                                GeosetId = skinnedModelData.Field<int>("GeosetID"),
                                Modifier = skinnedModelData.Field<int>("Modifier"),
                                Flags = 0
                            };
                        }

                        if (elem.Field<int>("ChrCustomizationCondModelID") != 0)
                        {
                            var condModelData = _dbcdStorageProvider["ChrCustomizationCondModel"][elem.Field<int>("ChrCustomizationCondModelID")];
                            var condCmdData = _dbcdStorageProvider["CreatureModelData"][condModelData.Field<int>("CreatureModelDataID")];
                            condModelFileDataId = condCmdData?.Field<uint>("FileDataID") ?? 0;
                        }

                        if (elem.Field<int>("ChrCustItemGeoModifyID") != 0)
                        {
                            var itemGeomodifyData = _dbcdStorageProvider["ChrCustItemGeoModify"][elem.Field<int>("ChrCustItemGeoModifyID")];
                            itemGeoModify = new()
                            {
                                GeosetType = itemGeomodifyData.Field<int>("GeosetType"),
                                Original = itemGeomodifyData.Field<int>("Original"),
                                Override = itemGeomodifyData.Field<int>("Override"),
                            };
                        }

                        var relationChoiceId = elem.Field<int>("RelatedChrCustomizationChoiceID");
                        _dbcdStorageProvider["ChrCustomizationChoice"].TryGetValue(relationChoiceId, out var variationChoice);
                        choiceData.Elements.Add(new CharacterCustomizationElementData()
                        {
                            Id = elem.ID,
                            RelationIndex = variationChoice?.Field<int>("OrderIndex") ?? -1,
                            RelationChoiceID = relationChoiceId,
                            BoneSet = boneSet,
                            ConditionalModelFileDataId = condModelFileDataId,
                            Geoset = geoset,
                            Material = material,
                            CustItemGeoModify = itemGeoModify,
                            SkinnedModel = skinnedModel,
                        });
                    }

                    optionData.Choices.Add(choiceData);
                }
                characterCustomizationData.Options.Add(optionData);
            }

            var textureLayersData = _dbcdStorageProvider["ChrModelTextureLayer"]
                .HavingColumnVal("CharComponentTextureLayoutsID", charComponentTextureLayouts.ID);
            foreach (var layer in textureLayersData)
            {
                var section = -1;
                var sectionBits = layer.Field<int>("TextureSectionTypeBitMask");
                if (sectionBits > 0)
                {
                    section++;
                    while ((sectionBits = sectionBits >> 1) > 0)
                    {
                        section++;
                    }
                }
                characterCustomizationData.TextureLayers.Add(new()
                {
                    TextureType = layer.Field<int>("TextureType"),
                    Layer = layer.Field<int>("Layer"),
                    BlendMode = layer.Field<int>("BlendMode"),
                    ChrModelTextureTargetId = layer.Field<int[]>("ChrModelTextureTargetID")[0],
                    TextureSection = section,
                });
            }
            var textureSectionsData = _dbcdStorageProvider["CharComponentTextureSections"]
                .HavingColumnVal("CharComponentTextureLayoutID", charComponentTextureLayouts.ID);
            foreach (var section in textureSectionsData)
            {
                characterCustomizationData.TextureSections.Add(new()
                {
                    SectionType = section.Field<int>("SectionType"),
                    X = section.FieldAs<float>("X"),
                    Y = section.FieldAs<float>("Y"),
                    Width = section.FieldAs<float>("Width"),
                    Height = section.FieldAs<float>("Height"),
                });
            }

            var chrRaceXChrModel = _dbcdStorageProvider["ChrRaceXChrModel"]
                .FirstOrDefault(x => x.Field<int>("ChrModelID") == chrModel.ID);
            if (chrRaceXChrModel == null)
            {
                return null;
            }
            var chrRaces = _dbcdStorageProvider["ChrRaces"][chrRaceXChrModel.Field<int>("ChrRacesID")];

            return new CharacterMetadata()
            {
                FileDataId = creatureModelData.Field<uint>("FileDataID"),
                Flags = chrModel.Field<int>("Flags"),
                RaceId = chrRaces.ID,
                GenderId = chrModel.Field<int>("Sex"),
                ChrModelId = chrModel.ID,
                CharacterCustomizationData = characterCustomizationData
            };
        }
    }
}

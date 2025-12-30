using ModelViewer.Core.Models;
using ModelViewer.Core.Providers;
using ModelViewer.Core.Utils;

namespace ModelViewer.Core.Components
{
    public class ItemVisualComponent : IComponent
    {
        private readonly IDBCDStorageProvider _dbcdStorageProvider;

        public ItemVisualComponent(IDBCDStorageProvider storageProvider)
        {
            _dbcdStorageProvider = storageProvider;
        }

        public ItemVisualMetadata? GetItemVisualMetadata(int visualId)
        {
            if (!_dbcdStorageProvider["ItemVisuals"].TryGetValue(visualId, out var visualInfo))
            {
                return null;
            }

            var effects = _dbcdStorageProvider["ItemVisualsXEffect"]
                .HavingColumnVal("ItemVisualsID", visualId);

            var result = new ItemVisualMetadata()
            {
                Effects = effects.Select(x =>
                {
                    SpellVisualKitData? kit = null;
                    if (x.Field<int>("SpellVisualKitID") != 0)
                    {
                        kit = new()
                        {
                            Effects = []
                        };
                        var kitData = _dbcdStorageProvider["SpellVisualKit"][x.Field<int>("SpellVisualKitID")];
                        var effects = _dbcdStorageProvider["SpellVisualKitEffect"]
                            .HavingColumnVal("ParentSpellVisualKitID", kitData.ID);
                        foreach (var effect in effects)
                        {
                            var effectData = GetEffectDataForEffectType(effect.Field<int>("EffectType"), effect.Field<int>("Effect"));
                            if (effectData != null)
                            {
                                kit.Effects.Add(effectData);
                            }
                        }
                    }
                    return new ItemVisualEffectsData()
                    {
                        AttachmentId = x.Field<int>("AttachmentID"),
                        SpellVisualKit = kit,
                        ModelFileDataId = x.Field<int>("AttachmentModelFileID"),
                        Scale = x.Field<float>("Scale"),
                        SubClassId = x.Field<int>("DisplayWeaponSubclassID")
                    };
                }).ToList()
            };

            return result;
        }

        private SpellVisualKitEffectData? GetEffectDataForEffectType(int type, int id)
        {
            switch (type)
            {
                case 1: return GetProceduralEffect(id);
                case 2: return GetSpellVisualKitModelAttachEffect(id);
                case 16: return GetGradientEffectData(id);
                default: return null;
            }
        }

        private SpellVisualKitEffectData GetProceduralEffect(int id)
        {

            var effectData = _dbcdStorageProvider["SpellProceduralEffect"][id];

            return new SpellVisualKitEffectData()
            {
                EffectType = 1,
                ProceduralEffectType = effectData.Field<int>("Type"),
                Value = effectData.Field<float[]>("Value")
            };
        }

        private SpellVisualKitEffectData GetSpellVisualKitModelAttachEffect(int id)
        {

            var effectData = _dbcdStorageProvider["SpellVisualKitModelAttach"][id];

            return new SpellVisualKitEffectData()
            {
                EffectType = 1,
                Offset = effectData.Field<int[]>("Offset"),
                OffsetVariation = effectData.Field<int[]>("OffsetVariation"),
                AttachmentId = effectData.Field<int>("AttachmentID"),
                PositionerId = effectData.Field<int>("PositionerID"),
                Yaw = effectData.Field<int>("Yaw"),
                Pitch = effectData.Field<int>("Pitch"),
                Roll = effectData.Field<int>("Roll"),
                Scale = effectData.Field<int>("Scale"),
                AnimID = effectData.Field<int>("AnimID"),
            };
        }

        private SpellVisualKitEffectData GetGradientEffectData(int id)
        {
            var effectData = _dbcdStorageProvider["GradientEffect"][id];
            return new SpellVisualKitEffectData()
            {
                Colors0 = [
                   effectData.Field<float>("Colors0_r"),
                   effectData.Field<float>("Colors0_g"),
                   effectData.Field<float>("Colors0_b"),
                ],
                Colors1 = [
                   effectData.Field<float>("Colors1_r"),
                   effectData.Field<float>("Colors1_g"),
                   effectData.Field<float>("Colors1_b"),
                ],
                Colors2 = [
                   effectData.Field<float>("Colors2_r"),
                   effectData.Field<float>("Colors2_g"),
                   effectData.Field<float>("Colors2_b"),
                ],
                Alpha = [
                   effectData.Field<float>("Alpha1"),
                   effectData.Field<float>("Alpha2"),
                ],
                EdgeColor = [
                   effectData.Field<float>("EdgeColor_r"),
                   effectData.Field<float>("EdgeColor_g"),
                   effectData.Field<float>("EdgeColor_b"),
                ],
                GradientFlags = effectData.Field<int>("Field_8_1_0_28440_015"),
                EffectType = 16
            };
        }
    }
}

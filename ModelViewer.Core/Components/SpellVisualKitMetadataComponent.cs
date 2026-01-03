using ModelViewer.Core.Models;
using ModelViewer.Core.Providers;
using ModelViewer.Core.Utils;

namespace ModelViewer.Core.Components
{
    public class SpellVisualKitMetadataComponent : IComponent
    {
        private readonly IDBCDStorageProvider _dbcdStorageProvider;

        public SpellVisualKitMetadataComponent(IDBCDStorageProvider storageProvider)
        {
            _dbcdStorageProvider = storageProvider;
        }

        public SpellVisualKitMetadata? GetSpellVisualKitMetadata(int kitId)
        {
            if (!_dbcdStorageProvider["SpellVisualKit"].TryGetValue(kitId, out var kitData))
            {
                return null;
            }
            var result = new SpellVisualKitMetadata()
            {
                SpellVisualKitId = kitId,
            };
            var effects = _dbcdStorageProvider["SpellVisualKitEffect"]
                .HavingColumnVal("ParentSpellVisualKitID", kitData.ID);
            foreach (var effect in effects)
            {
                var effectData = GetEffectDataForEffectType(effect.Field<int>("EffectType"), effect.Field<int>("Effect"));
                if (effectData != null)
                {
                    result.Effects.Add(effectData);
                }
            }
            return result;
        }


        private SpellVisualKitEffectData? GetEffectDataForEffectType(int type, int id)
        {
            switch ((SpellVisualKitEffectType) type)
            {
                case SpellVisualKitEffectType.ModelAttach: return GetSpellVisualKitModelAttachEffect(id);
                default: return null;
            }
        }

        private ModelAttachVisualKitEffectData GetSpellVisualKitModelAttachEffect(int id)
        {
            var effectData = _dbcdStorageProvider["SpellVisualKitModelAttach"][id];

            var result = new ModelAttachVisualKitEffectData()
            {
                Type = SpellVisualKitEffectType.ModelAttach,
                Offset = effectData.Field<float[]>("Offset"),
                OffsetVariation = effectData.Field<float[]>("OffsetVariation"),
                AttachmentId = effectData.Field<int>("AttachmentID"),
                Yaw = effectData.Field<float>("Yaw"),
                Pitch = effectData.Field<float>("Pitch"),
                Roll = effectData.Field<float>("Roll"),
                YawVariation = effectData.Field<float>("YawVariation"),
                PitchVariation = effectData.Field<float>("PitchVariation"),
                RollVariation = effectData.Field<float>("RollVariation"),
                Scale = effectData.Field<float>("Scale"),
                ScaleVariation = effectData.Field<float>("ScaleVariation"),
                StartAnimId = effectData.Field<int>("StartAnimID"),
                AnimId = effectData.Field<int>("AnimID"),
                EndAnimId = effectData.Field<int>("EndAnimID"),
                StartDelay = effectData.Field<float>("StartDelay"),
                AnimKitId = effectData.Field<int>("AnimKitID"),
                Flags = effectData.Field<int>("Flags"),
            };
            var spellVisualEffectNameId = effectData.Field<int>("SpellVisualEffectNameID");
            if (spellVisualEffectNameId > 0)
            {
                _dbcdStorageProvider["SpellVisualEffectName"].TryGetValue(spellVisualEffectNameId, out var svEffectName);
                if (svEffectName == null)
                {
                    throw new Exception("Unable to find spell visual effect name listed for spell visual kit model attach id: " + id);
                }
                result.SpellVisualEffectName = new()
                {
                    Id = svEffectName.ID,
                    ModelFileDataId = svEffectName.Field<int>("ModelFileDataID"),
                    BaseMissileSpeed = svEffectName.Field<int>("BaseMissileSpeed"),
                    Scale = svEffectName.Field<float>("Scale"),
                    MinAllowedScale = svEffectName.Field<float>("MinAllowedScale"),
                    MaxAllowedScale = svEffectName.Field<float>("MaxAllowedScale"),
                    Alpha = svEffectName.Field<float>("Alpha"),
                    Flags = svEffectName.Field<int>("Flags"),
                    TextureFileDataId = svEffectName.Field<int>("TextureFileDataID"),
                    Type = (SpellVisualEffectNameType)svEffectName.Field<int>("Type"),
                    GenericId = svEffectName.Field<int>("GenericID"),
                    DissolveEffectId = svEffectName.Field<int>("DissolveEffectID"),
                    ModelPosition = svEffectName.Field<int>("ModelPosition")
                };
            }
            var positionerId = effectData.Field<int>("PositionerID");
            if (positionerId > 0)
            {
                result.Positioner = GetPositionerData(positionerId);
            }

            return result;
        }

        public PositionerData GetPositionerData(int id)
        {
            _dbcdStorageProvider["Positioner"].TryGetValue(id, out var positioner);
            if (positioner == null)
            {
                throw new Exception("Unable to find positioner listed for spell visual kit model attach id: " + id);
            }
            var result = new PositionerData()
            {
                FirstStateId = positioner.Field<int>("FirstStateID"),
                Flags = positioner.Field<int>("Flags"),
                StartLife = positioner.Field<float>("StartLife"),
            };

            var currentStateId = result.FirstStateId;
            while (currentStateId > 0)
            {
                _dbcdStorageProvider["PositionerState"].TryGetValue(currentStateId, out var stateEntry);
                if (stateEntry == null)
                {
                    throw new Exception($"Unable to find positioner state {currentStateId} listed for positioner id: {positioner.ID}");
                }

                var stateData = new PositionerStateData()
                {
                    Id = currentStateId,
                    NextStateId = stateEntry.Field<int>("NextStateID"),
                    Flags = stateEntry.Field<int>("Flags"),
                    EndLife = stateEntry.Field<float>("EndLife"),
                    EndLifePercent = stateEntry.Field<int>("EndLifePercent"),
                };

                var matrixId = stateEntry.Field<int>("TransformMatrixID");
                if (matrixId > 0)
                {
                    stateData.TransformMatrix = GetMatrixData(matrixId, currentStateId, positioner.ID);
                }

                var posId = stateEntry.Field<int>("PosEntryID");
                if (posId > 0)
                {
                    stateData.PositionEntry = GetPositionerStateEntry(posId, currentStateId, positioner.ID);
                }

                var rotId = stateEntry.Field<int>("RotEntryID");
                if (rotId > 0)
                {
                    stateData.RotationEntry = GetPositionerStateEntry(rotId, currentStateId, positioner.ID);
                }

                var scaleId = stateEntry.Field<int>("ScaleEntryID");
                if (scaleId > 0)
                {
                    stateData.ScaleEntry = GetPositionerStateEntry(scaleId, currentStateId, positioner.ID);
                }

                result.States.Add(stateData);
                currentStateId = stateData.NextStateId;
            }
            return result;
        }

        public TransformMatrixData GetMatrixData(int matrixId, int currentStateId, int positionerId)
        {
            _dbcdStorageProvider["TransformMatrix"].TryGetValue(matrixId, out var matrixEntry);
            if (matrixEntry == null)
            {
                throw new Exception($"Unable to find transform matrix {matrixId} listed for positioner state {currentStateId} for positioner id: {positionerId}");
            }

            return new()
            {
                Pos = matrixEntry.Field<float[]>("Pos"),
                Yaw = matrixEntry.Field<float>("Yaw"),
                Pitch = matrixEntry.Field<float>("Pitch"),
                Roll = matrixEntry.Field<float>("Roll"),
                Scale = matrixEntry.Field<float>("Scale"),
            };
        }

        public PositionerStateEntryData GetPositionerStateEntry(int entryId, int currentStateId, int positionerId)
        {
            _dbcdStorageProvider["PositionerStateEntry"].TryGetValue(entryId, out var entry);
            if (entry == null)
            {
                throw new Exception($"Unable to find positioner state id '{entryId}' listed for positioner state {currentStateId} for positioner id: {positionerId}");
            }

            return new()
            {
                Id = entryId,
                ParamA = entry.Field<float>("ParamA"),
                ParamB = entry.Field<float>("ParamB"),
                SrcValType = entry.Field<int>("SrcValType"),
                SrcVal = entry.Field<float>("SrcVal"),
                DstValType = entry.Field<int>("DstValType"),
                DstVal = entry.Field<float>("DstVal"),
                Type = (PositionerStateEntryType)entry.Field<int>("EntryType"),
                Style = entry.Field<int>("Style"),
                SrcType = entry.Field<int>("SrcType"),
                DstType = entry.Field<int>("DstType")
            };
        }
    }
}

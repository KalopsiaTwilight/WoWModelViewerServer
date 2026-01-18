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
                case SpellVisualKitEffectType.Beam: return GetSpellVisualKitBeamEffect(id);
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

        private BeamVisualKitEffectData GetSpellVisualKitBeamEffect(int id)
        {
            var effectData = _dbcdStorageProvider["BeamEffect"][id];

            var result = new BeamVisualKitEffectData()
            {
                Type = SpellVisualKitEffectType.Beam,
                SourceMinDistance = effectData.Field<float>("SourceMinDistance"),
                FixedLength = effectData.Field<int>("FixedLength"),
                Flags = effectData.Field<int>("Flags"),
                SourceOffset = effectData.Field<int>("SourceOffset"),
                DestOffset = effectData.Field<int>("DestOffset"),
                DestAttachId = effectData.Field<int>("DestAttachID"),
                SourceAttachId = effectData.Field<int>("SourceAttachID"),
            };

            var srcPositionerId = effectData.Field<int>("SourcePositionerID");
            if (srcPositionerId > 0)
            {
                result.SourcePositioner = GetPositionerData(srcPositionerId);
            }
            var destPositionerId = effectData.Field<int>("DestPositionerID");
            if (destPositionerId > 0)
            {
                result.DestPositioner = GetPositionerData(destPositionerId);
            }

            var beamId = effectData.Field<ushort>("BeamID");
            result.Beam = GetSpellChainEffectData(beamId);
            return result;
        }

        public SpellChainEffectData GetSpellChainEffectData(ushort id)
        {
            _dbcdStorageProvider["SpellChainEffects"].TryGetValue(id, out var effect);
            if (effect == null)
            {
                throw new Exception("Unable to find spell chain effect with id: " + id);
            }

            var result = new SpellChainEffectData
            {
                Id = id,
                AvgSegLength = effect.Field<float>("AvgSegLen"),
                NoiseScale = effect.Field<float>("NoiseScale"),
                TexCoordScale = effect.Field<float>("TexCoordScale"),
                SegDuration = effect.Field<int>("SegDuration"),
                SegDelay = effect.Field<int>("SegDelay"),
                Flags = effect.Field<int>("Flags"),
                JointCount = effect.Field<int>("JointCount"),
                JointOffsetRadius = effect.Field<float>("JointOffsetRadius"),
                JointsPerMinorJoint = effect.Field<int>("JointsPerMinorJoint"),
                MinorJointsPerMajorJoint = effect.Field<int>("MinorJointsPerMajorJoint"),
                MinorJointScale = effect.Field<float>("MinorJointScale"),
                MajorJointScale = effect.Field<float>("MajorJointScale"),
                JointMoveSpeed = effect.Field<float>("JointMoveSpeed"),
                JointSmoothness = effect.Field<float>("JointSmoothness"),
                MinDurationBetweenJointsJumps = effect.Field<float>("MinDurationBetweenJointJumps"),
                MaxDurationBetweenJointsJumps = effect.Field<float>("MaxDurationBetweenJointJumps"),
                WaveHeight = effect.Field<float>("WaveHeight"),
                WaveFreq = effect.Field<float>("WaveFreq"),
                WaveSpeed = effect.Field<float>("WaveSpeed"),
                MinWaveAngle = effect.Field<float>("MinWaveAngle"),
                MaxWaveAngle = effect.Field<float>("MaxWaveAngle"),
                MinWaveSpin = effect.Field<float>("MinWaveSpin"),
                MaxWaveSpin = effect.Field<float>("MaxWaveSpin"),
                ArcHeight = effect.Field<float>("ArcHeight"),
                MinArcAngle = effect.Field<float>("MinArcAngle"),
                MaxArcAngle = effect.Field<float>("MaxArcAngle"),
                MinArcSpin = effect.Field<float>("MinArcSpin"),
                MaxArcSpin = effect.Field<float>("MaxArcSpin"),
                DelayBetweenEffects = effect.Field<float>("DelayBetweenEffects"),
                MinFlickerOnDuration = effect.Field<float>("MinFlickerOnDuration"),
                MaxFlickerOnDuration = effect.Field<float>("MaxFlickerOnDuration"),
                MinFlickerOffDuration = effect.Field<float>("MinFlickerOffDuration"),
                MaxFlickerOffDuration = effect.Field<float>("MaxFlickerOffDuration"),
                PulseSpeed = effect.Field<int>("PulseSpeed"),
                PulseOnLength = effect.Field<int>("PulseOnLength"),
                PulseFadeLength = effect.Field<int>("PulseFadeLength"),
                Alpha = effect.Field<byte>("Alpha"),
                Red = effect.Field<byte>("Red"),
                Green = effect.Field<byte>("Green"),
                Blue = effect.Field<byte>("Blue"),
                BlendMode = effect.Field<int>("BlendMode"),
                RenderLayer = effect.Field<int>("RenderLayer"),
                WavePhase = effect.Field<float>("WavePhase"),
                TimePerFlipFrame = effect.Field<float>("TimePerFlipFrame"),
                VariancePerFlipFrame = effect.Field<float>("VariancePerFlipFrame"),
                TextureParticleFileDataId = effect.Field<int>("TextureParticleFileDataID"),
                StartWidth = effect.Field<float>("StartWidth"),
                EndWidth = effect.Field<float>("EndWidth"),
                NumFlipFramesU = effect.Field<int>("NumFlipFramesU"),
                NumFlipFramesV = effect.Field<int>("NumFlipFramesV"),
                ParticleScaleMultiplier = effect.Field<float>("ParticleScaleMultiplier"),
                ParticleEmissionRateMultiplier = effect.Field<float>("ParticleEmissionRateMultiplier"),
                TextureCoordScaleU = effect.Field<float[]>("TextureCoordScaleU"),
                TextureCoordScaleV = effect.Field<float[]>("TextureCoordScaleV"),
                TextureRepeatLengthU = effect.Field<float[]>("TextureRepeatLengthU"),
                TextureRepeatLengthV = effect.Field<float[]>("TextureRepeatLengthV"),
                TextureFileDataIds = effect.Field<int[]>("TextureFileDataID"),
                SpellChainEffects = [.. effect.Field<ushort[]>("SpellChainEffectID").Where(x => x > 0).Select(GetSpellChainEffectData)]
            };

            var curveId = effect.Field<int>("WidthScaleCurveID");
            if (curveId > 0)
            {
                _dbcdStorageProvider["Curve"].TryGetValue(curveId, out var curve);
                if (curve == null)
                {
                    throw new Exception("Unable to find curve with id: " + id);
                }

                result.WidthScaleCurve = new CurveData()
                {
                    Id = curveId,
                    Flags = curve.Field<int>("Flags"),
                    Type = curve.Field<int>("Type"),
                };
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

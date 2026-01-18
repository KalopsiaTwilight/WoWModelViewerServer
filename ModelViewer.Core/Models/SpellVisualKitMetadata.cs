using ModelViewer.Core.JsonConverters;
using System.Text.Json.Serialization;

namespace ModelViewer.Core.Models
{
    public class SpellVisualKitMetadata
    {
        public int SpellVisualKitId { get; set; }
        public List<SpellVisualKitEffectData> Effects { get; set; } = [];
    };

    public enum SpellVisualKitEffectType
    {
        None = 0,
        Procedural = 1,
        ModelAttach = 2,
        CameraEffect = 3,
        CameraEffect2 = 4,
        SoundKit = 5,
        SpellVisualAnim = 6,
        Shadowy = 7,
        Emission = 8,
        Outline = 9,
        UnitSoundType = 10,
        Dissolve = 11,
        EdgeGlow = 12,
        Beam = 13,
        ClientScene = 14,
        Unknown1 = 15,
        Gradient = 16,
        Barrage = 17,
        Rope = 18,
        Screen = 19,
    }

    [JsonConverter(typeof(SpellVisualKitEffectDataConverter))]
    public abstract class SpellVisualKitEffectData
    {
        public SpellVisualKitEffectType Type { get; set; }
    }

    public class ModelAttachVisualKitEffectData : SpellVisualKitEffectData
    {
        public float[] Offset { get; set; } = [];
        public float[] OffsetVariation { get; set; } = [];
        public int AttachmentId { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public float YawVariation { get; set; }
        public float PitchVariation { get; set; }
        public float RollVariation { get; set; }
        public float Scale { get; set; }
        public float ScaleVariation { get; set; }
        public int StartAnimId { get; set; }
        public int AnimId { get; set;  }
        public int EndAnimId { get; set; }
        public int AnimKitId { get; set; }
        public int Flags { get; set; }
        public float StartDelay { get; set; }
        public SpellVisualEffectNameData? SpellVisualEffectName { get; set; }
        public PositionerData? Positioner { get; set; }
    }

    public class BeamVisualKitEffectData : SpellVisualKitEffectData
    {
        public float SourceMinDistance { get; set; }
        public int FixedLength { get; set; }
        public int Flags { get; set; }
        public int SourceOffset { get; set; }
        public int DestOffset { get; set; }
        public int SourceAttachId { get; set; }
        public int DestAttachId { get; set; }
        public PositionerData? SourcePositioner { get; set; } 
        public PositionerData? DestPositioner { get; set;  }
        public SpellChainEffectData? Beam { get; set; }
    }

    public class SpellChainEffectData
    {
        public int Id { get; set; }
        public float AvgSegLength { get; set;  }
        public float NoiseScale { get; set; }
        public float TexCoordScale { get; set; } 
        public int SegDuration { get; set;  }
        public int SegDelay { get; set; }
        public int Flags { get; set; }
        public int JointCount { get; set; }
        public float JointOffsetRadius { get; set;  }
        public int JointsPerMinorJoint { get; set; }
        public int MinorJointsPerMajorJoint { get; set; }
        public float MinorJointScale { get; set; }
        public float MajorJointScale { get; set; }
        public float JointMoveSpeed { get; set; }
        public float JointSmoothness { get; set; }
        public float MinDurationBetweenJointsJumps { get; set; }
        public float MaxDurationBetweenJointsJumps { get; set; }
        public float WaveHeight { get; set; }
        public float WaveFreq { get; set; }
        public float WaveSpeed { get; set; }
        public float MinWaveAngle { get; set; }
        public float MaxWaveAngle { get; set; }
        public float MinWaveSpin { get; set; }
        public float MaxWaveSpin { get; set; }
        public float ArcHeight { get; set; }
        public float MinArcAngle { get; set; }
        public float MaxArcAngle { get; set; }
        public float MinArcSpin { get; set; }
        public float MaxArcSpin { get; set; }
        public float DelayBetweenEffects { get; set; }
        public float MinFlickerOnDuration { get; set; }
        public float MaxFlickerOnDuration { get; set; }
        public float MinFlickerOffDuration { get; set; }
        public float MaxFlickerOffDuration { get; set; }
        public int PulseSpeed { get; set; }
        public float PulseOnLength { get; set; }
        public float PulseFadeLength { get; set; }
        public byte Alpha { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public int BlendMode { get; set; }
        public int RenderLayer { get; set; }
        public float WavePhase { get; set; }
        public float TimePerFlipFrame { get; set; }
        public float VariancePerFlipFrame { get; set; }
        public int TextureParticleFileDataId { get; set; }
        public float StartWidth { get; set; }
        public float EndWidth { get; set;  }
        public int NumFlipFramesU { get; set; }
        public int NumFlipFramesV { get; set; }
        public float ParticleScaleMultiplier { get; set; }
        public float ParticleEmissionRateMultiplier { get; set; }
        public float[] TextureCoordScaleU { get; set; } = [];
        public float[] TextureCoordScaleV { get; set; } = [];
        public float[] TextureRepeatLengthU { get; set; } = [];
        public float[] TextureRepeatLengthV { get; set; } = [];
        public int[] TextureFileDataIds { get; set; } = [];
        public CurveData? WidthScaleCurve { get; set; }
        public List<SpellChainEffectData> SpellChainEffects { get; set; } = [];
    }

    public class CurveData
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int Flags { get; set; }
    }

    public enum CurveType
    {
        Linear = 0,
        Cubic = 1,
        Bezier = 2,
        Cosine = 3,
    }

    public enum SpellVisualEffectNameType
    {
        FileDataID = 0,
        Item = 1,
        CreatureDisplayInfo = 2,
        Unk1 = 3,
        Unk2 = 4,
        Unk3 = 5,
        Unk4 = 6,
        Unk5 = 7,
        Unk6 = 8,
        Unk7 = 9,
        Unk8 = 10,
    }
    public class SpellVisualEffectNameData
    {
        public int Id { get; set; }
        public int ModelFileDataId { get; set; }
        public int BaseMissileSpeed { get; set; }
        public float Scale { get; set; }
        public float MinAllowedScale { get; set; }
        public float MaxAllowedScale { get; set; }
        public float Alpha { get; set; }
        public int Flags { get; set; }
        public int TextureFileDataId { get; set; }
        public SpellVisualEffectNameType Type { get; set; }
        public int GenericId { get; set; }
        public int DissolveEffectId { get; set; }
        public int ModelPosition { get; set; }
    }

    public class PositionerData
    {
        public int FirstStateId { get; set; }
        public int Flags { get; set; }
        public float StartLife { get; set; }
        public List<PositionerStateData> States { get; set; } = [];
    }

    public class PositionerStateData
    {
        public int Id { get; set; }
        public int NextStateId { get; set; }
        public TransformMatrixData? TransformMatrix { get; set; }
        public PositionerStateEntryData? PositionEntry { get; set; }
        public PositionerStateEntryData? RotationEntry { get; set; }
        public PositionerStateEntryData? ScaleEntry { get; set; }
        public float Flags { get; set;  }
        public float EndLife { get; set; }
        public int EndLifePercent { get; set; }
    }

    public enum PositionerStateEntryType
    {
        Position = 0,
        Rotation = 1,
        Scale = 2,
    }

    public class PositionerStateEntryData
    {
        public int Id { get; set;  }
        public float ParamA { get; set; }
        public float ParamB { get; set; }
        public int SrcValType { get; set; }
        public float SrcVal { get; set; }
        public int DstValType { get; set; }
        public float DstVal { get; set; }
        public PositionerStateEntryType Type { get; set; }
        public int Style { get; set; }
        public int SrcType { get; set; }
        public int DstType { get; set; } 
    }
}

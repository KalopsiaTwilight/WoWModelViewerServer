namespace Server.CM2
{
    using Float2 = (float X, float Y);
    using Float3 = (float X, float Y, float Z);
    using Float4 = (float X, float Y, float Z, float W);

    public class CM2Vertex
    {
        public Float3 Position { get; set; }
        public Float3 Normal { get; set; }

        public Float2 TexCoords1 { get; set; }
        public Float2 TexCoords2 { get; set; }

        public byte[] BoneWeights { get; set; } = [];
        public byte[] BoneIndices { get; set; } = [];
    }

    public class CM2Animation
    {
        public ushort Id { get; set; }
        public ushort VariationIndex { get; set; }
        public uint Duration { get; set; }
        public uint Flags { get; set; }
        public ushort Frequency { get; set; }
        public ushort BlendTimeIn { get; set; }
        public ushort BlendTimeOut { get; set; }
        public Float3 ExtentMin { get; set; }
        public Float3 ExtentMax { get; set; }
        public short VariationNext { get; set; }
        public ushort AliasNext { get; set; }
    }

    public class CM2AnimatedValue<T>
    {
        public int[] TimeStamps { get; set; } = [];
        public T[] Values { get; set; } = [];
    }

    public class CM2Track<T>
    {
        public short InterpolationType { get; set; }
        public short GlobalSequence { get; set; }
        public CM2AnimatedValue<T>[] Animations { get; set; } = [];
    }

    public class CM2Bone
    {
        public int KeyBoneId { get; set; }
        public uint Flags { get; set; }
        public short ParentBoneId { get; set; }
        public short SubMeshId { get; set; }
        public uint BoneNameCRC { get; set; }
        public Float3 Pivot { get; set; }
        public CM2Track<Float3> Translation { get; set; } = new();
        public CM2Track<Float4> Rotation { get; set; } = new();
        public CM2Track<Float3> Scale { get; set; } = new();
    }

    public class CM2Submesh
    {
        public ushort SubmeshId { get; set; }
        public ushort Level { get; set; }
        public ushort VertexStart { get; set; }
        public ushort VertexCount { get; set; }
        public ushort TriangleStart { get; set; }
        public ushort TriangleCount { get; set; }
        public ushort CenterBoneIndex { get; set; }
        public Float3 CenterPosition { get; set; }
        public Float3 SortCenterPosition { get; set; }
        public float SortRadius { get; set; }

    }

    public class CM2TextureUnit
    {
        public byte Flags { get; set; }
        public byte Priority { get; set; }
        public ushort ShaderId { get; set; }
        public ushort SkinSectionIndex { get; set; }
        public ushort Flags2 { get; set; }
        public short ColorIndex { get; set; }
        public ushort MaterialIndex { get; set; }
        public ushort MaterialLayer { get; set; }
        public ushort TextureCount { get; set; }
        public ushort TextureComboIndex { get; set; }
        public ushort TextureCoordComboIndex { get; set; }
        public ushort TextureWeightComboIndex { get; set; }
        public ushort TextureTransformComboIndex { get; set; }
    }

    public class CM2Material
    {
        public ushort Flags { get; set; }
        public ushort BlendingMode { get; set; }
    }

    public class CM2Texture
    {
        public int Type { get; set; }
        public uint Flags { get; set; }
        public uint TextureId { get; set; }
    }

    public class CM2TextureTransform
    {
        public CM2Track<Float3> Translation { get; set; } = new();
        public CM2Track<Float4> Rotation { get; set; } = new();
        public CM2Track<Float3> Scaling { get; set; } = new();
    }

    public class CM2Attachment
    {
        public int Id { get; set; }
        public int Bone { get; set; }
        public Float3 Position { get; set; }
    }

    public class CM2Color
    {
        public CM2Track<Float3> Color { get; set; } = new();
        public CM2Track<ushort> Alpha { get; set; } = new();
    }

    public class CM2TextureWeight
    {
        public CM2Track<ushort> Weights { get; set; } = new();
    }

    public class CM2LocalTrack<T>
    {
        public short[] Keys { get; set; } = [];
        public T[] Values { get; set; } = [];
    }

    public class CM2ParticleEmitter
    {
        public int ParticleId { get; set; }
        public uint Flags { get; set; }
        public Float3 Position { get; set; }
        public short Bone { get; set; }
        public short Texture { get; set; }
        public byte BlendingType { get; set; }
        public byte EmitterType { get; set; }
        public ushort ParticleColorIndex { get; set; }
        public ushort TextureTileRotation { get; set; }
        public ushort TextureDimensionsRows { get; set; }
        public ushort TextureDimensionsColumns { get; set; }
        public CM2Track<float> EmissionSpeed { get; set; } = new();
        public CM2Track<float> SpeedVariation { get; set; } = new();
        public CM2Track<float> VerticalRange { get; set; } = new();
        public CM2Track<float> HorizontalRange { get; set; } = new();
        public CM2Track<Float3> Gravity { get; set; } = new();
        public CM2Track<float> Lifespan { get; set; } = new();
        public float LifespanVary { get; set; }
        public CM2Track<float> EmissionRate { get; set; } = new();
        public float EmissionRateVary { get; set; }
        public CM2Track<float> EmissionAreaLength { get; set; } = new();
        public CM2Track<float> EmissionAreaWidth { get; set; } = new();
        public CM2Track<float> ZSource { get; set; } = new();
        public CM2LocalTrack<Float3> ColorTrack { get; set; } = new();
        public CM2LocalTrack<ushort> AlphaTrack { get; set; } = new();
        public CM2LocalTrack<Float2> ScaleTrack { get; set; } = new();
        public Float2 ScaleVary { get; set; }
        public CM2LocalTrack<ushort> HeadCellTrack { get; set; } = new();
        public CM2LocalTrack<ushort> TailCellTrack { get; set; } = new();
        public float TailLength { get; set; }
        public float TwinkleSpeed { get; set; }
        public float TwinklePercent { get; set; }
        public Float2 TwinkleScale { get; set; }
        public float BurstMultiplier { get; set; }
        public float Drag { get; set; }
        public float BaseSpin { get; set; }
        public float BaseSpinVary { get; set; }
        public float Spin { get; set; }
        public float SpinVary { get; set; }
        public Float3 TumbleModelRotationSpeedMin { get; set; }
        public Float3 TumbleModelRotationSpeedMax { get; set; }
        public Float3 WindVector { get; set; }
        public float WindTime { get; set; }
        public float FollowSpeed1 { get; set; }
        public float FollowScale1 { get; set; }
        public float FollowSpeed2 { get; set; }
        public float FollowScale2 { get; set; }
        public Float3[] SplinePoints { get; set; } = [];
        public CM2Track<byte> EnabledIn { get; set; } = new();
        public Float2 MultiTextureParamX { get; set; }
        public (Float2, Float2) MultiTextureParam0 { get; set; }
        public (Float2, Float2) MultiTextureParam1 { get; set; }
    }

    public class CM2ExtendedParticle
    {
        public float ZSource { get; set; }
        public float ColorMult { get; set; }
        public float AlphaMult { get; set; }
        public CM2LocalTrack<ushort> AlphaCutoff { get; set; } = new();
    }

    public class CM2RibbonEmiter
    {
        public int RibbonId { get; set; }
        public int BoneIndex { get; set; }
        public Float3 Position { get; set; }
        public short[] TextureIndices { get; set; } = [];
        public short[] MaterialIndices { get; set; } = [];
        public CM2Track<Float3> ColorTrack { get; set; } = new();
        public CM2Track<ushort> AlphaTrack { get; set; } = new();
        public CM2Track<float> HeightAboveTrack { get; set; } = new();
        public CM2Track<float> HeightBelowTrack { get; set; } = new();
        public float EdgesPerSecond { get; set; }
        public float EdgeLifetime { get; set; }
        public float Gravity { get; set; }
        public short TextureRows { get; set; }
        public short TextureCols { get; set; }
        public CM2Track<ushort> TexSlotTrack { get; set; } = new();
        public CM2Track<byte> VisibilityTrack { get; set; } = new();
        public short PriorityPlane { get; set; }
    }
}

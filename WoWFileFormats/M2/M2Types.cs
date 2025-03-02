using WoWFileFormats.Common;

namespace WoWFileFormats.M2
{
    public class M2Range
    {
        public uint Minimum { get; set; }
        public uint Maximum { get; set; }
    }


    public class M2Bounds
    {
        public CAxisAlignedBox Extent { get; set; }
        public float Radius { get; set; }
    }

    public class M2Sequence
    {
        public ushort Id { get; set; }
        public ushort VariationIndex { get; set; }
        public uint Duration { get; set; }
        public float Movespeed { get; set; }
        public uint Flags { get; set; }
        public ushort Frequency { get; set; }
        public ushort Padding { get; set; }
        public M2Range Replay { get; set; } = new M2Range();
        public ushort BlendTimeIn { get; set; }
        public ushort BlendTimeOut { get; set; }
        public M2Bounds Bounds { get; set; } = new M2Bounds();
        public short VariationNext { get; set; }
        public ushort AliasNext { get; set; }
    }

    public class M2TrackBase
    {
        public short InterpolationType { get; set; }
        public short GlobalSequence { get; set; }
        public int[][] TimeStamps { get; set; } = [];
    }

    public class M2Track<T> : M2TrackBase
    {
        public T[][] Values { get; set; } = [];

        public Dictionary<int, (uint, uint)> TimeStampsOutOfSequence { get; set; } = new();
        public Dictionary<int, (uint, uint)> ValuesOutOfSequence { get; set; } = new();
    }

    public class M2SplineKey<T> where T : struct
    {
        public T Value { get; set; }
        public T InTan { get; set; }
        public T OutTan { get; set; }
    }

    /** https://wowdev.wiki/M2#The_Fake-AnimationBlock */
    public class M2LocalTrack<T>
    {
        public short[] Keys { get; set; } = [];
        public T[] Values { get; set; } = [];
    }

    public class M2Box
    {
        public C3Vector ModelRotationSpeedMin { get; set; }
        public C3Vector ModelRotationSpeedMax { get; set; }
    }

    public class M2Loop
    {
        public uint Timestamp { get; set; }
    }

    [Flags]
    public enum M2CompBoneFlags
    {
        IgnoreParentTranslate = 0x1,
        IgnoreParentScale = 0x2,
        IgnoreParentRotation = 0x4,
        SphericalBillboard = 0x8,
        CylindricalBillboardLockX = 0x10,
        CylindricalBillboardLockY = 0x20,
        CylindricalBillboardLockZ = 0x40,
        Transformed = 0x200,
        KinematicBone = 0x400,       // MoP+: allow physics to influence this bone
        HelmetAnimScaled = 0x1000,  // set blend_modificator to helmetAnimScalingRec.m_amount for this bone
        SomethingSequenceId = 0x2000, // <=bfa+, parent_bone+submesh_id are a sequence id instead?!
    }

    public class M2Vertex
    {
        public C3Vector Position { get; set; }
        public byte[] BoneWeights { get; set; } = [0, 0, 0, 0];
        public byte[] BoneIndices { get; set; } = [0, 0, 0, 0];
        public C3Vector Normal { get; set; }
        public (C2Vector X, C2Vector Y) TexCoords { get; set; }
    }

    public enum M2TextureType
    {
        None = 0,
        Skin = 1,
        ObjectSkin = 2,
        WeaponBlade = 3,
        WeaponHandle = 4,
        Environment = 5,
        CharHair = 6,
        CharFacialHair = 7,
        SkinExtra = 8,
        UISkin = 9,
        TaurenMane = 10,
        Monster1 = 11,
        Monster2 = 12,
        Monster3 = 13,
        ItemIcon = 14,
        GuildBackgroundColor = 15,
        GuildEmblemColor = 16,
        GuildBorderColor = 17,
        GuildEmblem = 18,
        CharEyes = 19,
        CharJewelry = 20,
        CharSecondarySkin = 21,
        CharSecondaryHair = 22,
        CharSecondaryArmor = 23,
        Unknown24 = 24,
        Unknown25 = 25,
        Unknown26 = 26,
    }

    [Flags]
    public enum M2TextureFlags
    {
        None = 0x0,
        TextureWrapX = 0x1,
        TextureWrapY = 0x2
    }

    public class M2Texture
    {
        public M2TextureType Type { get; set; }
        public M2TextureFlags Flags { get; set; }
        public string Filename { get; set; } = string.Empty;
        public uint FileId { get; set; }
    }

    [Flags]
    public enum M2MaterialFlags
    {
        None = 0x0,
        Unlit = 0x01,
        Unfogged = 0x02,
        TwoSided = 0x04,
        DepthTest = 0x08,
        DepthWrite = 0x10,
        Unknown_0x40 = 0x40,
        Unknown_0x80 = 0x80,
        Unknown_0x100 = 0x100,
        Unknown_0x200 = 0x200,
        Unknown_0x400 = 0x400,
        PreventAlphaForCustomElements = 0x800,
    }

    public class M2Material
    {
        public M2MaterialFlags Flags { get; set; }
        public ushort BlendingMode { get; set; }
    }

    public class M2Event
    {
        public uint Identifier { get; set; }
        public uint Data { get; set; }
        public uint Bone { get; set; }
        public C3Vector Position { get; set; }
        public M2TrackBase Enabled { get; set; } = new M2TrackBase();
    }

    public class M2Camera
    {
        public uint Type { get; set; }
        public float FarClip { get; set; }
        public float NearClip { get; set; }
        public M2Track<M2SplineKey<C3Vector>> Positions { get; set; } = new();
        public C3Vector PositionBase { get; set; }
        public M2Track<M2SplineKey<C3Vector>> TargetPosition { get; set; } = new();
        public C3Vector TargetPositionBase { get; set; }
        public M2Track<M2SplineKey<float>> Roll { get; set; } = new();
        public M2Track<M2SplineKey<float>> FOV { get; set; } = new();
    }

    public class M2Ribbon
    {
        public int RibbonId { get; set; }
        public int BoneIndex { get; set; }
        public C3Vector Position { get; set; }
        public short[] TextureIndices { get; set; } = [];
        public short[] MaterialIndices { get; set; } = [];
        public M2Track<C3Vector> ColorTrack { get; set; } = new();
        public M2Track<Fixed16> AlphaTrack { get; set; } = new();
        public M2Track<float> HeightAboveTrack { get; set; } = new();
        public M2Track<float> HeightBelowTrack { get; set; } = new();
        public float EdgesPerSecond { get; set; }
        public float EdgeLifetime { get; set; }
        public float Gravity { get; set; }
        public short TextureRows { get; set; }
        public short TextureCols { get; set; }
        public M2Track<ushort> TexSlotTrack { get; set; } = new();
        public M2Track<byte> VisibilityTrack { get; set; } = new();
        public short PriorityPlane { get; set; }
        public byte RibbonColorIndex { get; set; }
        public byte TextureTransformLookupIndex { get; set; }
    }

    [Flags]
    public enum M2ParticleFlags
    {
        AffectedByLighting = 0x01,
        Unknown_0x02 = 0x02,
        OrientationIsAffectedByPlayerOrientation = 0x04,
        TravelUpInWorldspace = 0x08,
        DoNotTrail = 0x10,
        Unlightning = 0x20,
        UseBurstMulitpler = 0x40,
        ParticlesInModelSpace = 0x80,
        Unknown_0x100 = 0x100,
        Unknown_0x200 = 0x200,
        PinnedParticles = 0x400,
        Unknown_0x800 = 0x800,
        XYQuadParticles = 0x1000,
        ClampToGround = 0x2000,
        Unknown_0x4000 = 0x4000,
        Unknown_0x8000 = 0x8000,
        ChooseRandomTexture = 0x10000,
        OutwardParticles = 0x20000,
        Unknown_0x40000 = 0x40000,
        ScaleVaryAffectsXAndYIndependently = 0x80000,
        Unknown_0x100000 = 0x100000,
        RandomFlipBookStart = 0x200000,
        IgnoreDistance = 0x400000,
        GravityValuesAreCompressedVectors = 0x800000,
        BoneGeneratorIsBoneAndNotJoint = 0x1000000,
        Unknown_0x2000000 = 0x2000000,
        DoNotThrottleEmissionRateBasedOnDistance = 0x4000000,
        Unknown_0x8000000 = 0x8000000,
        UsesMultiTexturing = 0x10000000,
    }

    public class M2ParticleMultiTextureParameter
    {
        public FixedPoint16 X { get; set; }
        public FixedPoint16 Y { get; set; }
    }

    public class M2ParticleEmitter
    {
        public int ParticleId { get; set; }
        public M2ParticleFlags Flags { get; set; }
        public C3Vector Position { get; set; }
        public short Bone { get; set; }

        // Technically 3 5 bit numbers packed in here but let's ignore that for now
        public short Texture { get; set; }
        public string GeometryModelFilename { get; set; } = string.Empty;
        public string RecursionModelFilename { get; set; } = string.Empty;
        public byte BlendingType { get; set; }
        public byte EmitterType { get; set; }
        public ushort ParticleColorIndex { get; set; } 
        public (FixedPoint8, FixedPoint8) MultiTextureParamX { get; set; }
        public ushort TextureTileRotation { get; set; }
        public ushort TextureDimensionsRows { get; set; }
        public ushort TextureDimensionsColumns { get; set; }
        public M2Track<float> EmissionSpeed { get; set; } = new();
        public M2Track<float> SpeedVariation { get; set; } = new();
        public M2Track<float> VerticalRange { get; set; } = new();
        public M2Track<float> HorizontalRange { get; set; } = new();
        public M2Track<C3Vector> Gravity { get; set; } = new();
        public M2Track<float> Lifespan { get; set; } = new();
        public float LifespanVary { get; set; }
        public M2Track<float> EmissionRate { get; set; } = new();
        public float EmissionRateVary { get; set; }
        public M2Track<float> EmissionAreaLength { get; set; } = new();
        public M2Track<float> EmissionAreaWidth { get; set; } = new();
        public M2Track<float> ZSource { get; set; } = new();
        public M2LocalTrack<C3Vector> ColorTrack { get; set; } = new();
        public M2LocalTrack<Fixed16> AlphaTrack { get; set; } = new();
        public M2LocalTrack<C2Vector> ScaleTrack { get; set; } = new();
        public C2Vector ScaleVary { get; set; }
        public M2LocalTrack<ushort> HeadCellTrack { get; set; } = new();
        public M2LocalTrack<ushort> TailCellTrack { get; set; } = new();
        public float TailLength { get; set; }
        public float TwinkleSpeed { get; set; }
        public float TwinklePercent { get; set; }
        public CRange TwinkleScale { get; set; }
        public float BurstMultiplier { get; set; }
        public float Drag { get; set; }
        public float BaseSpin { get; set; }
        public float BaseSpinVary { get; set; }
        public float Spin { get; set; }
        public float SpinVary { get; set; }
        public M2Box Tumble { get; set; } = new();
        public C3Vector WindVector { get; set; }
        public float WindTime { get; set; }
        public float FollowSpeed1 { get; set; }
        public float FollowScale1 { get; set; }
        public float FollowSpeed2 { get; set; }
        public float FollowScale2 { get; set; }
        public C3Vector[] SplinePoints { get; set; } = [];
        public M2Track<byte> EnabledIn { get; set; } = new();
        public (M2ParticleMultiTextureParameter, M2ParticleMultiTextureParameter) MultiTextureParam0 { get; set; }
        public (M2ParticleMultiTextureParameter, M2ParticleMultiTextureParameter) MultiTextureParam1 { get; set; }
    }

    public class M2ExtendedParticle
    {
        public float ZSource { get; set; }
        public float ColorMult { get; set; }
        public float AlphaMult { get; set; }
        public M2LocalTrack<Fixed16> AlphaCutoff { get; set; } = new();
    }

    public class M2CompBone
    {
        public int KeyBoneId { get; set; }
        public M2CompBoneFlags Flags { get; set; }
        public short ParentBone { get; set; }
        public short SubmeshId { get; set; }
        public uint BoneNameCRC { get; set; }
        public M2Track<C3Vector> Translation { get; set; } = new();
        public M2Track<Quat16> Rotation { get; set; } = new();
        public M2Track<C3Vector> Scale { get; set; } = new();
        public C3Vector Pivot { get; set; }
    }

    public class M2Color
    {
        public M2Track<C3Vector> Color { get; set; } = new();
        public M2Track<Fixed16> Alpha { get; set; } = new();
    }


    public class M2TextureWeight
    {
        public M2Track<Fixed16> Weight { get; set; } = new();
    }

    public class M2TextureTransform
    {
        public M2Track<C3Vector> Translation { get; set; } = new();
        public M2Track<Quat32> Rotation { get; set; } = new();
        public M2Track<C3Vector> Scale { get; set; } = new();
    }

    public class M2Attachment
    {
        public int Id { get; set; }
        public ushort Bone { get; set; }
        public ushort Unknown { get; set; }
        public C3Vector Position { get; set; }
        public M2Track<byte> AnimateAttached { get; set; } = new();
    }

    public class M2Light
    {
        public ushort Type { get; set; }
        public short Bone { get; set; }
        public C3Vector Position { get; set; }

        public M2Track<C3Vector> AmbientColor { get; set; } = new();
        public M2Track<float> AmbientIntensity { get; set; } = new();
        public M2Track<C3Vector> DiffuseColor { get; set; } = new();
        public M2Track<float> DiffuseIntensity { get; set; } = new();
        public M2Track<float> AttenuationStart { get; set; } = new();
        public M2Track<float> AttenuationEnd { get; set; } = new();
        public M2Track<byte> Visibility { get; set; } = new();
    }

    public class M2SubMesh
    {
        public ushort SubmeshId { get; set; }
        public ushort Level { get; set; }
        public ushort VertexStart { get; set; }
        public ushort VertexCount { get; set; }
        public ushort TriangleStart { get; set; }
        public ushort TriangleCount { get; set; }
        public ushort BoneCount { get; set; }
        public ushort BoneStart { get; set; }
        public ushort BoneInfluences { get; set; }
        public ushort CenterBoneIndex { get; set; }
        public C3Vector CenterPosition { get; set; }
        public C3Vector SortCenterPosition { get; set; }
        public float SortRadius { get; set; }
    }

    public class M2TextureUnit
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
}


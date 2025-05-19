using WoWFileFormats.Common;

namespace WoWFileFormats.WMO
{
    [Flags]
    public enum WMORootFlags : short
    {
        DoNoAttenuateVerticesBasedOnDistanceToPortal = 0x1,
        UseUnifiedRenderPath = 0x2,
        UseLiquidTypeDbcId = 0x4,
        DoNotFixVertexColorAlpha = 0x8,
        Lod = 0x10,
        DefaultMaxLod = 0x20,
        Unknown_0x40 = 0x40,
        Unknown_0x80 = 0x80,
        Unknown_0x100 = 0x100,
        Unknown_0x200 = 0x200,
        Unknown_0x400 = 0x400,
        Unknown_0x800 = 0x800
    }

    [Flags]
    public enum WMODoodadFlags : byte
    {
        AcceptProjTexture = 0x1,
        UseInteriorLighting = 0x2,
        Unknown_0x4 = 0x4,
        Unknown_0x8 = 0x8,
        Unknown_0x10 = 0x10,
        Unknown_0x20 = 0x20,
        Unknown_0x40 = 0x40,
        Unknown_0x80 = 0x80
    }

    public class WMODoodadDef
    {
        public uint NameOffset { get; set; }
        public WMODoodadFlags Flags { get; set; }
        public C3Vector Position { get; set; }
        public Quat32 Rotation { get; set; }
        public float Scale { get; set; }
        public CImVector Color { get; set; }
    }

    public class WMODoodadSet
    {
        public string Name { get; set; } = string.Empty;
        public uint StartIndex { get; set; }
        public uint Count { get; set; }
        public uint Unused { get; set; }
    }


    [Flags]
    public enum WMOMaterialFlags : uint
    {
        Unlit = 0x1,
        Unfogged = 0x2,
        Unculled = 0x4,
        Extlight = 0x8,
        Sidn = 0x10,
        Window = 0x20,
        ClampS = 0x40,
        ClampT = 0x80,
        Unknown_0x100 = 0x100,
        Unknown_0x200 = 0x200,
        Unknown_0x400 = 0x400,
        Unknown_0x800 = 0x800,
        Unknown_0x1000 = 0x1000,
        Unknown_0x2000 = 0x2000,
        Unknown_0x4000 = 0x4000,
        Unknown_0x8000 = 0x8000,
        Unknown_0x10000 = 0x10000,
        Unknown_0x20000 = 0x20000,
        Unknown_0x40000 = 0x40000,
        Unknown_0x80000 = 0x80000,
        Unknown_0x100000 = 0x100000,
        Unknown_0x200000 = 0x200000,
        Unknown_0x400000 = 0x400000,
        Unknown_0x800000 = 0x800000
    }

    public enum WMOShader : uint
    {
        Diffuse = 0,
        Specular = 1,
        Metal = 2,
        Env = 3,
        Opaque = 4,
        EnvMetal = 5,
        TwoLayerDiffuse = 6,
        TwoLayerEnvMetal = 7,
        TwoLayerTerrain = 8,
        DiffuseEmissive = 9,
        WaterWindow = 10,
        MaskedEnvMetal = 11,
        EnvMetalEmissive = 12,
        TwoLayerDiffuseOpaque = 13,
        SubmarineWindow = 14,
        TwoLayerDiffuseEmissive = 15,
        DiffuseTerrain = 16,
        AdditiveMaskedEnvMetal = 17,
        TwoLayerDiffuseMod2x = 18,
        TwoLayerDiffuseMod2xNA = 19,
        TwoLayerDiffuseAlpha = 20,
        Lod = 21,
        Parallax = 22,
        Unknown_DF_Shader = 23
    }

    public class WMOMaterial
    {
        public WMOMaterialFlags Flags { get; set; }
        public WMOShader Shader { get; set; }
        public uint BlendMode { get; set; }
        public uint Texture1 { get; set; }
        public CImVector SidnColor { get; set; }
        public CImVector FrameSidnColor { get; set; }
        public uint Texture2 { get; set; }
        public CImVector DiffColor { get; set; }
        public uint GroundTypeId { get; set; }
        public uint Texture3 { get; set; }
        public uint Color2 { get; set; }
        public uint Flags2 { get; set; }

        public uint[] RunTimeData { get; set; } = new uint[4];
    }


    [Flags]
    public enum WMOGroupFileFlags : uint
    {
        BspTree = 0x1,
        LightMap = 0x2,
        VertexColors = 0x4,
        Exterior = 0x8,
        Unknown_0x10 = 0x10,
        Unknown_0x20 = 0x20,
        ExteriorLit = 0x40,
        Unreachable = 0x80,
        ShowExteriorSky = 0x100,
        HasLights = 0x200,
        Lod = 0x400,
        HasDoodads = 0x800,
        HasWater = 0x1000,
        Interior = 0x2000,
        Unknown_0x4000 = 0x4000,
        Unknown_0x8000 = 0x8000,
        AlwaysDraw = 0x10000,
        Unknown_0x20000 = 0x20000, 
        ShowSkybox = 0x40000, 
        IsOceanWater = 0x80000,
        Unknown_0x100000 = 0x100000,
        IsMountAllowed = 0x200000,
        Unknown_0x400000 = 0x400000,
        Unknown_0x800000 = 0x800000,
        Has2VertexColors = 0x1000000,
        Has2UVs = 0x2000000,
        AntiPortal = 0x4000000,
        Unknown_0x8000000 = 0x8000000,
        Unknown_0x10000000 = 0x10000000,
        Unknown_0x20000000 = 0x20000000,
        Has3UVs = 0x40000000,
        Unknown_0x80000000 = 0x80000000
    }


    public enum WmoGroupFileFlags2 : uint
    {
        CanCutTerrain = 0x1,
        Unknown_0x2 = 0x2,
        Unknown_0x4 = 0x4,
        Unknown_0x8 = 0x8,
        Unknown_0x10 = 0x10,
        Unknown_0x20 = 0x20,
        IsSplitGroupParent = 0x40,
        IsSplitGroupChild = 0x80,
        AttachmentMesh = 0x100,
    }

    public class WMOGroupInfo
    {
        public WMOGroupFileFlags Flags { get; set; }
        public CAxisAlignedBox BoundingBox { get; set; }
        public int NameOffset { get; set; }
    }

    public class WMOLodInfo
    {
        public WmoGroupFileFlags2 Flags2 { get; set; }
        public uint LodIndex { get; set; }
    }

    public class WMOFog
    {
        public uint Flags { get; set; }
        public C3Vector Position { get; set; }
        public float SmallerRadius { get; set; }
        public float LargerRadius { get; set; }
        public float FogEnd { get; set; }
        public float FogStartScalar { get; set; }
        public CImVector FogColor { get; set; }
        public float UwFogEnd { get; set; }
        public float UwFogStartScalar { get; set; }
        public CImVector UwFogColor { get; set; }
    }

    public class WMOPortal
    {
        public ushort StartVertex { get; set; }
        public ushort VertexCount { get; set; }
        public C4Plane Plane { get; set; }
    }

    public class WMOPortalRef
    {
        public ushort PortalIndex { get; set; }
        public ushort GroupIndex { get; set; }
        public short Side { get; set; }
        public ushort Filler { get; set; }
    }

    public class WMOAmbientVolume
    {
        public C3Vector Position { get; set; }
        public float Start { get; set; }
        public float End { get; set; }

        public CImVector Color1 { get; set; }
        public CImVector Color2 { get; set; }
        public CImVector Color3 { get; set; }
        public uint Flags { get; set; }
        public ushort DoodadSetId { get; set; }
        public byte[] Unknown { get; set; } = new byte[10];
    }

    public class WMOLiquidVertex
    {
        public uint Data { get; set; }
        public float Height { get; set; }
    }

    public class WMOLiquidTile
    {
        public byte LegacyLiquidType { get; set; }
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Fishable { get; set; }
        public byte Shared { get; set; }
    }

    public class WMOLiquid
    {
        public C2iVector LiquidVertices { get; set; }
        public C2iVector LiquidTiles { get; set; }

        public C3Vector Position { get; set; }

        public ushort MaterialId { get; set; }

        public WMOLiquidVertex[] Vertices { get; set; } = [];
        public WMOLiquidTile[] Tiles { get; set; } = [];
    }

    public enum WMOBspNodeFlags : ushort
    {
        XAxis = 0x0,
        YAxis = 0x1,
        ZAxis = 0x2,
        AxisMask = 0x3,
        Leaf = 0x4,
        NoChild = 0xFFFF
    }

    public class WMOBspNode
    {
        public WMOBspNodeFlags Flags { get; set; }
        public short NegChild { get; set; }
        public short PosChild { get; set; }
        public ushort Faces { get; set; }
        public uint FaceStart { get; set; }
        public float PlaneDistance { get; set; }
    }

    public class WMOBatch
    {
        public byte[] Unknown { get; set; } = new byte[0xA];
        public ushort MaterialIdLarge { get; set; }
        public uint StartIndex { get; set; }
        public ushort IndexCount { get; set; }
        public ushort FirstVertex { get; set; }
        public ushort LastVertex { get; set; }
        public byte UseMaterialIdLarge { get; set; }
        public byte MaterialId { get; set; }
    }

    public class WMOString
    {
        public string Value { get; set; } = string.Empty;
        public uint Offset { get; set; }
    }
}

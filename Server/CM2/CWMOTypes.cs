namespace Server.CM2
{
    using Float3 = (float X, float Y, float Z);
    using Float4 = (float X, float Y, float Z, float W);
    using Color = (byte R, byte G, byte B, byte A);
    using Int2 = (int X, int Y);

    public class CWMODoodadDef
    {
        public uint Flags { get; set; }
        public Float3 Position { get; set; }
        public Float4 Rotation { get; set; }
        public float Scale { get; set; }
        public Color Color { get; set; }
    }

    public class CWMODoodadSet
    {
        public uint StartIndex { get; set; }
        public uint Count { get; set; }
    }

    public class CWMOMaterial
    {
        public uint Flags { get; set; }
        public uint Shader { get; set; }
        public uint BlendMode { get; set; }
        public uint Texture1 { get; set; }
        public Color SidnColor { get; set; }
        public Color FrameSidnColor { get; set; }
        public uint Texture2 { get; set; }
        public Color DiffColor { get; set; }
        public uint GroundTypeId { get; set; }
        public uint Texture3 { get; set; }
        public uint Color2 { get; set; }
        public uint Flags2 { get; set; }
        public uint[] RunTimeData { get; set; } = new uint[4];
    }

    public class CWMOGroupInfo
    {
        public uint Flags { get; set; }
        public Float3 MinBoundingBox { get; set; }
        public Float3 MaxBoundingBox { get; set; }
    }

    public class CWMOLodInfo
    {
        public uint Flags2 { get; set; }
        public uint LodIndex { get; set; }
    }

    public class CWMOFog
    {
        public uint Flags { get; set; }
        public Float3 Position { get; set; }
        public float SmallerRadius { get; set; }
        public float LargerRadius { get; set; }
        public float FogEnd { get; set; }
        public float FogStartScalar { get; set; }
        public Color FogColor { get; set; }
        public float UwFogEnd { get; set; }
        public float UwFogStartScalar { get; set; }
        public Color UwFogColor { get; set; }
    }

    public class CWMOPortal
    {
        public ushort StartVertex { get; set; }
        public ushort VertexCount { get; set; }
        public Float3 PlaneNormal { get; set; }
        public float PlaneDistance { get; set; }
    }

    public class CWMOPortalRef
    {
        public ushort PortalIndex { get; set; }
        public ushort GroupIndex { get; set; }
        public short Side { get; set; }
    }

    public class CWMOAmbientVolume
    {
        public Float3 Position { get; set; }
        public float Start { get; set; }
        public float End { get; set; }

        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public Color Color3 { get; set; }
        public uint Flags { get; set; }
        public ushort DoodadSetId { get; set; }
    }

    public class CWMOLiquidVertex
    {
        public uint Data { get; set; }
        public float Height { get; set; }
    }

    public class CWMOLiquidTile
    {
        public byte LegacyLiquidType { get; set; }
        public byte Fishable { get; set; }
        public byte Shared { get; set; }
    }

    public class CWMOLiquid
    {
        public Int2 LiquidVertices { get; set; }
        public Int2 LiquidTiles { get; set; }
        public Float3 Position { get; set; }
        public ushort MaterialId { get; set; }
        public CWMOLiquidVertex[] Vertices { get; set; } = [];
        public CWMOLiquidTile[] Tiles { get; set; } = [];
    }

    public class CWMOBspNode
    {
        public ushort Flags { get; set; }
        public short NegChild { get; set; }
        public short PosChild { get; set; }
        public ushort Faces { get; set; }
        public uint FaceStart { get; set; }
        public float PlaneDistance { get; set; }
    }

    public class CWMOBatch
    {
        public ushort MaterialId { get; set; }
        public uint StartIndex { get; set; }
        public ushort IndexCount { get; set; }
        public ushort FirstVertex { get; set; }
        public ushort LastVertex { get; set; }
    }
}

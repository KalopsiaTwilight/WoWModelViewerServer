using WoWFileFormats.Common;
using WoWFileFormats.WMO;

namespace Server.CM2
{
    using Float2 = (float X, float Y);
    using Float3 = (float X, float Y, float Z);
    using Color = (byte R, byte G, byte B, byte A);

    public class CWMOFile
    {
        public uint Version { get; set; } = 1000;
        public uint FileDataID { get; set; }
        public short Flags { get; set; }
        public uint WMOId { get; set; }
        public uint SkyboxFileId { get; set; }
        public Color AmbientColor { get; set; }
        public Float3 MinBoundingBox { get; set; }
        public Float3 MaxBoundingBox { get; set; }
        public CWMOMaterial[] Materials { get; set; } = [];
        public CWMOGroupInfo[] GroupInfo { get; set; } = [];
        public CWMODoodadDef[] DoodadDefs { get; set; } = [];
        public uint[] DoodadIds { get; set; } = [];
        public CWMOFog[] Fogs{ get; set; } = [];
        public CWMODoodadSet[] DoodadSets { get; set; } = [];
        public CWMOPortalRef[] PortalRefs { get; set; } = [];
        public CWMOPortal[] Portals{ get; set; } = [];
        public CWMOAmbientVolume[] GlobalAmbientVolumes { get; set; } = [];
        public CWMOAmbientVolume[] AmbientVolumes { get; set; } = [];
        public Float3[] PortalVertices { get; set; } = [];
        public CWMOGroup[] Groups { get; set; } = [];
    }

    public class CWMOGroup
    {
        public uint FileDataID { get; set; }
        public uint Flags { get; set; }
        public Float3 BoundingBoxMin { get; set; }
        public Float3 BoundingBoxMax { get; set; }
        public ushort PortalsOffset { get; set; }
        public ushort PortalCount { get; set; }
        public ushort TransBatchCount { get; set; }
        public ushort IntBatchCount { get; set; }
        public ushort ExtBatchCount { get; set; }
        public ushort UnknownBatchCount { get; set; }
        public byte[] FogIndices { get; set; } = new byte[4];
        public uint GroupLiquid { get; set; }
        public uint GroupId { get; set; }
        public uint Flags2 { get; set; }
        public short SplitGroupindex { get; set; }
        public short NextSplitChildIndex { get; set; }
        public ushort[] Indices { get; set; } = [];
        public Color HeaderReplacementColor { get; set; }
        public CWMOLiquid[] LiquidData { get; set; } = [];
        public ushort[] BspIndices { get; set; } = [];
        public CWMOBspNode[] BspNodes { get; set; } = [];
        public Float3[] Vertices { get; set; } = [];
        public Float3[] Normals { get; set; } = [];
        public Float2[] UVList { get; set; } = [];
        public Color[] VertexColors { get; set; } = [];
        public CWMOBatch[] Batches { get; set; } = [];
        public ushort[] DoodadReferences { get; set; } = [];
    }
}

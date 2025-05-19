using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWFileFormats.Common;

namespace WoWFileFormats.WMO
{
    // CHUNKS LOADED BY NOCLIP:
    // X MOVI, X MDAL, X MLIQ, X MORB, X MOBN, X MOVT, X MONR, X MOTV, X MOCV, X MOBA, X MODR, X MOGP (parsed as header)

    internal class WMOGroupChunks
    {
    }

    public class MOGPChunk: IWowDataChunk
    {

        public const uint ID = 0x0;
        public uint ChunkId => ID;
        public uint GroupNameOffset { get; set; }
        public uint DescriptiveNameOffset { get; set; }
        public WMOGroupFileFlags Flags { get; set; }
        public CAxisAlignedBox BoundingBox { get; set; }
        public ushort PortalsOffset { get; set; }
        public ushort PortalCount { get; set; }
        public ushort TransBatchCount { get; set; }
        public ushort IntBatchCount { get; set; }
        public ushort ExtBatchCount { get; set; }
        public ushort UnknownBatchCount { get; set; }
        public byte[] FogIndices { get; set; } = new byte[4];
        public uint GroupLiquid { get; set; }
        public uint GroupId { get; set; }
        public WmoGroupFileFlags2 Flags2 { get; set; }
        public short SplitGroupindex { get; set; }
        public short NextSplitChildIndex { get; set; }
    }

    public class MOVIChunk : IWowDataChunk
    {
        public const uint ID = 0x49564F4D;
        public uint ChunkId => ID;

        public ushort[] Indices { get; set; } = [];
    }

    public class MDALChunk : IWowDataChunk
    {
        public const uint ID = 0x4C41444D;
        public uint ChunkId => ID;
        public CArgb HeaderColor { get; set; }  
    }

    public class MLIQChunk: IWowDataChunk
    {
        public const uint ID = 0x51494C4D;
        public uint ChunkId => ID;
        public WMOLiquid LiquidData { get; set; } = new();
    }

    public class MOBRChunk: IWowDataChunk
    {
        public const uint ID = 0x52424F4D;
        public uint ChunkId => ID;

        public ushort[] Entries { get; set; } = [];
    }

    public class MOBNChunk: IWowDataChunk
    {
        public const uint ID = 0x4E424F4D;
        public uint ChunkId => ID;

        public WMOBspNode[] Nodes { get; set; } = [];
    }

    public class MOVTChunk : IWowDataChunk
    {
        public const uint ID = 0x54564F4D;
        public uint ChunkId => ID;

        public C3Vector[] Vertices { get; set; } = [];
    }

    public class MONRChunk : IWowDataChunk
    {
        public const uint ID = 0x524E4F4D;
        public uint ChunkId => ID;
        public C3Vector[] Normals { get; set; } = [];
    }

    public class MOTVChunk : IWowDataChunk
    {
        public const uint ID = 0x56544F4D;
        public uint ChunkId => ID;
        public C2Vector[] UvList { get;set; } = [];
    }

    public class MOCVChunk : IWowDataChunk
    {
        public const uint ID = 0x56434F4D;
        public uint ChunkId => ID;

        public CImVector[] ColorVertexList { get; set; } = [];
    }
    public class MOBAChunk : IWowDataChunk
    {
        public const uint ID = 0x41424F4D;
        public uint ChunkId => ID;
        public WMOBatch[] Batches { get; set; } = [];
    }
    public class MODRChunk : IWowDataChunk
    {
        public const uint ID = 0x52444F4D;
        public uint ChunkId => ID;
        public ushort[] DoodadReferences { get; set; } = [];
    }
}

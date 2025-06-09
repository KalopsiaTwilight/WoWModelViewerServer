using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWFileFormats.Common;

namespace WoWFileFormats.WMO
{
    public class WMOGroupFile
    {
        public uint FileDataID { get; set; }

        public int Lod { get; set; }
        // MOGP
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
        // MOVI
        public List<ushort> Indices { get; set; } = [];
        // MDAL
        public CArgb HeaderReplacementColor { get; set; }
        // MILQ
        public List<WMOLiquid> LiquidData { get; set; } = [];
        // MOBR
        public List<ushort> BspIndices { get; set; } = [];
        // MOBN
        public List<WMOBspNode> BspNodes { get; set; } = [];
        // MOVT
        public List<C3Vector> Vertices { get; set; } = [];
        // MONR
        public List<C3Vector> Normals { get; set; } = [];
        // MOTV
        public List<C2Vector> UVList { get; set; } = [];
        // MOCV
        public List<CImVector> VertexColors { get; set; } = [];
        // MOBA
        public List<WMOBatch> Batches { get; set; } = [];
        // MODR
        public List<ushort> DoodadReferences { get; set; } = [];
    }
}

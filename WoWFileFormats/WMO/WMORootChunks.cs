using WoWFileFormats.Common;

namespace WoWFileFormats.WMO
{
    // CHUNKS LOADED BY NOCLIP:
    // X MOHD, X MOMT, X MOGI, X MODD, X MFOG, X MOPV, X MOGN, X MOPT, X MOPR, X GFID, X MAVD, X MAVG, X MOSI, X MOSB, X MODS, X MVER

    public class MOHDChunk : IWowDataChunk
    {
        public const uint ID = 0x4D4F4844;
        public uint ChunkId => ID;

        public uint TexturesCount { get; set; }
        public uint GroupsCount { get; set; }
        public uint PortalsCount { get; set; }
        public uint LightsCount { get; set; }
        public uint DoodadNamesCount { get; set; }
        public uint DoodadDefsCount { get; set; }
        public uint DoodadSetsCount { get; set; }
        public CArgb AmbientColor { get; set; }
        public uint WMOId { get; set; }
        public CAxisAlignedBox BoundingBox { get; set; }
        public WMORootFlags Flags { get; set; }
        public ushort LodCount { get; set; }
    }

    public class MODIChunk: IWowDataChunk
    {
        public const uint ID = 0x4D4F4449;

        public uint ChunkId => ID;
        public uint[] DoodIdList { get; set; } = [];
    }

    public class MODNChunk : IWowDataChunk
    {
        public const uint ID = 0x4D4F444E;

        public uint ChunkId => ID;

        public WMOString[] DoodadNamesList { get; set; } = [];
    }

    public class MODDChunk : IWowDataChunk
    {
        public const uint ID = 0x4D4F4444;

        public uint ChunkId => ID;
        public WMODoodadDef[] DoodadDefList { get; set; } = [];
    }

    public class MODSChunk : IWowDataChunk
    {
        public const uint ID = 0x4D4F4453;
        public uint ChunkId => ID;

        public WMODoodadSet[] DoodadSetList { get; set; } = [];
    }

    public class MOTXChunk : IWowDataChunk
    {
        public const uint ID = 0x4D4F5458;
        public uint ChunkId => ID;
        public WMOString[] TextureFileNames { get; set; } = [];
    }

    public class MOMTChunk : IWowDataChunk
    {
        public const uint ID = 0x4D4F4D54;
        public uint ChunkId => ID;
        public WMOMaterial[] MaterialList { get; set; } = [];
    }

    public class MOGNChunk : IWowDataChunk
    {
        public const uint ID = 0x4D4F474E;
        public uint ChunkId => ID;
        public WMOString[] GroupNameList { get; set; } = [];
    }

    public class MOGIChunk : IWowDataChunk
    {
        public const uint ID = 0x4D4F4749;
        public uint ChunkId => ID;
        public WMOGroupInfo[] GroupInfoList { get; set; } = [];
    }

    public class MGI2Chunk : IWowDataChunk
    {
        public const uint ID = 0x4D474932;
        public uint ChunkId => ID;
        public WMOLodInfo[] MapObjectGroupInfoV2 { get; set; } = [];
    }

    public class MFOGChunk: IWowDataChunk
    {
        public const uint ID = 0x4D464F47;
        public uint ChunkId => ID;

        public WMOFog[] FogList { get; set; } = [];
    }

    public class MOPVChunk: IWowDataChunk
    {
        public const uint ID = 0x4D4F5056;
        public uint ChunkId => ID;

        public C3Vector[] PortalVertexList { get; set; } = [];
    }

    public class MOPTChunk: IWowDataChunk
    {
        public const uint ID = 0x4D4F5054;
        public uint ChunkId => ID;

        public WMOPortal[] PortalList { get; set; } = [];
    }

    public class MOPRChunk: IWowDataChunk
    {
        public const uint ID = 0x4D4F5052;
        public uint ChunkId => ID;
        public WMOPortalRef[] PortalRefList { get; set; } = [];
    }

    public class GFIDChunk : IWowDataChunk
    {
        public const uint ID = 0x47464944;
        public uint ChunkId => ID;

        public uint[] FileIds { get; set; } = [];
    }

    public class MAVDChunk: IWowDataChunk
    {
        public const uint ID = 0x4D415644;
        public uint ChunkId => ID;
        public WMOAmbientVolume[] AmbientVolumes { get; set; } = [];
    }

    public class MAVGChunk : IWowDataChunk
    {
        public const uint ID = 0x4D415647;
        public uint ChunkId => ID;
        public WMOAmbientVolume[] GlobalAmbientVolumes { get; set; } = [];
    }

    public class MOSIChunk: IWowDataChunk
    {
        public const uint ID = 0x4D4F5349;
        public uint ChunkId => ID;
        public uint SkyboxFileId { get; set; }
    }

    public class MOSBChunk: IWowDataChunk
    {
        public const uint ID = 0x4D4F5342;
        public uint ChunkId => ID;

        public string SkyboxName { get; set; } = string.Empty;
    }
    public class MVERChunk: IWowDataChunk
    {
        public const uint ID = 0x4D564552;
        public uint ChunkId => ID;

        public uint Version { get; set; }
    }
}

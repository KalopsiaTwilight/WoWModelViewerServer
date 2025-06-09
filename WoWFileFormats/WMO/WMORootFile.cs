using WoWFileFormats.Common;
using WoWFileFormats.Interfaces;

namespace WoWFileFormats.WMO
{
    public class WMORootFile
    {
        public uint FileDataID { get; set; }
        // MVER
        public uint Version { get; set; }
        // MOHD
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

        // MODN
        public List<WMOString> DoodadNamesList { get; set; } = [];
        // MOMT
        public List<WMOMaterial> MaterialList { get; set; } = [];
        // MOGI
        public List<WMOGroupInfo> GroupInfoList { get; set; } = [];
        // MODD
        public List<WMODoodadDef> DoodadDefList { get; set; } = [];
        // MODI
        public List<uint> DoodadIdList { get; set; } = [];
        // MFOG
        public List<WMOFog> FogList { get; set; } = [];
        // GFID
        public List<uint> FileIds { get; set; } = [];
        // MOSI
        public uint SkyboxFileId { get; set; }
        // MOSB
        public string SkyboxName { get; set; } = string.Empty;
        // MODS
        public List<WMODoodadSet> DoodadSetList { get; set; } = [];
        // MOPR
        public List<WMOPortalRef> PortalRefList { get; set; } = [];
        // MOPT
        public List<WMOPortal> PortalList { get; set; } = [];
        // MOGN
        public List<WMOString> GroupNameList { get; set; } = [];
        // MAVG
        public List<WMOAmbientVolume> GlobalAmbientVolumes { get; set; } = [];
        // MAVD
        public List<WMOAmbientVolume> AmbientVolumes { get; set; } = [];
        // MOTX
        public List<WMOString> TextureFileNames { get; set; } = [];
        // MGI2
        public List<WMOLodInfo> MapObjectGroupInfoV2 { get; set; } = [];
        // MOPV
        public List<C3Vector> PortalVertexList { get; set; } = [];
        // Referenced files
        public List<WMOGroupFile> GroupFiles { get; set; } = [];

        public void LoadGroupFiles(IFileDataProvider dataProvider)
        {
            if (LodCount > 0)
            {
                for (var i = 0; i < LodCount; i++)
                {
                    for (var j = 0; j < GroupsCount; j++)
                    {
                        var fileId = FileIds[(int)(i * GroupsCount + j)];
                        var group = LoadGroupFile(dataProvider, fileId);
                        if (group == null)
                        {
                            continue;
                        }
                        group.Lod = i;
                        GroupFiles.Add(group);
                    }
                }
            } 
            else
            {
                foreach(var fileId in FileIds)
                {
                    var group = LoadGroupFile(dataProvider, fileId); 
                    if (group == null)
                    {
                        continue;
                    }
                    group.Lod = 0;
                    GroupFiles.Add(group);
                }
            }
        }

        private WMOGroupFile? LoadGroupFile(IFileDataProvider dataProvider, uint fileId)
        {
            ;
            if (!dataProvider.FileIdExists(fileId))
            {
                return null;
            }
            using var dataStream = dataProvider.GetFileById(fileId);
            using var reader = new WMOFileReader(fileId, dataStream);
            return reader.ReadWMOGroupFile();
        }
    }
}

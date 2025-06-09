
using System.Drawing;
using System.Text;
using WoWFileFormats.Common;

namespace WoWFileFormats.WMO
{
    public class WMOChunksReader : WMOTypesReader
    {
        internal long _currentChunkSize;

        // GROUP: MOGP (parsed as header)

        public WMOChunksReader(Stream stream) : base(stream)
        {
            _currentChunkSize = 0;
        }

        public MOGPChunk ReadMOGPChunk()
        {
            return new MOGPChunk()
            {
                GroupNameOffset = _reader.ReadUInt32(),
                DescriptiveNameOffset = _reader.ReadUInt32(),
                Flags = (WMOGroupFileFlags)_reader.ReadUInt32(),
                BoundingBox = ReadCAxisAlignedBox(),
                PortalsOffset = _reader.ReadUInt16(),
                PortalCount = _reader.ReadUInt16(),
                TransBatchCount = _reader.ReadUInt16(),
                IntBatchCount = _reader.ReadUInt16(),
                ExtBatchCount = _reader.ReadUInt16(),
                UnknownBatchCount = _reader.ReadUInt16(),
                FogIndices = _reader.ReadBytes(4),
                GroupLiquid = _reader.ReadUInt32(),
                GroupId = _reader.ReadUInt32(),
                Flags2 = (WmoGroupFileFlags2)_reader.ReadUInt32(),
                SplitGroupindex = _reader.ReadInt16(),
                NextSplitChildIndex = _reader.ReadInt16()
            };
        }

        public MODIChunk ReadMODIChunk()
        {
            var elements = _currentChunkSize / 4;
            var chunk = new MODIChunk()
            {
                DoodIdList = new uint[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.DoodIdList[i] = _reader.ReadUInt32();
            }
            ;
            return chunk;
        }

        public MODNChunk ReadMODNChunk()
        {
            var doodadNames = ReadWMOStrings();
            return new MODNChunk()
            {
                DoodadNamesList = [.. doodadNames.Select(x => {
                    x.Value = x.Value.Replace("..", ".").Replace(".mdx", ".m2");
                    return x;
                })]
            };
        }

        public MOTXChunk ReadMOTXChunk()
        {
            return new MOTXChunk()
            {
                TextureFileNames = ReadWMOStrings().ToArray(),
            };
        }

        public MODRChunk ReadMODRChunk()
        {
            var elements = _currentChunkSize / 2;
            var chunk = new MODRChunk()
            {
                DoodadReferences = new ushort[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.DoodadReferences[i] = _reader.ReadUInt16();
            };
            return chunk;
        }

        public MOBAChunk ReadMOBAChunk()
        {
            var elements = _currentChunkSize / 24;
            var chunk = new MOBAChunk()
            {
                Batches = new WMOBatch[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.Batches[i] = ReadWMOBatch();
            };
            return chunk;
        }

        public MOCVChunk ReadMOCVChunk()
        {
            var elements = _currentChunkSize / 4;
            var chunk = new MOCVChunk()
            {
                ColorVertexList = new CImVector[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.ColorVertexList[i] = ReadCImVector();
            };
            return chunk;
        }

        public MOTVChunk ReadMOTVChunk()
        {
            var elements = _currentChunkSize / 8;
            var chunk = new MOTVChunk()
            {
                UvList = new C2Vector[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.UvList[i] = ReadC2Vector();
            };
            return chunk;
        }

        public MONRChunk ReadMONRChunk()
        {
            var elements = _currentChunkSize / 12;
            var chunk = new MONRChunk()
            {
                Normals = new C3Vector[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.Normals[i] = ReadC3Vector();
            };
            return chunk;
        }

        public MOVTChunk ReadMOVTChunk()
        {
            var elements = _currentChunkSize / 12;
            var chunk = new MOVTChunk()
            {
                Vertices = new C3Vector[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.Vertices[i] = ReadC3Vector();
            };
            return chunk;
        }

        public MOBNChunk ReadMOBNChunk()
        {
            var elements = _currentChunkSize / 16;
            var chunk = new MOBNChunk()
            {
                Nodes = new WMOBspNode[elements],
            };

            for (var i = 0; i < elements; i++)
            {
                chunk.Nodes[i] = new WMOBspNode()
                {
                    Flags = (WMOBspNodeFlags)_reader.ReadUInt16(),
                    NegChild = _reader.ReadInt16(),
                    PosChild = _reader.ReadInt16(),
                    Faces = _reader.ReadUInt16(),
                    FaceStart = _reader.ReadUInt32(),
                    PlaneDistance = _reader.ReadSingle()
                };
            }

            return chunk;
        }

        public MOBRChunk ReadMORBChunk()
        {
            var elements = _currentChunkSize / 2;
            var chunk = new MOBRChunk()
            {
                Entries = new ushort[elements],
            };

            for (var i = 0; i < elements; i++)
            {
                chunk.Entries[i] = _reader.ReadUInt16();
            }
            return chunk;
        }

        public MDALChunk ReadMDALChunk()
        {
            return new MDALChunk()
            {
                HeaderColor = ReadCArgb()
            };
        }

        public MOVIChunk ReadMOVIChunk()
        {
            var elements = _currentChunkSize / 2;
            var chunk = new MOVIChunk()
            {
                Indices = new ushort[elements]
            };

            for (var i = 0; i < elements; i++)
            {
                chunk.Indices[i] = _reader.ReadUInt16();
            }

            return chunk;
        }

        public MVERChunk ReadMVERChunk()
        {
            return new MVERChunk()
            {
                Version = _reader.ReadUInt32(),
            };
        }

        public MODSChunk ReadMODSChunk()
        {
            var elements = _currentChunkSize / 32;
            var chunk = new MODSChunk()
            {
                DoodadSetList = new WMODoodadSet[elements]
            };

            for (var i = 0; i < elements; i++)
            {
                chunk.DoodadSetList[i] = ReadWMODoodadSet();
            };

            return chunk;
        }

        public MOSBChunk ReadMOSBChunk()
        {
            var chunkBytes = _reader.ReadBytes((int)_currentChunkSize);
            return new MOSBChunk()
            {
                SkyboxName = Encoding.UTF8.GetString(chunkBytes)
            };
        }

        public MOSIChunk ReadMOSIChunk()
        {
            return new MOSIChunk()
            {
                SkyboxFileId = _reader.ReadUInt32(),
            };
        }

        public MAVDChunk ReadMAVDChunk()
        {
            var elements = _currentChunkSize / 48;
            var chunk = new MAVDChunk()
            {
                AmbientVolumes = new WMOAmbientVolume[elements],
            };

            for (var i = 0; i < elements; i++)
            {
                chunk.AmbientVolumes[i] = ReadWMOAmbientVolume();
            }

            return chunk;
        }

        public GFIDChunk ReadGFIDChunk()
        {
            var elements = _currentChunkSize / 4;

            var chunk = new GFIDChunk()
            {
                FileIds = new uint[elements],
            };

            for (var i = 0; i < elements; i++)
            {
                chunk.FileIds[i] = _reader.ReadUInt32();
            }

            return chunk;
        }

        public MOHDChunk ReadMOHDChunk()
        {
            return new MOHDChunk()
            {
                TexturesCount = _reader.ReadUInt32(),
                GroupsCount = _reader.ReadUInt32(),
                PortalsCount = _reader.ReadUInt32(),
                LightsCount = _reader.ReadUInt32(),
                DoodadNamesCount = _reader.ReadUInt32(),
                DoodadDefsCount = _reader.ReadUInt32(),
                DoodadSetsCount = _reader.ReadUInt32(),
                AmbientColor = ReadCArgb(),
                WMOId = _reader.ReadUInt32(),
                BoundingBox = ReadCAxisAlignedBox(),
                Flags = (WMORootFlags)_reader.ReadUInt16(),
                LodCount = _reader.ReadUInt16(),
            };
        }

        public MOPVChunk ReadMOPVChunk()
        {
            var elements = _currentChunkSize / 12;
            var chunk = new MOPVChunk()
            {
                PortalVertexList = new C3Vector[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.PortalVertexList[i] = ReadC3Vector();
            };
            return chunk;
        }

        public MOGNChunk ReadMOGNChunk()
        {
            return new MOGNChunk
            {
                GroupNameList = ReadWMOStrings().ToArray()
            };
        }

        public MODDChunk ReadMODDChunk()
        {
            var elements = _currentChunkSize / 40;
            var chunk = new MODDChunk()
            {
                DoodadDefList = new WMODoodadDef[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.DoodadDefList[i] = ReadWMODoodadDef();
            }
            return chunk;
        }

        public MOMTChunk ReadMOMTChunk()
        {
            var elements = _currentChunkSize / 64;
            var chunk = new MOMTChunk()
            {
                MaterialList = new WMOMaterial[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.MaterialList[i] = ReadWMOMaterial();
            }
            return chunk;
        }

        public MOGIChunk ReadMOGIChunk()
        {
            var elements = _currentChunkSize / 32;
            var chunk = new MOGIChunk()
            {
                GroupInfoList = new WMOGroupInfo[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.GroupInfoList[i] = ReadWMOGroupInfo();
            }
            return chunk;
        }

        public MGI2Chunk ReadMGI2Chunk()
        {
            var elements = _currentChunkSize / 8;
            var chunk = new MGI2Chunk()
            {
                MapObjectGroupInfoV2 = new WMOLodInfo[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.MapObjectGroupInfoV2[i] = ReadWMOLodInfo();
            }
            return chunk;
        }

        public MFOGChunk ReadMFOGChunk()
        {
            var elements = _currentChunkSize / 48;
            var chunk = new MFOGChunk()
            {
                FogList = new WMOFog[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.FogList[i] = ReadWMOFog();
            }
            return chunk;
        }

        public MOPTChunk ReadMOPTChunk()
        {
            var elements = _currentChunkSize / 20;
            var chunk = new MOPTChunk()
            {
                PortalList = new WMOPortal[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.PortalList[i] = ReadWMOPortal();
            }
            return chunk;
        }

        public MOPRChunk ReadMOPRChunk()
        {
            var elements = _currentChunkSize / 8;
            var chunk = new MOPRChunk()
            {
                PortalRefList = new WMOPortalRef[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.PortalRefList[i] = ReadWMOPortalRef();
            }
            return chunk;
        }

        public MAVGChunk ReadMAVGChunk()
        {
            var elements = _currentChunkSize / 48;
            var chunk = new MAVGChunk()
            {
                GlobalAmbientVolumes = new WMOAmbientVolume[elements]
            };
            for (var i = 0; i < elements; i++)
            {
                chunk.GlobalAmbientVolumes[i] = ReadWMOAmbientVolume();
            }
            return chunk;
        }

        public MLIQChunk ReadMLIQChunk()
        {
            return new MLIQChunk()
            {
                LiquidData = ReadWMOLiquid()
            };
        }

        private List<WMOString> ReadWMOStrings()
        {
            var stringBuilder = new StringBuilder();
            var groupNames = new List<WMOString>();

            var chunkBytes = _reader.ReadBytes((int)_currentChunkSize);
            for (uint i = 0; i < _currentChunkSize; i++)
            {
                var nextByte = chunkBytes[i];
                if (nextByte == '\0')
                {
                    if (stringBuilder.Length > 1)
                    {
                        groupNames.Add(new WMOString()
                        {
                            Value = stringBuilder.ToString(),
                            Offset = (uint)(i - stringBuilder.Length)
                        });
                    }
                    stringBuilder = stringBuilder.Clear();
                }
                else
                {
                    stringBuilder.Append((char)nextByte);
                }
            }

            return groupNames;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WoWFileFormats.Common;

namespace WoWFileFormats.WMO
{
    public class WMOFileReader : WMOChunksReader
    {

        private uint _fileDataId;
        public WMOFileReader(uint fileDataId, Stream input) : base(input)
        {
            _fileDataId = fileDataId;
        }

        public WMORootFile? ReadWMORootFile()
        {
            WMORootFile result = new()
            {
                FileDataID = _fileDataId
            };

            var processedChunks = 0;
            while (_stream.Position < _stream.Length)
            {
                var chunkId = _reader.ReadUInt32();
                _currentChunkSize = _reader.ReadUInt32();
                var nextChunkPos = _stream.Position + _currentChunkSize;

                switch (chunkId)
                {
                    case MVERChunk.ID:
                        {
                            var chunk = ReadMVERChunk();
                            result.Version = chunk.Version;
                            if (result.Version != 17)
                            {
                                throw new Exception($"Unexpected WMO version encountered. Received: {chunk.Version}, supported: 17");
                            }
                            break;
                        }
                    case MOHDChunk.ID:
                        {
                            var chunk = ReadMOHDChunk();
                            result.TexturesCount = chunk.TexturesCount;
                            result.GroupsCount = chunk.GroupsCount;
                            result.PortalsCount = chunk.PortalsCount;
                            result.LightsCount = chunk.LightsCount;
                            result.DoodadNamesCount = chunk.DoodadNamesCount;
                            result.DoodadDefsCount = chunk.DoodadDefsCount;
                            result.DoodadSetsCount = chunk.DoodadSetsCount;
                            result.AmbientColor = chunk.AmbientColor;
                            result.WMOId = chunk.WMOId;
                            result.BoundingBox = chunk.BoundingBox;
                            result.Flags = chunk.Flags;
                            result.LodCount = chunk.LodCount;
                            processedChunks++;
                            break;
                        }
                    case MODIChunk.ID:
                        {
                            var chunk = ReadMODIChunk();
                            result.DoodadIdList.AddRange(chunk.DoodIdList);
                            processedChunks++;
                            break;
                        }
                    case MODNChunk.ID:
                        {
                            var chunk = ReadMODNChunk();
                            result.DoodadNamesList.AddRange(chunk.DoodadNamesList);
                            processedChunks++;
                            break;
                        }
                    case MODDChunk.ID:
                        {
                            var chunk = ReadMODDChunk();
                            result.DoodadDefList.AddRange(chunk.DoodadDefList);
                            processedChunks++;
                            break;
                        }
                    case MODSChunk.ID:
                        {
                            var chunk = ReadMODSChunk();
                            result.DoodadSetList.AddRange(chunk.DoodadSetList);
                            processedChunks++;
                            break;
                        }
                    case MOTXChunk.ID:
                        {
                            var chunk = ReadMOTXChunk();
                            result.TextureFileNames.AddRange(chunk.TextureFileNames);
                            processedChunks++;
                            break;
                        }
                    case MOMTChunk.ID:
                        {
                            var chunk = ReadMOMTChunk();
                            result.MaterialList.AddRange(chunk.MaterialList);
                            processedChunks++;
                            break;
                        }
                    case MOGNChunk.ID:
                        {
                            var chunk = ReadMOGNChunk();
                            result.GroupNameList.AddRange(chunk.GroupNameList);
                            processedChunks++;
                            break;
                        }
                    case MOGIChunk.ID:
                        {
                            var chunk = ReadMOGIChunk();
                            result.GroupInfoList.AddRange(chunk.GroupInfoList);
                            processedChunks++;
                            break;
                        }
                    case MGI2Chunk.ID:
                        {
                            var chunk = ReadMGI2Chunk();
                            result.MapObjectGroupInfoV2.AddRange(chunk.MapObjectGroupInfoV2);
                            processedChunks++;
                            break;
                        }
                    case MFOGChunk.ID:
                        {
                            var chunk = ReadMFOGChunk();
                            result.FogList.AddRange(chunk.FogList);
                            processedChunks++;
                            break;
                        }
                    case MOPVChunk.ID:
                        {
                            var chunk = ReadMOPVChunk();
                            result.PortalVertexList.AddRange(chunk.PortalVertexList);
                            processedChunks++;
                            break;
                        }
                    case MOPTChunk.ID:
                        {
                            var chunk = ReadMOPTChunk();
                            result.PortalList.AddRange(chunk.PortalList);
                            processedChunks++;
                            break;
                        }
                    case MOPRChunk.ID:
                        {
                            var chunk = ReadMOPRChunk();
                            result.PortalRefList.AddRange(chunk.PortalRefList);
                            processedChunks++;
                            break;
                        }
                    case GFIDChunk.ID:
                        {
                            var chunk = ReadGFIDChunk();
                            result.FileIds.AddRange(chunk.FileIds);
                            processedChunks++;
                            break;
                        }
                    case MAVDChunk.ID:
                        {
                            var chunk = ReadMAVDChunk();
                            result.AmbientVolumes.AddRange(chunk.AmbientVolumes);
                            processedChunks++;
                            break;
                        }
                    case MAVGChunk.ID:
                        {
                            var chunk = ReadMAVGChunk();
                            result.GlobalAmbientVolumes.AddRange(chunk.GlobalAmbientVolumes);
                            processedChunks++;
                            break;
                        }
                    case MOSIChunk.ID:
                        {
                            var chunk = ReadMOSIChunk();
                            result.SkyboxFileId = chunk.SkyboxFileId;
                            processedChunks++;
                            break;
                        }
                    case MOSBChunk.ID:
                        {
                            var chunk = ReadMOSBChunk();
                            result.SkyboxName = chunk.SkyboxName;
                            processedChunks++;
                            break;
                        }
                    default:
                        {
                            // Just skip unimplemented chunks for now
                            break;
                        }
                }

                _stream.Position = nextChunkPos;
            }

            if (processedChunks <= 1)
            {
                return null;
            }

            return result;
        }


        public WMOGroupFile ReadWMOGroupFile()
        {
            WMOGroupFile result = new()
            {
                FileDataID = _fileDataId
            };

            while (_stream.Position < _stream.Length)
            {
                var chunkId = _reader.ReadUInt32();
                _currentChunkSize = _reader.ReadUInt32();
                var nextChunkPos = _stream.Position + _currentChunkSize;

                switch (chunkId)
                {
                    case MOGPChunk.ID:
                        {
                            var chunk = ReadMOGPChunk();
                            result.GroupNameOffset = chunk.GroupNameOffset;
                            result.DescriptiveNameOffset = chunk.DescriptiveNameOffset;
                            result.Flags = chunk.Flags;
                            result.BoundingBox = chunk.BoundingBox;
                            result.PortalsOffset = chunk.PortalsOffset;
                            result.PortalCount = chunk.PortalCount;
                            result.TransBatchCount = chunk.TransBatchCount;
                            result.IntBatchCount = chunk.IntBatchCount;
                            result.ExtBatchCount = chunk.ExtBatchCount;
                            result.UnknownBatchCount = chunk.UnknownBatchCount;
                            result.FogIndices = chunk.FogIndices;
                            result.GroupLiquid = chunk.GroupLiquid;
                            result.GroupId = chunk.GroupId;
                            result.Flags2 = chunk.Flags2;
                            result.SplitGroupindex = chunk.SplitGroupindex;
                            result.NextSplitChildIndex = chunk.NextSplitChildIndex;

                            // Other chunks are contained in this chunk for some reason, so read next chunk from pos after this header.
                            nextChunkPos = _stream.Position;
                            break;
                        }
                    case MOVIChunk.ID:
                        {
                            var chunk = ReadMOVIChunk();
                            result.Indices.AddRange(chunk.Indices);
                            break;
                        }
                    case MDALChunk.ID:
                        {
                            var chunk = ReadMDALChunk();
                            result.HeaderReplacementColor = chunk.HeaderColor;
                            break;
                        }
                    case MLIQChunk.ID:
                        {
                            var chunk = ReadMLIQChunk();
                            result.LiquidData.Add(chunk.LiquidData);
                            break;
                        }
                    case MOBRChunk.ID:
                        {
                            var chunk = ReadMORBChunk();
                            result.BspIndices.AddRange(chunk.Entries);
                            break;
                        }
                    case MOBNChunk.ID:
                        {
                            var chunk = ReadMOBNChunk();
                            result.BspNodes.AddRange(chunk.Nodes);
                            break;
                        }
                    case MOVTChunk.ID:
                        {
                            var chunk = ReadMOVTChunk();
                            result.Vertices.AddRange(chunk.Vertices);
                            break;
                        }
                    case MONRChunk.ID:
                        {
                            var chunk = ReadMONRChunk();
                            result.Normals.AddRange(chunk.Normals);
                            break;
                        }
                    case MOTVChunk.ID:
                        {
                            var chunk = ReadMOTVChunk();
                            result.UVList.AddRange(chunk.UvList);
                            break;
                        }
                    case MOCVChunk.ID:
                        {
                            var chunk = ReadMOCVChunk();
                            result.VertexColors.AddRange(chunk.ColorVertexList);
                            break;
                        }
                    case MOBAChunk.ID:
                        {
                            var chunk = ReadMOBAChunk();
                            result.Batches.AddRange(chunk.Batches);
                            break;
                        }
                    case MODRChunk.ID:
                        {
                            var chunk = ReadMODRChunk();
                            result.DoodadReferences.AddRange(chunk.DoodadReferences);
                            break;
                        }
                    default:
                        {
                            // Just skip unimplemented chunks for now
                            break;
                        }
                }

                _stream.Position = nextChunkPos;
            }

            return result;
        }
    }
}

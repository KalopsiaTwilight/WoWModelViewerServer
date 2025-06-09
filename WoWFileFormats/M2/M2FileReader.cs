namespace WoWFileFormats.M2
{
    public class M2FileReader : M2ChunksReader
    {
        private uint _fileDataId;
        public M2FileReader(uint fileDataId, Stream input): base(input) {
            _fileDataId = fileDataId;
        }


        public M2File? ReadM2File()
        {
            M2File result = new()
            {
                FileDataID = _fileDataId
            };

            _chunkOffSet = 0;

            try
            {
                while (_stream.Position < _stream.Length)
                {
                    var chunkId = _reader.ReadUInt32();
                    chunkSize = _reader.ReadUInt32();
                    var nextChunkPos = _stream.Position + chunkSize;

                    switch (chunkId)
                    {
                        case MD21Chunk.ID:
                            {
                                Process_MD21Chunk(result);
                                break;
                            }
                        case PFIDChunk.ID: { Process_PFIDChunk(result); break; }
                        case SFIDChunk.ID: { Process_SFIDChunk(result); break; }
                        case AFIDChunk.ID: { Process_AFIDChunk(result); break; }
                        case BFIDChunk.ID: { Process_BFIDChunk(result); break; }
                        case TXACChunk.ID: { Process_TXACChunk(result); break; }
                        case EXPTChunk.ID: { Process_EXPTChunk(result); break; }
                        case EXP2Chunk.ID: { Process_EXP2Chunk(result); break; }
                        case PABCChunk.ID: { Process_PABCChunk(result); break; }
                        case PADCChunk.ID: { Process_PADCChunk(result); break; }
                        case PSBCChunk.ID: { Process_PSBCChunk(result); break; }
                        case PEDCChunk.ID: { Process_PEDCChunk(result); break; }
                        case SKIDChunk.ID: { Process_SKIDChunk(result); break; }
                        case TXIDChunk.ID: { Process_TXIDChunk(result); break; }
                        case LDV1Chunk.ID: { Process_LDV1Chunk(result); break; }
                        case RPIDChunk.ID: { Process_RPIDChunk(result); break; }
                        case GPIDChunk.ID: { Process_GPIDChunk(result); break; }
                        case WFV1Chunk.ID: { break; }
                        case WFV2Chunk.ID: { break; }
                        case PGD1Chunk.ID: { Process_PGD1Chunk(result); break; }
                        case WFV3Chunk.ID: { Process_WFV3Chunk(result); break; }
                        case PFDCChunk.ID: { break; }
                        case EDGFChunk.ID: { Process_EDGFChunk(result); break; }
                        case NERFChunk.ID: { Process_NERFChunk(result); break; }
                        case DETLChunk.ID: { Process_DETLChunk(result); break; }
                        case DBOCChunk.ID: { Process_DBOCChunk(result); break; }
                        default:
                            {
                                // Unknown chunk, data is probably not as it should be
                                return null;
                            }
                    }

                    _stream.Position = nextChunkPos;
                }
            }
            catch(InvalidArrayOffsetException)
            {
                return null;
            }

            return result;
        }


        private void Process_MD21Chunk(M2File file)
        {
            var chunk = ReadMD21Chunk();
            file.Version = chunk.Version;
            file.Name = chunk.Name;
            file.Flags = chunk.Flags;
            file.GlobalLoops = chunk.GlobalLoops;
            file.Sequences = chunk.Sequences;
            _sequences = chunk.Sequences;
            file.SequenceIdLookup = chunk.SequenceIdLookup;
            _sequenceLookup = chunk.SequenceIdLookup;
            file.Bones = chunk.Bones;
            file.BoneIdLookup = chunk.BoneIdLookup;
            file.Vertices = chunk.Vertices;
            file.Colors = chunk.Colors;
            file.Textures = chunk.Textures;
            file.TextureWeights = chunk.TextureWeights;
            file.TextureTransforms = chunk.TextureTransforms;
            file.TextureIdLookup = chunk.TextureIdLookup;
            file.Materials = chunk.Materials;
            file.BoneCombos = chunk.BoneCombos;
            file.TextureCombos = chunk.TextureCombos;
            file.TextureCoordCombos = chunk.TextureCoordCombos;
            file.TextureWeightCombos = chunk.TextureWeightCombos;
            file.TextureTransformCombos = chunk.TextureTransformCombos;
            file.BoundingBox = chunk.BoundingBox;
            file.BoundingSphereRadius = chunk.BoundingSphereRadius;
            file.CollisionBox = chunk.CollisionBox;
            file.CollisionSphereRadius = chunk.CollisionSphereRadius;
            file.CollisionIndices = chunk.CollisionIndices;
            file.CollisionPosition = chunk.CollisionPosition;
            file.CollisionFaceNormals = chunk.CollisionFaceNormals;
            file.Attachments = chunk.Attachments;
            file.AttachmentIdLookup = chunk.AttachmentIdLookup;
            file.Events = chunk.Events;
            file.Lights = chunk.Lights;
            file.Cameras = chunk.Cameras;
            file.CameraIdLookup = chunk.CameraIdLookup;
            file.RibbonEmitters = chunk.RibbonEmitters;
            file.ParticleEmitters = chunk.ParticleEmitters;
            file.TextureCombinerCombos = chunk.TextureCombinerCombos;
            file.NumSkinProfiles = chunk.NumSkinProfiles;
        }

        private void Process_PFIDChunk(M2File file)
        {
            file.PhysFileId = ReadPFIDChunk().PhysFileId;
        }

        private void Process_SFIDChunk(M2File file)
        {
            if (_numSkinProfiles == 0)
            {
                throw new Exception("Attempted to deserialize SFID chunk without receiving any SkinProfile data.");
            }

            var chunk = ReadSFIDChunk();
            file.SkinFileIds = chunk.SkinFileIds;
            file.LodSkinFileIds = chunk.LodSkinFileIds;
        }

        private void Process_AFIDChunk(M2File file)
        {
            file.AnimationFiles = ReadAFIDChunk().Entries;
        }

        private void Process_BFIDChunk(M2File file)
        {
            file.BoneFileDataIds = ReadBFIDChunk().FileDataIds;
        }

        private void Process_TXACChunk(M2File file)
        {
            file.TextureAcData = ReadTXACChunk().Entries;
        }

        private void Process_EXPTChunk(M2File file)
        {
            file.Particles = ReadEXPTChunk().Entries
                .Select(x => x.ToM2ExtendedParticle()).ToArray();
        }

        private void Process_EXP2Chunk(M2File file)
        {
            file.Particles = ReadEXP2Chunk().Particles;
        }

        private void Process_PABCChunk(M2File file)
        {
            file.ReplacementParentSequenceLookups = ReadPABCChunk().ReplacementParentSequenceLookups;
        }

        private void Process_PADCChunk(M2File file)
        {
            file.TextureWeights = ReadPADCChunk().TextureWeights;
        }

        private void Process_PSBCChunk(M2File file)
        {
            file.ParentSequenceBounds = ReadPSBCChunk().ParentSequenceBounds;
        }

        private void Process_SKIDChunk(M2File file)
        {
            file.SkeletonFileDataId = ReadSKIDChunk().FileDataId;
        }

        private void Process_TXIDChunk(M2File file)
        {
            var chunk = ReadTXIDChunk();
            for (var i = 0; i < file.Textures.Length; i++)
            {
                file.Textures[i].FileId = chunk.FileDataIds[i];
            }
        }

        private void Process_LDV1Chunk(M2File file)
        {
            file.LDV1Data = ReadLDV1Chunk();
        }

        private void Process_RPIDChunk(M2File file)
        {
            file.RecursiveParticleFileDataIds = ReadRPIDChunk().FileDataIds;
        }

        private void Process_GPIDChunk(M2File file)
        {
            file.GeometryParticleFileDataIds = ReadGPIDChunk().FileDataIds;
        }

        private void Process_PGD1Chunk(M2File file)
        {
            file.ParticleEmitterGeosets = ReadPGD1Chunk().Geosets;
        }

        private void Process_WFV3Chunk(M2File file)
        {
            file.WFV3Data = ReadWFV3Chunk();
        }

        private void Process_EDGFChunk(M2File file)
        {
            file.EdgeFadeData = ReadEDGFChunk().Entries;
        }

        private void Process_NERFChunk(M2File file)
        {
            file.AlphaCoeffecient = ReadNERFChunk().Coefs;
        }

        private void Process_DETLChunk(M2File file)
        {
            file.LightsData = ReadDETLChunk().Entries;
        }

        private void Process_DBOCChunk(M2File file)
        {
            file.DBOCData = ReadDBOCChunk();
        }

        private void Process_PEDCChunk(M2File file)
        {
            file.ParentEventData = ReadPEDCChunk().ParentEventData;
        }
    }
}

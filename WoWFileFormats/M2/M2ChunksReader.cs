using WoWFileFormats.Common;

namespace WoWFileFormats.M2
{

    public abstract class M2ChunksReader : M2TypesReader
    {
        protected uint _numSkinProfiles;
        protected int _numWeights;

        internal M2ChunksReader(Stream inputStream) : base(inputStream)
        {
        }

        internal BOMTChunk ReadBOMTChunk()
        {
            var result = new BOMTChunk();
            var numEntries = chunkSize / 64;
            result.BoneOffsetMatrices = new C44Matrix[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.BoneOffsetMatrices[i] = ReadC44Matrix();
            }
            return result;
        }

        internal BIDAChunk ReadBIDAChunk()
        {
            var result = new BIDAChunk();
            var numEntries = chunkSize / 2;
            result.BoneIds = new ushort[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.BoneIds[i] = _reader.ReadUInt16();
            }
            return result;
        }

        internal SKPDChunk ReadSKPDChunk()
        {
            return new SKPDChunk
            {
                UnkArray3 = _reader.ReadBytes(8),
                ParentSkeletonFileId = _reader.ReadUInt32(),
                UnkArray4 = _reader.ReadBytes(4)
            };
        }

        internal SKS1Chunk ReadSKS1Chunk()
        {
            _chunkOffSet = _stream.Position;
            var result = new SKS1Chunk()
            {
                GlobalLoops = ReadM2Array(ReadM2Loop),
                Sequences = ReadM2Array(ReadM2Sequence),
                SequenceLookup = ReadM2Array(_reader.ReadUInt16),
                UnkArray2 = _reader.ReadBytes(8)
            };
            return result;
        }

        internal SKB1Chunk ReadSKB1Chunk()
        {
            _chunkOffSet = _stream.Position;
            var result = new SKB1Chunk()
            {
                Bones = ReadM2Array(ReadM2Bone),
                KeyBoneLookup = ReadM2Array(_reader.ReadInt16)
            };
            return result;
        }

        internal SKA1Chunk ReadSKA1Chunk()
        {
            _chunkOffSet = _stream.Position;
            var result = new SKA1Chunk()
            {
                Attachments = ReadM2Array(ReadM2Attachment),
                AttachmentLookup = ReadM2Array(_reader.ReadInt16)
            };
            return result;
        }

        internal SKL1Chunk ReadSKL1Chunk()
        {
            _chunkOffSet = _stream.Position;
            var chunk = new SKL1Chunk()
            {
                Flags = _reader.ReadUInt32(),
                Name = ReadM2String(),
                UnkArray1 = _reader.ReadBytes(4)
            };
            return chunk;
        }

        internal DBOCChunk ReadDBOCChunk()
        {
            return new DBOCChunk()
            {
                Unk1_1 = _reader.ReadSingle(),
                Unk1_2 = _reader.ReadSingle(),
                Unk1_3 = _reader.ReadUInt32(),
                Unk1_4 = _reader.ReadUInt32(),
                Unk1_5 = _reader.ReadSingle(),
                Unk1_6 = _reader.ReadSingle(),
                Unk1_7 = _reader.ReadUInt32(),
                Unk1_8 = _reader.ReadUInt32(),
            };
        }

        internal DETLChunk ReadDETLChunk()
        {
            var result = new DETLChunk();
            var numEntries = chunkSize / 12;
            result.Entries = new DETLChunkEntry[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.Entries[i] = ReadDETLChunkEntry();
            }
            return result;
        }

        internal DETLChunkEntry ReadDETLChunkEntry()
        {
            return new DETLChunkEntry
            {
                Flags = _reader.ReadUInt16(),
                PackedFloat0 = _reader.ReadUInt16(),
                PackedFloat1 = _reader.ReadUInt16(),
                Unk0 = _reader.ReadUInt16(),
                Unk1 = _reader.ReadUInt16(),
            };
        }

        internal NERFChunk ReadNERFChunk()
        {
            return new NERFChunk()
            {
                Coefs = ReadC2Vector()
            };
        }

        internal EDGFChunk ReadEDGFChunk()
        {
            var result = new EDGFChunk();
            var numEntries = chunkSize / 24;
            result.Entries = new EDGFChunkEntry[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.Entries[i] = ReadEDGFChunkEntry();
            }
            return result;
        }

        internal EDGFChunkEntry ReadEDGFChunkEntry()
        {
            return new EDGFChunkEntry
            {
                Unk0 = (_reader.ReadSingle(), _reader.ReadSingle()),
                Unk1 = _reader.ReadSingle(),
                Unk2 = _reader.ReadBytes(12)
            };
        }

        internal WFV3Chunk ReadWFV3Chunk()
        {
            return new WFV3Chunk()
            {
                BumpScale = _reader.ReadSingle(),
                Value0_X = _reader.ReadSingle(),
                Value0_Y = _reader.ReadSingle(),
                Value0_Z = _reader.ReadSingle(),
                Value1_W = _reader.ReadSingle(),
                Value0_W = _reader.ReadSingle(),
                Value1_X = _reader.ReadSingle(),
                Value1_Y = _reader.ReadSingle(),
                Value2_W = _reader.ReadSingle(),
                Value3_Y = _reader.ReadSingle(),
                Value3_X = _reader.ReadSingle(),
                BaseColor = ReadCImVector(),
                Flags = _reader.ReadUInt16(),
                Unk0 = _reader.ReadUInt16(),
                Value3_W = _reader.ReadSingle(),
                Value3_Z = _reader.ReadSingle(),
                Value4_Y = _reader.ReadSingle(),
                Unk1 = _reader.ReadSingle(),
                Unk2 = _reader.ReadSingle(),
                Unk3 = _reader.ReadSingle(),
                Unk4 = _reader.ReadSingle(),
            };
        }

        internal PGD1Chunk ReadPGD1Chunk()
        {
            _chunkOffSet = _stream.Position;
            var result = new PGD1Chunk()
            {
                Geosets = ReadM2Array(_reader.ReadInt16)
            };
            return result;
        }

        internal GPIDChunk ReadGPIDChunk()
        {
            var result = new GPIDChunk();
            var numEntries = chunkSize / 4;
            result.FileDataIds = new uint[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.FileDataIds[i] = _reader.ReadUInt32();
            }
            return result;
        }

        internal RPIDChunk ReadRPIDChunk()
        {
            var result = new RPIDChunk();
            var numEntries = chunkSize / 4;
            result.FileDataIds = new uint[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.FileDataIds[i] = _reader.ReadUInt32();
            }
            return result;
        }

        internal LDV1Chunk ReadLDV1Chunk()
        {
            return new LDV1Chunk()
            {
                Unk0 = _reader.ReadUInt16(),
                LodCount = _reader.ReadUInt16(),
                Unk2 = _reader.ReadSingle(),
                ParticleBoneLod = _reader.ReadBytes(4),
                Unk4 = _reader.ReadUInt32(),
            };
        }

        internal TXIDChunk ReadTXIDChunk()
        {
            var result = new TXIDChunk();
            var numEntries = chunkSize / 4;
            result.FileDataIds = new uint[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.FileDataIds[i] = _reader.ReadUInt32();
            }
            return result;
        }

        internal SKIDChunk ReadSKIDChunk()
        {
            return new SKIDChunk()
            {
                FileDataId = _reader.ReadUInt32()
            };
        }

        internal PEDCChunk ReadPEDCChunk()
        {
            _chunkOffSet = _stream.Position;
            var result = new PEDCChunk()
            {
                ParentEventData = ReadM2Array(ReadM2TrackBase)
            };
            return result;
        }

        internal PSBCChunk ReadPSBCChunk()
        {
            _chunkOffSet = _stream.Position;
            var result = new PSBCChunk()
            {
                ParentSequenceBounds = ReadM2Array(ReadM2Bounds)
            };
            return result;
        }

        internal PADCChunk ReadPADCChunk()
        {
            var result = new PADCChunk
            {
                TextureWeights = new M2TextureWeight[_numWeights]
            };
            for (var i = 0; i < _numWeights; i++)
            {
                result.TextureWeights[i] = ReadM2TextureWeight();
            }
            return result;
        }

        internal PABCChunk ReadPABCChunk()
        {
            var entryCount = chunkSize / 2;
            var result = new PABCChunk()
            {
                ReplacementParentSequenceLookups = new ushort[entryCount],
            };
            for (var i = 0; i < entryCount; i++)
            {
                result.ReplacementParentSequenceLookups[i] = _reader.ReadUInt16();
            }
            return result;
        }

        internal EXP2Chunk ReadEXP2Chunk()
        {
            _chunkOffSet = _stream.Position;
            var result = new EXP2Chunk()
            {
                Particles = ReadM2Array(ReadM2Particle)
            };
            return result;
        }

        internal EXPTChunkEntry ReadEXPTChunkEntry()
        {
            return new EXPTChunkEntry
            {
                ZSource = _reader.ReadSingle(),
                ColorMult = _reader.ReadSingle(),
                AlphaMult = _reader.ReadSingle(),
            };
        }


        internal EXPTChunk ReadEXPTChunk()
        {
            var result = new EXPTChunk();
            var numEntries = chunkSize / 12;
            result.Entries = new EXPTChunkEntry[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.Entries[i] = ReadEXPTChunkEntry();
            }
            return result;
        }

        internal TXACChunk ReadTXACChunk()
        {
            var result = new TXACChunk();
            var numEntries = chunkSize / 2;
            result.Entries = new byte[numEntries][];
            for (var i = 0; i < numEntries; i++)
            {
                result.Entries[i] = _reader.ReadBytes(2);
            }
            return result;
        }

        internal BFIDChunk ReadBFIDChunk()
        {
            var result = new BFIDChunk();
            var numEntries = chunkSize / 4;
            result.FileDataIds = new uint[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.FileDataIds[i] = _reader.ReadUInt32();
            }
            return result;
        }

        internal AnimationFileEntry ReadAnimationFileEntry()
        {
            return new AnimationFileEntry
            {
                AnimId = _reader.ReadUInt16(),
                SubAnimId = _reader.ReadUInt16(),
                FileId = _reader.ReadUInt32()
            };
        }

        internal AFIDChunk ReadAFIDChunk()
        {
            var result = new AFIDChunk();
            var numEntries = chunkSize / 8;
            result.Entries = new AnimationFileEntry[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                result.Entries[i] = ReadAnimationFileEntry();
            }
            return result;
        }

        internal SFIDChunk ReadSFIDChunk()
        {
            var result = new SFIDChunk();

            var lodSkinCount = (chunkSize / 4) - _numSkinProfiles;
            result.SkinFileIds = new uint[_numSkinProfiles];
            result.LodSkinFileIds = new uint[lodSkinCount];

            for (var i = 0; i < _numSkinProfiles; i++)
            {
                result.SkinFileIds[i] = _reader.ReadUInt32();
            }

            for (var i = 0; i < lodSkinCount; i++)
            {
                result.LodSkinFileIds[i] = _reader.ReadUInt32();
            }

            return result;
        }

        internal PFIDChunk ReadPFIDChunk()
        {
            return new PFIDChunk()
            {
                PhysFileId = _reader.ReadUInt32(),
            };
        }

        internal MD21Chunk ReadMD21Chunk()
        {
            var result = new MD21Chunk();
            _chunkOffSet = _stream.Position;
            var magic = _reader.ReadUInt32();
            if (magic != 0x3032444D)
            {
                throw new Exception("Invalid M2 Magic encountered: " + magic);
            }

            result.Magic = magic;
            result.Version = _reader.ReadUInt32();
            result.Name = ReadM2String();
            result.Flags = (MD21ChunkFlags)_reader.ReadUInt32();
            result.GlobalLoops = ReadM2Array(ReadM2Loop);
            _sequences = ReadM2Array(ReadM2Sequence);
            result.Sequences = _sequences;
            result.SequenceIdLookup = ReadM2Array(_reader.ReadUInt16);
            result.Bones = ReadM2Array(ReadM2Bone);
            result.BoneIdLookup = ReadM2Array(_reader.ReadInt16);
            result.Vertices = ReadM2Array(ReadM2Vertex);
            _numSkinProfiles = _reader.ReadUInt32();
            result.NumSkinProfiles = _numSkinProfiles;
            result.Colors = ReadM2Array(ReadM2Color);
            result.Textures = ReadM2Array(ReadM2Texture);
            result.TextureWeights = ReadM2Array(ReadM2TextureWeight);
            _numWeights = result.TextureWeights.Length;
            result.TextureTransforms = ReadM2Array(ReadM2TextureTransform);
            result.TextureIdLookup = ReadM2Array(_reader.ReadInt16);
            result.Materials = ReadM2Array(ReadM2Material);
            result.BoneCombos = ReadM2Array(_reader.ReadInt16);
            result.TextureCombos = ReadM2Array(_reader.ReadInt16);
            result.TextureCoordCombos = ReadM2Array(_reader.ReadInt16);
            result.TextureWeightCombos = ReadM2Array(_reader.ReadInt16);
            result.TextureTransformCombos = ReadM2Array(_reader.ReadInt16);
            result.BoundingBox = ReadCAxisAlignedBox();
            result.BoundingSphereRadius = _reader.ReadSingle();
            result.CollisionBox = ReadCAxisAlignedBox();
            result.CollisionSphereRadius = _reader.ReadSingle();
            result.CollisionIndices = ReadM2Array(_reader.ReadUInt16);
            result.CollisionPosition = ReadM2Array(ReadC3Vector);
            result.CollisionFaceNormals = ReadM2Array(ReadC3Vector);
            result.Attachments = ReadM2Array(ReadM2Attachment);
            result.AttachmentIdLookup = ReadM2Array(_reader.ReadInt16);
            result.Events = ReadM2Array(ReadM2Event);
            result.Lights = ReadM2Array(ReadM2Light);
            result.Cameras = ReadM2Array(ReadM2Camera);
            result.CameraIdLookup = ReadM2Array(_reader.ReadUInt16);
            result.RibbonEmitters = ReadM2Array(ReadM2RibbonEmitter);
            result.ParticleEmitters = ReadM2Array(ReadM2ParticleEmitter);

            if (result.Flags.HasFlag(MD21ChunkFlags.UseTextureCombinerCombos))
            {
                result.TextureCombinerCombos = ReadM2Array(_reader.ReadUInt16);
            }
            return result;
        }
    }
}

using WoWFileFormats.Common;

namespace WoWFileFormats.M2
{
    internal class M2Chunks
    {
    }

    public interface M2Chunk
    {
        public uint ChunkId { get; }
    }

    [Flags]
    public enum MD21ChunkFlags
    {
        None = 0x00,
        TiltX = 0x01,
        TiltY = 0x02,
        Unk_0x04 = 0x04,
        UseTextureCombinerCombos = 0x08,
        Unk_0x10 = 0x10,
        LoadPhysData = 0x20,
        Unk_0x40 = 0x40,
        Unk_0x80 = 0x80,
        CameraRelated = 0x100,
        NewParticleRecord = 0x200,
        Unk_0x400 = 0x400,
        TextureTransformsUseBoneSequences = 0x800,
        Unk_0x1000 = 0x1000,
        ChunkedAnimFiles = 0x2000,
        Unk_0x4000 = 0x4000,
        Unk_0x8000 = 0x8000,
        Unk_0x10000 = 0x10000,
        Unk_0x20000 = 0x20000,
        Unk_0x40000 = 0x40000,
        Unk_0x80000 = 0x80000,
        Unk_0x100000 = 0x100000,
        AnimDataIsChunked = 0x200000,
    }

    public class MD21Chunk : M2Chunk
    {
        public const uint ID = 0x3132444D;

        public uint ChunkId => ID;
        public uint Magic { get; set; }
        public uint Version { get; set; }
        public string Name { get; set; } = string.Empty;
        public MD21ChunkFlags Flags { get; set; }
        public M2Loop[] GlobalLoops { get; set; } = [];
        public M2Sequence[] Sequences { get; set; } = [];
        public ushort[] SequenceIdLookup { get; set; } = [];
        public M2CompBone[] Bones { get; set; } = []; 
        public short[] BoneIdLookup { get; set; } = [];
        public M2Vertex[] Vertices { get; set; } = [];
        public uint NumSkinProfiles { get; set; }
        public M2Color[] Colors { get; set; } = [];
        public M2Texture[] Textures { get; set; } = [];
        public M2TextureWeight[] TextureWeights { get; set; } = [];
        public M2TextureTransform[] TextureTransforms { get; set; } = [];
        public short[] TextureIdLookup { get; set; } = [];
        public M2Material[] Materials { get; set; } = [];
        public short[] BoneCombos { get; set; } = [];
        public short[] TextureCombos { get; set; } = [];
        public short[] TextureCoordCombos { get; set; } = [];
        public short[] TextureWeightCombos { get; set; } = [];
        public short[] TextureTransformCombos { get; set; } = [];
        public CAxisAlignedBox BoundingBox { get; set; }
        public float BoundingSphereRadius { get; set; }
        public CAxisAlignedBox CollisionBox { get; set; }
        public float CollisionSphereRadius { get; set; }
        public ushort[] CollisionIndices { get; set; } = [];
        public C3Vector[] CollisionPosition { get; set; } = [];
        public C3Vector[] CollisionFaceNormals { get; set; } = [];
        public M2Attachment[] Attachments { get; set; } = [];
        public short[] AttachmentIdLookup { get; set; } = [];
        public M2Event[] Events { get; set; } = [];
        public M2Light[] Lights { get; set; } = [];
        public M2Camera[] Cameras { get; set; } = [];
        public ushort[] CameraIdLookup { get; set; } = [];
        public M2Ribbon[] RibbonEmitters { get; set; } = [];
        public M2ParticleEmitter[] ParticleEmitters { get; set; } = [];
        public ushort[] TextureCombinerCombos { get; set; } = [];
    }


    public class PFIDChunk : M2Chunk
    {
        public const uint ID = 0x44494650;
        public uint ChunkId => ID;
        public uint PhysFileId { get; set; }
    }

    public class SFIDChunk : M2Chunk
    {
        public const uint ID = 0x44494653;
        public uint ChunkId => ID;
        public uint[] SkinFileIds { get; set; } = [];
        public uint[] LodSkinFileIds { get; set; } = [];
    }

    public class AnimationFileEntry
    {
        public ushort AnimId { get; set; }
        public ushort SubAnimId { get; set; }
        public uint FileId { get; set; }
    }

    public class AFIDChunk : M2Chunk
    {
        public const uint ID = 0x44494641;

        public uint ChunkId => ID;

        public AnimationFileEntry[] Entries { get; set; } = [];
    }

    public class BFIDChunk : M2Chunk
    {
        public const uint ID = 0x44494642;

        public uint ChunkId => ID;

        public uint[] FileDataIds { get; set; } = [];
    }

    public class TXACChunk : M2Chunk
    {
        public const uint ID = 0x43415854;

        public uint ChunkId => ID;
        public byte[][] Entries { get; set; } = [];

    }

    public class EXPTChunkEntry
    {
        public float ZSource { get; set; }
        public float ColorMult { get; set; }
        public float AlphaMult { get; set; }

        public M2ExtendedParticle ToM2ExtendedParticle()
        {
            return new M2ExtendedParticle()
            {
                AlphaMult = AlphaMult,
                ColorMult = ColorMult,
                ZSource = ZSource
            };
        }
    }
    public class EXPTChunk : M2Chunk
    {
        public const uint ID = 0x54505845;

        public uint ChunkId => ID;
        public EXPTChunkEntry[] Entries { get; set; } = [];

    }
    public class EXP2Chunk : M2Chunk
    {
        public const uint ID = 0x32505845;

        public uint ChunkId => ID;

        public M2ExtendedParticle[] Particles { get; set; } = [];
    }

    public class PABCChunk : M2Chunk
    {
        public const uint ID = 0x43424150;

        public uint ChunkId => ID;

        public ushort[] ReplacementParentSequenceLookups { get; set; } = [];
    }

    public class PADCChunk : M2Chunk
    {
        public const uint ID = 0x43444150;

        public uint ChunkId => ID;

        public M2TextureWeight[] TextureWeights { get; set; } = [];
    }

    public class PSBCChunk : M2Chunk
    {
        public const uint ID = 0x43425350;

        public uint ChunkId => ID;

        public M2Bounds[] ParentSequenceBounds { get; set; } = [];
    }

    public class PEDCChunk : M2Chunk
    {
        public const uint ID = 0x43444550;

        public uint ChunkId => ID;

        public M2TrackBase[] ParentEventData { get; set; } = [];
    }

    public class SKIDChunk : M2Chunk
    {
        public const uint ID = 0x44494B53;

        public uint ChunkId => ID;

        public uint FileDataId { get; set; }
    }

    public class TXIDChunk : M2Chunk
    {
        public const uint ID = 0x44495854;

        public uint ChunkId => ID;

        public uint[] FileDataIds { get; set; } = [];
    }

    public class LDV1Chunk : M2Chunk
    {
        public const uint ID = 0x3156444C;

        public uint ChunkId => ID;
        public ushort Unk0 { get; set; }
        public ushort LodCount { get; set; }
        public float Unk2 { get; set; }
        public byte[] ParticleBoneLod { get; set; } = [];
        public uint Unk4 { get; set; }
    }

    public class RPIDChunk : M2Chunk
    {
        public const uint ID = 0x44495052;

        public uint ChunkId => ID;

        public uint[] FileDataIds { get; set; } = [];
    }

    public class GPIDChunk : M2Chunk
    {
        public const uint ID = 0x44495047;

        public uint ChunkId => ID;

        public uint[] FileDataIds { get; set; } = [];
    }

    /* Structure unknown */
    public class WFV1Chunk : M2Chunk
    {
        public const uint ID = 0x31564657;

        public uint ChunkId => ID;
    }

    /* Structure unknown */
    public class WFV2Chunk : M2Chunk
    {
        public const uint ID = 0x32564657;

        public uint ChunkId => ID;
    }

    public class PGD1Chunk : M2Chunk
    {
        public const uint ID = 0x31444750;

        public uint ChunkId => ID;
        public short[] Geosets { get; set; } = [];
    }

    public class WFV3Chunk : M2Chunk
    {
        public const uint ID = 0x33564657;
        public uint ChunkId => ID;
        public float BumpScale { get; set; }
        public float Value0_X { get; set; }
        public float Value0_Y { get; set; }
        public float Value0_Z { get; set; }
        public float Value1_W { get; set; }
        public float Value0_W { get; set; }
        public float Value1_X { get; set; }
        public float Value1_Y { get; set; }
        public float Value2_W { get; set; }
        public float Value3_Y { get; set; }
        public float Value3_X { get; set; }
        public CImVector BaseColor { get; set; }
        public ushort Flags { get; set; }
        public ushort Unk0 { get; set; }
        public float Value3_W { get; set; }
        public float Value3_Z { get; set; }
        public float Value4_Y { get; set; }
        public float Unk1 { get; set; }
        public float Unk2 { get; set; }
        public float Unk3 { get; set; }
        public float Unk4 { get; set; }
    }

    /* Inline physics, let's hope we can ignore this */
    public class PFDCChunk : M2Chunk
    {
        public const uint ID = 0x43444650;

        public uint ChunkId => ID;
    }

    public class EDGFChunkEntry
    {
        public (float, float) Unk0 { get; set; }
        public float Unk1 { get; set; }
        public float Unk2 { get; set; }
    }
    public class EDGFChunk : M2Chunk
    {
        public const uint ID = 0x46474445;

        public uint ChunkId => ID;
        public EDGFChunkEntry[] Entries { get; set; } = [];
    }

    public class NERFChunk : M2Chunk
    {
        public const uint ID = 0x4652454E;

        public uint ChunkId => ID;
        public C2Vector Coefs { get; set; }
    }

    public class DETLChunkEntry
    {
        public ushort Flags { get; set; }
        public ushort PackedFloat0 { get; set; }
        public ushort PackedFloat1 { get; set; }
        public ushort Unk0 { get; set; }
        public ushort Unk1 { get; set; }
    }
    public class DETLChunk : M2Chunk
    {
        public const uint ID = 0x4C544544;

        public uint ChunkId => ID;
        public DETLChunkEntry[] Entries { get; set; } = [];
    }

    public class DBOCChunk : M2Chunk
    {
        public const uint ID = 0x434F4244;

        public uint ChunkId => ID;
        public float Unk1_1 { get; set; }
        public float Unk1_2 { get; set; }
        public uint Unk1_3 { get; set; }
        public uint Unk1_4 { get; set; }
        public float Unk1_5 { get; set; }
        public float Unk1_6 { get; set; }
        public uint Unk1_7 { get; set; }
        public uint Unk1_8 { get; set; }
    }

    public class SKL1Chunk : M2Chunk
    {
        public const uint ID = 0x314C4B53;

        public uint ChunkId => ID;
        public uint Flags { get; set; }
        public string Name { get; set; } = string.Empty;

        public byte[] UnkArray1 { get; set; } = [];
    }

    public class SKA1Chunk : M2Chunk
    {
        public const uint ID = 0x31414B53;

        public uint ChunkId => ID;
        public M2Attachment[] Attachments { get; set; } = [];
        public short[] AttachmentLookup { get; set; } = [];
    }

    public class SKB1Chunk : M2Chunk
    {
        public const uint ID = 0x31424B53;

        public uint ChunkId => ID;
        public M2CompBone[] Bones { get; set; } = [];
        public short[] KeyBoneLookup { get; set; } = [];
    }

    public class SKS1Chunk : M2Chunk
    {
        public const uint ID = 0x31534B53;

        public uint ChunkId => ID;
        public M2Loop[] GlobalLoops { get; set; } = [];
        public M2Sequence[] Sequences { get; set; } = [];
        public ushort[] SequenceLookup { get; set; } = [];
        public byte[] UnkArray2 { get; set; } = [];
    }

    public class SKPDChunk : M2Chunk
    {
        public const uint ID = 0x44504B53;

        public uint ChunkId => ID;
        public byte[] UnkArray3 { get; set; } = [];
        public uint ParentSkeletonFileId { get; set; }
        public byte[] UnkArray4 { get; set; } = [];
    }

    public class BIDAChunk : M2Chunk
    {
        public const uint ID = 0x41444942;

        public uint ChunkId => ID;
        public ushort[] BoneIds { get; set; } = [];
    }

    public class BOMTChunk : M2Chunk
    {
        public const uint ID = 0x544D4F42;

        public uint ChunkId => ID;

        public C44Matrix[] BoneOffsetMatrices { get; set; } = [];
    }
}

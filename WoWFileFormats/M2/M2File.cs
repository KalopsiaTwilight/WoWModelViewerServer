using WoWFileFormats.Common;
using WoWFileFormats.Interfaces;

namespace WoWFileFormats.M2
{
    public class M2File
    {
        public uint FileDataID { get; set; }

        // MD21
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

        // SFID
        public uint[] SkinFileIds { get; set; } = [];
        public uint[] LodSkinFileIds { get; set; } = [];

        //PFID
        public uint PhysFileId { get; set; }

        //AFID
        public AnimationFileEntry[] AnimationFiles { get; set; } = [];

        //BFID
        public uint[] BoneFileDataIds { get; set; } = [];

        //TXAC
        public byte[][] TextureAcData { get; set; } = [];

        //EXPT/EXP2
        public M2ExtendedParticle[] Particles { get; set; } = [];

        //PABC
        public ushort[] ReplacementParentSequenceLookups { get; set; } = [];

        //PSBC
        public M2Bounds[] ParentSequenceBounds { get; set; } = [];

        //PEDC
        public M2TrackBase[] ParentEventData { get; set; } = [];

        //SKID
        public uint SkeletonFileDataId { get; set; }

        //LDV1
        public LDV1Chunk LDV1Data { get; set; } = new();

        //RPID
        public uint[] RecursiveParticleFileDataIds { get; set; } = [];

        //GPID
        public uint[] GeometryParticleFileDataIds { get; set; } = [];

        //PGD1
        public short[] ParticleEmitterGeosets { get; set; } = [];

        //WFV3
        public WFV3Chunk WFV3Data { get; set; } = new();

        //EDGF
        public EDGFChunkEntry[] EdgeFadeData { get; set; } = [];

        //NERF
        public C2Vector AlphaCoeffecient { get; set; }

        //DETL
        public DETLChunkEntry[] LightsData { get; set; } = [];

        //DBOC
        public DBOCChunk DBOCData { get; set; } = new();

        public SKINFile[] Skins { get; set; } = [];
        public SKINFile[] LodSkins { get; set; } = [];

        public void LoadSkins(IFileDataProvider fileProvider)
        {
            var skins = new List<SKINFile>();
            var lodSKins = new List<SKINFile>();

            foreach (var skinId in SkinFileIds)
            {
                using (var r = new SKINFileReader(fileProvider.GetFileById(skinId)))
                {
                    skins.Add(r.ReadSKINFile());
                }
            }

            foreach (var skinId in LodSkinFileIds)
            {
                using (var r = new SKINFileReader(fileProvider.GetFileById(skinId)))
                {
                    lodSKins.Add(r.ReadSKINFile());
                }
            }

            Skins = [.. skins];
            LodSkins = [.. lodSKins];
        }

        public void LoadSkeleton(IFileDataProvider fileProvider)
        {
            if (SkeletonFileDataId > 0)
            {
                using (var r = new SKELFileReader(fileProvider.GetFileById(SkeletonFileDataId)))
                {
                    var skelFile = r.ReadSKELFile();
                    BoneIdLookup = skelFile.BoneLookup;
                    Bones = skelFile.Bones.Length > 0 ? skelFile.Bones : Bones;
                    Sequences = skelFile.Sequences.Length > 0 ? skelFile.Sequences : Sequences;
                    SequenceIdLookup = skelFile.SequenceLookup.Length > 0 ? skelFile.SequenceLookup : SequenceIdLookup;
                    Attachments = skelFile.Attachments.Length > 0 ? skelFile.Attachments : Attachments;
                    AttachmentIdLookup = skelFile.AttachmentLookup.Length > 0 ? skelFile.AttachmentLookup : AttachmentIdLookup;
                    BoneFileDataIds = skelFile.BoneFileIds.Length > 0 ? skelFile.BoneFileIds : BoneFileDataIds;
                    AnimationFiles = skelFile.AnimFiles.Length > 0 ? skelFile.AnimFiles : AnimationFiles;
                    GlobalLoops = skelFile.GlobalLoops.Length > 0 ? skelFile.GlobalLoops : GlobalLoops;
                };
            }
        }

        public void LoadAnims(IFileDataProvider fileProvider)
        {
            var animData = new Dictionary<int, byte[]>();

            for (int i = 0; i < Sequences.Length; i++)
            {
                var sequence = Sequences[i];
                while ((sequence.Flags & 0x40) == 0x40 && sequence != Sequences[sequence.AliasNext])
                {
                    sequence = Sequences[sequence.AliasNext];
                }

                if ((sequence.Flags & 0x20) == 0x20)
                {
                    // Animation is stored in sequence
                    continue;
                }

                var fileEntry = AnimationFiles.FirstOrDefault(x => x.AnimId == sequence.Id && x.SubAnimId == sequence.VariationIndex);
                if (fileEntry == null || fileEntry.FileId == 0)
                {
                    continue;
                }

                var chunked = Flags.HasFlag(MD21ChunkFlags.AnimDataIsChunked) || SkeletonFileDataId > 0;

                using (var r = new ANIMFileReader(fileProvider.GetFileById(fileEntry.FileId)))
                {
                    var animFile = r.ReadANIMFile(chunked);
                    if (animFile.BoneData.Length > 0)
                    {
                        animData.Add(i, animFile.BoneData);
                    }
                    else
                    {
                        animData.Add(i, animFile.AnimData);
                    }
                }
            }

            ReadOutOfSequenceTracks(animData);
        }
        private void ReadOutOfSequenceTracks(Dictionary<int, byte[]> animData)
        {
            foreach (var bone in Bones)
            {
                ReadOutOfSequenceTrack(bone.Translation, animData, (r) => r.ReadC3Vector());
                ReadOutOfSequenceTrack(bone.Rotation, animData, (r) => r.ReadQuat16());
                ReadOutOfSequenceTrack(bone.Scale, animData, (r) => r.ReadC3Vector());
            }
            foreach (var color in Colors)
            {
                ReadOutOfSequenceTrack(color.Alpha, animData, (r) => r.ReadFixed16());
                ReadOutOfSequenceTrack(color.Color, animData, (r) => r.ReadC3Vector());
            }
            foreach (var weight in TextureWeights)
            {
                ReadOutOfSequenceTrack(weight.Weight, animData, (r) => r.ReadFixed16());
            }
            foreach (var transform in TextureTransforms)
            {
                ReadOutOfSequenceTrack(transform.Translation, animData, (r) => r.ReadC3Vector());
                ReadOutOfSequenceTrack(transform.Rotation, animData, (r) => r.ReadQuat32());
                ReadOutOfSequenceTrack(transform.Scale, animData, (r) => r.ReadC3Vector());
            }
            foreach (var attachment in Attachments)
            {
                ReadOutOfSequenceTrack(attachment.AnimateAttached, animData, (r) => r._reader.ReadByte());
            }
            foreach (var light in Lights)
            {
                ReadOutOfSequenceTrack(light.AmbientColor, animData, (r) => r.ReadC3Vector());
                ReadOutOfSequenceTrack(light.AmbientIntensity, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(light.DiffuseColor, animData, (r) => r.ReadC3Vector());
                ReadOutOfSequenceTrack(light.DiffuseIntensity, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(light.AttenuationStart, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(light.AttenuationEnd, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(light.Visibility, animData, (r) => r._reader.ReadByte());
            }
            foreach(var camera in Cameras)
            {
                ReadOutOfSequenceTrack(camera.Positions, animData, (r) => r.ReadM2SplineKey(r.ReadC3Vector));
                ReadOutOfSequenceTrack(camera.TargetPosition, animData, (r) => r.ReadM2SplineKey(r.ReadC3Vector));
                ReadOutOfSequenceTrack(camera.Roll, animData, (r) => r.ReadM2SplineKey(r._reader.ReadSingle));
                ReadOutOfSequenceTrack(camera.FOV, animData, (r) => r.ReadM2SplineKey(r._reader.ReadSingle));
            }
            foreach(var ribbonEmitter in RibbonEmitters)
            {
                ReadOutOfSequenceTrack(ribbonEmitter.ColorTrack, animData, (r) => r.ReadC3Vector());
                ReadOutOfSequenceTrack(ribbonEmitter.AlphaTrack, animData, (r) => r.ReadFixed16());
                ReadOutOfSequenceTrack(ribbonEmitter.HeightAboveTrack, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(ribbonEmitter.HeightBelowTrack, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(ribbonEmitter.TexSlotTrack, animData, (r) => r._reader.ReadUInt16());
                ReadOutOfSequenceTrack(ribbonEmitter.VisibilityTrack, animData, (r) => r._reader.ReadByte());
            }
            foreach (var particleEmitter in ParticleEmitters)
            {
                ReadOutOfSequenceTrack(particleEmitter.EmissionSpeed, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(particleEmitter.SpeedVariation, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(particleEmitter.VerticalRange, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(particleEmitter.HorizontalRange, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(particleEmitter.Gravity, animData, (r) => r.ReadM2ParticleEmitterGravity(particleEmitter.Flags));
                ReadOutOfSequenceTrack(particleEmitter.Lifespan, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(particleEmitter.EmissionRate, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(particleEmitter.EmissionAreaLength, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(particleEmitter.EmissionAreaWidth, animData, (r) => r._reader.ReadSingle());
                ReadOutOfSequenceTrack(particleEmitter.ZSource, animData, (r) => r._reader.ReadSingle()); ;
                ReadOutOfSequenceTrack(particleEmitter.EnabledIn, animData, (r) => r._reader.ReadByte());
            }
        }

        private void ReadOutOfSequenceTrack<T>(M2Track<T> track, Dictionary<int, byte[]> animData, Func<M2FileReader, T> deserializeFn)
        {
            if (track.TimeStamps.Length == 0)
            {
                return;
            }

            foreach (var key in track.TimeStampsOutOfSequence.Keys)
            {
                if (!animData.ContainsKey(key))
                {
                    continue;
                }
                using var reader = new M2FileReader(0, new MemoryStream(animData[key]));

                var (timeStampCount, timeStampOffset) = track.TimeStampsOutOfSequence[key];
                var (valueCount, valueOffset) = track.ValuesOutOfSequence[key];

                reader._stream.Position = timeStampOffset;
                track.TimeStamps[key] = new int[timeStampCount];
                for (var i = 0; i < timeStampCount; i++)
                {
                    track.TimeStamps[key][i] = reader._reader.ReadInt32();
                }

                reader._stream.Position = valueOffset;
                track.Values[key] = new T[valueCount];
                for (var i = 0; i < valueCount; i++)
                {
                    track.Values[key][i] = deserializeFn(reader);
                }
            }

            track.TimeStampsOutOfSequence.Clear();
            track.ValuesOutOfSequence.Clear();
        }
    }
}

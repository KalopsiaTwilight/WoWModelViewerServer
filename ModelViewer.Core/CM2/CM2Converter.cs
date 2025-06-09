using WoWFileFormats.M2;

namespace ModelViewer.Core.CM2
{
    public class CM2Converter: BaseCompressedConverter
    {
        public static CM2File Convert(M2File file)
        {
            return new CM2File()
            {
                Version = 1000,
                M2Flags = (uint)file.Flags,
                Vertices = file.Vertices.Select(Convert).ToArray(),
                SkinTriangles = file.Skins.Length > 0 ? file.Skins[0].Triangles : [],
                Submeshes = file.Skins.Length > 0 ? file.Skins[0].Submeshes.Select(Convert).ToArray() : [],
                Bones = file.Bones.Select(Convert).ToArray(),
                BoneCombos = file.BoneCombos,
                BoneIdLookup = file.BoneIdLookup,
                TextureUnits = file.Skins.Length > 0 ? file.Skins[0].TextureUnits.Select(Convert).ToArray() : [],
                Materials = file.Materials.Select(Convert).ToArray(),
                Textures = file.Textures.Select(Convert).ToArray(),
                TextureCombos = file.TextureCombos,
                TextureIdLookup = file.TextureIdLookup,
                GlobalLoops = file.GlobalLoops.Select(x => x.Timestamp).ToArray(),
                Animations = file.Sequences.Select(Convert).ToArray(),
                AnimationLookup = file.SequenceIdLookup,
                TextureWeights = file.TextureWeights.Select(Convert).ToArray(),
                TextureWeightCombos = file.TextureWeightCombos,
                TextureTransforms = file.TextureTransforms.Select(Convert).ToArray(),
                TextureTransformCombos = file.TextureTransformCombos,
                Attachments = file.Attachments.Select(Convert).ToArray(),
                AttachmentIdLookup = file.AttachmentIdLookup,
                Colors = file.Colors.Select(Convert).ToArray(),
                ParticleEmitters = file.ParticleEmitters.Select(Convert).ToArray(),
                ParticleEmitterGeosets = file.ParticleEmitterGeosets,
                Particles = file.Particles.Select(Convert).ToArray(),
                RibbonEmitters = file.RibbonEmitters.Select(Convert).ToArray(),
            };
        }

        public static CM2BoneFile Convert(BONEFile file)
        {
            return new CM2BoneFile()
            {
                BoneIds = file.BoneIds,
                BoneOffsetMatrices = file.BoneOffsetMatrices.Select(Convert).ToArray()
            };
        }
        public static CM2Vertex Convert(M2Vertex vertex)
        {
            return new CM2Vertex()
            {
                Position = Convert(vertex.Position),
                Normal = Convert(vertex.Normal),
                TexCoords1 = Convert(vertex.TexCoords.X),
                TexCoords2 = Convert(vertex.TexCoords.Y),
                BoneWeights = vertex.BoneWeights,
                BoneIndices = vertex.BoneIndices,
            };
        }

        public static CM2Animation Convert(M2Sequence sequence)
        {
            return new CM2Animation()
            {
                Id = sequence.Id,
                VariationIndex = sequence.VariationIndex,
                Duration = sequence.Duration,
                Flags = sequence.Flags,
                Frequency = sequence.Frequency,
                BlendTimeIn = sequence.BlendTimeIn,
                BlendTimeOut = sequence.BlendTimeOut,
                ExtentMin = Convert(sequence.Bounds.Extent.Min),
                ExtentMax = Convert(sequence.Bounds.Extent.Max),
                VariationNext = sequence.VariationNext,
                AliasNext = sequence.AliasNext,
            };
        }

        public static CM2Bone Convert(M2CompBone bone)
        {
            return new CM2Bone()
            {
                KeyBoneId = bone.KeyBoneId,
                Flags = (uint)bone.Flags,
                ParentBoneId = bone.ParentBone,
                SubMeshId = bone.SubmeshId,
                BoneNameCRC = bone.BoneNameCRC,
                Pivot = Convert(bone.Pivot),
                Translation = Convert(bone.Translation, Convert),
                Rotation = Convert(bone.Rotation, Convert),
                Scale = Convert(bone.Scale, Convert),
            };
        }

        public static CM2Submesh Convert(M2SubMesh subMesh)
        {
            return new CM2Submesh()
            {
                SubmeshId = subMesh.SubmeshId,
                Level = subMesh.Level,
                VertexStart = subMesh.VertexStart,
                VertexCount = subMesh.VertexCount,
                TriangleStart = subMesh.TriangleStart,
                TriangleCount = subMesh.TriangleCount,
                CenterBoneIndex = subMesh.CenterBoneIndex,
                CenterPosition = Convert(subMesh.CenterPosition),
                SortCenterPosition = Convert(subMesh.SortCenterPosition),
                SortRadius = subMesh.SortRadius
            };
        }

        public static CM2TextureUnit Convert(M2TextureUnit texUnit)
        {
            return new CM2TextureUnit()
            {
                Flags = texUnit.Flags,
                Priority = texUnit.Priority,
                ShaderId = texUnit.ShaderId,
                SkinSectionIndex = texUnit.SkinSectionIndex,
                Flags2 = texUnit.Flags2,
                ColorIndex = texUnit.ColorIndex,
                MaterialIndex = texUnit.MaterialIndex,
                MaterialLayer = texUnit.MaterialLayer,
                TextureCount = texUnit.TextureCount,
                TextureComboIndex = texUnit.TextureComboIndex,
                TextureCoordComboIndex = texUnit.TextureCoordComboIndex,
                TextureWeightComboIndex = texUnit.TextureWeightComboIndex,
                TextureTransformComboIndex = texUnit.TextureTransformComboIndex,
            };
        }

        public static CM2Material Convert(M2Material mat)
        {
            return new CM2Material()
            {
                Flags = (ushort)mat.Flags,
                BlendingMode = mat.BlendingMode,
            };
        }

        public static CM2Texture Convert(M2Texture texture)
        {
            return new CM2Texture()
            {
                Type = (int)texture.Type,
                Flags = (uint)texture.Flags,
                TextureId = texture.FileId
            };
        }

        public static CM2TextureTransform Convert(M2TextureTransform transform)
        {
            return new CM2TextureTransform()
            {
                Translation = Convert(transform.Translation, Convert),
                Rotation = Convert(transform.Rotation, Convert),
                Scaling = Convert(transform.Scale, Convert),
            };
        }

        public static CM2Attachment Convert(M2Attachment attachment)
        {
            return new CM2Attachment()
            {
                Id = attachment.Id,
                Bone = attachment.Bone,
                Position = Convert(attachment.Position),
            };
        }

        public static CM2TextureWeight Convert(M2TextureWeight weight)
        {
            return new CM2TextureWeight()
            {
                Weights = Convert(weight.Weight, (x) => x.Raw)
            };
        }

        public static CM2ParticleEmitter Convert(M2ParticleEmitter emitter)
        {
            return new CM2ParticleEmitter()
            {
                ParticleId = emitter.ParticleId,
                Flags = (uint)emitter.Flags,
                Position = Convert(emitter.Position),
                Bone = emitter.Bone,
                Texture = emitter.Texture,
                BlendingType = emitter.BlendingType,
                EmitterType = emitter.EmitterType,
                ParticleColorIndex = emitter.ParticleColorIndex,
                TextureTileRotation = emitter.TextureTileRotation,
                TextureDimensionsRows = emitter.TextureDimensionsRows,
                TextureDimensionsColumns = emitter.TextureDimensionsColumns,
                EmissionSpeed = Convert(emitter.EmissionSpeed),
                SpeedVariation = Convert(emitter.SpeedVariation),
                VerticalRange = Convert(emitter.VerticalRange),
                HorizontalRange = Convert(emitter.HorizontalRange),
                Gravity = Convert(emitter.Gravity, Convert),
                Lifespan = Convert(emitter.Lifespan),
                LifespanVary = emitter.LifespanVary,
                EmissionRate = Convert(emitter.EmissionRate),
                EmissionRateVary = emitter.EmissionRateVary,
                EmissionAreaLength = Convert(emitter.EmissionAreaLength),
                EmissionAreaWidth = Convert(emitter.EmissionAreaWidth),
                ZSource = Convert(emitter.ZSource),
                ColorTrack = Convert(emitter.ColorTrack, Convert),
                AlphaTrack = Convert(emitter.AlphaTrack, (x) => x.Raw),
                ScaleTrack = Convert(emitter.ScaleTrack, Convert),
                ScaleVary = Convert(emitter.ScaleVary),
                HeadCellTrack = Convert(emitter.HeadCellTrack),
                TailCellTrack = Convert(emitter.TailCellTrack),
                TailLength = emitter.TailLength,
                TwinkleSpeed = emitter.TwinkleSpeed,
                TwinklePercent = emitter.TwinklePercent,
                TwinkleScale = Convert(emitter.TwinkleScale),
                BurstMultiplier = emitter.BurstMultiplier,
                Drag = emitter.Drag,
                BaseSpin = emitter.BaseSpin,
                BaseSpinVary = emitter.BaseSpinVary,
                Spin = emitter.Spin,
                SpinVary = emitter.SpinVary,
                TumbleModelRotationSpeedMin = Convert(emitter.Tumble.ModelRotationSpeedMin),
                TumbleModelRotationSpeedMax = Convert(emitter.Tumble.ModelRotationSpeedMax),
                WindVector = Convert(emitter.WindVector),
                WindTime = emitter.WindTime,
                FollowSpeed1 = emitter.FollowSpeed1,
                FollowScale1 = emitter.FollowScale1,
                FollowSpeed2 = emitter.FollowSpeed2,
                FollowScale2 = emitter.FollowScale2,
                SplinePoints = emitter.SplinePoints.Select(Convert).ToArray(),
                EnabledIn = Convert(emitter.EnabledIn),
                MultiTextureParamX = (emitter.MultiTextureParamX.Item1.ToFloat(), emitter.MultiTextureParamX.Item2.ToFloat()),
                MultiTextureParam0 = (
                    (emitter.MultiTextureParam0.Item1.X.ToFloat(), emitter.MultiTextureParam0.Item1.Y.ToFloat()),
                    (emitter.MultiTextureParam0.Item2.X.ToFloat(), emitter.MultiTextureParam0.Item2.Y.ToFloat())
                ),
                MultiTextureParam1 = (
                    (emitter.MultiTextureParam1.Item1.X.ToFloat(), emitter.MultiTextureParam1.Item1.Y.ToFloat()),
                    (emitter.MultiTextureParam1.Item2.X.ToFloat(), emitter.MultiTextureParam1.Item2.Y.ToFloat())
                ),
            };
        }

        public static CM2Color Convert(M2Color color)
        {
            return new CM2Color()
            {
                Color = Convert(color.Color, Convert),
                Alpha = Convert(color.Alpha, (x) => x.Raw)
            };
        }

        public static CM2ExtendedParticle Convert(M2ExtendedParticle particle)
        {
            return new CM2ExtendedParticle()
            {
                ZSource = particle.ZSource,
                ColorMult = particle.ColorMult,
                AlphaMult = particle.AlphaMult,
                AlphaCutoff = Convert(particle.AlphaCutoff, (x) => x.Raw),
            };
        }

        public static CM2RibbonEmiter Convert(M2Ribbon ribbon)
        {
            return new CM2RibbonEmiter()
            {
                RibbonId = ribbon.RibbonId,
                BoneIndex = ribbon.BoneIndex,
                Position = Convert(ribbon.Position),
                TextureIndices = ribbon.TextureIndices,
                MaterialIndices = ribbon.MaterialIndices,
                ColorTrack = Convert(ribbon.ColorTrack, Convert),
                AlphaTrack = Convert(ribbon.AlphaTrack, (x) => x.Raw),
                HeightAboveTrack = Convert(ribbon.HeightAboveTrack),
                HeightBelowTrack = Convert(ribbon.HeightBelowTrack),
                EdgesPerSecond = ribbon.EdgesPerSecond,
                EdgeLifetime = ribbon.EdgeLifetime,
                Gravity = ribbon.Gravity,
                TextureRows = ribbon.TextureRows,
                TextureCols = ribbon.TextureCols,
                TexSlotTrack = Convert(ribbon.TexSlotTrack),
                VisibilityTrack = Convert(ribbon.VisibilityTrack),
                PriorityPlane = ribbon.PriorityPlane
            };
        }

        public static CM2Track<X> Convert<X>(M2Track<X> track)
        {
            return Convert(track, (x) => x);
        }

        public static CM2Track<Y> Convert<X, Y>(M2Track<X> track, Func<X, Y> convertFn)
        {
            return new CM2Track<Y>()
            {
                InterpolationType = track.InterpolationType,
                GlobalSequence = track.GlobalSequence,
                Animations = track.TimeStamps.Select((x, i) => new CM2AnimatedValue<Y>
                {
                    TimeStamps = x,
                    Values = track.Values[i].Select(convertFn).ToArray(),
                }).ToArray()
            };
        }

        public static CM2LocalTrack<Y> Convert<X, Y>(M2LocalTrack<X> track, Func<X, Y> convertFn)
        {
            return new CM2LocalTrack<Y>()
            {
                Keys = track.Keys,
                Values = track.Values.Select(convertFn).ToArray(),
            };
        }
        public static CM2LocalTrack<T> Convert<T>(M2LocalTrack<T> track)
        {
            return new CM2LocalTrack<T>()
            {
                Keys = track.Keys,
                Values = track.Values,
            };
        }
    }
}

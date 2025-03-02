using System.IO.Compression;

namespace Server.CM2
{
    public class CM2Writer
    {
        protected readonly Stream _stream;
        protected BinaryWriter _writer;

        public CM2Writer(Stream stream)
        {
            _stream = stream;
            _writer = new BinaryWriter(stream);
        }

        public void Write(CM2BoneFile boneFile)
        {
            using var memory = new MemoryStream(GetOutputSize(boneFile));
            _writer = new BinaryWriter(memory);

            var boneIdsPos = 0;
            memory.Position = boneIdsPos;
            WriteArray(boneFile.BoneIds, _writer.Write);

            var boneOffsetMatricesPos = memory.Position;
            WriteArray(boneFile.BoneOffsetMatrices, (x) => { foreach (var val in x) { _writer.Write(val); } });

            var dataEndPos = memory.Position;

            memory.Position = 0;

            using var compressor = new ZLibStream(_stream, CompressionMode.Compress);
            memory.CopyTo(compressor);
        }

        public void Write(CM2File cm2File)
        {
            using var memory = new MemoryStream(GetOutputSize(cm2File));
            _writer = new BinaryWriter(memory);

            var vertexPos = 0;
            memory.Position = vertexPos;
            WriteArray(cm2File.Vertices, Write);

            var skinTrianglesPos = (uint)memory.Position;
            WriteArray(cm2File.SkinTriangles, _writer.Write);

            var submeshesPos = (uint)memory.Position;
            WriteArray(cm2File.Submeshes, Write);

            var bonesPos = (uint)memory.Position;
            WriteArray(cm2File.Bones, Write);

            var boneCombosPos = (uint)memory.Position;
            WriteArray(cm2File.BoneCombos, _writer.Write);

            var boneLookupPos = (uint)memory.Position;
            WriteArray(cm2File.BoneIdLookup, _writer.Write);

            var textureUnitsPos = (uint)memory.Position;
            WriteArray(cm2File.TextureUnits, Write);

            var materialsPos = (uint)memory.Position;
            WriteArray(cm2File.Materials, Write);

            var texturesPos = (uint)memory.Position;
            WriteArray(cm2File.Textures, Write);

            var textureCombosPos = (uint)memory.Position;
            WriteArray(cm2File.TextureCombos, _writer.Write);

            var textureLookupPos = (uint)memory.Position;
            WriteArray(cm2File.TextureIdLookup, _writer.Write);

            var globalLoopsPos = (uint)memory.Position;
            WriteArray(cm2File.GlobalLoops, _writer.Write);

            var animationsPos = (uint)memory.Position;
            WriteArray(cm2File.Animations, Write);

            var animationLookupPos = (uint)memory.Position;
            WriteArray(cm2File.AnimationLookup, _writer.Write);

            var textureWeightsPos = (uint)memory.Position;
            WriteArray(cm2File.TextureWeights, Write);

            var textureWeighComboPos = (uint)memory.Position;
            WriteArray(cm2File.TextureWeightCombos, _writer.Write);

            var textureTransformsPos = (uint)memory.Position;
            WriteArray(cm2File.TextureTransforms, Write);

            var textureTransformCombosPos = (uint)memory.Position;
            WriteArray(cm2File.TextureTransformCombos, _writer.Write);

            var attachmentsPos = (uint)memory.Position;
            WriteArray(cm2File.Attachments, Write);

            var attachmentLookupPos = (uint)memory.Position;
            WriteArray(cm2File.AttachmentIdLookup, _writer.Write);

            var colorsPos = (uint)memory.Position;
            WriteArray(cm2File.Colors, Write);

            var particleEmittersPos = (uint)memory.Position;
            WriteArray(cm2File.ParticleEmitters, Write);

            var particleEmitterGeosetsPos = (uint)memory.Position;
            WriteArray(cm2File.ParticleEmitterGeosets, _writer.Write);

            var particleDataPos = (uint)memory.Position;
            WriteArray(cm2File.Particles, Write);

            var ribbonEmittersPos = (uint)memory.Position;
            WriteArray(cm2File.RibbonEmitters, Write);

            var dataEndPos = (uint)memory.Position;

            _writer = new BinaryWriter(_stream);
            _writer.Write((uint)0x434D3246);
            _writer.Write(cm2File.Version);
            _writer.Write(cm2File.M2Flags);
            _writer.Write(vertexPos);
            _writer.Write(skinTrianglesPos);
            _writer.Write(submeshesPos);
            _writer.Write(bonesPos);
            _writer.Write(boneCombosPos);
            _writer.Write(boneLookupPos);
            _writer.Write(textureUnitsPos);
            _writer.Write(materialsPos);
            _writer.Write(texturesPos);
            _writer.Write(textureCombosPos);
            _writer.Write(textureLookupPos);
            _writer.Write(globalLoopsPos);
            _writer.Write(animationsPos);
            _writer.Write(animationLookupPos);
            _writer.Write(textureWeightsPos);
            _writer.Write(textureWeighComboPos);
            _writer.Write(textureTransformsPos);
            _writer.Write(textureTransformCombosPos);
            _writer.Write(attachmentsPos);
            _writer.Write(attachmentLookupPos);
            _writer.Write(colorsPos);
            _writer.Write(particleEmittersPos);
            _writer.Write(particleEmitterGeosetsPos);
            _writer.Write(particleDataPos);
            _writer.Write(ribbonEmittersPos);
            _writer.Write(dataEndPos);

            memory.Position = 0;

            using var compressor = new ZLibStream(_stream, CompressionMode.Compress);
            memory.CopyTo(compressor);
        }

        public void Write(CM2Vertex vertex)
        {
            Write(vertex.Position);
            Write(vertex.Normal);
            Write(vertex.TexCoords1);
            Write(vertex.TexCoords2);
            _writer.Write(vertex.BoneWeights);
            _writer.Write(vertex.BoneIndices);
        }

        internal void Write(CM2Animation animation)
        {
            _writer.Write(animation.Id);
            _writer.Write(animation.VariationIndex);
            _writer.Write(animation.Duration);
            _writer.Write(animation.Flags);
            _writer.Write(animation.Frequency);
            _writer.Write(animation.BlendTimeIn);
            _writer.Write(animation.BlendTimeOut);
            Write(animation.ExtentMin);
            Write(animation.ExtentMax);
            _writer.Write(animation.VariationNext);
            _writer.Write(animation.AliasNext);
        }

        internal void Write(CM2Bone bone)
        {
            _writer.Write(bone.KeyBoneId);
            _writer.Write(bone.Flags);
            _writer.Write(bone.ParentBoneId);
            _writer.Write(bone.SubMeshId);
            _writer.Write(bone.BoneNameCRC);
            Write(bone.Pivot);
            WriteArray(bone.Translation, (x) => Write(x, Write));
            WriteArray(bone.Rotation, (x) => Write(x, Write));
            WriteArray(bone.Scale, (x) => Write(x, Write));
        }

        internal void Write(CM2Submesh mesh)
        {
            _writer.Write(mesh.SubmeshId);
            _writer.Write(mesh.Level);
            _writer.Write(mesh.VertexStart);
            _writer.Write(mesh.VertexCount);
            _writer.Write(mesh.TriangleStart);
            _writer.Write(mesh.TriangleCount);
            _writer.Write(mesh.CenterBoneIndex);
            Write(mesh.CenterPosition);
            Write(mesh.SortCenterPosition);
            _writer.Write(mesh.SortRadius);
        }

        internal void Write(CM2TextureUnit texUnit)
        {
            _writer.Write(texUnit.Flags);
            _writer.Write(texUnit.Priority);
            _writer.Write(texUnit.ShaderId);
            _writer.Write(texUnit.SkinSectionIndex);
            _writer.Write(texUnit.Flags2);
            _writer.Write(texUnit.ColorIndex);
            _writer.Write(texUnit.MaterialIndex);
            _writer.Write(texUnit.MaterialLayer);
            _writer.Write(texUnit.TextureCount);
            _writer.Write(texUnit.TextureComboIndex);
            _writer.Write(texUnit.TextureCoordComboIndex);
            _writer.Write(texUnit.TextureWeightComboIndex);
            _writer.Write(texUnit.TextureTransformComboIndex);
        }

        internal void Write(CM2Material material)
        {
            _writer.Write(material.Flags);
            _writer.Write(material.BlendingMode);
        }

        internal void Write(CM2Texture texture)
        {
            _writer.Write(texture.Type);
            _writer.Write(texture.Flags);
            _writer.Write(texture.TextureId);
        }

        internal void Write(CM2TextureTransform transform)
        {
            WriteArray(transform.Translation, (x) => Write(x, Write));
            WriteArray(transform.Rotation, (x) => Write(x, Write));
            WriteArray(transform.Scaling, (x) => Write(x, Write));
        }

        internal void Write(CM2Attachment modelAttachment)
        {
            _writer.Write(modelAttachment.Id);
            _writer.Write(modelAttachment.Bone);
            Write(modelAttachment.Position);
        }

        internal void Write(CM2Color color)
        {
            WriteArray(color.Color, (x) => Write(x, Write));
            WriteArray(color.Alpha, (x) => Write(x, _writer.Write));
        }

        internal void Write(CM2TextureWeight weight)
        {
            WriteArray(weight.Weights, (x) => Write(x, _writer.Write));
        }

        internal void Write(CM2ParticleEmitter emitter)
        {
            _writer.Write(emitter.ParticleId);
            _writer.Write(emitter.Flags);
            Write(emitter.Position);
            _writer.Write(emitter.Bone);
            _writer.Write(emitter.Texture);
            _writer.Write(emitter.BlendingType);
            _writer.Write(emitter.EmitterType);
            _writer.Write(emitter.ParticleColorIndex);
            _writer.Write(emitter.TextureTileRotation);
            _writer.Write(emitter.TextureDimensionsRows);
            _writer.Write(emitter.TextureDimensionsColumns);
            _writer.Write(emitter.LifespanVary);
            _writer.Write(emitter.EmissionRateVary);
            Write(emitter.ColorTrack, Write);
            Write(emitter.AlphaTrack, _writer.Write);
            Write(emitter.ScaleTrack, Write);
            Write(emitter.ScaleVary);
            Write(emitter.HeadCellTrack, _writer.Write);
            Write(emitter.TailCellTrack, _writer.Write);
            _writer.Write(emitter.TailLength);
            _writer.Write(emitter.TwinkleSpeed);
            _writer.Write(emitter.TwinklePercent);
            Write(emitter.TwinkleScale);
            _writer.Write(emitter.BurstMultiplier);
            _writer.Write(emitter.Drag);
            _writer.Write(emitter.BaseSpin);
            _writer.Write(emitter.BaseSpinVary);
            _writer.Write(emitter.Spin);
            _writer.Write(emitter.SpinVary);
            Write(emitter.TumbleModelRotationSpeedMin);
            Write(emitter.TumbleModelRotationSpeedMax);
            Write(emitter.WindVector);
            _writer.Write(emitter.WindTime);
            _writer.Write(emitter.FollowSpeed1);
            _writer.Write(emitter.FollowScale1);
            _writer.Write(emitter.FollowSpeed2);
            _writer.Write(emitter.FollowScale2);
            Write(emitter.MultiTextureParamX);
            Write(emitter.MultiTextureParam0.Item1);
            Write(emitter.MultiTextureParam0.Item2);
            Write(emitter.MultiTextureParam1.Item1);
            Write(emitter.MultiTextureParam1.Item2);
            WriteArray(emitter.EmissionSpeed, (x) => Write(x, _writer.Write));
            WriteArray(emitter.SpeedVariation, (x) => Write(x, _writer.Write));
            WriteArray(emitter.VerticalRange, (x) => Write(x, _writer.Write));
            WriteArray(emitter.HorizontalRange, (x) => Write(x, _writer.Write));
            WriteArray(emitter.Gravity, (x) => Write(x, Write));
            WriteArray(emitter.Lifespan, (x) => Write(x, _writer.Write));
            WriteArray(emitter.EmissionAreaLength, (x) => Write(x, _writer.Write));
            WriteArray(emitter.EmissionAreaWidth, (x) => Write(x, _writer.Write));
            WriteArray(emitter.ZSource, (x) => Write(x, _writer.Write));
            WriteArray(emitter.EmissionRate, (x) => Write(x, _writer.Write));
            WriteArray(emitter.SplinePoints, Write);
            WriteArray(emitter.EnabledIn, (x) => Write(x, _writer.Write));
        }

        internal void Write(CM2ExtendedParticle particle)
        {
            _writer.Write(particle.ZSource);
            _writer.Write(particle.ColorMult);
            _writer.Write(particle.AlphaMult);
            Write(particle.AlphaCutoff, _writer.Write);
        }

        internal void Write(CM2RibbonEmiter emitter)
        {
            _writer.Write(emitter.RibbonId);
            _writer.Write(emitter.BoneIndex);
            Write(emitter.Position);
            _writer.Write(emitter.EdgesPerSecond);
            _writer.Write(emitter.EdgeLifetime);
            _writer.Write(emitter.Gravity);
            _writer.Write(emitter.TextureRows);
            _writer.Write(emitter.TextureCols);
            _writer.Write(emitter.PriorityPlane);
            WriteArray(emitter.TexSlotTrack, (x) => Write(x, _writer.Write));
            WriteArray(emitter.VisibilityTrack, (x) => Write(x, _writer.Write));
            WriteArray(emitter.TextureIndices, _writer.Write);
            WriteArray(emitter.MaterialIndices, _writer.Write);
            WriteArray(emitter.ColorTrack, (x) => Write(x, Write));
            WriteArray(emitter.AlphaTrack, (x) => Write(x, _writer.Write));
            WriteArray(emitter.HeightAboveTrack, (x) => Write(x, _writer.Write));
            WriteArray(emitter.HeightBelowTrack, (x) => Write(x, _writer.Write));
        }

        #region Generic Writes
        internal void Write<T>(CM2LocalTrack<T> track, Action<T> writeFn)
        {
            WriteArray(track.Keys, _writer.Write);
            WriteArray(track.Values, writeFn);
        }

        private void Write<T>(CM2Track<T> track, Action<T> writeFn)
        {
            _writer.Write(track.InterpolationType);
            _writer.Write(track.GlobalSequence);
            WriteArray(track.TimeStamps, _writer.Write);
            WriteArray(track.Values, writeFn);
        }

        private void Write((float, float) value)
        {
            _writer.Write(value.Item1);
            _writer.Write(value.Item2);
        }

        private void Write((float, float, float) value)
        {
            _writer.Write(value.Item1);
            _writer.Write(value.Item2);
            _writer.Write(value.Item3);
        }

        private void Write((float, float, float, float) value)
        {
            _writer.Write(value.Item1);
            _writer.Write(value.Item2);
            _writer.Write(value.Item3);
            _writer.Write(value.Item4);
        }

        private void WriteArray<T>(T[] value, Action<T> writeFn)
        {
            _writer.Write(value.Length);
            foreach (var elem in value)
            {
                writeFn(elem);
            }
        }
        #endregion

        public void Dispose()
        {
            _writer.Dispose();
            _stream.Dispose();
        }


        #region Output Sizes

        private static int GetOutputSize(CM2File file)
        {
            return 4 + file.Vertices.Sum(GetOutputSize) +
                4 + file.SkinTriangles.Length * 2 +
                4 + file.GlobalLoops.Length * 2 +
                4 + file.Animations.Sum(GetOutputSize) +
                4 + file.AnimationLookup.Length * 2 +
                4 + file.Bones.Sum(GetOutputSize) +
                4 + file.BoneCombos.Length * 2 +
                4 + file.BoneIdLookup.Length * 2 +
                4 + file.Submeshes.Sum(GetOutputSize) +
                4 + file.TextureUnits.Sum(GetOutputSize) +
                4 + file.Materials.Sum(GetOutputSize) +
                4 + file.Textures.Sum(GetOutputSize) +
                4 + file.TextureCombos.Length * 2 +
                4 + file.TextureTransforms.Sum(GetOutputSize) +
                4 + file.TextureTransformCombos.Length * 2 +
                4 + file.TextureIdLookup.Length * 2 +
                4 + file.Attachments.Sum(GetOutputSize) +
                4 + file.AttachmentIdLookup.Length * 2 +
                4 + file.Colors.Sum(GetOutputSize) +
                4 + file.TextureWeights.Sum(GetOutputSize) +
                4 + file.TextureWeightCombos.Length * 2 +
                4 + file.ParticleEmitters.Sum(GetOutputSize) +
                4 + file.RibbonEmitters.Sum(GetOutputSize) +
                4 + file.Particles.Sum(GetOutputSize) +
                4 + file.ParticleEmitterGeosets.Length * 2;
        }

        private static int GetOutputSize(CM2BoneFile boneFile)
        {
            return 4 + boneFile.BoneIds.Length * 2
                + 4 + boneFile.BoneOffsetMatrices.Length * 4 * 16;
        }

        private static int GetOutputSize(CM2RibbonEmiter emitter)
        {
            return 38 +
                4 + emitter.TextureIndices.Length * 2 +
                4 + emitter.MaterialIndices.Length * 2 +
                4 + emitter.ColorTrack.Sum(x => GetOutputSize(x, 12)) +
                4 + emitter.AlphaTrack.Sum(x => GetOutputSize(x, 2)) +
                4 + emitter.HeightAboveTrack.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.HeightBelowTrack.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.TexSlotTrack.Sum(x => GetOutputSize(x, 2)) +
                4 + emitter.VisibilityTrack.Sum(x => GetOutputSize(x, 2));
        }

        private static int GetOutputSize(CM2ExtendedParticle particle)
        {
            return 12 + GetOutputSize(particle.AlphaCutoff, 2);
        }

        private static int GetOutputSize(CM2ParticleEmitter emitter)
        {
            return 190 +
                4 + emitter.EmissionSpeed.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.SpeedVariation.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.VerticalRange.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.HorizontalRange.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.Gravity.Sum(x => GetOutputSize(x, 12)) +
                4 + emitter.Lifespan.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.EmissionRate.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.EmissionAreaLength.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.EmissionAreaWidth.Sum(x => GetOutputSize(x, 4)) +
                4 + emitter.ZSource.Sum(x => GetOutputSize(x, 4)) +
                GetOutputSize(emitter.ColorTrack, 12) +
                GetOutputSize(emitter.AlphaTrack, 2) +
                GetOutputSize(emitter.ScaleTrack, 8) +
                GetOutputSize(emitter.HeadCellTrack, 2) +
                GetOutputSize(emitter.TailCellTrack, 2) +
                4 + emitter.SplinePoints.Length * 12 +
                4 + emitter.EnabledIn.Sum(x => GetOutputSize(x, 1));
        }

        private static int GetOutputSize<T>(CM2LocalTrack<T> track, int sizeOfT)
        {
            return 4 + track.Keys.Length * 2 +
                4 + track.Values.Length * sizeOfT;
        }

        private static int GetOutputSize(CM2TextureWeight weight)
        {
            return 4 + weight.Weights.Sum(x => GetOutputSize(x, 2));
        }

        private static int GetOutputSize(CM2Color color)
        {
            return 4 + color.Color.Sum(x => GetOutputSize(x, 12)) +
                4 + color.Alpha.Sum(x => GetOutputSize(x, 2));
        }

        private static int GetOutputSize(CM2Attachment attachment)
        {
            return 20;
        }

        private static int GetOutputSize(CM2TextureTransform transform)
        {
            return
                4 + transform.Translation.Sum(x => GetOutputSize(x, 12)) +
                4 + transform.Rotation.Sum(x => GetOutputSize(x, 16)) +
                4 + transform.Scaling.Sum(x => GetOutputSize(x, 12));
        }

        private static int GetOutputSize(CM2Texture texture)
        {
            return 12;
        }

        private static int GetOutputSize(CM2Material mat)
        {
            return 4;
        }

        private static int GetOutputSize(CM2TextureUnit unit)
        {
            return 24;
        }

        private static int GetOutputSize(CM2Submesh mesh)
        {
            return 42;
        }

        private static int GetOutputSize(CM2Bone bone)
        {
            return 28 +
                4 + bone.Translation.Sum(x => GetOutputSize(x, 12)) +
                4 + bone.Rotation.Sum(x => GetOutputSize(x, 16)) +
                4 + bone.Scale.Sum(x => GetOutputSize(x, 12));
        }

        private static int GetOutputSize<T>(CM2Track<T> track, int sizeOfT)
        {
            return 4 + 4 + track.TimeStamps.Length * 4 + 4 + track.Values.Length * sizeOfT;
        }

        private static int GetOutputSize(CM2Vertex vertex)
        {
            return 52;
        }

        private static int GetOutputSize(CM2Animation animation)
        {
            return 46;
        }
        #endregion
    }
}

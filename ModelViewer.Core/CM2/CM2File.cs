namespace ModelViewer.Core.CM2
{
    public class CM2File
    {
        public uint Version { get; set; }
        public uint M2Flags { get; set; }
        public CM2Vertex[] Vertices { get; set; } = [];
        public ushort[] SkinTriangles { get; set; } = [];
        public CM2Submesh[] Submeshes { get; set; } = [];
        public CM2Bone[] Bones { get; set; } = [];
        public short[] BoneCombos { get; set; } = [];
        public short[] BoneIdLookup { get; set; } = [];
        public CM2TextureUnit[] TextureUnits { get; set; } = [];
        public CM2Material[] Materials { get; set; } = [];
        public CM2Texture[] Textures { get; set; } = [];
        public short[] TextureCombos { get; set; } = [];
        public short[] TextureIdLookup { get; set; } = [];
        public uint[] GlobalLoops { get; set; } = [];
        public CM2Animation[] Animations { get; set; } = [];
        public ushort[] AnimationLookup { get; set; } = [];
        public CM2TextureWeight[] TextureWeights { get; set; } = [];
        public short[] TextureWeightCombos { get; set; } = [];
        public CM2TextureTransform[] TextureTransforms { get; set; } = [];
        public short[] TextureTransformCombos { get; set; } = [];
        public CM2Attachment[] Attachments { get; set; } = [];
        public short[] AttachmentIdLookup { get; set; } = [];
        public CM2Color[] Colors { get; set; } = [];
        public CM2ParticleEmitter[] ParticleEmitters { get; set; } = [];
        public short[] ParticleEmitterGeosets { get; set; } = [];
        public CM2ExtendedParticle[] Particles { get; set; } = [];
        public CM2RibbonEmiter[] RibbonEmitters { get; set; } = [];
    }
}

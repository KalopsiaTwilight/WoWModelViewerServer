namespace WoWFileFormats.M2
{
    // https://wowdev.wiki/M2/.skin#Bones
    public class SKINFile
    {
        public uint BoneCountMax { get; set; }
        public ushort[] Vertices { get; set; } = [];
        public ushort[] Triangles { get; set; } = [];
        public byte[][] Bones { get; set; } = [];
        public M2SubMesh[] Submeshes { get; set; } = [];
        public M2TextureUnit[] TextureUnits { get; set; } = [];
    }
}

namespace WoWFileFormats.M2
{
    public class SKINFileReader : M2ChunksReader
    {
        internal const uint MAGIC_SKIN = 0x4E494B53;

        public SKINFileReader(Stream inputStream) : base(inputStream)
        {
        }

        public SKINFile ReadSKINFile()
        {
            var result = new SKINFile();

            var magic = _reader.ReadUInt32();
            if (magic != MAGIC_SKIN)
            {
                throw new Exception("Invalid magic encountered for SKINFILE: " + magic);
            }

            result.Vertices = ReadM2Array(_reader.ReadUInt16);
            result.Triangles = ReadM2Array(_reader.ReadUInt16);
            result.Bones = ReadM2Array(() => _reader.ReadBytes(4));
            result.Submeshes = ReadM2Array(ReadM2SubMesh);
            result.TextureUnits = ReadM2Array(ReadM2TextureUnit);
            result.BoneCountMax = _reader.ReadUInt32();
            return result;
        }
    }
}

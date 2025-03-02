namespace WoWFileFormats.M2
{
    public class ANIMFileReader : M2ChunksReader
    {
        public const uint CHUNK_AFM2 = 0x324D4641;
        public const uint CHUNK_AFSA = 0x41534641;
        public const uint CHUNK_AFSB = 0x42534641;

        public ANIMFileReader(Stream inputStream) : base(inputStream)
        {
        }

        public ANIMFile ReadANIMFile(bool chunked)
        {
            ANIMFile animFile = new();
            if (!chunked)
            {
                animFile.AnimData = _reader.ReadBytes((int)(_stream.Length - _stream.Position));
                return animFile;
            }

            while (_stream.Position != _stream.Length)
            {
                var chunkId = _reader.ReadUInt32();
                var chunkSize = _reader.ReadUInt32();
                var nextChunkPos = _stream.Position + chunkSize;

                switch (chunkId)
                {
                    case CHUNK_AFM2: animFile.AnimData = _reader.ReadBytes((int)chunkSize); break;
                    case CHUNK_AFSA: animFile.AttachmentData = _reader.ReadBytes((int)chunkSize); break;
                    case CHUNK_AFSB: animFile.BoneData = _reader.ReadBytes((int)chunkSize); break;
                }

                _stream.Position = nextChunkPos;
            }
            return animFile;
        }
    }
}

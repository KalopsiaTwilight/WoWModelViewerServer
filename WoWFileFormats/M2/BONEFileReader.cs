namespace WoWFileFormats.M2
{
    public class BONEFileReader: M2ChunksReader
    {
        public BONEFileReader(Stream inputStream) : base(inputStream)
        {
        }

        public BONEFile ReadBONEFile()
        {
            var result = new BONEFile();
            _reader.ReadUInt32();
            while (_stream.Position < _stream.Length)
            {
                var chunkId = _reader.ReadUInt32();
                chunkSize = _reader.ReadUInt32();
                var nextChunkPos = _stream.Position + chunkSize;

                switch (chunkId)
                {
                    case BIDAChunk.ID:
                        {
                            var chunk = ReadBIDAChunk();
                            result.BoneIds = chunk.BoneIds;
                            break;
                        }
                    case BOMTChunk.ID:
                        {
                            var chunk = ReadBOMTChunk();
                            result.BoneOffsetMatrices = chunk.BoneOffsetMatrices;
                            break;
                        }
                    default: throw new Exception("Encountered unknown chunk type while parsing file: " + chunkId);
                }

                _stream.Position = nextChunkPos;
            }
            return result;
        }
    }
}

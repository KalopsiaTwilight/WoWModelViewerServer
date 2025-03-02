namespace WoWFileFormats.M2
{
    public class SKELFileReader: M2ChunksReader
    {
        public SKELFileReader(Stream inputStream) : base(inputStream)
        {
        }

        public SKELFile ReadSKELFile()
        {
            var result = new SKELFile();
            long fileOffset = 0;
            while (_stream.Position < _stream.Length)
            {
                var chunkId = _reader.ReadUInt32();
                chunkSize = _reader.ReadUInt32();
                var nextChunkPos = _stream.Position + chunkSize;

                switch (chunkId)
                {
                    case SKL1Chunk.ID:
                        {
                            fileOffset = _stream.Position;
                            var chunk = ReadSKL1Chunk();
                            result.Flags = chunk.Flags;
                            result.Name = chunk.Name;
                            result.UnkArray1 = chunk.UnkArray1;
                            break;
                        }
                    case SKA1Chunk.ID:
                        {
                            var chunk = ReadSKA1Chunk();
                            result.Attachments = chunk.Attachments;
                            result.AttachmentLookup = chunk.AttachmentLookup;
                            break;
                        }
                    case SKB1Chunk.ID:
                        {
                            var chunk = ReadSKB1Chunk();
                            result.Bones = chunk.Bones;
                            result.BoneLookup = chunk.KeyBoneLookup;
                            break;
                        }
                    case SKS1Chunk.ID:
                        {
                            var chunk = ReadSKS1Chunk();
                            result.GlobalLoops = chunk.GlobalLoops;
                            result.Sequences = chunk.Sequences;
                            _sequences = chunk.Sequences;
                            result.SequenceLookup = chunk.SequenceLookup;
                            _sequenceLookup = chunk.SequenceLookup;
                            break;
                        }
                    case SKPDChunk.ID:
                        {
                            var chunk = ReadSKPDChunk();
                            result.ParentSkeletonFileId = chunk.ParentSkeletonFileId;
                            result.UnkArray3 = chunk.UnkArray3;
                            result.UnkArray4 = chunk.UnkArray4;
                            break;
                        }
                    case AFIDChunk.ID:
                        {
                            var chunk = ReadAFIDChunk();
                            result.AnimFiles = chunk.Entries;
                            break;
                        }
                    case BFIDChunk.ID:
                        {
                            var chunk = ReadBFIDChunk();
                            result.BoneFileIds = chunk.FileDataIds;
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unknown SKELFile chunk: " + chunkId);
                        }
                }

                _stream.Position = nextChunkPos;
            }
            return result;
        }
    }
}

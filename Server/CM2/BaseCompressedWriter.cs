namespace Server.CM2
{
    public class BaseCompressedWriter
    {
        protected readonly Stream _stream;
        protected BinaryWriter _writer;

        public BaseCompressedWriter(Stream stream)
        {
            _stream = stream;
            _writer = new BinaryWriter(stream);
        }

        protected void Write((int, int) value)
        {
            _writer.Write(value.Item1);
            _writer.Write(value.Item2);
        }

        protected void Write((byte, byte, byte, byte) value)
        {
            _writer.Write(value.Item1);
            _writer.Write(value.Item2);
            _writer.Write(value.Item3);
            _writer.Write(value.Item4);
        }

        protected void Write((float, float) value)
        {
            _writer.Write(value.Item1);
            _writer.Write(value.Item2);
        }

        protected void Write((float, float, float) value)
        {
            _writer.Write(value.Item1);
            _writer.Write(value.Item2);
            _writer.Write(value.Item3);
        }

        protected void Write((float, float, float, float) value)
        {
            _writer.Write(value.Item1);
            _writer.Write(value.Item2);
            _writer.Write(value.Item3);
            _writer.Write(value.Item4);
        }

        protected void WriteArray<T>(T[] value, Action<T> writeFn)
        {
            _writer.Write(value.Length);
            foreach (var elem in value)
            {
                writeFn(elem);
            }
        }
    }
}

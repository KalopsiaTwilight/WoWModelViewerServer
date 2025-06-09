namespace WoWFileFormats.M2
{
    public class InvalidArrayOffsetException : Exception
    {
        public InvalidArrayOffsetException(long offset, long length) : 
            base($"Read invalid array offset position. Offset {offset} was greater than stream length of {length}")
        {
        }
    }
}

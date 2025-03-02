namespace ModelViewer.Core.Models
{
    public class TextureFileData
    {
        public uint FileDataId { get; set; }
        public int ClassId { get; set; }
        public int GenderId { get; set; }
        public int RaceId { get; set; }
        public int UsageType { get; set; }
    }

    public class ModelFileData
    {
        public uint FileDataId { get; set; }
        public int ClassId { get; set; }
        public int GenderId { get; set; }
        public int RaceId { get; set; }
        public int PositionIndex { get; set; }
    }
}

namespace ModelViewer.Core.Models
{
    public class LiquidTypeMetadata
    {
        public int Id { get; set; }
        public int Flags { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Color0 { get; set; }
        public int Color1 { get; set; }
        public int MaterialId { get; set; }
        public float Float0 { get; set; }
        public float Float1 { get; set; }
        public string[] NamedTextures { get; set; } = [];
        public List<LiquidTypeTexture> Textures { get; set; } = [];
    }

    public class LiquidObjectMetadata
    {
        public int Id { get; set; }
        public int FlowDirection { get; set; }
        public float FlowSpeed  { get; set; }
        public int LiquidTypeId { get; set; }
        public bool Reflection { get; set; }
    }

    public class LiquidTypeTexture
    {
        public int Id { get; set; }
        public int FileDataId { get; set; }
        public int OrderIndex { get; set; }
        public int Type { get; set; }
    }
}

namespace ModelViewer.Core.Models
{
    public class ItemVisualMetadata
    {
        public int[] ModelFileIds { get; set; } = [];
        public List<ItemVisualEffectsData>? Effects { get; set; }
    }

    public class ItemVisualEffectsData
    {
        public int AttachmentId { get; set; }
        public int SubClassId { get; set; }
        public int ModelFileDataId { get; set; }
        public SpellVisualKitData? SpellVisualKit { get; set; }
        public float Scale { get; set; }
    }

}

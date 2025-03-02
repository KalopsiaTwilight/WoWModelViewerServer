namespace ModelViewer.Core.Models
{
    public class SpellVisualKitData
    {
        public List<SpellVisualKitEffectData> Effects { get; set; } = [];
    };

    public class SpellVisualKitEffectData
    {
        public int EffectType { get; set; }
        public int ProceduralEffectType { get; set; }
        public float[]? Value { get; set; }
        public int[]? Offset { get; set; }
        public int[]? OffsetVariation { get; set; }
        public int AttachmentId { get; set; }
        public int Yaw { get; set; }
        public int Pitch { get; set; }
        public int Roll { get; set; }
        public int Scale { get; set; }
        public int AnimID { get; set; }
        public int PositionerId { get; set; }
        public float[] Colors0 { get; set; } = [];
        public float[] Colors1 { get; set; } = [];
        public float[] Colors2 { get; set; } = [];
        public float[] Alpha { get; set; } = [];
        public float[] EdgeColor { get; set; } = [];
        public int GradientFlags { get; set; }
    }
}

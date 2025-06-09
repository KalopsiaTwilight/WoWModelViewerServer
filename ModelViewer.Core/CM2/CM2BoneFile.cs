namespace ModelViewer.Core.CM2
{
    public class CM2BoneFile
    {
        public ushort[] BoneIds { get; set; } = [];
        public float[][] BoneOffsetMatrices { get; set; } = [];
    }
}

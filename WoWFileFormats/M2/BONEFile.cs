using WoWFileFormats.Common;

namespace WoWFileFormats.M2
{
    public class BONEFile
    {
        public ushort[] BoneIds { get; set; } = [];
        public C44Matrix[] BoneOffsetMatrices { get; set; } = [];
    }
}

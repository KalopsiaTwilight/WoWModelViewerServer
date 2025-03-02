namespace WoWFileFormats.M2
{
    public class SKELFile
    {
        public uint Flags { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] UnkArray1 { get; set; } = [];

        public M2Attachment[] Attachments { get; set; } = [];
        public short[] AttachmentLookup { get; set; } = [];

        public M2CompBone[] Bones { get; set; } = [];
        public short[] BoneLookup { get; set; } = [];

        public M2Loop[] GlobalLoops { get; set; } = [];
        public M2Sequence[] Sequences { get; set; } = [];
        public ushort[] SequenceLookup { get; set; } = [];
        public byte[] UnkArray2 { get; set; } = [];

        public byte[] UnkArray3 { get; set; } = [];

        public uint ParentSkeletonFileId { get; set; }
        public byte[] UnkArray4 { get; set; } = [];

        public AnimationFileEntry[] AnimFiles { get; set; } = [];
        public uint[] BoneFileIds { get; set; } = [];
    }
}

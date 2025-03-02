namespace ModelViewer.Core.Models
{
    public class ItemMetadata
    {
        public int Flags { get; set; }
        public int InventoryType { get; set; }
        public int ClassId { get; set; }
        public int SubclassId { get; set; }
        public int[] GeosetGroup { get; set; } = [];
        public int[] AttachmentGeosetGroup { get; set; } = [];
        public int GeosetGroupOverride { get; set; }
        public int ItemVisual { get; set; }
        public List<ComponentSectionData> ComponentSections { get; set; } = [];
        public ItemParticleColorOverrideData? ParticleColor { get; set; }
        public List<ItemHideGeosetData>? HideGeoset1 { get; set; }
        public List<ItemHideGeosetData>? HideGeoset2 { get; set; }
        public ItemComponentData? Component1 { get; set; }
        public ItemComponentData? Component2 { get; set; }
    }

    public class ComponentSectionData
    {
        public int Section { get; set; }
        public List<TextureFileData> Textures { get; set; } = [];
    }

    public class ItemComponentData
    {
        public List<ModelFileData> ModelFiles { get; set; } = [];
        public List<TextureFileData> TextureFiles { get; set; } = [];
    }

    public class ItemHideGeosetData
    {
        public int RaceId { get; set; }
        public int GeosetGroup { get; set; }
        public int? RaceBitSelection { get; set; }
    }

    public class ItemParticleColorOverrideData
    {
        public int Id { get; set; }
        public uint[] Start { get; set; } = [];
        public uint[] Mid { get; set; } = [];
        public uint[] End { get; set; } = [];
    }
}

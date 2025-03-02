namespace ModelViewer.Core.Models
{
    public class CharacterCustomizationMetadata
    {
        public List<CharacterCustomizationMaterialsData> ModelMaterials { get; set; } = [];
        public List<CharacterCustomizationOptionData> Options { get; set; } = [];
        public List<CharacterCustomizationTextureLayerData> TextureLayers { get; set; } = [];
        public List<CharacterCustomizationTextureSectionData> TextureSections { get; set; } = [];
    }

    public class CharacterCustomizationMaterialsData
    {
        public int Flags { get; set; }
        public int Height { get; set; }
        public int TextureType { get; set; }
        public int Width { get; set; }
    }

    public class CharacterCustomizationOptionData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public List<CharacterCustomizationOptionChoiceData> Choices { get; set; } = [];
    }

    public class CharacterCustomizationOptionChoiceData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public List<CharacterCustomizationOptionChoiceElementData> Elements { get; set; } = [];
    }

    public class CharacterCustomizationOptionChoiceElementData
    {
        public int Id { get; set; }
        public int ChrCustItemGeoModifyId { get; set; }
        public uint ConditionalModelFileDataId { get; set; }
        public int RelationChoiceID { get; set; }
        public int RelationIndex { get; set; }
        public CharacterCustomizationOptionChoiceElementBoneSetData? BoneSet { get; set; }
        public CharacterCustomizationOptionChoiceElementGeosetData? Geoset { get; set; }
        public CharacterCustomizationOptionChoiceElementMaterialData? Material { get; set; }
        public CharacterCustomizationOptionChoiceElementSkinnedModelData? SkinnedModel { get; set; }
    }

    public class CharacterCustomizationOptionChoiceElementBoneSetData
    {
        public int BoneFileDataId { get; set; }
        public int ModelFileDataId { get; set; }
    }

    public class CharacterCustomizationOptionChoiceElementMaterialData
    {
        public int ChrModelTextureTargetId { get; set; }
        public List<TextureFileData> TextureFiles { get; set; } = [];
    }
    public class CharacterCustomizationOptionChoiceElementGeosetData
    {
        public int GeosetType { get; set; }
        public int GeosetId { get; set; }
        public int Modifier { get; set; }
    }
    public class CharacterCustomizationOptionChoiceElementSkinnedModelData
    {
        public int CollectionsFileDataId { get; set; }
        public int GeosetType { get; set; }
        public int GeosetId { get; set; }
        public int Modifier { get; set; }
        public int Flags { get; set; }
    }

    public class CharacterCustomizationTextureLayerData
    {
        public int BlendMode { get; set; }
        public int ChrModelTextureTargetId { get; set; }
        public int Layer { get; set; }
        public int TextureSection { get; set; }
        public int TextureType { get; set; }
    }

    public class CharacterCustomizationTextureSectionData
    {
        public float Height { get; set; }
        public int SectionType { get; set; }
        public float Width { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}

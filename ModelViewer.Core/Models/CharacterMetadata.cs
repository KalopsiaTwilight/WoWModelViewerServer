namespace ModelViewer.Core.Models
{
    public class CharacterMetadata
    {
        public uint FileDataId { get; set; }
        public int Flags { get; set; }
        public int RaceId { get; set; }
        public int GenderId { get; set; }
        public int ChrModelId { get; set; }

        public CharacterCustomizationMetadata? CharacterCustomizationData { get; set; }
    }


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
        public List<CharacterCustomizationChoiceData> Choices { get; set; } = [];
    }

    public class CharacterCustomizationChoiceData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public List<CharacterCustomizationElementData> Elements { get; set; } = [];
    }

    public class CharacterCustomizationElementData
    {
        public int Id { get; set; }
        public uint ConditionalModelFileDataId { get; set; }
        public int RelationChoiceID { get; set; }
        public int RelationIndex { get; set; }
        public CharacterCustomizationBoneSetData? BoneSet { get; set; }
        public CharacterCustomizationGeosetData? Geoset { get; set; }
        public CharacterCustomizationMaterialData? Material { get; set; }
        public CharacterCustomizationSkinnedModelData? SkinnedModel { get; set; }
        public CharacterCustomizationtItemGeoModifyData? CustItemGeoModify { get; set; }
    }

    public class CharacterCustomizationBoneSetData
    {
        public int BoneFileDataId { get; set; }
        public int ModelFileDataId { get; set; }
    }

    public class CharacterCustomizationMaterialData
    {
        public int ChrModelTextureTargetId { get; set; }
        public List<TextureFileData> TextureFiles { get; set; } = [];
    }
    public class CharacterCustomizationGeosetData
    {
        public int GeosetType { get; set; }
        public int GeosetId { get; set; }
        public int Modifier { get; set; }
    }
    public class CharacterCustomizationSkinnedModelData
    {
        public int CollectionsFileDataId { get; set; }
        public int GeosetType { get; set; }
        public int GeosetId { get; set; }
        public int Modifier { get; set; }
        public int Flags { get; set; }
    }

    public class CharacterCustomizationtItemGeoModifyData
    {
        public int GeosetType { get; set; }
        public int Original { get; set; }
        public int Override { get; set; }
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

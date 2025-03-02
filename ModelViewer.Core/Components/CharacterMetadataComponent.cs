using ModelViewer.Core.Models;
using ModelViewer.Core.Providers;
using ModelViewer.Core.Utils;

namespace ModelViewer.Core.Components
{
    public class CharacterMetadataComponent : IComponent
    {
        private readonly IDBCDStorageProvider _dbcdStorageProvider;

        public CharacterMetadataComponent(IDBCDStorageProvider storageProvider)
        {
            _dbcdStorageProvider = storageProvider;
        }

        public CharacterMetadata? GetMetadataForCharacter(int characterModelId)
        {
            // Load DBC Data
            if (!_dbcdStorageProvider["ChrModel"].TryGetValue(characterModelId, out var chrModel))
            {
                return null;
            }
            var creatureDisplayInfo = _dbcdStorageProvider["CreatureDisplayInfo"][chrModel.Field<int>("DisplayID")];
            var creatureModelData = _dbcdStorageProvider["CreatureModelData"][creatureDisplayInfo.Field<int>("ModelID")];
            var chrRaceXChrModel = _dbcdStorageProvider["ChrRaceXChrModel"]
                .FirstOrDefault(x => x.Field<int>("ChrModelID") == chrModel.ID);
            if (chrRaceXChrModel == null)
            {
                return null;
            }
            var chrRaces = _dbcdStorageProvider["ChrRaces"][chrRaceXChrModel.Field<int>("ChrRacesID")];

            return new CharacterMetadata()
            {
                FileDataId = creatureModelData.Field<uint>("FileDataID"),
                Flags = chrModel.Field<int>("Flags"),
                RaceId = chrRaces.ID,
                GenderId = chrModel.Field<int>("Sex"),
                ChrModelId = chrModel.ID
            };
        }
    }
}

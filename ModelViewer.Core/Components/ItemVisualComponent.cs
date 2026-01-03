using ModelViewer.Core.Models;
using ModelViewer.Core.Providers;
using ModelViewer.Core.Utils;

namespace ModelViewer.Core.Components
{
    public class ItemVisualComponent : IComponent
    {
        private readonly IDBCDStorageProvider _dbcdStorageProvider;

        public ItemVisualComponent(IDBCDStorageProvider storageProvider)
        {
            _dbcdStorageProvider = storageProvider;
        }

        public ItemVisualMetadata? GetItemVisualMetadata(int visualId)
        {
            if (!_dbcdStorageProvider["ItemVisuals"].TryGetValue(visualId, out var visualInfo))
            {
                return null;
            }

            var effects = _dbcdStorageProvider["ItemVisualsXEffect"]
                .HavingColumnVal("ItemVisualsID", visualId);
            var result = new ItemVisualMetadata()
            {
                Effects = effects.Select(x =>
                {
                    return new ItemVisualEffectsData()
                    {
                        AttachmentId = x.Field<int>("AttachmentID"),
                        SpellVisualKitId = x.Field<int>("SpellVisualKitID"),
                        ModelFileDataId = x.Field<int>("AttachmentModelFileID"),
                        Scale = x.Field<float>("Scale"),
                        SubClassId = x.Field<int>("DisplayWeaponSubclassID")
                    };
                }).ToList()
            };

            return result;
        }
    }
}

using DBCD.Providers;
using ModelViewer.Core.Providers;
using WoWFileFormats.Interfaces;

namespace CharacterViewer.Core.Providers
{
    public class FileDataDBCProvider : IDBCProvider
    {
        private readonly IFileDataProvider _fileDataProvider;

        public FileDataDBCProvider(IFileDataProvider fileDataProvider)
        {
            _fileDataProvider = fileDataProvider;
        }

        public Stream StreamForTableName(string tableName, string build)
        {
            return _fileDataProvider.GetFileById(DBCTableMapper.GetFileIdForTable(tableName));
        }
    }
}

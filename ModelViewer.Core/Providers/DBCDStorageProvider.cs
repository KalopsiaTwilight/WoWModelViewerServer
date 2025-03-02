using DBCD;

namespace ModelViewer.Core.Providers
{
    public interface IDBCDStorageProvider
    {
        public IDBCDStorage GetTableByName(string name);
        public IDBCDStorage GetTableById(uint fileDataId);
        public IDBCDStorage this[string name] { get; }
    }

    public class DBCDStorageProvider : IDBCDStorageProvider
    {
        private readonly Dictionary<string, IDBCDStorage> _openedDbs = [];
        private readonly DBCD.DBCD _dbcd;

        public DBCDStorageProvider(DBCD.DBCD dbcd)
        {
            _dbcd = dbcd;
        }

        public IDBCDStorage this[string name]
        {
            get { return GetTableByName(name); }
        }

        public IDBCDStorage GetTableById(uint fileDataId)
        {
            return GetTableByName(DBCTableMapper.GetTableNameForFileId(fileDataId));
        }

        public IDBCDStorage GetTableByName(string name)
        {
            if (_openedDbs.ContainsKey(name))
            {
                return _openedDbs[name];
            }
            var storage = _dbcd.Load(name);
            _openedDbs.Add(name, storage);
            return storage;
        }
    }
}

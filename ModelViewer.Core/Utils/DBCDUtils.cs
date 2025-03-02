using DBCD;
using ModelViewer.Core.Utils;

namespace ModelViewer.Core.Utils
{
    public static class DBCDUtils
    {
        public static DBCDRow? FirstOrDefault(this IDBCDStorage storage)
        {
            return storage.ToDictionary().Select(x => x.Value).FirstOrDefault();
        }
        public static DBCDRow? FirstOrDefault(this IDBCDStorage storage, Func<DBCDRow, bool> filterFn)
        {
            return storage.ToDictionary().Select(x => x.Value).FirstOrDefault(filterFn);
        }

        public static IEnumerable<DBCDRow> Where(this IDBCDStorage storage, Func<DBCDRow, bool> filterFn)
        {
            return storage.ToDictionary().Select(x => x.Value).Where(filterFn);
        }

        public static IEnumerable<DBCDRow> HavingColumnVal<T>(this IDBCDStorage storage, string column, T val) where T : IEquatable<T>
        {
            return storage.Where((x) => x.Field<T>(column).Equals(val));
        }
    }
}

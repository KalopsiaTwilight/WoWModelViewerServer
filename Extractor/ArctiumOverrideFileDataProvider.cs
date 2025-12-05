using WoWFileFormats.Interfaces;

namespace Extractor
{
    internal class ArctiumOverrideFileDataProvider : IFileDataProvider
    {
        private readonly IFileDataProvider _fallBackProvider;
        private readonly Dictionary<uint, string> _fileOverrides;

        public ArctiumOverrideFileDataProvider(IFileDataProvider fallback, string rootDir)
        {
            _fallBackProvider = fallback;
            _fileOverrides = [];

            var mappingsDir = Path.Join(rootDir, "mappings");
            var mappingFiles = Directory.GetFiles(mappingsDir);
            foreach(var file in mappingFiles)
            {
                var lines = File.ReadAllLines(file);
                var kvPairs = lines.Select(x => {
                    var split = x.Split(';');
                    uint fileId = uint.Parse(split[0]);
                    var filePath = Path.Join(rootDir, split[1]);
                    return new KeyValuePair<uint, string>(fileId, filePath);
                });
                foreach(var kvPair in kvPairs)
                {
                    if (_fileOverrides.ContainsKey(kvPair.Key))
                    {
                        _fileOverrides[kvPair.Key] = kvPair.Value;
                    } else
                    {
                        _fileOverrides.Add(kvPair.Key, kvPair.Value);
                    }
                }
            }
        }

        public bool FileIdExists(uint fileDataId)
        {
            if (_fileOverrides.ContainsKey(fileDataId))
            {
                return true;
            }
            return _fallBackProvider.FileIdExists(fileDataId);
        }

        public Stream GetFileById(uint filedataId)
        {
            if (_fileOverrides.ContainsKey(filedataId))
            {
                return File.OpenRead(_fileOverrides[filedataId]);
            }
            return _fallBackProvider.GetFileById(filedataId);
        }
    }
}

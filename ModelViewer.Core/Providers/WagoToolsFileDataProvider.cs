using System.Text.Json;
using WoWFileFormats.Interfaces;

namespace ModelViewer.Core.Providers
{
    public class WagoToolsFileDataProvider : IFileDataProvider
    {
        private readonly HttpClient _client = new();
        private Dictionary<uint, string> _files = [];

        public bool FileIdExists(uint fileDataId)
        {
            if (_files.Count == 0)
            {
                LoadFileList();
            }
            return _files.ContainsKey(fileDataId);
        }

        public Stream GetFileById(uint filedataId)
        {
            if (!_files.ContainsKey(filedataId))
            {
                throw new Exception("Wago tools does not have file: " + filedataId);
            }

            var resp = _client.GetAsync("https://wago.tools/api/casc/" + filedataId).Result;
            resp.EnsureSuccessStatusCode();

            return resp.Content.ReadAsStream();
        }

        private void LoadFileList()
        {
            var resp = _client.GetAsync("https://wago.tools/api/files").Result;
            resp.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<Dictionary<uint, string>>(resp.Content.ReadAsStream());
            if (result == null)
            {
                throw new Exception("Error parsing file list from WagoTools");
            }
            _files = result;
        }
    }
}


using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using TACTSharp;
using WoWFileFormats.Interfaces;

namespace ModelViewer.Core.Providers
{
    public class TACTSharpFileDataProvider : IFileDataProvider
    {

        private readonly BuildInstance _buildInstance;

        public TACTSharpFileDataProvider(IConfiguration config, HttpClient httpClient)
        {
            _buildInstance = new BuildInstance();

            var productName = "wow";

            _buildInstance.Settings.Product = productName;
            _buildInstance.Settings.Locale = RootInstance.LocaleFlags.enUS;
            _buildInstance.Settings.RootMode = RootInstance.LoadMode.Full;
            _buildInstance.Settings.TryCDN = false;

            var baseFolder = config["CASC:BasePath"];
            if (string.IsNullOrEmpty(baseFolder))
            {
                throw new InvalidOperationException("No WoW path set under CASC:BasePath. Invalid configuration.");
            }

            _buildInstance.Settings.BaseDir = baseFolder;

            // Load encryption keys;
            if (!File.Exists("WoW.txt"))
            {
                var githubUrl = "https://raw.githubusercontent.com/wowdev/TACTKeys/refs/heads/master/WoW.txt";
                httpClient.GetStreamAsync(githubUrl).ContinueWith(async data =>
                {
                    var stream = await data;
                    var outStream = File.OpenWrite("WoW.txt");
                    stream.CopyTo(outStream);
                }).Wait();
            }

            var buildInfoPath = Path.Combine(baseFolder, ".build.info");
            if (!File.Exists(buildInfoPath))
                throw new InvalidOperationException("No build.info found in base directory");
            var buildInfo = new BuildInfo(buildInfoPath, _buildInstance.Settings, _buildInstance.cdn);

            var build = buildInfo.Entries.First(x => x.Product == productName);
            _buildInstance.Settings.BuildConfig = build.BuildConfig;
            _buildInstance.Settings.CDNConfig = build.CDNConfig;
            if (!string.IsNullOrEmpty(build.Armadillo))
            {
                _buildInstance.cdn.ArmadilloKeyName = build.Armadillo;
            }

            _buildInstance.cdn.OpenLocal();
            _buildInstance.LoadConfigs(_buildInstance.Settings.BuildConfig, _buildInstance.Settings.CDNConfig);
            _buildInstance.Load();
        }

        public bool FileIdExists(uint fileDataId)
        {
            return _buildInstance.Root!.FileExists(fileDataId);
        }

        public Stream GetFileById(uint filedataId)
        {
            var fileBytes = _buildInstance.OpenFileByFDID(filedataId);
            return new MemoryStream(fileBytes);
        }
    }
}


using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using TACTSharp;
using WoWFileFormats.Interfaces;

namespace ModelViewer.Core.Providers
{
    public class TACTSharpFileDataProvider : IFileDataProvider
    {

        private readonly BuildInstance _buildInstance;

        public TACTSharpFileDataProvider(IConfiguration config)
        {
            _buildInstance = new BuildInstance();

            var productName = "wow";

            _buildInstance.Settings.Product = productName;
            _buildInstance.Settings.Locale = RootInstance.LocaleFlags.enUS;
            _buildInstance.Settings.RootMode = RootInstance.LoadMode.Full;

            var baseFolder = config["CASC:BasePath"];
            if (string.IsNullOrEmpty(baseFolder))
            {
                throw new InvalidOperationException("No WoW path set under CASC:BasePath. Invalid configuration.");
            }

            var buildInfoPath = Path.Combine(baseFolder, ".build.info");
            if (!File.Exists(buildInfoPath))
                throw new InvalidOperationException("No build.info found in base directory");


            _buildInstance.Settings.BaseDir = baseFolder;
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
            var rootEntries = _buildInstance.Root!.GetEntriesByFDID(filedataId);
            if (rootEntries.Count == 0) {
                throw new FileNotFoundException();
            }

            var preferredEntry = rootEntries.FirstOrDefault(x =>
                !x.contentFlags.HasFlag(RootInstance.ContentFlags.LowViolence) &&
                (x.localeFlags.HasFlag(_buildInstance.Settings.Locale) || x.localeFlags.HasFlag(RootInstance.LocaleFlags.All_WoW)
            ));

            if (preferredEntry.fileDataID == 0)
            {
                preferredEntry = rootEntries.First();
            }
            var fileEKeys = _buildInstance.Encoding!.FindContentKey(preferredEntry.md5);
            if (!fileEKeys)
            {
                throw new Exception("EKey not found in encoding.");
            }

            var eKey = fileEKeys[0];
            var (offset, size, archiveIndex) = _buildInstance.GroupIndex!.GetIndexInfo(eKey);
            byte[] fileBytes;
            if (offset == -1)
                fileBytes = _buildInstance.cdn.GetFile("data", Convert.ToHexStringLower(eKey), 0, fileEKeys.DecodedFileSize, true);
            else
                fileBytes = _buildInstance.cdn.GetFileFromArchive(Convert.ToHexStringLower(eKey), _buildInstance.CDNConfig!.Values["archives"][archiveIndex], offset, size, fileEKeys.DecodedFileSize, true);
            return new MemoryStream(fileBytes);
        }
    }
}

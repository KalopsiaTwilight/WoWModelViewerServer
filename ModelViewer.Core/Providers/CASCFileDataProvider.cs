using CASCLib;
using Microsoft.Extensions.Configuration;
using WoWFileFormats.Interfaces;

namespace ModelViewer.Core.Providers
{
    public class CASCFileDataProvider : IFileDataProvider
    {
        private readonly CASCHandler cascHandler;

        public CASCFileDataProvider(IConfiguration config)
        {
            CASCConfig.LoadFlags = LoadFlags.Install;
            CASCConfig.ValidateData = false;
            CASCConfig.ThrowOnFileNotFound = false;
            CASCConfig.ThrowOnMissingDecryptionKey = false;
            CASCConfig.UseOnlineFallbackForMissingFiles = false;
            CASCConfig.UseWowTVFS = false;

            var basePath = config["CASC:BasePath"];

            var cascConfig = CASCConfig.LoadLocalStorageConfig(basePath, "wow");

            cascHandler = CASCHandler.OpenStorage(cascConfig);
            cascHandler.Root.SetFlags(LocaleFlags.enUS);
        }

        public bool FileIdExists(uint fileDataId)
        {
            return cascHandler.FileExists((int)fileDataId);
        }

        public Stream GetFileById(uint filedataId)
        {
            return cascHandler.OpenFile((int)filedataId);
        }
    }
}

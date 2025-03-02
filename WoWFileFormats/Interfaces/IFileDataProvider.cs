using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWFileFormats.Interfaces
{
    public interface IFileDataProvider
    {
        public bool FileIdExists(uint fileDataId);
        public Stream GetFileById(uint filedataId);
    }
}

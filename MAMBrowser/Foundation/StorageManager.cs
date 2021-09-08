using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Foundation
{
    public class StorageManager
    {
        public string Name { get; set; }
        public string UploadHost { get; set; }
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        public string Protocol { get; set; }
        public int EncodingType { get; set; }
        public IFileProtocol FileSystem { get; set; }
        public StorageManager()
        {
        }
    }
}

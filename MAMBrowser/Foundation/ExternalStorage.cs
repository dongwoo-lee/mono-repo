using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class ExternalStorage
    {
        public Dictionary<string, object> MusicConnection { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> StorageMaps { get; set; } = new Dictionary<string, object>();
    }
}

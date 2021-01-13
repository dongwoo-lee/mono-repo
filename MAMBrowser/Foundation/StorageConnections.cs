using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class StorageConnections
    {
        public Dictionary<string, object> PrivateWorkConnection { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> PublicWorkConnection { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> MirosConnection { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> MusicConnection { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> DLArchiveConnection { get; set; } = new Dictionary<string, object>();
    }
}

using M30.AudioFile.Common.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class AppSettings
    {
        public string TokenIssuer { get; set; }
        public string TokenSignature { get; set; }
        public string ConnectionString { get; set; }
        public List<DTO_NAMEVALUE> DiskScope { get; set; } = new List<DTO_NAMEVALUE>();
        public string TempDownloadPath { get; set; }
        public int SessionTimeout { get; set; }

        public string DBName { get; set; }
        public string BroadcastStartNetwork { get; set; }
        public string BroadcastEndNetwork { get; set; }
        public int ExpireMusicTokenHour { get; set; }
        public Dictionary<string, Dictionary<string, int>> MasteringPriorities { get; set; } = new Dictionary<string, Dictionary<string, int>>();
    }
}

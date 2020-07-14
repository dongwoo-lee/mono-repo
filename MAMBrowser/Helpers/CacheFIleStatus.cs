using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class CacheFIleStatus
    {
        public string SourceFullPath { get; set; }
        public int Progress { get; set; }
        public string CacheFullPath { get; set; }
    }
}

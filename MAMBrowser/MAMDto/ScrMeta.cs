using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.MAMDto
{
    public class ScrMeta : MasteringMetaBase
    {
        public string Title { get; set; }
        public string Memo { get; set; }
        public string Usage { get; set; }
        public string Advertiser { get; set; }
        public string Editor { get; set; }
        public string Media { get; set; }
        public string OnAirTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.MAMDto
{
    public class ReportMeta : MasteringMetaBase
    {
        public string Media { get; set; }
        public string OnAirTime { get; set; }
        public string Reporter { get; set; }
        public string Editor { get; set; }
        public string Memo { get; set; }
        public string ProductId { get; set; }
    }
}

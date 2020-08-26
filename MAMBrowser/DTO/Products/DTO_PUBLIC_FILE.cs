using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_PUBLIC_FILE : DTO_BASE
    {
        public long Seq { get; set; }
        public string MediaCD { get; set; }
        public string MediaName { get; set; }
        public string CategoryCD { get; set; }
        public string CatetoryName { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string FileExt { get; set; }
        public string AudioFormat { get; set; }
        public long UserExtID { get; set; }
        public string UserName { get; set; }
        public string EditedDtm { get; set; }
    }
}

using MAMBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_PRIVATE_FILE : DTO_BASE
    {
        public long Seq { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
        //public string FilePath { get; set; }
        //public string FileName { get; set; }
        public string AudioFormat { get; set; }
        public string Used { get; set; }
        public string EditorID { get; set; }
        public string EditorName { get; set; }
        public string EditDtm { get; set; }
        public string DeleteDtm { get; set; }
    }
}

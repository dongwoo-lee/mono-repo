using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_PRIVATE_FILE : DTO_BASE
    {
        public string FileID { get; set; }
        public string Catetory { get; set; }
        public string EditorID { get; set; }
        public string EditorName { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public string FileType { get; set; }
        public string SoundDetail { get; set; }
        public string Used { get; set; }
        public string EditDtm { get; set; }
    }
}

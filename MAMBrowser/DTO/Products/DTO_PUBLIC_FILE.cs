using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_PUBLIC_FILE : DTO_BASE
    {
        public string FileID { get; set; }
        public string Catetory { get; set; }        //공유소재는 기존 (구)프로 소재를 대체. 해당 카테고리를 동일하게 사용 해야 함.
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

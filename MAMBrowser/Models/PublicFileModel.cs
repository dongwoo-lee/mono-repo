using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Models
{
    public class PublicFileModel
    {
        public long Seq { get; set; }
        public string UserExtID { get; set; }
        public string Title { get; set; }
        public string MediaCD { get; set; }
        public string CatetoryID { get; set; }
        public string Memo { get; set; }
        public string AudioFormat { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        //public string Used { get; set; }      //DB에서 추가/수정 될 때 기록
        //public string EditDtm { get; set; }   //DB에서 추가/수정 될 때 기록
        //public string DeleteDtm { get; set; } //DB에서 추가/수정 될 때 기록
        
       
    }
}

using MAMBrowser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_MCR_SPOT : DTO_FILEBASE
    {
        [SortName("MEDIANAME")]
        public string MediaName { get; set; }   //매체명
        [SortName("SPOTID")]
        public string ID { get; set; }    //소재ID
        [SortName("SPOTNAME")]
        public string Name { get; set; }    //소재명
        [SortName("ONAIRDATE")]
        public string BrdDT { get; set; }   //방송일
        [SortName("STATENAME")]
        public string Status { get; set; }  //상태    
        [SortName("MILLISEC")]
        public string Duration { get; set; }  //길이 (xx:xx:xx.xx)
        public string Track { get; set; }   //트랙
        [SortName("EDITOR")]
        public string EditorID { get; set; }  //제작자ID
        [SortName("EDITORNAME")]
        public string EditorName { get; set; }   //제작자
        [SortName("EDITTIME")]
        public string EditDtm { get; set; } //편집일시
        [SortName("REQTIME")]
        public string ReqCompleteDtm { get; set; } //방송의뢰일시
    }
}

using MAMBrowser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_PGM_INFO : DTO_FILEBASE
    {
        [SortName("MEDIANAME")]
        public string MediaName { get; set; }   //매체명
        [SortName("EVENTNAME")]
        public string Name { get; set; }    //프로그램명
        [SortName("ONAIRDATE")]
        public string BrdDT { get; set; }   //방송일
        [SortName("ONAIRTIME")]
        public string BrdTime { get; set; } //방송시간
        [SortName("STATENAME")]
        public string Status { get; set; }  //상태
        [SortName("MILLISEC")]
        public string Duration { get; set; }  //길이
        [SortName("")]
        public string Track { get; set; }   //트랙명
        [SortName("EDITOR")]
        public string EditorID { get; set; } //제작자
        [SortName("EDITORNAME")]
        public string EditorName { get; set; } //제작자
        [SortName("EDITTIME")]
        public string EditDtm { get; set; } //편집일시
        [SortName("REQTIME")]
        public string ReqCompleteDtm { get; set; } //방송의뢰일시
    }
}

using MAMBrowser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_SCR_SPOT : DTO_FILEBASE
    {
        [SortName("MEDIA")]
        public string MediaCD { get; set; }
        [SortName("MEDIANAME")]
        public string MediaName { get; set; }
        [SortName("SPOTNAME")]
        public string Name { get; set; }    //소재명
        [SortName("CODENAME")]
        public string CategoryName { get; set; }    //분류명
        [SortName("MILLISEC")]
        public string Duration { get; set; }  //길이
        //[SortName("MEDIA")]
        public string Track { get; set; }   //트랙
        [SortName("ONAIRDATE")]
        public string BrdDT { get; set; }   //방송일
        [SortName("EDITOR")]
        public string EditorID { get; set; }  //제작자ID
        [SortName("EVENTNAME")]
        public string PGMName { get; set; } //사용처명
        [SortName("MASTERTIME")]
        public string MasteringDtm { get; set; }    //마스터링 일시
        [SortName("EDITORNAME")]
        public string EditorName { get; set; }    //제작자
        [SortName("EDITTIME")]
        public string EditDtm { get; set; } //편집일시
    }

}

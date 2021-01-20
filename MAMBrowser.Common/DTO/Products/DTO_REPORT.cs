using MAMBrowser.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_REPORT : DTO_FILEBASE
    {
        [SortName("REPORTNAME")]
        public string Name { get; set; }    //소재명
        [SortName("CODENAME")]
        public string CategoryName { get; set; }    //분류명
        [SortName("REPORTER")]
        public string Reporter { get; set; }    //취재인
        [SortName("PRODUCTID")]
        public string PGMID { get; set; } //사용처
        [SortName("EVENTNAME")]
        public string PGMName { get; set; } //사용처
        [SortName("ONAIRDATE")]
        public string BrdDT { get; set; }  //방송일
        [SortName("MILLISEC")]
        public string Duration { get; set; }  //길이
        public string Track { get; set; }   //트랙
        [SortName("EDITOR")]
        public string EditorID { get; set; }  //제작자ID
        [SortName("EDITORNAME")]
        public string EditorName { get; set; }    //제작자
        [SortName("EDITTIME")]
        public string EditDtm { get; set; } //편집일시
        [SortName("MASTERTIME")]
        public string MasteringDtm { get; set; }    //마스터링 일시
    }
}

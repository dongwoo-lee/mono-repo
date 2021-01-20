using MAMBrowser.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_FILLER : DTO_FILEBASE
    {
        [SortName("FILLERID")]
        public string ID { get; set; }    //소재ID
        [SortName("FILLERNAME")]
        public string Name { get; set; }    //소재명
        [SortName("ENDDATE")]
        public string BrdDT { get; set; }   //방송유효일
        [SortName("CODEID")]
        public string CategoryID { get; set; }  //분류코드
        [SortName("CODENAME")]
        public string CategoryName { get; set; }  //분류코드
        [SortName("MILLISEC")]
        public string Duration { get; set; }  //길이 (xx:xx:xx.xx)
        public string Track { get; set; }   //트랙
        [SortName("EDITOR")]
        public string EditorID { get; set; }  //제작자ID
        [SortName("EDITORNAME")]
        public string EditorName { get; set; }   //제작자
        [SortName("EDITTIME")]
        public string EditDtm { get; set; } //편집일시
        [SortName("MASTERTIME")]
        public string MasteringDtm { get; set; } //마스터링 일시
    }
}

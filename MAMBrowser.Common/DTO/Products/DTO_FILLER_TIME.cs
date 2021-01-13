using MAMBrowser.Common.Foundation;

namespace MAMBrowser.DTO
{
    public class DTO_FILLER_TIME : DTO_FILEBASE
    {
        [SortName("MEDIANAME")]
        public string MediaName { get; set; }    //매체명
        [SortName("SPOTID")]
        public string ID { get; set; }    //소재ID
        [SortName("SPOTNAME")]
        public string Name { get; set; }    //소재명
        [SortName("STARTDATE")]
        public string StartDT { get; set; }   //방송 시작일
        [SortName("ONAIRDATE")]
        public string BrdDate { get; set; }   //방송 개시일
        [SortName("ENDDATE")]
        public string EndDT { get; set; }   //방송 종료일
        [SortName("STATENAME")]
        public string Status { get; set; }   //상태
        [SortName("SPOTTYPE")]
        public string CategoryID { get; set; }  //분류코드
        [SortName("FILLERID")]
        public string CategoryName { get; set; }  //분류코드명
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
        public string MasteringDtm { get; set; }

        [SortName("SPOTCLIPID")]
        public string FileName { get; set; }//파일명이 SPOTCLIPID와 같음.
    }
}

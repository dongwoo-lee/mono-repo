namespace MAMBrowser.DTO
{
    public class DTO_FILLER_TIME : DTO_BASE
    {
        public string MediaName { get; set; }    //매체명
        public string ID { get; set; }    //소재ID
        public string Name { get; set; }    //소재명
        public string StartDT { get; set; }   //방송 시작일
        public string EndDT { get; set; }   //방송 종료일
        public string Status { get; set; }   //상태
        public string CategoryID { get; set; }  //분류코드
        public string CategoryName { get; set; }  //분류코드명
        public string Duration { get; set; }  //길이 (xx:xx:xx.xx)
        public string Track { get; set; }   //트랙
        public string EditorID { get; set; }  //제작자ID
        public string EditorName { get; set; }   //제작자
        public string EditDtm { get; set; } //편집일시
        public string MasteringDtm { get; set; }
        public string FilePath { get; set; }//파일 경로
        public string FileName { get; set; }//파일명
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_FILLER : DTO_FILEBASE
    {
        public string ID { get; set; }    //소재ID
        public string Name { get; set; }    //소재명
        public string BrdDT { get; set; }   //방송유효일
        public string CategoryID { get; set; }  //분류코드
        public string CategoryName { get; set; }  //분류코드
        public string Duration { get; set; }  //길이 (xx:xx:xx.xx)
        public string Track { get; set; }   //트랙
        public string EditorID { get; set; }  //제작자ID
        public string EditorName { get; set; }   //제작자
        public string EditDtm { get; set; } //편집일시
        public string MasteringDtm { get; set; } //마스터링 일시
    }
}

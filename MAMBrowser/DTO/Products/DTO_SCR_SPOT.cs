using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_SCR_SPOT : DTO_BASE
    {
        public string Name { get; set; }    //소재명
        public string CategoryName { get; set; }    //분류명
        public string Duration { get; set; }  //길이
        public string Track { get; set; }   //트랙
        public string BrdDT { get; set; }   //방송일
        public string EditorID { get; set; }  //제작자ID
        public string PGMName { get; set; } //사용처명
        public string MasteringDtm { get; set; }    //마스터링 일시
        public string FilePath { get; set; }    //파일경로
        public string EditorName { get; set; }    //제작자
        public string EditDtm { get; set; } //편집일시
        
    }
}

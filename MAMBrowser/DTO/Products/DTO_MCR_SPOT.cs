using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_MCR_SPOT : DTO_BASE
    {
        public string MediaName { get; set; }   //매체명
        public string ID { get; set; }    //소재ID
        public string Name { get; set; }    //소재명
        public string BrdDT { get; set; }   //방송일
        public string Status { get; set; }  //상태    
        public string Duration { get; set; }  //길이 (xx:xx:xx.xx)
        public string Track { get; set; }   //트랙
        public string EditorID { get; set; }  //제작자ID

        public string EditorName { get; set; }   //제작자
        public string EditDtm { get; set; } //편집일시
        public string ReqCompleteDtm { get; set; } //방송의뢰일시
        public string FilePath { get; set; }//파일 경로
    }
}

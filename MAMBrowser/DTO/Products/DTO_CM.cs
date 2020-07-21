using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_CM : DTO_BASE
    {
        public string BrdDT { get; set; }  //방송일
        public string ID { get; set; }          //그룹ID
        public string Name { get; set; }    //그룹명
        public string Length { get; set; }      //길이 (xx:xx)
        public string Capacity { get; set; }    //용량 (int)
        public string Status { get; set; }      //상태 (코드)
        public string EditorID { get; set; }      //담당자ID
        public string EditorName { get; set; }   //담당자명
        public string EditDtm { get; set; }   //편집일시
    }
}

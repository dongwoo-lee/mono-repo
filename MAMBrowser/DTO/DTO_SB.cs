using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_SB
    {
        public string BrdDT { get; set; }   //방송일
        public string ID { get; set; }  //SB ID
        public string Name { get; set; }    //SB 명
        public string Length { get; set; }      //길이
        public int Capacity { get; set; }    //용량
        public string Status { get; set; }  //상태
        public string PGMName { get; set; }  //사용처
        public string EditorID { get; set; }  //담당자ID
        public string EditorName { get; set; }   //담당자
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_PGM_INFO
    {
        public string MediaName { get; set; }   //매체명
        public string Name { get; set; }    //프로그램명
        public string BrdDT { get; set; }   //방송일
        public string BrdTime { get; set; } //방송시간
        public string Status { get; set; }  //상태
        public string Duration { get; set; }  //길이
        public string Track { get; set; }   //트랙명
        public string UserName { get; set; } //제작자
        public string EditDtm { get; set; } //편집일시
        public string CompleteDtm { get; set; } //방송의뢰일시
        public string FilePath { get; set; }    //파일 경로   

        public long RowNum { get; set; }   
        public long Count { get; set; }    
    }
}

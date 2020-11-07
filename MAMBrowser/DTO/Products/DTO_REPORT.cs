﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_REPORT : DTO_FILEBASE
    {
        public string Name { get; set; }    //소재명
        public string CategoryName { get; set; }    //분류명
        public string Reporter { get; set; }    //취재인
        public string PGMID { get; set; } //사용처
        public string PGMName { get; set; } //사용처
        public string BrdDT { get; set; }  //방송일
        public string Duration { get; set; }  //길이
        public string Track { get; set; }   //트랙

        public string EditorID { get; set; }  //제작자ID
        public string EditorName { get; set; }    //제작자
        public string EditDtm { get; set; } //편집일시
        public string MasteringDtm { get; set; }    //마스터링 일시
    }
}

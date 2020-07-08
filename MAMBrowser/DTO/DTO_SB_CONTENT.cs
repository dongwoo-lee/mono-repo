﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_SB_CONTENT
    {
        public string Order { get; set; }   //순서
        public string CategoryCode { get; set; }    //구분
        public string CategoryName { get; set; }    //    광고주/분류  
        public string MaterialID { get; set; }  //소재 ID
        public string MaterlalName { get; set; }    //소재명
        public string Length { get; set; }  //길이
        public string Format { get; set; }  //포맷 
    }
}

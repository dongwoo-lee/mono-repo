using System;
using System.Collections.Generic;

namespace MAMBrowser.DTO
{
    public class ProgramInfomationDTO
    {
        public string STARTDATE { get; set; }
        public char MEDIA { get; set; }
        public string PGMCODE { get; set; }
        public string PGMNAME { get; set; }
        public string STAFFS { get; set; }
        public string KEYWORD { get; set; }
        public string DESCRIPTION { get; set; }
        public List<ProductIdInfo> ProductIdInfos { get; set; }
    }

    public class ProductIdInfo
    {
        public string PRODUCTID { get; set; }
        public string EVENTNAME { get; set; }
        public char LIVEFLAG { get; set; }
        public DateTime ONAIRTIME { get; set; }
        public int CUEID { get; set; }
        public string TDNAME { get; set; }
        public string STUDIOID { get; set; }
    }
    public class ProgramInfoParamDTO
    {
        public string brddate { get; set; }
        public string media { get; set; }
        public string pgmcode { get; set; }
    }


}

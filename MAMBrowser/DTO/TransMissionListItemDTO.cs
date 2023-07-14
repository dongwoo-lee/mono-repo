﻿using System;

namespace MAMBrowser.DTO
{
    public class TransMissionListItemDTO
    {
        public int ROWNO { get; set; }
        public string MAINMACHINE { get; set; }
        public int SEQNUM { get; set; }
        public string PRODUCTID { get; set; }
        public char PRODUCTTYPE { get; set; }
        public string SOURCEID { get; set; }
        public DateTime ONAIRTIME { get; set; }
        public int DURATION { get; set; }
        public string EVENTNAME { get; set; }
        //기술감독 필요
        public string DLFILEPATH_1 { get; set; }
        public string DLFILETOKEN_1 { get; set; }
        public string DLFILEPATH_2 { get; set; }
        public string DLFILETOKEN_2 { get; set; }
        public string PGMFILEPATH { get; set; }
        public string PGMFILETOKEN { get; set; }
        public int CUEID { get; set; }
    }
    public class TransMissionListParamDTO : PageParamDTO
    {
        public string brddate { get; set; }
        public char media { get; set; }
        public string producttype { get; set; }
    }

}

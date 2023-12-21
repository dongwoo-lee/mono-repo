using System;

namespace MAMBrowser.DTO
{
    public class TransMissionListItemDTO
    {
        public int ROWNO { get; set; }
        public string MAINMACHINE { get; set; }
        public string STUDIONAME { get; set; }
        public string TDNAME { get; set; }
        public int SEQNUM { get; set; }
        public string PRODUCTID { get; set; }
        public string PGMCODE { get; set; }
        public char PRODUCTTYPE { get; set; }
        public char EVENTMODF { get; set; }
        public string SOURCEID { get; set; }
        public DateTime ONAIRTIME { get; set; }
        public int DURATION { get; set; }
        public string EVENTNAME { get; set; }
        public string SEQ_MAIN { get; set; }
        public string SEQ_SUB { get; set; }
        public string PGMFILEPATH { get; set; }
        public string PGMFILETOKEN { get; set; }
        public int CUEID { get; set; }
        public bool ISHISTORY { get; set; }
    }
    public class TransMissionListParamDTO : PageParamDTO
    {
        public string brddate { get; set; }
        public char media { get; set; }
        public string producttype { get; set; }
    }

}

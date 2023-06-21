using System;

namespace MAMBrowser.DTO
{
    public class TransMissionListItemDTO
    {
        public string MAINMACHINE { get; set; }
        public int SEQNUM { get; set; }
        public string PRODUCTID { get; set; }
        public string SOURCEID { get; set; }
        public DateTime ONAIRTIME { get; set; }
        public int DURATION { get; set; }
        public string EVENTNAME { get; set; }
        //기술감독 필요
        public string DLFILEPATH { get; set; }
        public string PGMFILEPATH { get; set; }
        public int CUEID { get; set; }
    }
    public class TransMissionListParamDTO : PageParamDTO
    {
        public string brddate { get; set; }
        public char media { get; set; }
    }

}

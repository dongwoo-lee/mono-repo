using System;

namespace MAMBrowser.DTO
{
    public class PlaylistPerBrdProgramDTO
    {
        public int ROWNO { get; set; }
        public int SEQ { get; set; }
        public string SCHDATE { get; set; }
        public string AUDIOCLIPID { get; set; }
        public string MUSICID { get; set; }
        public string ENCODEDATE { get; set; }
        public string AUDIOFILETYPE { get; set; }
        public string PRODUCTID { get; set; }
        public string PRODUCTNAME { get; set; }
        public string PGMCODE { get; set; }
        public string PGMNAME { get; set; }
        public DateTime STARTDTM { get; set; }
        public DateTime ENDDTM { get; set; }
        public string STUDIONAME { get; set; }
        public string SLAPNAME { get; set; }
        public string BRDCTYPE { get; set; }
        //public string MAINTITLE { get; set; }
        //public string SUBTITLE { get; set; }
        public string SONGNAME { get; set; }
        public string ARTIST { get; set; }
        public string ALBUMNAME { get; set; }
        public string PRODUCTYEAR { get; set; }
        public int PLAYTIME { get; set; }
        public int TOTALTIME { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public DateTime REGDTM { get; set; }

    }
    public class PlaylistStatisticsDTO
    {
        public int RANK { get; set; }
        //public int SEQ { get; set; }
        public string AUDIOCLIPID { get; set; }
        public string MUSICID { get; set; }
        //public string PERIOD { get; set; }
        //public string MEDIA { get; set; }
        //public string PERSONID { get; set; }
        //public string PERSONNAME { get; set; }
        public string SONGNAME { get; set; }
        public string ARTIST { get; set; }
        public string ALBUMNAME { get; set; }
        //public string PGMNAME { get; set; }
        public int PLAYCNT { get; set; }
        //public int PLAYTIME { get; set; }
        //public int TOTALTIME { get; set; }
        public string SUMMARYDATE { get; set; }
        public string FilePath { get; set; }
        public string FileToken { get; set; }
        public bool ExistFile { get; set; }
    }
    public class PlaylistPerBrdProgramParamDTO : PageParamDTO
    {
        public string brddate { get; set; }
        public string productid { get; set; }
        public string enddate { get; set; }
        public string userid { get; set; }
        public string audioclipid { get; set; }
        public string period { get; set; }
    }
    public class PlaylistStatisticsParamDTO : PageParamDTO
    {
        public string enddate { get; set; }
        public string personid { get; set; }
        public string period { get; set; }
        public string media { get; set; }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class CueSheetCollectionDTO
    {
        //public CueSheetCollectionDTO pram { get; set; }
        //큐시트 정보
        public CueSheetDTO CueSheetDTO { get; set; }
        //기본큐시트 정보
        public List<CueSheetDTO> DefCueSheetDTO { get; set; }
        //프린트 정보
        public List<PrintDTO> PrintDTO { get; set; }
        //AB
        public List<CueSheetConDTO> NormalCon { get; set; }
        //C
        public IDictionary<string, List<CueSheetConDTO>> InstanceCon { get; set; }

        //기본큐시트 기존요일 제거 
        public int[] delParams { get; set; }


        //첨부파일 정보
        //public List<AttachmentDTO> AttachmentDTO { get; set; }
        //태그 정보
        //public List<string> TagDTO { get; set; }
    }

    public class CueSheetDTO
    {
        public string CUETYPE { get; set; }
        public List<ViewDetail> DETAIL { get; set; }
        public string PRODUCTID { get; set; }
        public string PGMCODE { get; set; }
        public string MEDIA { get; set; }
        public string PERSONID { get; set; }
        public string TITLE { get; set; }
        public string DIRECTORNAME { get; set; }
        public string DJNAME { get; set; }
        public DateTime EDITTIME { get; set; }
        public string FOOTERTITLE { get; set; }
        public string HEADERTITLE { get; set; }
        public string MEMBERNAME { get; set; }
        public string MEMO { get; set; }
        public string BRDDATE { get; set; }
        public DateTime BRDTIME { get; set; }

        //일일큐시트
        public int SEQNUM { get; set; }
        public char LIVEFLAG { get; set; }
        public string STARTDATE { get; set; }
        public string ONAIRDAY { get; set; }

        //기본큐시트
        //public List<string> CheckedWeekList { get; set; }

        //템플릿
        public DateTime CREATETIME { get; set; }

    }

    public class CueSheetConDTO
    {
        public int ROWNUM { get; set; }
        public string CARTCODE { get; set; }
        public string CHANNELTYPE { get; set; }
        public string CARTID { get; set; }
        public string ONAIRDATE { get; set; }
        public int DURATION { get; set; }
        public int STARTPOSITION { get; set; }
        public int ENDPOSITION { get; set; }
        public int FADEINTIME { get; set; }
        public int FADEOUTTIME { get; set; }
        public string MAINTITLE { get; set; }
        public string SUBTITLE { get; set; }
        public string MEMO { get; set; }
        public string TRANSTYPE { get; set; }
        public string USEFLAG { get; set; }
        //public List<string> FILEPATH { get; set; } = new List<string>();
        //public List<string> FILETOKEN { get; set; } = new List<string>();
        public string FILEPATH { get; set; }
        public string FILETOKEN { get; set; }
        public string CARTTYPE { get; set; }

        // C
        public bool EDITTARGET { get; set; }

        // Fav
        public string PERSONID { get; set; }
    }


    //public class AttachmentDTO
    //{
    //    public string FILEPATH { get; set; }
    //    public int FILESIZE { get; set; }
    //}

    public class PrintDTO
    {
        public int ROWNUM { get; set; }
        public string CODE { get; set; }
        public string CONTENTS { get; set; }
        public string ETC { get; set; }
        public int USEDTIME { get; set; }
    }

    public class ViewDetail
    {
        public int CUEID { get; set; }
        public string WEEK { get; set; }
    }

    //public class ViewInstance
    //{
    //    public List<CueSheetConDTO> channel_1 { get; set; }
    //    public List<CueSheetConDTO> channel_2 { get; set; }
    //    public List<CueSheetConDTO> channel_3 { get; set; }
    //    public List<CueSheetConDTO> channel_4 { get; set; }
    //}

    //public enum InstaceType
    //{

    //    channel_1,
    //    channel_2,
    //    channel_3,
    //    channel_4
    //}
}

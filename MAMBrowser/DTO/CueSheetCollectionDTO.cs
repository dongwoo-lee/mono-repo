using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class CueSheetCollectionDTO
    {
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
        public List<AttachmentDTO> Attachments { get; set; }
        //태그 정보
        public List<string> Tags { get; set; }
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
        public bool FADEINTIME { get; set; }
        public bool FADEOUTTIME { get; set; }
        public string MAINTITLE { get; set; }
        public string SUBTITLE { get; set; }
        public string MEMO { get; set; }
        public string TRANSTYPE { get; set; }
        public string USEFLAG { get; set; }
        public string FILEPATH { get; set; }
        public string FILETOKEN { get; set; }
        public string CARTTYPE { get; set; }
        public string PGMCODE { get; set; }
        public bool ExistFile { get; set; }
        // C
        public bool EDITTARGET { get; set; }

        // Fav
        public string PERSONID { get; set; }

        public List<CueSheetConAudioDTO> AUDIOS { get; set; }
    }

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
        public DateTime ONAIRTIME { get; set; }
    }

    public class CueSheetConAudioDTO
    {
        public string P_TYPE { get; set; }
        public int P_SEQNUM { get; set; }
        public string P_CLIPID { get; set; }
        public string P_MAINTITLE { get; set; }
        public string P_SUBTITLE { get; set; }
        public int P_DURATION { get; set; }
        public string P_MASTERFILE { get; set; }
        public string P_ENERGYFILE { get; set; }
        public string P_OWNER { get; set; }
        public string P_CODEID { get; set; }
        public int P_LENGTH { get; set; }
    }

    public class AttachmentDTO
    {
        public long FILEID { get; set; }
        public string FILEPATH { get; set; }
        public string FILENAME { get; set; }
        public int FILESIZE { get; set; }
        public bool DELSTATE { get; set; }
    }

    public class DelFilePath
    {
        public int F_SEQNUM { get; set; } // 있으면 storage 저장, 없으면 신규
    }

    public class Pram
    {
        public string userid { get; set; }
        public string guid { get; set; }
        public string downloadName { get; set; }
    }
}

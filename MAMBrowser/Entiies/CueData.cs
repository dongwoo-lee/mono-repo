using DAP3.CueSheetCommon.DTO.Param;
using System;
using System.Collections.Generic;

namespace MAMBrowser.Entiies
{
    public class CueData
    {
        public CueSheetParamDTO cueParam { get; set; }
        public DayCueSheetParamDTO dayParam { get; set; }
        public IEnumerable<CueSheetConParamDTO> conParams { get; set; }
        public IEnumerable<string> tagParams { get; set; }
        public IEnumerable<PrintParamDTO> printParams { get; set; }
        public IEnumerable<AttachmentParamDTO> attParams { get; set; }
        public int[] delParams { get; set; }

        //기본큐시트
        public IEnumerable<string> defParams { get; set; }

        //템플릿
        public TemplateParamDTO temParam { get; set; }

        //즐겨찾기
        public IEnumerable<FavConParamDTO> favConParam { get; set; }
    }

    public class ViewCueSheetCollection
    {
        public List<ViewCueSheetConDTO> abCartCon { get; set; }
        public List<List<ViewCueSheetConDTO>> cCartCon { get; set; }
    }

    public class ViewCueSheetConDTO 
    {
        public int RowNum { get; set; }
        public string CartCode { get; set; }
        public string ChannelType { get; set; }
        public string CartId { get; set; }
        public string OnairDate { get; set; }
        public int Duration { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
        public int FadeInTime { get; set; }
        public int FadeOutTime { get; set; }
        public string MainTitle { get; set; }
        public string SubTitle { get; set; }
        public string Memo { get; set; }
        public string TransType { get; set; }
        public string UseFlag { get; set; }
        public List<string> FilePath_test { get; set; } = new List<string>();
        public List<string> FileToken { get; set; } = new List<string>();
        public string FilePath { get; set; }

        public bool Editting { get; set; }
    }

   

}

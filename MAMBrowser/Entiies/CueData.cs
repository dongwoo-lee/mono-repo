using DAP3.CueSheetCommon.DTO.Param;
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
        
        //기본큐시트
        public IEnumerable<string> defParams { get; set; }

        //템플릿
        public TemplateParamDTO temParam { get; set; }

        //즐겨찾기
        public IEnumerable<FavConParamDTO> favConParam { get; set; }
    }
}

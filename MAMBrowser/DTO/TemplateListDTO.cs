using M30.AudioFile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class TemplateListDTO
    {
        public int CUEID { get; set; }
        public string CUETYPE { get; set; }
        public string PERSONID { get; set; }
        public DateTime EDITTIME { get; set; }
        public DateTime CREATETIME { get; set; }
        public string TMPTITLE { get; set; }
    }
    public class TempCueList_Page
    {
        public IEnumerable<TemplateListDTO> Data { get; set; }
        public int RowPerPage { get; set; }     //페이지당 행 수
        public int SelectPage { get; set; }     //현재 페이지
        public long TotalRowCount { get; set; } = 0;    //전체 행 수
    }
    public class TempCueList_Result
    {
        public TempCueList_Page ResultObject { get; set; }
        public RESUlT_CODES ResultCode { get; set; }
        public string ErrorMsg { get; set; }
        public string Token { get; set; }
    }
}

using MAMBrowser.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DefCueSheetListDTO
    {
        public string PRODUCTID { get; set; }
        public string EVENTNAME { get; set; }
        public string SERVICENAME { get; set; }
        public string CODENAME { get; set; }
        public string MEDIA { get; set; }
        public string CUETYPE { get; set; }
        public DateTime EDITTIME { get; set; }
        public string PGMCODE { get; set; }
        public List<DefCueSheetListDetailDTO> DETAIL { get; set; }
    }

    public class DefCueSheetListDetailDTO
    {
        public int CUEID { get; set; }
        public string WEEK { get; set; }
    }
    public class DefCueList_Page
    {
        public IEnumerable<DefCueSheetListDTO> Data { get; set; }
        public int RowPerPage { get; set; }     //페이지당 행 수
        public int SelectPage { get; set; }     //현재 페이지
        public long TotalRowCount { get; set; } = 0;    //전체 행 수
    }
    public class DefCueList_Result
    {
        public DefCueList_Page ResultObject { get; set; }
        public RESUlT_CODES ResultCode { get; set; }
        public string ErrorMsg { get; set; }
        public string Token { get; set; }
    }
}

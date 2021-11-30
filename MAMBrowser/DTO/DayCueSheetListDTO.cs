using MAMBrowser.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DayCueSheetListDTO
    {
        public string PGMCODE { get; set; }
        public string SERVICENAME { get; set; }
        public string CODENAME { get; set; }
        public string ONAIRDAY { get; set; }
        public string STARTDATE { get; set; }
        public string PRODUCTID { get; set; }
        public string EVENTNAME { get; set; }
        public string DAY { get; set; }
        public DateTime R_ONAIRTIME { get; set; }
        public int CUEID { get; set; }
        public char MEDIA { get; set; }
        public string EDIT { get; set; }
        public int SEQNUM { get; set; }
        public char LIVEFLAG { get; set; }

    }
    public class DayCueList_Page
    {
        public IEnumerable<DayCueSheetListDTO> Data { get; set; }
        public int RowPerPage { get; set; }     //페이지당 행 수
        public int SelectPage { get; set; }     //현재 페이지
        public long TotalRowCount { get; set; } = 0;    //전체 행 수
    }
    public class DayCueList_Result
    {
        public DayCueList_Page ResultObject { get; set; }
        public RESUlT_CODES ResultCode { get; set; }
        public string ErrorMsg { get; set; }
        public string Token { get; set; }
    }
}

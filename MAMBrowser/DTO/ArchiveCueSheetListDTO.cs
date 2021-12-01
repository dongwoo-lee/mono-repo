using M30.AudioFile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class ArchiveCueSheetListDTO
    {
        public int CUEID { get; set; }
        public string PRODUCTID { get; set; }
        public string TITLE { get; set; }
        public char MEDIA { get; set; }
        public string BRDDATE { get; set; }
        public DateTime BRDTIME { get; set; }
        public string CUETYPE { get; set; }

    }
    public class ArchiveCueList_Page
    {
        public IEnumerable<ArchiveCueSheetListDTO> Data { get; set; }
        public int RowPerPage { get; set; }     //페이지당 행 수
        public int SelectPage { get; set; }     //현재 페이지
        public long TotalRowCount { get; set; } = 0;    //전체 행 수
    }
    public class ArchiveCueList_Result
    {
        public ArchiveCueList_Page ResultObject { get; set; }
        public RESUlT_CODES ResultCode { get; set; }
        public string ErrorMsg { get; set; }
        public string Token { get; set; }
    }
}

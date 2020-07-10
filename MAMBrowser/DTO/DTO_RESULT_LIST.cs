using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_RESULT_LIST
    {
        public string StrDataList { get; set; }
        public int RowPerPage { get; set; }     //페이지당 행수
        public int SelectPage { get; set; }     //현재 페이지
        public int TotalPageCount { get; set; }     //전체 페이지 수
    }
}

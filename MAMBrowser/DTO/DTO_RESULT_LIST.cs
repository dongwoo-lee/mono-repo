using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_RESULT_LIST<T>
    {
        public IList<T> DataList { get; set; }
        public int RowPerPage { get; set; }     //페이지당 행 수
        public int SelectPage { get; set; }     //현재 페이지
        public long TotalRowCount { get; set; }     //전체 행 수
    }
}

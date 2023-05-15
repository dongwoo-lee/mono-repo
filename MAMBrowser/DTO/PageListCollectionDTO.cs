using System.Collections.Generic;

namespace MAMBrowser.DTO
{
    public class PageListCollectionDTO<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int RowPerPage { get; set; }     //페이지당 행 수
        public int SelectPage { get; set; }     //현재 페이지
        public long TotalRowCount { get; set; } = 0;    //전체 행 수
    }
    public class PageParamDTO
    {
        public int RowPerPage { get; set; }     //페이지당 행 수
        public int SelectPage { get; set; }     //현재 페이지
        public long TotalRowCount { get; set; } = 0;    //전체 행 수
    }
}

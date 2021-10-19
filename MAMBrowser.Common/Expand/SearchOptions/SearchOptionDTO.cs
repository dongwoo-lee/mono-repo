using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.Common.Expand.SearchOptions
{
    public abstract class SearchOptionDTO 
    {
        public int RowPerPage = 30;
        public int SelectPage = 1;
        public string SortKey;
        public string SortValue;
    }
}

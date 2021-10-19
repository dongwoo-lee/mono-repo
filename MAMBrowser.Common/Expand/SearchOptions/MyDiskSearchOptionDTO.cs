using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.Common.Expand.SearchOptions
{
    public class MyDiskSearchOptionDTO : SearchOptionDTO
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
    }
}

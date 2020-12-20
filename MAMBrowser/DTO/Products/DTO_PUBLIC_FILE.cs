using MAMBrowser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_PUBLIC_FILE : DTO_FILEBASE
    {
        public long Seq { get; set; }
        [SortName("mediaCD")]
        public string MediaCD { get; set; }
        [SortName("mediaName")]
        public string MediaName { get; set; }
        [SortName("categoryCD")]
        public string CategoryCD { get; set; }
        [SortName("categoryName")]
        public string CategoryName { get; set; }
        [SortName("title")]
        public string Title { get; set; }
        [SortName("memo")]
        public string Memo { get; set; }
        [SortName("fileSize")]
        public long FileSize { get; set; }
        [SortName("fileExt")]
        public string FileExt { get; set; }
        [SortName("audioFormat")]
        public string AudioFormat { get; set; }
        [SortName("userId")]
        public string UserId { get; set; }
        [SortName("userName")]
        public string UserName { get; set; }
        [SortName("editedDtm")]
        public string EditedDtm { get; set; }
    }
}

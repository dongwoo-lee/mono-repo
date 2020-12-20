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
        [SortName("MEDIA_CD")]
        public string MediaCD { get; set; }
        [SortName("MEDIA_NAME")]
        public string MediaName { get; set; }
        [SortName("CATE_CD")]
        public string CategoryCD { get; set; }
        [SortName("CATE_NAME")]
        public string CategoryName { get; set; }
        [SortName("TITLE")]
        public string Title { get; set; }
        [SortName("MEMO")]
        public string Memo { get; set; }
        [SortName("FILE_SIZE")]
        public long FileSize { get; set; }
        //[SortName("fileExt")]
        public string FileExt { get; set; }
        [SortName("AUDIO_FORMAT")]
        public string AudioFormat { get; set; }
        [SortName("USER_ID")]
        public string UserId { get; set; }
        [SortName("USER_NAME")]
        public string UserName { get; set; }
        [SortName("EDITED_DTM")]
        public string EditedDtm { get; set; }
    }
}

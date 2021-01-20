using MAMBrowser.Common;
using MAMBrowser.Common.Foundation;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static Func<dynamic, DTO_PUBLIC_FILE> ResultMapping()
        {
            return new Func<dynamic, DTO_PUBLIC_FILE>((row) =>
            {
                return new DTO_PUBLIC_FILE
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Seq = Convert.ToInt64(row.SEQ),
                    MediaCD = row.MEDIA_CD,
                    MediaName = row.MEDIA_NAME,
                    CategoryCD = row.CATE_CD,
                    CategoryName = row.CATE_NAME,
                    UserId = row.USER_ID,
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Define.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    FileExt = Path.GetExtension(row.FILE_PATH)
                };
            });
        }
      
    }
}

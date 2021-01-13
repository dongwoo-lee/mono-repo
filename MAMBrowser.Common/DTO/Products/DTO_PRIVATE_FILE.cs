using MAMBrowser.Common;
using MAMBrowser.Common.Foundation;
using System;
using System.IO;

namespace MAMBrowser.DTO
{
    public class DTO_PRIVATE_FILE : DTO_FILEBASE
    {
        public long Seq { get; set; }
        [SortName("TITLE")]
        public string Title { get; set; }
        [SortName("MEMO")]
        public string Memo { get; set; }
        [SortName("AUDIO_FORMAT")]
        public string AudioFormat { get; set; }
        [SortName("USED")]
        public string Used { get; set; }
        [SortName("USER_ID")]
        public string UserId { get; set; }
        [SortName("USER_NAME")]
        public string UserName { get; set; }
        [SortName("FILE_SIZE")]
        public long FileSize { get; set; }
        //[SortName("FileExt")]
        public string FileExt { get; set; }
        [SortName("EDITED_DTM")]
        public string EditedDtm { get; set; }
        [SortName("DELETED_DTM")]
        public string DeletedDtm { get; set; }

        public static Func<dynamic, DTO_PRIVATE_FILE> ResultMapping()
        {
            return new Func<dynamic, DTO_PRIVATE_FILE>((row) =>
            {
                return new DTO_PRIVATE_FILE
                {
                    Seq = Convert.ToInt64(row.SEQ),
                    UserId = row.USER_ID,
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Define.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    DeletedDtm = row.DELETED_DTM == null ? "" : ((DateTime)row.DELETED_DTM).ToString(Define.DTM19),
                    Used = row.USED,
                    FileExt = Path.GetExtension(row.FILE_PATH)
                };
            });
        }
    }
}

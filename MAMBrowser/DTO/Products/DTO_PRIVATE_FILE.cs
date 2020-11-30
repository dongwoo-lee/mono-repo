using MAMBrowser.Helpers;
using System;
using System.IO;

namespace MAMBrowser.DTO
{
    public class DTO_PRIVATE_FILE : DTO_FILEBASE
    {
        public long Seq { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
        public string AudioFormat { get; set; }
        public string Used { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public long FileSize { get; set; }
        public string FileExt { get; set; }
        public string EditedDtm { get; set; }
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
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(MAMUtility.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    DeletedDtm = row.DELETED_DTM == null ? "" : ((DateTime)row.DELETED_DTM).ToString(MAMUtility.DTM19),
                    Used = row.USED,
                    FileExt = Path.GetExtension(row.FILE_PATH)
                };
            });
        }
    }
}

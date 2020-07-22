using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Models
{
    public class PublicFileModel
    {
        public string Media { get; set; }
        public string FileID { get; set; }
        public string CatetoryID { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
        //public string FilePath { get; set; }
        //public string FileName { get; set; }
        public string AudioFormat { get; set; }
        public string Used { get; set; }
        public string EditorID { get; set; }
        //public string EditDtm { get; set; }   //DB에서 추가/수정 될 때 기록

        public PublicFileModel()
        {

        }
        public PublicFileModel(DTO_PUBLIC_FILE dto)
        {
            FileID = dto.FileID;
            CatetoryID = dto.CatetoryID;
            Title = dto.Title;
            Memo = dto.Memo;
            AudioFormat = dto.AudioFormat;
            Used = dto.Used;
            EditorID = dto.EditorID;
        }
    }
}

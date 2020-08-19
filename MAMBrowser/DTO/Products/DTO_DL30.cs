using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_DL30 : DTO_BASE
    {
        //DAMS 처리 여부
        // 주/예비 어떤단말인지?
        public long Seq { get; set; }
        public string MediaCD { get; set; }
        public string MediaName { get; set; }
        public string ProgramCD { get; set; }
        public string RecName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DeviceName { get; set; }    //DL #1
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string RegDtm { get; set; }    //등록일시
    }
}

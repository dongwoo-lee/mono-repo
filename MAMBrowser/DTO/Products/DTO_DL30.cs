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
        public long MediaID { get; set; }
        public long MediaName { get; set; }
        public long ProgramID { get; set; }
        public long RecName { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public long DeviceName { get; set; }    //DL #1
        public long FilePath { get; set; }
        public long FileSize { get; set; }
        public long RegDtm { get; set; }    //등록일시
    }
}

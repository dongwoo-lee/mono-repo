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
        public long DeviceSeq { get; set; }
        public string DeviceName { get; set; }
        public string MediaCD { get; set; }
        public string MediaName { get; set; }
        /// <summary>
        /// 송출표에서 편성일
        /// </summary>
        public string SchDate { get; set; }
        /// <summary>
        /// 송출표에서 실제 방송일시
        /// </summary>
        public string BrdDate { get; set; }
        public string ProgramID { get; set; }
        public string SourceID { get; set; }
        public string RecName { get; set; }
        public int Duration { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string RegDtm { get; set; }    //등록일시
    }
}

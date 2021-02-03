using MAMBrowser.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_DL30 : DTO_FILEBASE
    {
        //DAMS 처리 여부
        // 주/예비 어떤단말인지?
        public long Seq { get; set; }
        [SortName("DEVICE_SEQ")]
        public long DeviceSeq { get; set; }
        [SortName("DEVICE_NAME")]
        public string DeviceName { get; set; }
        [SortName("MEDIA_CD")]
        public string MediaCD { get; set; }
        [SortName("MEDIA_CD")]
        public string MediaName { get; set; }
        /// <summary>
        /// 송출표에서 편성일
        /// </summary>
        [SortName("SCH_DATE")]
        public string SchDate { get; set; }
        /// <summary>
        /// 송출표에서 실제 방송일시
        /// </summary>
        [SortName("BRD_DTM")]
        public string BrdDate { get; set; }
        [SortName("PRODUCT_ID")]
        public string ProgramID { get; set; }
        [SortName("SOURCE_ID")]
        public string SourceID { get; set; }
        [SortName("REC_NAME")]
        public string RecName { get; set; }
        [SortName("LENGTH")]
        public string Duration { get; set; }
        [SortName("FILE_SIZE")]
        public long FileSize { get; set; }
        [SortName("REG_DTM")]
        public string RegDtm { get; set; }    //등록일시

    }
}

using M30.AudioFile.Common;
using static MAMBrowser.Foundation.MAMDefine;

namespace MAMBrowser.MAMDto
{
     public class MasteringMetaBase
    {
        public SoundDataTypes SoundType { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 등록일시 : ex)yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string RegDtm { get; set; }
        public string FilePath { get; set; }
    }
}

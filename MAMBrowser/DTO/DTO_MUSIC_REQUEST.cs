using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_MUSIC_REQUEST
    {
        /// <summary>
        /// Seed 암호화된 경로
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 만료 일자.
        /// </summary>
        public string Expire { get; set; }
        public string Ip { get; set; }
    }
}

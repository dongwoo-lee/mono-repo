using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.MAMDto
{
    public class ProgramMeta : MasteringMetaBase
    {
        public string Memo { get; set; }
        public string Media { get; set; }
        public string ProductId { get; set; }
        public string BrdDTM { get; set; }
        public string Editor{ get; set; }

    }
}

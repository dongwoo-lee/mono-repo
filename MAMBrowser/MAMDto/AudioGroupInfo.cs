using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.MAMDto
{
    public class AudioGroupInfo
    {
        public List<AudioGroupData> GroupData { get; set; } = new List<AudioGroupData>();
        public string FileName { get; set; }
    }
}

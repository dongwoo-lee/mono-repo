using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAMBrowser.Models
{
    public class M30_COMM_USER_EXT
    {
        [JsonPropertyName("UserExtID")]
        public long USER_EXT_ID { get; set; }
        [JsonPropertyName("ID")]
        public string USER_ID { get; set; }
        [JsonPropertyName("DiskMax")]
        public int DISK_MAX { get; set; }
        [JsonPropertyName("DiskUsed")]
        public long DISK_USED { get; set; }
        [JsonPropertyName("MenuGrpID")]
        public string MENU_GRP_CD { get; set; }
        [JsonPropertyName("Used")]
        public string USED { get; set; }
    }
}

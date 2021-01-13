using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAMBrowser.Models
{
    public class M30_COMM_ROLE_EXT
    {
        [JsonPropertyName("ID")]
        public string ROLE_ID { get; set; }
        [JsonPropertyName("AuthorCode")]
        public string AUTHOR_CD { get; set; }
    }
}

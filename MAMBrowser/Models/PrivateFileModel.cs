using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAMBrowser.Models
{
    public class PrivateFileModel
    {
        [JsonPropertyName("seq")]
        public long Seq { get; set; }
        [JsonPropertyName("userExtID")]
        public long UserExtID { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        [JsonPropertyName("audioFormat")]
        public string AudioFormat { get; set; }
        [JsonPropertyName("fileSize")]
        public long FileSize { get; set; }
        [JsonPropertyName("filePath")]
        public string FilePath { get; set; }

    }
}

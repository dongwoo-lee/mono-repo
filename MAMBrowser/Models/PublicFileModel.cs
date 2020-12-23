using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAMBrowser.Models
{
    public class PublicFileModel
    {
        [JsonPropertyName("seq")]
        public long SEQ { get; set; }
        [JsonPropertyName("userId")]
        public string USER_ID { get; set; }
        [JsonPropertyName("title")]
        public string TITLE { get; set; }
        [JsonPropertyName("mediaCD")]
        public string MEDIA_CD { get; set; }
        [JsonPropertyName("categoryCD")]
        public string CATE_CD { get; set; }
        [JsonPropertyName("memo")]
        public string MEMO { get; set; }
        [JsonPropertyName("audioFormat")]
        public string AUDIO_FORMAT { get; set; }
        [JsonPropertyName("storageHost")]
        public string STORAGE_HOST { get; set; }
        [JsonPropertyName("fileSize")]
        public long FILE_SIZE { get; set; }
        [JsonPropertyName("filePath")]
        public string FILE_PATH { get; set; }
    }
}

using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAMBrowser.Models
{
    public class PrivateFileModel : IValidatableObject
    {
        [JsonPropertyName("seq")]
        public long SEQ { get; set; }
        [JsonPropertyName("userExtID")]
        public long USER_EXT_ID { get; set; }
        //[Remote(action: "VerifyModel", controller: "PrivateFileController")]
        //[Required]
        [JsonPropertyName("title")]
        public string TITLE { get; set; }
        [JsonPropertyName("memo")]
        public string MEMO { get; set; }
        [JsonPropertyName("audioFormat")]
        public string AUDIO_FORMAT { get; set; }
        [JsonPropertyName("fileSize")]
        public long FILE_SIZE { get; set; }
        [JsonPropertyName("filePath")]
        public string FILE_PATH { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> resultList = new List<ValidationResult>();
            if (string.IsNullOrEmpty(TITLE))
            {
                yield return new ValidationResult(
                    $" invaliate data",
                    new[] { nameof(TITLE) });
            }
            //return resultList;
            //return null;
        }
    }
}

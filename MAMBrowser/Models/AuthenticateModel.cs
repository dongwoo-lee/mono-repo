using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAMBrowser.Models
{
    public class AuthenticateModel
    {
        [Required]
        [JsonPropertyName("UserID")]
        public string PERSONID { get; set; }

        [Required]
        [JsonPropertyName("Pass")]
        public string PASSWD { get; set; }
    }
}

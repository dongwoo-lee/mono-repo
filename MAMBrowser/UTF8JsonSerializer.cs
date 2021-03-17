using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MAMBrowser
{
    public static class UTF8JsonSerializer
    {
        public static string Serialize<T>(T value)
        {
            var options = new JsonSerializerOptions();
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            return JsonSerializer.Serialize<T>(value, options);
        }
    }
}

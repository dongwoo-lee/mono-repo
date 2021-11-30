using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class ExternalStorage
    {
        //public Dictionary<string, object> MusicConnection { get; set; } = new Dictionary<string, object>();
        //public Dictionary<string, object> StorageMaps { get; set; } = new Dictionary<string, object>();

        public string MbcDomain { get; set; }
        public string AuthorKey { get; set; }
        public string SearchDomain { get; set; }
        public string SearchSongUrl { get; set; }
        public string SearchEffectUrl { get; set; }
        public string SearchLyricsUrl { get; set; }
        public string ImageListUrl { get; set; }
        public string FileUrl { get; set; }
        //public IDictionary<string, object> StorageMaps { get; set; } = new Dictionary<string, object>();
    }
}

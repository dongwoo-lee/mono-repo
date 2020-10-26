using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class AppSettings
    {
        private static AppSettings _obj;
        public string TokenIssuer { get; set; }
        public string TokenSignature { get; set; }
        public string ConnectionString { get; set; }
        public List<DTO_NAMEVALUE> DiskScope { get; set; } = new List<DTO_NAMEVALUE>();
        
        public AppSettings GetInstance()
        {
            return _obj;
        }
    }
}

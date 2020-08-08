using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class AppSettings
    {
        public string TokenIssuer { get; set; }
        public string TokenSignature { get; set; }
        public string ConnectionString { get; set; }

        public string FtpUri { get; set; }
        public string FtpId { get; set; }
        public string FtpPass { get; set; }
        public string FtpTmpUploadFolder { get; set; }
        public string FtpPrivateUploadFolder { get; set; }
        public string FtpPublicUploadFolder { get; set; }
        public string MusicSystemUri { get; set; }

        
            
    }
}

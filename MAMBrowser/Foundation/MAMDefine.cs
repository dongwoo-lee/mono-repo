using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Foundation
{
    public class MAMDefine
    {
        public const string PrivateWorkConnection = "PrivateWorkConnection";
        public const string PublicWorkConnection = "PublicWorkConnection";
        public const string MirosConnection = "MirosConnection";
        public const string DLArchiveConnection = "DLArchiveConnection";
        public const string FTP = "FTP";
        public const string SMB = "SMB";

        public enum SoundDataTypes
        {
            MY_DISK = 0,
            PRO = 1,
            PROGRAM = 2,
            MCR_SPOT =3,
            SCR_SPOT=4,
            FILLER=5,
            REPORT=6,
            STATIC_SPOT=7,
            VAR_SPOT = 8,
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class StorageConnections
    {
        public InternalStorage Internal { get; set; } = new InternalStorage();
        public ExternalStorage External { get; set; } = new ExternalStorage();
    }
}

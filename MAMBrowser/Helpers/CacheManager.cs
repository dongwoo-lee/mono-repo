using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class CacheManager
    {
        public List<string> CurrentProcess { get; set; }

        public string CacheFile()
        {
            var guid = Guid.NewGuid().ToString();
            CurrentProcess.Add(guid);
            Task.Run(() =>
            {

            });
            return guid;
        }
    }
}

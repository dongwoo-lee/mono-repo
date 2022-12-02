using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class PgmListDTO
    {
        public string PGMCODE { get; set; }
        public string PGMNAME { get; set; }
        public char MEDIA { get; set; }
        public List<PgmItem> pgmItem { get; set; }
    }

    public class PgmItem
    {
        public string PRODUCTID { get; set; }
        public string EVENTNAME { get; set; }
        public string SERVICENAME { get; set; }
    }

}

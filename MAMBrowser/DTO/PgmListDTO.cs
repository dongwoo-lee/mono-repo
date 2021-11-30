using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class PgmListDTO
    {
        public string PERSONID { get; set; }
        public string MEDIA { get; set; }
        public List<ProgramListDetailDTO> DETAILS { get; set; }
    }

    public class ProgramListDetailDTO
    {
        public string PRODUCTID { get; set; }
        public string EVENTNAME { get; set; }
        public string SERVICENAME { get; set; }
    }
}

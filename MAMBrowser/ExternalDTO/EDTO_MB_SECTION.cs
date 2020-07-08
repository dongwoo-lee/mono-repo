using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAMBrowser.DTO
{
    public class EDTO_MB_SECTION
    {
        [XmlElement(ElementName = "totcnt")]
        public List<EDTO_SONG> TOTAL_COUNT { get; set; }
        [XmlElement(ElementName = "maxcnt")]
        public List<EDTO_SONG> MAX_CNT { get; set; }
        [XmlElement(ElementName = "outcnt")]
        public List<EDTO_SONG> OUT_CNT { get; set; }
        [XmlElement(ElementName = "pagenum")]
        public List<EDTO_SONG> PAGE_NO { get; set; }

        [XmlElement(ElementName = "doc")]
        public List<EDTO_SONG> SONG_LIST { get; set; }
    }
}

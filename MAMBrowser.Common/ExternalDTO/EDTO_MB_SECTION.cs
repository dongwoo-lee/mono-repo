using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAMBrowser.DTO
{
    public class EDTO_MB_SECTION<T>
    {
        [XmlElement(ElementName = "totcnt")]
        public long TOTAL_COUNT { get; set; }
        [XmlElement(ElementName = "maxcnt")]
        public long MAX_CNT { get; set; }
        [XmlElement(ElementName = "outcnt")]
        public long OUT_CNT { get; set; }
        [XmlElement(ElementName = "pagenum")]
        public int PAGE_NO { get; set; }

        [XmlElement(ElementName = "doc")]
        public List<T> Data { get; set; }
    }
}

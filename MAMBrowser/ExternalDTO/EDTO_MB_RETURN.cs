using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAMBrowser.DTO
{
    [XmlRoot("meta_storage_list")]
    public class EDTO_MB_RETURN
    {
        [XmlElement(ElementName = "section")]
        public EDTO_MB_SECTION Section { get; set; }
    }
}

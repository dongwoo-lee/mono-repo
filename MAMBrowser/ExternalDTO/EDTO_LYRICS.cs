using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAMBrowser.DTO
{
    public class EDTO_LYRICS
    {
        [XmlElement("SONG_WORD_SR")]
        public string SONG_WORD_SR { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAMBrowser.DTO
{
    public class EDTO_EFFECT
    {
        [XmlElement("TITLE_SR")]
        public string Name { get; set; }    //효과음명
        [XmlElement("DESCRIPTION_SR")]
        public string DESCRIPTION { get; set; } //설명
        [XmlElement("REC_LENGTH_SR")]
        public string Duration { get; set; }    //길이
        [XmlElement("REC_TYPE_SR")]
        public string AUDIO_FORMAT { get; set; }    //오디오 포맷
        [XmlElement("MP2_PATH_SR")]
        public string FilePath { get; set; }    //파일경로 (파일명 제외)
        [XmlElement("MP2_NAME_SR")]
        public string FILE_NAME { get; set; }   //파일명
    }
}

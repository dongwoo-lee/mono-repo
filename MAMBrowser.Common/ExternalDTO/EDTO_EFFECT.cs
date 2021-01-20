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
        public string Description { get; set; } //설명
        [XmlElement("REC_LENGTH_SR")]
        public string Duration { get; set; }    //길이
        [XmlElement("CD_LENGTH_SR")]
        public string Duration2 { get; set; }    //길이
        [XmlElement("REC_TYPE_SR")]
        public string AudioFormat { get; set; }    //오디오 포맷
        [XmlElement("MP2_PATH_SR")]
        public string MP2FilePath { get; set; }    //파일경로 (파일명 제외)
        [XmlElement("MP2_NAME_SR")]
        public string MP2FileName { get; set; }   //파일명

        [XmlElement("WAV_PATH_SR")]
        public string WavFilePath { get; set; }    //파일경로 (파일명 제외)
        [XmlElement("WAV_NAME_SR")]
        public string WavFileName { get; set; }   //파일명

        //그외 mp2 샘플레이트, mp2 채널, mp2 bps 등값이 있긴 하지만....
    }
}

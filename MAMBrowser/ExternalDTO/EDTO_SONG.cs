using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAMBrowser.DTO
{
    [XmlRoot(ElementName = "alert")]
    public class EDTO_SONG
    {
        [XmlElement(ElementName = "TRACK_NO_SR")]
        public string Track_NO { get; set; }    //트랙번호
        [XmlElement(ElementName = "COMPOSER_SR")]
        public string COMPOSER { get; set; }//작곡가
        [XmlElement(ElementName = "ARRANGER_SR")]
        public string ARRANGER { get; set; }//편곡가
        [XmlElement(ElementName = "WRITER_SR")]
        public string WRITER { get; set; }//작사가
        [XmlElement(ElementName = "PLAY_TIME_SR")]
        public string PLAY_TIME { get; set; }//재생시간

        [XmlElement(ElementName = "ARTIST_NAME_SR")]
        public string ARTIST_NAME { get; set; } //아티스트 이름

        [XmlElement(ElementName = "DISC_ARR_NUM_SR")]
        public string ALBUM_NO { get; set; } // 앨범 번호
        [XmlElement(ElementName = "DISC_NAME_SR")]
        public string ALBUM_NAME { get; set; } //앨번 이름

        [XmlElement(ElementName = "JPG_FILE_PATH_SR")]
        public string JPG_FILE_PATH_SR { get; set; } // 앨범커버 경로
        [XmlElement(ElementName = "JPG_FILE_NAME_SR")]
        public string JPG_FILE_NAME_SR { get; set; } //앨범커버 파일명


        [XmlElement(ElementName = "ORIGIN_PATH_SR")]
        public string ORIGIN_PATH_SR { get; set; } // 파일경로??
        [XmlElement(ElementName = "FILE_NAME_SR")]
        public string FILE_NAME_SR { get; set; } //파일이름

        //<MB_FILE_PATH_SR><![CDATA[\\MIBIS-WAVE1\MIDAS_52\20140709\14]]></MB_FILE_PATH_SR> //뮤직뱅크 파일경로??


        [XmlElement(ElementName = "ISSUE_YEAR_SR")]
        public string ISSUE_YEAR_SR { get; set; } //발매년도??? 2004.02.14

        [XmlElement(ElementName = "JPG_FILE_NAME_SR")]
        public string WK_DATE_SR { get; set; } //등록일??? 2004.02.14


        //<SONG_NAME_SR><![CDATA[花(화)]]></SONG_NAME_SR>     //제목들????
        //<SNAME_TRANS_SR><![CDATA[꽃]]></SNAME_TRANS_SR>
        //<SNAME_PRON_SR><![CDATA[하나]]></SNAME_PRON_SR>

    }
}

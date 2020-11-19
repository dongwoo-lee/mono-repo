using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAMBrowser.DTO
{
    public class EDTO_SONG
    {
    //    [XmlElement(ElementName = "DISC_COMM_SEQ_SR")]
    //    public string DISC_COMM_SEQ_SR { get; set; }    //고유번호
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
        public string DISC_ARR_NUM_SR { get; set; } // 배열번호
        [XmlElement(ElementName = "DISC_NAME_SR")]
        public string ALBUM_NAME { get; set; } //앨번 이름
        [XmlElement(ElementName = "DISC_DNAME_PRON_SR")]
        public string ALBUM_PRON_NAME { get; set; } //앨번 이름
        [XmlElement(ElementName = "DISC_DNAME_TRANS_SR")]
        public string ALBUM_TRANS_NAME { get; set; } //앨번 이름

        [XmlElement(ElementName = "JPG_COUNT_SR")]
        public string JPG_COUNT_SR { get; set; } // 앨범커버 개수

        [XmlElement(ElementName = "JPG_FILE_PATH_SR")]
        public string JPG_FILE_PATH_SR { get; set; } // 앨범커버 경로
        [XmlElement(ElementName = "JPG_FILE_NAME_SR")]
        public string JPG_FILE_NAME_SR { get; set; } //앨범커버 파일명
        //[XmlElement(ElementName = "MB_FILE_PATH_SR")]
        //public string MB_FILE_PATH_SR { get; set; } // 파일경로

        [XmlElement(ElementName = "MR_SONG_WAV_PATH_SR")]
        public string MR_SONG_WAV_PATH_SR { get; set; } // wav파일경로

        [XmlElement(ElementName = "FILE_NAME_SR")]
        public string FILE_NAME_SR { get; set; } //파일이름

        //<MB_FILE_PATH_SR><![CDATA[\\MIBIS-WAVE1\MIDAS_52\20140709\14]]></MB_FILE_PATH_SR> //뮤직뱅크 파일경로??
        [XmlElement(ElementName = "ISSUE_YEAR_SR")]
        public string ISSUE_YEAR_SR { get; set; } //발매년도??? 2004.02.14

        [XmlElement(ElementName = "WK_DATE_SR")]
        public string WK_DATE_SR { get; set; } //등록일??? 2004.02.14

        [XmlElement(ElementName = "SONG_NAME_SR")]  //음원명
        public string SONG_NAME_SR { get; set; }
        [XmlElement(ElementName = "SNAME_TRANS_SR")]
        public string SNAME_TRANS_SR { get; set; } //음원 번역명

        [XmlElement(ElementName = "SNAME_PRON_SR")]
        public string SNAME_PRON_SR { get; set; } //음원 발음명

        [XmlElement(ElementName = "SONG_WORD_SEQ_SR")]
        public string SONG_WORD_SEQ_SR { get; set; } //가사 검색용 일련번호
        
    }
}

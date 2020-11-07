using MAMBrowser.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_SONG : DTO_FILEBASE
    {
        /// <summary>
        /// 배열번호
        /// </summary>
        public string SequenceNO { get; set; }
        /// <summary>
        /// 곡명
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 아티스트명
        /// </summary>
        public string ArtistName { get; set; }
        /// <summary>
        /// 재생시간
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// 앨범명
        /// </summary>
        public string AlbumName { get; set; }
        /// <summary>
        /// 트랙번호
        /// </summary>
        public string TrackNO { get; set; }  
        /// <summary>
        /// 발매년도
        /// </summary>
        public string ReleaseDate { get; set; } 
        /// <summary>
        /// 작곡가
        /// </summary>
        public string Composer { get; set; }
        /// <summary>
        /// 작사가
        /// </summary>
        public string Writer { get; set; }
        /// <summary>
        /// 파일경로
        /// </summary>
        public string FilePath { get; set; }


        //------------------------------------------추가정보
        /// <summary>
        /// 편곡가
        /// </summary>
        public string Arranger { get; set; }
        /// <summary>
        /// 앨범 번호
        /// </summary>
        public string AlbumNO { get; set; } 
        /// <summary>
        /// 앨범커버 경로
        /// </summary>
        public string AlbumCoverFilePath { get; set; }     
        //public string RegDate { get; set; } //등록일??? 2004.02.14
        public string Lyrics { get; set; }      //가사

        public DTO_MUSIC_REQUEST FileRequestInfo { get; set; }

        public DTO_SONG()
        {

        }
        public DTO_SONG(EDTO_SONG edto)
        {
            this.SequenceNO = edto.DISC_COMM_SEQ_SR;
            this.Name = string.IsNullOrEmpty(edto.SNAME_PRON_SR) ? edto.SONG_NAME_SR : $"{edto.SONG_NAME_SR} ({edto.SNAME_PRON_SR})";
            this.ArtistName = edto.ARTIST_NAME;
            this.Duration = edto.PLAY_TIME;
            this.AlbumName = string.IsNullOrEmpty(edto.ALBUM_PRON_NAME) ? edto.ALBUM_NAME : $"{edto.ALBUM_NAME} ({edto.ALBUM_PRON_NAME})";
            this.TrackNO = edto.Track_NO;
            this.ReleaseDate = edto.ISSUE_YEAR_SR;
            this.Composer = edto.COMPOSER;
            this.Writer = edto.WRITER;
            this.FilePath = Path.Combine(edto.MB_FILE_PATH_SR, edto.FILE_NAME_SR);
            this.Arranger = edto.ARRANGER;
            this.AlbumNO = edto.ALBUM_NO;
            this.AlbumCoverFilePath = Path.Combine(edto.JPG_FILE_PATH_SR, edto.JPG_FILE_NAME_SR);
            //this.Lyrics = edto.ly;
            MAMUtility.GetMusicRequest(this.FilePath);
        }
    }
}

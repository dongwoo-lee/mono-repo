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

        //------------------------------------------추가정보
        /// <summary>
        /// 편곡가
        /// </summary>
        public string Arranger { get; set; }
        //public string RegDate { get; set; } //등록일??? 2004.02.14
        public string LyricsSeq { get; set; }      //가사
        

       

        public override string FilePath
        {
            get => filePath;
            set
            {
                if (filePath == value)
                    return;

                filePath = value;
                FileToken = MAMUtility.GenerateMusicToken(filePath);
            }
        }
        
        protected string albumImageFilePath;
        /// <summary>
        /// 앨범커버 경로
        /// </summary>
        public string AlbumImageFilePath
        {
            get => albumImageFilePath;
            set
            {
                if (albumImageFilePath == value)
                    return;

                albumImageFilePath = value;
                AlbumToken = MAMUtility.GenerateMusicToken(filePath);
            }
        }
        public string AlbumToken { get; set; } // music 구조체 토큰

        public DTO_SONG()
        {

        }
        public DTO_SONG(EDTO_SONG edto)
        {
            this.SequenceNO = edto.DISC_ARR_NUM_SR;
            this.Name = string.IsNullOrEmpty(edto.SNAME_PRON_SR) ? edto.SONG_NAME_SR : $"{edto.SONG_NAME_SR} ({edto.SNAME_PRON_SR})";
            this.ArtistName = edto.ARTIST_NAME;
            this.Duration = edto.PLAY_TIME;
            this.AlbumName = string.IsNullOrEmpty(edto.ALBUM_PRON_NAME) ? edto.ALBUM_NAME : $"{edto.ALBUM_NAME} ({edto.ALBUM_PRON_NAME})";
            this.TrackNO = edto.Track_NO;
            this.ReleaseDate = edto.ISSUE_YEAR_SR;
            this.Composer = edto.COMPOSER;
            this.Writer = edto.WRITER;
            this.FilePath = Path.Combine(edto.MR_SONG_WAV_PATH_SR, edto.FILE_NAME_SR+MAMUtility.WAV);
            this.Arranger = edto.ARRANGER;
            this.AlbumImageFilePath = Path.Combine(edto.JPG_FILE_PATH_SR, edto.JPG_FILE_NAME_SR);
            this.LyricsSeq = edto.SONG_WORD_SEQ_SR;
            //this.file = MAMUtility.GenerateMusicToken(this.FilePath);
            //this.AlbumToken = MAMUtility.GenerateMusicToken(this.AlbumImageFilePath);
        }
    }
}

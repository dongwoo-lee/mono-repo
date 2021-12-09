namespace MAMBrowser.DTO
{
    public class SongCacheDTO
    {
        /// <summary>
        /// PD_AUDIOFILE 테이블의 AudioClipID 필드
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 노래 제목
        /// </summary>
        public string SongName { get; set; } //메인으로
        /// <summary>
        /// 앨범명
        /// </summary>
        public string AlbumName { get; set; } 
        /// <summary>
        /// 가수명
        /// </summary>
        public string ArtistName { get; set; } //서브
        /// <summary>
        /// 발매년도
        /// </summary>
        public string ReleaseDate { get; set; }
        /// <summary>
        /// 파일토큰
        /// </summary>
        public string FileToken { get; set; }
        /// <summary>
        /// 파일경로
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 음원 길이(msec)
        /// </summary>
        public int IntDuration { get; set; }
        

    }
}

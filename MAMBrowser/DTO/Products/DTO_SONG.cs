using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_SONG : DTO_BASE
    {
        public string Name { get; set; }    //곡명
        public string ArtistName { get; set; } //아티스트명
        public string Duration { get; set; }//재생시간
        public string AlbumName { get; set; } //앨범명
        public string TrackNO { get; set; }    //트랙번호
        public string ReleaseDate { get; set; } //발매년도??? 2004.02.14
        public string Composer { get; set; }//작곡가
        public string Writer { get; set; }//작사가
        public string SequenceNO { get; set; }  //배열번호
        public string FilePath { get; set; }    //파일경로


        //------------------------------------------추가정보
        public string Arranger { get; set; }//편곡가
        public string AlbumNO { get; set; } // 앨범 번호
        public string AlbumCoverFilePath { get; set; } // 앨범커버 경로        
        //public string RegDate { get; set; } //등록일??? 2004.02.14
        public string Lyrics { get; set; }      //가사
    }
}

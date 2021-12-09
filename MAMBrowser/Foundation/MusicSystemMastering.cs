using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Foundation;
using MAMBrowser.DTO;

namespace MAMBrowser.Foundation
{
    public class MusicSystemMastering
    {
        public SongCacheDTO SongMastering(DTO_SONG song)
        {

            string code = "SM01";
            string code2 = "SS01";


            SongCacheDTO songCacheDTO = new SongCacheDTO();
            songCacheDTO.ID = "SS00519332";
            songCacheDTO.SongName = "SHE";
            songCacheDTO.ArtistName = "ELVIS COSTELLO";
            songCacheDTO.AlbumName = "25TH 배철수의 음악캠프, SINCE 1990: RADIOHEAD 에서 BRITNEY SPEARS 까지 (CD 1)";
            songCacheDTO.ReleaseDate = "2015";
            songCacheDTO.FilePath = song.FilePath;
            if (!string.IsNullOrEmpty(song.FilePath))
                songCacheDTO.FileToken = TokenGenerator.GenerateFileToken(song.FilePath);
            songCacheDTO.IntDuration = 208849;



            return songCacheDTO;
        }
        public SongCacheDTO EffectMastering(DTO_EFFECT effect)
        {

            string code = "SM01";
            string code2 = "SS01";


            SongCacheDTO songCacheDTO = new SongCacheDTO();
            songCacheDTO.ID = "SS00528088";
            songCacheDTO.SongName = "나무바닥에 동전 떨어지는 소리 - 동전 한 개, 여러 개";
            songCacheDTO.ArtistName = "ELVIS COSTELLO";
            songCacheDTO.AlbumName = "FX COLLECTION - INDUSTRY AND OFFICE 3";
            songCacheDTO.ReleaseDate = "1993";
            songCacheDTO.FilePath = effect.FilePath;
            if (!string.IsNullOrEmpty(effect.FilePath))
                songCacheDTO.FileToken = TokenGenerator.GenerateFileToken(effect.FilePath);
            songCacheDTO.IntDuration = 208849;
            //encoding date 20140619000000

            return songCacheDTO;
        }
    }
}

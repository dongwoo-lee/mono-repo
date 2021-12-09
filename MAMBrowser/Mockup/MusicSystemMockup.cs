using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.DAL.WebService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MAMBrowser
{
    public class MusicSystemMockup : IMusicService
    {
        public Stream GetFileStream(string domain, int port, string jsonRequestContent, out long fileSize)
        {
            fileSize = 0;
            return null;
        }

        public object[] GetRequestInfo(Dictionary<string, string> musicInfo)
        {
            //var encodedFilePath = musicInfo[Define.MUSIC_FILEPATH];
            //var filePath = MusicSeedWrapper.SeedDecrypt(encodedFilePath);
            //var host = CommonUtility.GetHost(filePath);
            //var domainAndPort = _info.StorageMaps[host.ToUpper()] as string;        // 뮤직서비스에 구현.
            //var domain = domainAndPort.Split(':').First();
            //var port = Convert.ToInt32(domainAndPort.Split(':').Last());
            //var fileName = Path.GetFileName(filePath);

            object[] returnData = new object[3];
            returnData[0] = "\\test_svr";
            returnData[1] = "";
            returnData[2] = @"Music001.wav";
            return returnData;
        }

        public IList<DTO_EFFECT> SearchEffect(string text, int rowPerPage, int selectPage, out long totalCount)
        {
            totalCount = 1;
            using (FileStream fs = new FileStream(@".\Mockup\effect.xml", FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_EFFECT>));
                var mbReturnDto = (EDTO_MB_RETURN<EDTO_EFFECT>)serializer.Deserialize(fs);
                return mbReturnDto.Section.Data.Select(edto => new DTO_EFFECT(edto)).ToList();
            }
        }

        public string SearchLyrics(string lyricsSeq)
        {
            return "가사 테스트 입니다.";
        }

        public IList<DTO_SONG> SearchSong(MusicSearchTypes1 searchType, string searchType2, GradeTypes gradeType, string searchText, int rowPerPage, int selectPage, out long totalCount)
        {
            totalCount = 1;
            using (FileStream fs = new FileStream(@".\Mockup\song.xml", FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_SONG>));
                var mbReturnDto = (EDTO_MB_RETURN<EDTO_SONG>)serializer.Deserialize(fs);
                return mbReturnDto.Section.Data.Select(edto => new DTO_SONG(edto)).ToList();
            }
        }

        public void TempDownloadCore(string domain, int port, string jsonRequestContent, string targetSoundPath)
        {
            return;
        }

        public void TempDownloadWavAndEgy(string userId, string remoteIp, string token)
        {
            return;
        }

        public List<string> TempImageDownload(string userId, string remoteIp, string musicToken, string albumToken)
        {
            return null;
        }
    }
}

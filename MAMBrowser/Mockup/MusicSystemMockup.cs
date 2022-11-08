using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.DAL.WebService;
using MAMBrowser.Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace MAMBrowser
{
    public class MusicSystemMockup : IMusicService
    {
        IMusicService _originService;
        public MusicSystemMockup(IMusicService originService)
        {
            _originService = originService;
        }
        public Stream GetFileStream(string domain, int port, string jsonRequestContent, out long fileSize)
        {
            string filePath = @"Mockup\CDO28190A-10.wav";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileSize = fs.Length;
            return fs;
        }

        public Stream GetFileStreamByFilePath(string filePath, out long fileSize)
        {
            throw new Exception("실패");
        }

        public object[] GetRequestInfo(Dictionary<string, string> musicInfo)
        {
            var encodedFilePath = musicInfo[Define.MUSIC_FILEPATH];
            var filePath = MusicSeedWrapper.SeedDecrypt(encodedFilePath);
            var host = CommonUtility.GetHost(filePath);
            var domainAndPort = "192.168.1.201:80" as string;        // 뮤직서비스에 구현.
            var domain = domainAndPort.Split(':').First();
            var port = Convert.ToInt32(domainAndPort.Split(':').Last());
            var fileName = Path.GetFileName(filePath);

            object[] returnData = new object[3];
            returnData[0] = domain;
            returnData[1] = port;
            returnData[2] = fileName;
            return returnData;
        }

        public void LocalDownloadCore(string domain, int port, string jsonRequestContent, string targetSoundPath)
        {
            _originService.LocalDownloadCore(domain, port, jsonRequestContent, targetSoundPath);
        }

        public void LocalDownloadWavAndEgy(string targetFolder, string musicToken)
        {
            string filePath = @"Mockup\CDO28190A-10.wav";
            string filePath2 = @"Mockup\CDO28190A-10.egy";
            string target = Path.Combine(@"C:\TempDownload\adsoft_localhost", "CDO28190A-10.wav");
            string target2 = Path.Combine(@"C:\TempDownload\adsoft_localhost", "CDO28190A-10.egy");
            File.Copy(filePath, target, true);
            File.Copy(filePath2, target2, true);
        }

        public List<string> LocalImageDownload(string targetFolder, string musicToken, string albumToken)
        {
            string filePath = @"\\ad2022-nas\AUDIO-FILE\mbcdata\Music\CDO28190.jpg";
            string target = Path.Combine(@"C:\TempDownload\adsoft_localhost", "CDO28190.jpg");
            File.Copy(filePath, target, true);
            List<string> fileNames = new List<string>();
            fileNames.Add("CDO28190.jpg");
            return fileNames;
        }

        public IList<DTO_EFFECT> SearchEffect(string text, int rowPerPage, int selectPage, out long totalCount)
        {
            string filePath = @"Mockup\effect.xml";
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int rowNo = 1;
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_EFFECT>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_EFFECT>)serializer.Deserialize(reader);
                    totalCount = mbReturnDto.Section.TOTAL_COUNT;
                    var dtoList = mbReturnDto.Section.Data.Select(edto => new DTO_EFFECT(edto)).ToList();
                    dtoList.ForEach(dto =>
                    {
                        dto.RowNO = rowNo;
                        rowNo++;
                    });
                }
            }
            throw new Exception("실패");
        }

        public string SearchLyrics(string lyricsSeq)
        {
            if (!string.IsNullOrEmpty(lyricsSeq))
                return $"가사 리턴합니다. 가사번호는 {lyricsSeq}";
            else
                return null;
        }

        public IList<DTO_MUSIC> SearchMusic(MusicSearchTypes1 searchType, string searchType2, GradeTypes gradeType, string searchText, int rowPerPage, int selectPage, out long totalCount)
        {
            string filePath = @"Mockup\song2.xml";
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int rowNo = 1;
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_MUSIC>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_MUSIC>)serializer.Deserialize(reader);
                    totalCount = mbReturnDto.Section.TOTAL_COUNT;
                    var dtoList = mbReturnDto.Section.Data.Select(edto => new DTO_MUSIC(edto)).ToList();
                    dtoList.ForEach(dto =>
                    {
                        dto.RowNO = rowNo;
                        rowNo++;
                    });
                    return dtoList;
                }
            }
            throw new Exception("실패");
        }
    }
}

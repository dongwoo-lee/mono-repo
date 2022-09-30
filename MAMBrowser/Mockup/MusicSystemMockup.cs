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
            throw new NotImplementedException();
        }

        public Stream GetFileStreamByFilePath(string filePath, out long fileSize)
        {
            throw new NotImplementedException();
        }

        public object[] GetRequestInfo(Dictionary<string, string> musicInfo)
        {
            throw new NotImplementedException();
        }

        public IList<DTO_EFFECT> SearchEffect(string text, int rowPerPage, int selectPage, out long totalCount)
        {
            throw new NotImplementedException();
        }

        public string SearchLyrics(string lyricsSeq)
        {
            throw new NotImplementedException();
        }

        public IList<DTO_MUSIC> SearchSong(MusicSearchTypes1 searchType, string searchType2, GradeTypes gradeType, string searchText, int rowPerPage, int selectPage, out long totalCount)
        {
            throw new NotImplementedException();
        }

        public void TempDownloadCore(string domain, int port, string jsonRequestContent, string targetSoundPath)
        {
            throw new NotImplementedException();
        }

        public void TempDownloadWavAndEgy(string userId, string remoteIp, string token)
        {
            throw new NotImplementedException();
        }

        public List<string> TempImageDownload(string userId, string remoteIp, string musicToken, string albumToken)
        {
            throw new NotImplementedException();
        }
    }
}

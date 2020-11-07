using FluentFTP;
using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Helpers;
using MAMBrowser.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using static MAMBrowser.Helpers.MAMUtility;
using static System.Net.Mime.MediaTypeNames;

namespace MAMBrowser.Services
{
    public class MusicService : IFileService
    {
        public string Name { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        public string MbcDomain { get; set; }
        public string AuthorKey { get; set; }
        public string Host { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MusicService()
        {
        }
        private string GetMusicParam(SearchTypes searchType)
        {
            Dictionary<SearchTypes, string> words = new Dictionary<SearchTypes, string>();
            words.Add(SearchTypes.None, "");
            words.Add(SearchTypes.Internal, "001*");
            words.Add(SearchTypes.External, "002*");
            words.Add(SearchTypes.Classic, "003*");
            words.Add(SearchTypes.All, "");
            string defaultString = @"&csq={song_idx:*}";


            string lastValue = defaultString;
            List<string> wordValueList = new List<string>();
            if (searchType > 0)
            {
                foreach (SearchTypes value in Enum.GetValues(searchType.GetType()))
                {
                    if (value == SearchTypes.None)
                        continue;

                    if (searchType.HasFlag(value))
                    {
                        var returnString = words[value];
                        if (!string.IsNullOrEmpty(returnString))
                            wordValueList.Add(returnString);
                    }
                }
                wordValueList = wordValueList.FindAll(dt => !string.IsNullOrEmpty(dt));
                string valueWord = string.Join(" | ", wordValueList);
                lastValue += @"{disc_sect_idx:" + valueWord + "}";
            }
            return lastValue;
        }
        private string GetMusicParam(SearchTypes searchType, GradeTypes gradeType)
        {
            Dictionary<GradeTypes, string> words = new Dictionary<GradeTypes, string>();
            words.Add(GradeTypes.None, "");
            words.Add(GradeTypes.Heat, "p4=1");
            words.Add(GradeTypes.Forbid, "p5=2");
            words.Add(GradeTypes.Caution, "p8=1");
            words.Add(GradeTypes.HarmfulJuveniles, "p7=1");
            words.Add(GradeTypes.All, "");

            var defaultString = GetMusicParam(searchType);
            string lastValue = defaultString;
            List<string> wordValueList = new List<string>();
            if (gradeType > 0)
            {
                foreach (GradeTypes value in Enum.GetValues(gradeType.GetType()))
                {
                    if (value == GradeTypes.None)
                        continue;

                    if (gradeType.HasFlag(value))
                    {
                        var returnString = words[value];
                        if (!string.IsNullOrEmpty(returnString))
                            wordValueList.Add(returnString);
                    }
                }
                wordValueList = wordValueList.FindAll(dt => !string.IsNullOrEmpty(dt));
                string valueWord = "&" + string.Join("&", wordValueList);
                lastValue += valueWord;
            }
            return lastValue;
        }
        public IList<DTO_SONG> SearchSong(DTO_MUSIC_REQUEST requestInfo, SearchTypes searchType, GradeTypes gradeType, string searchText, int rowPerPage, int selectPage, out long totalCount)
        {
            //HttpValueCollection
            totalCount = 0;
            string pageParam = $"viewRow={rowPerPage}&pageCnt{selectPage}&fsort={50}";
            //var query = QueryHelpers.ParseQuery("");
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("authorKey", HttpUtility.UrlPathEncode(AuthorKey));
            query.Add("strSearch", HttpUtility.UrlPathEncode(searchText));
            query.Add("viewRow", rowPerPage.ToString());
            query.Add("pageCnt", selectPage.ToString());
            query.Add("fsort", "50");
            query.Add("query", HttpUtility.UrlPathEncode(GetMusicParam(searchType, gradeType)));
            //var queryString = QueryString.Create(query);
            //var queryString = query.ToString();
            var queryString = QueryHelpers.AddQueryString("", query);
            //var encodedQueryString = HttpUtility.UrlEncode(queryString.ToString());
            HttpClient client = new HttpClient();
            var builder = new UriBuilder("http", MbcDomain, 80, "MusicService.asmx/MirosGetSongSearchData"); 
            builder.Query = queryString.ToString();

            
            HttpContent content = new StringContent(JsonSerializer.Serialize(requestInfo));
            var result = client.PostAsync(builder.Uri, content).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (var stream = result.Content.ReadAsStreamAsync().Result)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_SONG>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_SONG>)serializer.Deserialize(stream);
                    var songList = mbReturnDto.Section.Data.Select(edto => new DTO_SONG(edto)).ToList();
                    totalCount = mbReturnDto.Section.TOTAL_COUNT;
                    return songList;
                }
            }
            else
                throw new Exception(result.StatusCode.ToString());
        }
        public IList<DTO_EFFECT> SearchLyrics(DTO_MUSIC_REQUEST requestInfo, string searchText, int rowPerPage, int selectPage, out long totalCount)
        {
            //HttpValueCollection
            totalCount = 0;
            string pageParam = $"viewRow={rowPerPage}&pageCnt{selectPage}&fsort={50}";
            var query = QueryHelpers.ParseQuery("");
            query.Add("authorKey", AuthorKey);
            query.Add("strSearch", searchText);
            query.Add("viewRow", rowPerPage.ToString());
            query.Add("pageCnt", selectPage.ToString());
            query.Add("fsort", "50");
            var queryString = QueryString.Create(query);
            //var encodedQueryString = HttpUtility.UrlEncode(queryString.ToString());
            HttpClient client = new HttpClient();
            var builder = new UriBuilder("MusicService.asmx/MirosGetSongSearchData", MbcDomain);
            builder.Query = queryString.ToString();

            HttpContent content = new StringContent(JsonSerializer.Serialize(requestInfo));
            var result = client.PostAsync(builder.Path, content).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (var stream = result.Content.ReadAsStreamAsync().Result)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_EFFECT>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_EFFECT>)serializer.Deserialize(stream);
                    var songList = mbReturnDto.Section.Data.Select(edto => new DTO_EFFECT(edto)).ToList();
                    totalCount = mbReturnDto.Section.TOTAL_COUNT;
                    return songList;
                }
            }
            else
                return null;

        }

        public IList<string> GetImages()
        {
            List<string> pathList = new List<string>();
            pathList.Add(@"\\192.168.1.203\share\3. 개인자료\이동우\mam storage\Product\PRD001.jpg");
            pathList.Add(@"\\192.168.1.203\share\3. 개인자료\이동우\mam storage\Product\PRD001-a.jpg");
            pathList.Add(@"\\192.168.1.203\share\3. 개인자료\이동우\mam storage\Product\PRD001-b.jpg");
            pathList.Add(@"\\192.168.1.203\share\3. 개인자료\이동우\mam storage\Product\PRD001-c.jpg");
            return pathList;
        }


        private string GetAlbumPath(string filePath)
        {
            var wordArray = filePath.Split(Path.DirectorySeparatorChar);
            var albumDomain = $"http://{wordArray[2] }.{MbcDomain}/";
            var relativePath = string.Join("/", albumDomain.Skip(2));
            return albumDomain + relativePath;
        }


        public void MakeDirectory(string relativeDirectoryPath)
        {
            throw new NotImplementedException();
        }

        public void Upload(Stream fileStream, string sourcePath, long fileLength)
        {
            throw new NotImplementedException();
        }

        public void Move(string source, string destination)
        {
            throw new NotImplementedException();
        }

        public Stream GetFileStream(string sourcePath, long offSet)
        {
            throw new NotImplementedException();
        }
        public bool DownloadFile(string fromRelativePath, string toRelativePath)
        {
            return true;
        }

        public bool ExistFile(string fromRelativePath)
        {
            throw new NotImplementedException();
        }
    }
}

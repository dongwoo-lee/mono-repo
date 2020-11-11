﻿using FluentFTP;
using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Helpers;
using MAMBrowser.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using static MAMBrowser.Helpers.MAMUtility;
using static System.Net.Mime.MediaTypeNames;

namespace MAMBrowser.Services
{
    public class MusicService : IFileDownloadService
    {
        public string Host
        {
            get
            {
                return MbcDomain;
            }
        }

        public string MbcDomain { get; set; }
        public string AuthorKey { get; set; }
        public string SearchDomain { get; set; }
        public string SearchSongUrl { get; set; }
        public string SearchEffectUrl { get; set; }
        public string SearchLyricsUrl { get; set; }
        public string ImageListUrl { get; set; }
        public string FileUrl { get; set; }

        private readonly IDictionary<string,object> _storageMap;

        public MusicService(IOptions<StorageConnections> appSesstings, IOptions<StorageMaps> storageMap)
        {
            _storageMap = storageMap.Value.Map;
            MbcDomain = appSesstings.Value.MusicConnection["MbcDomain"].ToString();
            AuthorKey = appSesstings.Value.MusicConnection["AuthorKey"].ToString();
            SearchDomain = appSesstings.Value.MusicConnection["SearchDomain"].ToString();
            SearchSongUrl = appSesstings.Value.MusicConnection["SearchSongUrl"].ToString();
            SearchEffectUrl = appSesstings.Value.MusicConnection["SearchEffectUrl"].ToString();
            SearchLyricsUrl = appSesstings.Value.MusicConnection["SearchLyricsUrl"].ToString();
            ImageListUrl = appSesstings.Value.MusicConnection["ImageListUrl"].ToString();
            FileUrl = appSesstings.Value.MusicConnection["FileUrl"].ToString();
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
        public IList<DTO_SONG> SearchSong(SearchTypes searchType, int searchType2, GradeTypes gradeType, string searchText, int rowPerPage, int selectPage, out long totalCount)
        {
            var builder = new UriBuilder("http", SearchDomain, 80, SearchSongUrl);
            string param = $"authorKey={WebUtility.UrlEncode(AuthorKey)}&strSearch={WebUtility.UrlEncode(searchText)}&viewRow={rowPerPage}&pageCnt={selectPage}&fsort={50}&query={WebUtility.UrlEncode(GetMusicParam(searchType, gradeType))}";
            builder.Query = param;

            HttpClient client = new HttpClient();
            var result = client.GetAsync(builder.ToString()).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (var stream = result.Content.ReadAsStreamAsync().Result)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_SONG>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_SONG>)serializer.Deserialize(stream);
                    totalCount = mbReturnDto.Section.TOTAL_COUNT;
                    var dtoList = mbReturnDto.Section.Data.Select(edto => new DTO_SONG(edto)).ToList();
                    return dtoList;
                }
            }
            else
                throw new Exception(result.StatusCode.ToString());
        }
        public IList<DTO_EFFECT> SearchEffect(string text, int rowPerPage, int selectPage, out long totalCount)
        {
            var builder = new UriBuilder("http", SearchDomain, 80, SearchEffectUrl);
            string param = $"authorKey={WebUtility.UrlEncode(AuthorKey)}&strSearch={WebUtility.UrlEncode(text)}&viewRow={rowPerPage}&pageCnt={selectPage}&fsort=50";
            builder.Query = param;

            HttpClient client = new HttpClient();
            var result = client.GetAsync(builder.ToString()).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (var stream = result.Content.ReadAsStreamAsync().Result)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_EFFECT>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_EFFECT>)serializer.Deserialize(stream);
                    totalCount = mbReturnDto.Section.TOTAL_COUNT;
                    var dtoList = mbReturnDto.Section.Data.Select(edto => new DTO_EFFECT(edto)).ToList();
                    return dtoList;
                }
            }
            else
                throw new Exception(result.StatusCode.ToString());
        }
        public string SearchLyrics(long seq)
        {
            var builder = new UriBuilder("http", SearchDomain, 80, SearchLyricsUrl);
            string param = $"authorKey={WebUtility.UrlEncode(AuthorKey)}&strWordSeq={seq}";
            builder.Query = param;

            HttpClient client = new HttpClient();
            var result = client.GetAsync(builder.ToString()).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (var stream = result.Content.ReadAsStreamAsync().Result)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_LYRICS>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_LYRICS>)serializer.Deserialize(stream);
                    var lyrics = mbReturnDto.Section.Data.Select(edto => edto.SONG_WORD_SR).First();
                    return lyrics;
                }
            }
            else
                throw new Exception(result.StatusCode.ToString());
        }

        public IList<string> GetImageTokenList(string musicToken, string albumToken)
        {
            IList<string> imageTokenList = new List<string>();

            var strAlumbInfo = MAMUtility.ParseMusicToken(albumToken);
            var musicInfo = MAMUtility.GetMusicInfo(musicToken);
            
            var encryptedWavFilePath = musicInfo["filePath"];
            var decryptedFilePath = MAMUtility.SeedDecrypt(encryptedWavFilePath);
            var domain = MAMUtility.GetDomainPath(decryptedFilePath);
            var HostAndPort = _storageMap[domain] as string;
            
            var host = HostAndPort.Split(":").First();
            var port = Convert.ToInt32(HostAndPort.Split(":").Last());
            StringContent sc = new StringContent(strAlumbInfo, Encoding.UTF8, "application/json");   //euc-kr 인지 확인 필요.
            var builder = new UriBuilder("http", host, port, ImageListUrl);
            HttpClient client = new HttpClient();
            var response = client.PostAsync(builder.ToString(), sc).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var strReturnData = response.Content.ReadAsStringAsync().Result;
                var imageFilePathList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(strReturnData);

                imageFilePathList.ForEach(path => imageTokenList.Add(GenerateMusicToken(path)));
                return imageTokenList;
            }
            else
                return new List<string>();
        }
        public Stream GetAlbumImage(string musicToken, out string contentType)
        {
            var strAlumbInfo = MAMUtility.ParseMusicToken(musicToken);
            var info = MAMUtility.GetMusicInfo(musicToken);
            var encryptedFilePath = info["filePath"];
            var decryptedFilePath = MAMUtility.SeedDecrypt(encryptedFilePath);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            if (!fileExtProvider.TryGetContentType(decryptedFilePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var domain = MAMUtility.GetDomainPath(decryptedFilePath);
          
            var host = domain;
            var port = 90;
            var relativePath = MAMUtility.GetRelativePath(decryptedFilePath);
            relativePath = relativePath.Replace(@"\", @"/");
            StringContent sc = new StringContent(strAlumbInfo, Encoding.UTF8, "application/json");   //euc-kr 인지 확인 필요.
            var builder = new UriBuilder("http", host, port, relativePath);
            HttpClient client = new HttpClient();
            var response = client.PostAsync(builder.ToString(), sc).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content.ReadAsStreamAsync().Result;
            }
            else
                return null;
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

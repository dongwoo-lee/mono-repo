using MAMBrowser.Helper;
using MAMBrowser.Common;
using MAMBrowser.DTO;
using MAMBrowser.ExternalDTO;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using MAMBrowser.Common.Foundation;

namespace MAMBrowser.Services
{
    public class MusicService 
    {
        public string MbcDomain { get; set; }
        public string AuthorKey { get; set; }
        public string SearchDomain { get; set; }
        public string SearchSongUrl { get; set; }
        public string SearchEffectUrl { get; set; }
        public string SearchLyricsUrl { get; set; }
        public string ImageListUrl { get; set; }
        public string FileUrl { get; set; }
        private IDictionary<string, object> StorageMaps { get; set; }



        public int ExpireMusicTokenHour { get; set; }
        private readonly ILogger<MusicService> _logger;
        private readonly WebServerFileHelper _fileHelper;
        public MusicService()
        {

        }
        public MusicService(ILogger<MusicService> logger, WebServerFileHelper fileHelper)
        {
            //var musicCon = externalInfo.Value.MusicConnection;
            //_storageMap = externalInfo.Value.StorageMaps;
            //MbcDomain = musicCon["MbcDomain"].ToString();
            //AuthorKey = musicCon["AuthorKey"].ToString();
            //SearchDomain = musicCon["SearchDomain"].ToString();
            //SearchSongUrl = musicCon["SearchSongUrl"].ToString();
            //SearchEffectUrl = musicCon["SearchEffectUrl"].ToString();
            //SearchLyricsUrl = musicCon["SearchLyricsUrl"].ToString();
            //ImageListUrl = musicCon["ImageListUrl"].ToString();
            //FileUrl = musicCon["FileUrl"].ToString();
            
            ExpireMusicTokenHour = Startup.AppSetting.ExpireMusicTokenHour;
            _logger = logger;
            _fileHelper = fileHelper;
        }
        private string GetMusicParam(MusicSearchTypes1 searchType, string searchType2)
        {
            Dictionary<MusicSearchTypes1, string> words = new Dictionary<MusicSearchTypes1, string>();
            words.Add(MusicSearchTypes1.None, "");
            words.Add(MusicSearchTypes1.Internal, "001*");
            words.Add(MusicSearchTypes1.External, "002*");
            words.Add(MusicSearchTypes1.Classic, "003*");
            words.Add(MusicSearchTypes1.All, "");
            string defaultString = @"&csq={"+searchType2+":*}";


            string lastValue = defaultString;
            List<string> wordValueList = new List<string>();
            if (searchType > 0)
            {
                foreach (MusicSearchTypes1 value in Enum.GetValues(searchType.GetType()))
                {
                    if (value == MusicSearchTypes1.None)
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
        private string GetMusicParam(MusicSearchTypes1 searchType, string searchType2, GradeTypes gradeType)
        {
            Dictionary<GradeTypes, string> words = new Dictionary<GradeTypes, string>();
            words.Add(GradeTypes.None, "");
            words.Add(GradeTypes.Heat, "p4=1");
            words.Add(GradeTypes.Forbid, "p5=2");
            words.Add(GradeTypes.Caution, "p8=1");
            words.Add(GradeTypes.HarmfulJuveniles, "p7=1");
            words.Add(GradeTypes.All, "");

            var defaultString = GetMusicParam(searchType, searchType2);
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
        public IList<DTO_SONG> SearchSong(MusicSearchTypes1 searchType, string searchType2, GradeTypes gradeType, string searchText, int rowPerPage, int selectPage, out long totalCount)
        {
            var builder = new UriBuilder("http", SearchDomain, 80, SearchSongUrl);
            string param = $"authorKey={WebUtility.UrlEncode(AuthorKey)}&strSearch={WebUtility.UrlEncode(searchText)}&viewRow={rowPerPage}&pageCnt={selectPage}&fsort={10}&query={WebUtility.UrlEncode(GetMusicParam(searchType, searchType2, gradeType))}";
            builder.Query = param;
            
            HttpClient client = new HttpClient();
            var result = client.GetAsync(builder.ToString()).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (var stream = result.Content.ReadAsStreamAsync().Result)
                {
                    int rowNo = selectPage == 1 ? 1 : ((selectPage - 1) * rowPerPage) + 1;
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_SONG>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_SONG>)serializer.Deserialize(stream);
                    totalCount = mbReturnDto.Section.TOTAL_COUNT;
                    var dtoList = mbReturnDto.Section.Data.Select(edto => new DTO_SONG(edto)).ToList();
                    dtoList.ForEach(dto => 
                    {
                        dto.RowNO = rowNo;
                        rowNo++;
                    });
                    return dtoList;
                }
            }
            else
                throw new Exception(result.StatusCode.ToString());
        }
        public IList<DTO_EFFECT> SearchEffect(string text, int rowPerPage, int selectPage, out long totalCount)
        {
            var builder = new UriBuilder("http", SearchDomain, 80, SearchEffectUrl);
            string param = $"authorKey={WebUtility.UrlEncode(AuthorKey)}&strSearch={WebUtility.UrlEncode(text)}";
            builder.Query = param;

            HttpClient client = new HttpClient();
            var result = client.GetAsync(builder.ToString()).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                using (var stream = result.Content.ReadAsStreamAsync().Result)
                {
                    int rowNo = selectPage == 1 ? 1 : ((selectPage - 1) * rowPerPage) + 1;
                    XmlSerializer serializer = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_EFFECT>));
                    var mbReturnDto = (EDTO_MB_RETURN<EDTO_EFFECT>)serializer.Deserialize(stream);
                    totalCount = mbReturnDto.Section.TOTAL_COUNT;
                    var dtoList = mbReturnDto.Section.Data.Select(edto => new DTO_EFFECT(edto)).ToList();
                    dtoList.ForEach(dto =>
                    {
                        dto.RowNO = rowNo;
                        rowNo++;
                    });
                    return dtoList;
                }
            }
            else
                throw new Exception(result.StatusCode.ToString());
        }
        public string SearchLyrics(string lyricsSeq)
        {
            var builder = new UriBuilder("http", SearchDomain, 80, SearchLyricsUrl);
            string param = $"authorKey={WebUtility.UrlEncode(AuthorKey)}&strWordSeq={lyricsSeq}";
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


        public List<string> TempImageDownload(string userId, string remoteIp, string musicToken, string albumToken)
        {
            _logger.LogDebug($"{userId} {remoteIp} {musicToken} {albumToken}");
            List<string> imgUriList = new List<string>();
            var jsonWavInfo = MAMUtility.ParseToJsonRequestContent(musicToken);
            var wavInfo = MAMUtility.ParseToRequestContent(musicToken);
            var wavEncodedPath = wavInfo[Define.MUSIC_FILEPATH];
            var wavPath = MusicSeedWrapper.SeedDecrypt(wavEncodedPath);
            _logger.LogDebug($"wav path : {wavPath}");

            var jsonImgInfo = MAMUtility.ParseToJsonRequestContent(albumToken);
            var imgInfo = MAMUtility.ParseToRequestContent(albumToken);
            var imgEncodedPath = imgInfo[Define.MUSIC_FILEPATH];
            var imgPath = MusicSeedWrapper.SeedDecrypt(imgEncodedPath);
            _logger.LogDebug($"img path : {imgPath}");

            var host = MAMUtility.GetHost(wavPath).ToUpper();
            var domainAndPort = StorageMaps[host] as string;
            var domain = domainAndPort.Split(":").First();
            var port = Convert.ToInt32(domainAndPort.Split(":").Last());

            var imgDomain = MAMUtility.GetHost(imgPath);
            var imgFileName = Path.GetFileName(imgPath);
            var imgRelativePath = MAMUtility.GetRelativePath(imgPath);
            string imgFullUri = imgRelativePath.Replace(@"\", @"/");

            string downloadImageUri = $"http://{imgDomain}.{MbcDomain}/{imgFullUri}";
            _logger.LogDebug($"image streaming uri : {downloadImageUri}");

            //이미지 목록 검색
            StringContent sc = new StringContent(jsonImgInfo, Encoding.UTF8, "application/json");   //euc-kr 인지 확인 필요.
            var builder = new UriBuilder("http", domain, port, ImageListUrl);
            _logger.LogDebug($"Image file name list uri : {builder.ToString()}");

            HttpClient client = new HttpClient();
            var response = client.PostAsync(builder.ToString(), sc).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.LogDebug($"received image file list");
                var strReturnData = response.Content.ReadAsStringAsync().Result;
                var edto = System.Text.Json.JsonSerializer.Deserialize<EDTO_RESULT>(strReturnData);

                foreach (var fileName in edto.result)
                {
                    _logger.LogDebug($"image file name : {fileName}");
                    if (!string.IsNullOrEmpty(fileName) && Path.GetExtension(fileName).ToUpper() == Define.JPG)
                    {
                        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName).ToUpper();
                        if (fileNameWithoutExt[fileNameWithoutExt.Length - 1] == 'T')    //썸네일 제외
                            continue;

                        var requestUri = downloadImageUri.Replace(imgFileName, fileName);
                        imgUriList.Add(requestUri);
                    }
                }
            }

            //이미지 목록 임시 다운로드
            foreach (var imgUri in imgUriList)
            {
                using (HttpClient client2 = new HttpClient())
                {
                    _logger.LogDebug($"request image file : {imgUri}");
                    var response2 = client2.GetAsync(imgUri).Result;
                    if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogDebug($"received image file");
                        var filePath = MAMUtility.GetTempFilePath(userId, remoteIp, Path.GetFileName(imgUri));
                        var folderPath = MAMUtility.GetTempFolder(userId, remoteIp);
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            var stream = response2.Content.ReadAsStreamAsync().Result;
                            _logger.LogDebug($"response stream length : {stream.Length}");
                            stream.CopyTo(fs);
                        }
                    }
                }
            }
            List<string> fileNameList = new List<string>();
            imgUriList.ForEach(uri => fileNameList.Add(Path.GetFileName(uri)));
            return fileNameList;
        }
      
        public Stream GetFileStream(string domain, int port, string jsonRequestContent, out long fileSize)
        {
            var builder = new UriBuilder("http", domain, port, FileUrl);
            HttpClient client = new HttpClient();
            StringContent sc = new StringContent(jsonRequestContent, Encoding.UTF8, "application/json");
            var response = client.PostAsync(builder.ToString(), sc).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //wav파일경로 이므로 wav파일만 받는다.  mp2를 변환할 일이 없음.
                var content = response.Content;
                fileSize = content.Headers.ContentLength ?? 0;
                var stream = content.ReadAsStreamAsync().Result;
                return stream;
            }
            else
            {
                fileSize = 0;
                _logger.LogError($"response status : {response.StatusCode}");
                _logger.LogError($"response status : {response.Content.ReadAsStringAsync().Result}");
                return null;
            }
        }
        public void TempDownloadWavAndEgy(string userId, string remoteIp, string token)
        {
            var jsonWavInfo = MAMUtility.ParseToJsonRequestContent(token);
            var wavInfo = MAMUtility.ParseToRequestContent(token);
            var wavEncodedPath = wavInfo[Define.MUSIC_FILEPATH];

            var wavPath = MusicSeedWrapper.SeedDecrypt(wavEncodedPath);
            var egyPath = Path.ChangeExtension(wavPath, Define.EGY);
            var jsonEgyInfo = TokenGenerator.GetJsonRequestContentFromPath(egyPath, DateTime.Now.AddHours(ExpireMusicTokenHour));
            
            var host = MAMUtility.GetHost(wavPath);
            var domainAndPort = StorageMaps[host] as string;
            var domain = domainAndPort.Split(":").First();
            var port = Convert.ToInt32(domainAndPort.Split(":").Last());

            MAMUtility.ClearTempFolder(userId, remoteIp);

            var targetFolder = MAMUtility.GetTempFolder(userId, remoteIp);
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var waveFileName = Path.GetFileName(wavPath);
            var wavTargetPath = MAMUtility.GetTempFilePath(userId, remoteIp, waveFileName);
            var egyTargetPath = Path.ChangeExtension(wavTargetPath, Define.EGY);

            TempDownloadCore(domain, port, jsonWavInfo, wavTargetPath);   //wav
            TempDownloadCore(domain, port, jsonEgyInfo, egyTargetPath);   //egy
        }
        /// <summary>
        /// wav, egy 파일 임시다운로드
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="remoteIp"></param>
        /// <param name="musicToken"></param>
        /// <param name="path"></param>
        public void TempDownloadCore(string domain, int port, string jsonRequestContent ,string targetSoundPath)
        {
            var builder = new UriBuilder("http", domain, port, FileUrl);
            HttpClient client = new HttpClient();
            StringContent sc = new StringContent(jsonRequestContent, Encoding.UTF8, "application/json");
            var response = client.PostAsync(builder.ToString(), sc).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //wav파일경로 이므로 wav파일만 받는다.  mp2를 변환할 일이 없음.
                using (var stream = response.Content.ReadAsStreamAsync().Result)
                {
                   using(var outStream = new FileStream(targetSoundPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        stream.CopyTo(outStream);
                    }
                }
            }
            else
            {
                _logger.LogError($"response status : {response.StatusCode}");
                _logger.LogError($"response status : {response.Content.ReadAsStringAsync().Result}");
                //로그처리
            }
        }

        public object[] GetRequestInfo(Dictionary<string, string> musicInfo)
        {
            var encodedFilePath = musicInfo[Define.MUSIC_FILEPATH];
            var filePath =  MusicSeedWrapper.SeedDecrypt(encodedFilePath);
            var host = MAMUtility.GetHost(filePath);
            var domainAndPort = StorageMaps[host] as string;        // 뮤직서비스에 구현.
            var domain = domainAndPort.Split(":").First();
            var port = Convert.ToInt32(domainAndPort.Split(":").Last());
            var fileName = Path.GetFileName(filePath);

            object[] returnData = new object[3];
            returnData[0] = domain;
            returnData[1] = port;
            returnData[2] = fileName;
            return returnData;
        }
       
    }
}

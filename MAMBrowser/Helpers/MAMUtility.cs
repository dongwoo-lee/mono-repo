using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Processor;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace MAMBrowser.Helpers
{
    
    public static class MAMUtility
    {
        public const string DTM8 = "yyyyMMdd";
        public const string DTM10 = "yyyy-MM-dd";
        public const string DTM19 = "yyyy-MM-dd HH:mm:ss";
        public const string WAV = ".WAV";
        public const string MP2 = ".MP2";
        public const string MP3 = ".MP3";
        public const string EGY = ".EGY";
        //public const string TIF = ".TIF";
        public const string JPG = ".JPG";
        public const string USER_ID = "UserId";

        public const string MUSIC_FILEPATH = "filePath";
        public const string MUSIC_IP = "ip";
        public const string MUSIC_EXPIRE = "expire";


        public static string LocalIpAddress { get; set; }
        public static string TempDownloadPath { get => Startup.AppSetting.TempDownloadPath; }
        public static string TokenIssuer { get => Startup.AppSetting.TokenIssuer; }
        public static string TokenSignature { get => Startup.AppSetting.TokenSignature; }


        public static FileStreamResult Download(string token, HttpResponse response, IFileService fileService, string inline)
        {
            string filePath = "";
            if (MAMUtility.ValidateMAMToken(token, ref filePath))
            {
                string fileName = Path.GetFileName(filePath);
                var fileExtProvider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!fileExtProvider.TryGetContentType(filePath, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                var stream = fileService.GetFileStream(filePath, 0);
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = WebUtility.UrlEncode(fileName),
                    Inline = inline == "Y" ? true : false
                };
                response.Headers.Add("Content-Disposition", cd.ToString());
                return new FileStreamResult(stream, contentType);
            }
            else
                throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
        }
        public static void TempDownload(string token, string userId, string remoteIp, IFileService fileService)
        {
            string filePath = "";
            if (MAMUtility.ValidateMAMToken(token, ref filePath))
            {
                string fileName = Path.GetFileName(filePath);

                var egyFileName = Path.GetFileNameWithoutExtension(filePath) + MAMUtility.EGY;
                var egyFilePath = filePath.Replace(fileName, egyFileName);

                MAMUtility.TempDownloadToLocal(userId, remoteIp, fileService, filePath);       //임시파일 다운로드
                MAMUtility.TempDownloadToLocal(userId, remoteIp, fileService, egyFilePath);    //파형임시파일 다운로드
            }
            else
                throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
        }
        public static List<float> GetWaveform(string token, string userId, string remoteIp)
        {
            string filePath = "";
            if (MAMUtility.ValidateMAMToken(token, ref filePath))
            {
                string fileName = Path.GetFileName(filePath);
                //파형검색시 mp2파일은 wav로 치환됨.
                fileName = Path.GetExtension(fileName).ToUpper() == MAMUtility.MP2 ? fileName.ToUpper().Replace(MAMUtility.MP2, MAMUtility.WAV) : fileName;
                string tempSoundPath = MAMUtility.GetTempFilePath(userId, remoteIp, fileName);
                return MAMUtility.GetWaveformCore(tempSoundPath);
            }
            else
                throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
        }
        public static IActionResult Streaming(string token, string userId, string remoteIp)
        {
            string filePath = "";
            if (MAMUtility.ValidateMAMToken(token, ref filePath))
            {
                string fileName = Path.GetFileName(filePath);
                fileName = Path.GetExtension(fileName).ToUpper() == MAMUtility.MP2 ? fileName.ToUpper().Replace(MAMUtility.MP2, MAMUtility.WAV) : fileName;
                //if (direct.ToUpper() == "Y")
                //{
                //    int fileSize = 0;
                //    return new PushStreamResult(filePath, fileName, fileSize, fileService);
                //}
                //else
                //{
                    var provider = new FileExtensionContentTypeProvider();
                    string contentType;
                    if (!provider.TryGetContentType(fileName, out contentType))
                    {
                        contentType = "application/octet-stream";
                    }
                    string tempDownloadedPath = MAMUtility.GetTempFilePath(userId, remoteIp, fileName);
                    var result = new PhysicalFileResult(tempDownloadedPath, contentType);
                    result.EnableRangeProcessing = true;
                    return result;
                //}
            }
            else
                throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
        }


        public static FileStreamResult DownloadFromPath(string filePath, HttpResponse response, IFileService fileService, string inline)
        {
            string fileName = Path.GetFileName(filePath);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!fileExtProvider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var stream = fileService.GetFileStream(filePath, 0);
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = WebUtility.UrlEncode(fileName),
                Inline = inline == "Y" ? true : false
            };
            response.Headers.Add("Content-Disposition", cd.ToString());
            return new FileStreamResult(stream, contentType);
        }
        public static void TempDownloadFromPath(string filePath, string userId, string remoteIp, IFileService fileService)
        {
            string fileName = Path.GetFileName(filePath);
            var egyFilePath = Path.ChangeExtension(filePath, MAMUtility.EGY);

            MAMUtility.TempDownloadToLocal(userId, remoteIp, fileService, filePath);       //임시파일 다운로드
            MAMUtility.TempDownloadToLocal(userId, remoteIp, fileService, egyFilePath);    //파형임시파일 다운로드
        }
        public static List<float> GetWaveformFromPath(string filePath, string userId, string remoteIp)
        {
            string fileName = Path.GetFileName(filePath);
            //파형검색시 mp2파일은 wav로 치환됨.
            fileName = Path.GetExtension(fileName).ToUpper() == MAMUtility.MP2 ? fileName.ToUpper().Replace(MAMUtility.MP2, MAMUtility.WAV) : fileName;
            string tempSoundPath = MAMUtility.GetTempFilePath(userId, remoteIp, fileName);
            return MAMUtility.GetWaveformCore(tempSoundPath);
        }
        public static IActionResult StreamingFromPath(string filePath, string userId, string remoteIp)
        {
            string relativePath = MAMUtility.GetRelativePath(filePath);
            string fileName = Path.GetFileName(filePath);
            fileName = Path.GetExtension(fileName).ToUpper() == MAMUtility.MP2 ? fileName.ToUpper().Replace(MAMUtility.MP2, MAMUtility.WAV) : fileName;
            //if (direct.ToUpper() == "Y")
            //{
            //    int fileSize = 0;
            //    return new PushStreamResult(relativePath, fileName, fileSize, fileService);
            //}
            //else
            //{
                var provider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!provider.TryGetContentType(fileName, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                string tempDownloadedPath = MAMUtility.GetTempFilePath(userId, remoteIp, fileName);
                var result = new PhysicalFileResult(tempDownloadedPath, contentType);
                result.EnableRangeProcessing = true;
                return result;
            //}
        }
        public static IActionResult StreamingFromFileName(string fileName, string userId, string remoteIp)
        {
            fileName = Path.GetExtension(fileName).ToUpper() == MAMUtility.MP2 ? fileName.ToUpper().Replace(MAMUtility.MP2, MAMUtility.WAV) : fileName;
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            string tempDownloadedPath = MAMUtility.GetTempFilePath(userId, remoteIp, fileName);
            var result = new PhysicalFileResult(tempDownloadedPath, contentType);
            result.EnableRangeProcessing = true;
            return result;
        }

        public static void TempDownloadToLocal(string userId, string remoteIp, IFileDownloadService fileService, string filePath)
        {
            ClearTempFolder(userId, remoteIp);
            var targetFolder = GetTempFolder(userId, remoteIp);
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var soundFileName = Path.GetFileName(filePath);
            var ext = Path.GetExtension(filePath).ToUpper();
            var targetSoundPath = GetTempFilePath(userId, remoteIp, soundFileName);
            if (fileService.ExistFile(filePath))
            {
                fileService.DownloadFile(filePath, targetSoundPath);
                //다운로드된 mp2는 wav로 변환함.(mp3는 상관없음)
                if (ext == MAMUtility.MP2)
                {
                    var convertFilePath = GetTempFilePath(userId, remoteIp, soundFileName.ToUpper().Replace(ext,MAMUtility.WAV));
                    using (FileStream inStream = new FileStream(targetSoundPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (FileStream outStream = new FileStream(convertFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            AudioEngine.ConvertMp2ToWav(inStream, outStream);
                        }
                    }
                }
            }
        }
        public static string GetTempFolder(string userId, string remoteIp)
        {
            if (string.IsNullOrEmpty(remoteIp) || remoteIp == "::1" || remoteIp== "127.0.0.1")
            {
                remoteIp = "localhost";
            }
            return @$"{TempDownloadPath}\{userId}_{remoteIp}";
        }
        public static string GetTempFilePath(string userId, string remoteIp, string fileName)
        {
            return @$"{GetTempFolder(userId, remoteIp)}\{fileName}";
        }

        public static void ClearTempFolder(string userId, string remoteIp)
        {
            var targetFolder = GetTempFolder(userId, remoteIp);
            if (Directory.Exists(targetFolder))
            {
                DateTime now = DateTime.Now;
                var fileFullPathList = Directory.GetFiles(targetFolder);
                foreach (var filePath in fileFullPathList)
                {
                    try
                    {
                        var createdDtm = File.GetLastAccessTime(filePath);
                        
                        if (now.Subtract(createdDtm).TotalSeconds > 300)
                            File.Delete(filePath);
                    }
                    catch (IOException ex)
                    {
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
        public static List<float> GetWaveformCore(string soundFilePath)
        {
            var soundfileName = Path.GetFileName(soundFilePath);
            var egyFileName = Path.GetFileNameWithoutExtension(soundFilePath) + EGY;
            var egyFilePath = soundFilePath.Replace(soundfileName, egyFileName);
            if (File.Exists(egyFilePath))
            {
                using (var stream = new FileStream(egyFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    return AudioEngine.GetDecibelFromEgy(stream);
                }
            }
            else
            {
                using (var stream = new FileStream(soundFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    var ext = Path.GetExtension(soundFilePath).ToUpper();
                    switch (ext)
                    {
                        case WAV:
                            return AudioEngine.GetDecibelFromWav(stream, 2);
                        //case MP2:       파형검색시 mp2파일은 wav로 치환됨.
                        //    break;
                        case MP3:
                            return AudioEngine.GetDecibelFromMp3(stream, 2);
                        default:
                            return new List<float>();
                    }
                }
            }
        }
    
       
        public static string SeedEncrypt(string data)
        {
            byte[] pbUserKey = { (byte)0x7e, (byte)0x7f, (byte)0x45, (byte)0x85, (byte)0x12, (byte)0x0d, (byte)0x6f, (byte)0xe7, (byte)0xdf, (byte)0xe9, (byte)0x8a, (byte)0x2d, (byte)0x14, (byte)0xca, (byte)0x0d, (byte)0x7f };
            byte[] bszIV =
            {
                (byte)0x01c, (byte)0x083, (byte)0x05c, (byte)0x09d,
                (byte)0x02b, (byte)0x09e, (byte)0x010, (byte)0x077,
                (byte)0x065, (byte)0x0b0, (byte)0x0cf, (byte)0x0f0,
                (byte)0x02c, (byte)0x00c, (byte)0x01b, (byte)0x00b
            };
            //Encoding euckr = Encoding.GetEncoding("euc-kr");
            var encodingCode = 51949;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding euckr = Encoding.GetEncoding("euc-kr");
            var encodingString = euckr.GetBytes(data);
            var seedData = KISA_SEED_CBC.SEED_CBC_Encrypt(pbUserKey, bszIV, encodingString, 0, encodingString.Length);
            var base64Data = Convert.ToBase64String(seedData);
            var utf8UrlEncodingData = HttpUtility.UrlEncode(base64Data);
            return utf8UrlEncodingData;
        }
        public static string SeedDecrypt(string data)
        {
            byte[] pbUserKey = { (byte)0x7e, (byte)0x7f, (byte)0x45, (byte)0x85, (byte)0x12, (byte)0x0d, (byte)0x6f, (byte)0xe7, (byte)0xdf, (byte)0xe9, (byte)0x8a, (byte)0x2d, (byte)0x14, (byte)0xca, (byte)0x0d, (byte)0x7f };
            byte[] bszIV =
            {
                (byte)0x01c, (byte)0x083, (byte)0x05c, (byte)0x09d,
                (byte)0x02b, (byte)0x09e, (byte)0x010, (byte)0x077,
                (byte)0x065, (byte)0x0b0, (byte)0x0cf, (byte)0x0f0,
                (byte)0x02c, (byte)0x00c, (byte)0x01b, (byte)0x00b
            };

            var decodeString = HttpUtility.UrlDecode(data);
            var seedData = Convert.FromBase64String(decodeString);
            var decrypByteArray = KISA_SEED_CBC.SEED_CBC_Decrypt(pbUserKey, bszIV, seedData, 0, seedData.Length);
            Encoding euckr = Encoding.GetEncoding("euc-kr");
            var strData = euckr.GetString(decrypByteArray);
            return strData;
        }
        public static string GenerateMAMToken(string data)
        {
            var now = DateTime.Now;
            var claims = new[] {
             new Claim("file", data)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSignature));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(TokenIssuer,
              null,
              claims,
              expires: now.AddHours(24),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static bool ValidateMAMToken(string token, ref string decodingData)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(TokenSignature);
                var result = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidAudience = TokenIssuer,
                    ValidIssuer = TokenIssuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                decodingData = jwtToken.Claims.First(x => x.Type == "file").Value;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string GetRelativePath(string filePath)
        {
            string domainFullPath = "";
            var domainPath = GetHost(filePath);
            var relativePath = filePath.Remove(0, filePath.IndexOf(domainPath) + domainPath.Length);
            
            if (relativePath[0] == '/' || relativePath[0] == '\\')
                relativePath = relativePath.Remove(0, 1);

            return relativePath;
        }
        public static string GetHost(string filePath)
        {
            string domainPath = "";
            if (filePath.IndexOf("\\\\") == 0)
            {
                domainPath = filePath.Replace("\\\\", "");
            }
            else
                domainPath = filePath;

            return domainPath.Split("\\").First();
        }
        public static string GenerateMusicToken(string filePath)
        {
            var strInfo = GetJsonRequestContentFromPath(filePath);
            return GenerateMAMToken(strInfo);
        }
        public static string GetJsonRequestContentFromPath(string filePath)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("filePath", SeedEncrypt(filePath));
            info.Add("ip", LocalIpAddress);
            info.Add("expire", DateTime.Now.AddHours(1).ToString(DTM19));
            var strInfo = System.Text.Json.JsonSerializer.Serialize(info);
            return strInfo;
        }
        public static string ParseToJsonRequestContent(string musicToken)
        {
            string strMusicToken = "";
            if (ValidateMAMToken(musicToken, ref strMusicToken))
            {
                return strMusicToken;
            }
            else
                throw new Exception("invalid token");
            
        }
        public static Dictionary<string, string> ParseToRequestContent(string musicToken)
        {
            var strToken = ParseToJsonRequestContent(musicToken);
            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(strToken);
        }
        public static string GetFilePathFromMusicToken(string musicToken)
        {
            var requestContent = MAMUtility.ParseToRequestContent(musicToken);
            var decodedFilePath = MAMUtility.SeedDecrypt(requestContent[MAMUtility.MUSIC_FILEPATH]);
            return decodedFilePath;
        }

        public static void InitTempFoler(string userId, string remoteIp)
        {
            MAMUtility.ClearTempFolder(userId, remoteIp);
            var targetFolder = MAMUtility.GetTempFolder(userId, remoteIp);
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);
        }
        public static string GetSortString(Type dtoType, string sortKey, string sortValue)
        {
            string sortFiled = "";
            string sortDirection = "";
            Dictionary<string, string> sortMap = new Dictionary<string, string>();
            foreach (var prop in dtoType.GetProperties())
            {
                var attrList = prop.GetCustomAttributes(true);
                foreach (var attr in attrList)
                {
                    if (attr.GetType() == typeof(SortNameAttribute))
                    {
                        sortMap.Add(prop.Name.ToUpper(), ((SortNameAttribute)attr).Name);
                        break;
                    }

                }
            }
            sortFiled = sortMap[sortKey.ToUpper()];
            if (string.IsNullOrEmpty(sortFiled))
                return "";

            switch (sortValue.ToUpper())
            {
                case "ASC":
                    sortDirection = "ASC";
                    break;
                case "DESC":
                    sortDirection = "DESC";
                    break;
                default:
                    sortDirection = "ASC";
                    break;
            }
            return $"ORDER BY {sortFiled} {sortDirection}";
        }

        public static string NetworkName()
        {
            return "사내망";
        }
    }
}

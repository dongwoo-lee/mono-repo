using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Processor;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.JsonWebTokens;
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
        public static string LocalIpAddress;
        public static string TempDownloadPath;

        public static string TokenIssuer { get; set; }
        public static string TokenSignature { get; set; }

        public static string TempDownloadToLocal(string remoteIp, IFileDownloadService fileService, string relativePath, out string contentType)
        {
            ClearTempDownloadFolder(remoteIp);
            
            var fileExtProvider = new FileExtensionContentTypeProvider();
            if (!fileExtProvider.TryGetContentType(relativePath, out contentType))
            {
                contentType = "application/octet-stream";
            }

            if (string.IsNullOrEmpty(remoteIp))
            {
                remoteIp = "system";
            }

            var targetFolder = @$"{TempDownloadPath}\{remoteIp}";
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var targetPath = @$"{targetFolder}\{ Guid.NewGuid().ToString()}.tmp";
            var fileExt = Path.GetExtension(relativePath).ToLower();
            if (fileExt.ToUpper() == ".WAV")
            {
                fileService.DownloadFile(relativePath, targetPath);
            }
            else if(fileExt.ToUpper() == ".MP2")
            {
                using (var mp2InputStream = fileService.GetFileStream(relativePath, 0))
                {
                    using (var outWavStream = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        AudioEngine.ConvertMp2ToWav(mp2InputStream, outWavStream);
                    }
                }
            }
            return targetPath;
        }

        public static void ClearTempDownloadFolder(string ip)
        {
            var targetFolder = @$"{TempDownloadPath}\{ip}";
            if (Directory.Exists(targetFolder))
            {
                var fileFullPathList = Directory.GetFiles(targetFolder);
                foreach (var filePath in fileFullPathList)
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
        public static List<float> GetWaveform(string ip, IFileDownloadService fileService, string relativePath)
        {
            string waveFileName = Path.GetFileName(relativePath);
            string waveformFileName = $"{Path.GetFileNameWithoutExtension(relativePath)}.egy";
            string waveformFilePath = relativePath.Replace(waveFileName, waveformFileName);

            if (fileService.ExistFile(waveformFilePath))
            {
                var downloadStream = fileService.GetFileStream(waveformFilePath, 0);
                return AudioEngine.GetDecibelFromEgy(downloadStream);
            }
            else
            {
                var fileExt = Path.GetExtension(relativePath);
                Stream inputStream = fileService.GetFileStream(relativePath, 0);
                if (fileExt.ToUpper() == ".MP2")
                {
                    var targetFolder = @$"{TempDownloadPath}\{ip}";
                    if (!Directory.Exists(targetFolder))
                        Directory.CreateDirectory(targetFolder);

                    var targetPath = @$"{targetFolder}\{ Guid.NewGuid().ToString()}.tmp";
                    FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite);
                    AudioEngine.ConvertMp2ToWav(inputStream, fs);

                    inputStream = new FileStream(targetPath, FileMode.Open, FileAccess.Read);
                }

                //var targetPath = @$"{TempDownloadPath}\{userId}\{Guid.NewGuid().ToString()}.tmp";
                //using (FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite))
                // 향후에 수정 필요. ftp는 wavefilereader로 직접 못읽으니..헤더크기만큼 빼고 바로 스트림으로 넘겨주고.
                // 함수안에서 바로 데이터 뽑아낼수 있도록..
                MemoryStream ms = new MemoryStream();
                inputStream.CopyTo(ms);
                inputStream.Dispose();

                ms.Position = 0;
                WaveFileReader reader = new WaveFileReader(ms);
                var data = AudioEngine.GetDecibelFromWav(ms, 2);
                ms.Dispose();
                return data;
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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSignature);
            try
            {
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
            var domainPath = GetDomain(filePath);
            return filePath.Remove(0, filePath.IndexOf(domainPath) + domainPath.Length);
        }
        public static string GetDomain(string filePath)
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
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("filePath", SeedEncrypt(filePath));
            info.Add("ip", LocalIpAddress);
            info.Add("expire", DateTime.Now.AddHours(1).ToString(DTM19));
            var strInfo = System.Text.Json.JsonSerializer.Serialize(info);
            return GenerateMAMToken(strInfo);
        }
        public static string ParseMusicToken(string musicFileToken)
        {
            string musicFileInfo = "";
            if (ValidateMAMToken(musicFileToken, ref musicFileInfo))
            {
                return musicFileInfo;
            }
            else
                throw new Exception("invalid token");
            
        }
        public static Dictionary<string, string> GetMusicInfo(string musicFileToken)
        {
            string strMusicInfo = "";
            if (ValidateMAMToken(musicFileToken, ref strMusicInfo))
            {
                return ParseMusicInfo(strMusicInfo);
            }
            else
                return null;
        }
        private static Dictionary<string, string> ParseMusicInfo(string strMusicInfo)
        {
            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(strMusicInfo);
        }

    }
}

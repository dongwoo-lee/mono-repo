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

        public static string TokenIssuer { get; set; }
        public static string TokenSignature { get; set; }

        public static string DownloadFile(IFileService fileService, string relativePath, string newFileName, out string contentType)
        {
            var fileExtProvider = new FileExtensionContentTypeProvider();
            if (!fileExtProvider.TryGetContentType(relativePath, out contentType))
            {
                contentType = "application/octet-stream";
            }

            var targetPath = @$"c:\tmpwork\{newFileName}";
            fileService.DownloadFile(relativePath, targetPath);
            return targetPath;
        }
        public static List<float> GetWaveform(IFileService fileService, string relativePath)
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

                var inputStream = fileService.GetFileStream(relativePath, 0);
                MemoryStream ms = new MemoryStream();
                inputStream.CopyTo(ms);
                ms.Position = 0;

                WaveFileReader reader = new WaveFileReader(ms);
                var data = AudioEngine.GetDecibelFromWav(ms, 2);
                inputStream.Close();
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

            Encoding euckr = Encoding.GetEncoding("euc-kr");
            var encodingString = euckr.GetBytes(data);
            var seedData = KISA_SEED_CBC.SEED_CBC_Encrypt(pbUserKey, bszIV, encodingString, 0, encodingString.Length);
            var base64Data = Convert.ToBase64String(seedData);
            var utf8UrlEncodingData = HttpUtility.UrlEncode(base64Data);
            return utf8UrlEncodingData;
        }
        public static string GetJwtFileToken(string filePath)
        {
            var now = DateTime.Now;
            var claims = new[] {
            new Claim("sub", "file"),
             new Claim("filePath", filePath)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSignature));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(TokenIssuer,
              TokenIssuer,
              claims,
              expires: now.AddHours(24),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static bool ValidateJwtFileToken(string token, ref string filePath)
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
                    ValidateAudience = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                filePath = jwtToken.Claims.First(x => x.Type == "filePath").Value;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string GetRelativePath(string filePath)
        {
            string domainPath = "";

            if (filePath.IndexOf("\\\\") == 0)
            {
                domainPath = filePath.Replace("\\\\", "");
            }
            return domainPath.Remove(0, domainPath.Split("\\").First().Length);
        }
        public static DTO_MUSIC_REQUEST GetMusicRequest(string filePath)
        {
            DTO_MUSIC_REQUEST info = new DTO_MUSIC_REQUEST();
            info.FilePath = SeedEncrypt(filePath);
            info.Ip = LocalIpAddress;
            info.Expire = DateTime.Now.AddDays(1).ToString(DTM19);
            return info;
        }
   
    }
}

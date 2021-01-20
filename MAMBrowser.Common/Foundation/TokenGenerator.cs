using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MAMBrowser.Common.Foundation
{
    public static class TokenGenerator
    {
        public static string TokenIssuer { get; set; }
        public static string TokenSignature { get; set; }
        public static string IPAddress { get; set; }
        public static int ExpireHour { get; set; }

        public static string GenerateFileToken(string data)
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
        public static bool ValidateFileToken(string token, ref string decodingData)
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
        public static string GenerateMusicToken(string filePath)
        {
            var expire = DateTime.Now.AddHours(ExpireHour);
            var strInfo = GetJsonRequestContentFromPath(filePath, expire);
            return GenerateFileToken(strInfo);
        }
        public static string GetJsonRequestContentFromPath(string filePath, DateTime expire)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add(Define.MUSIC_FILEPATH, MusicSeedWrapper.SeedEncrypt(filePath));
            info.Add(Define.MUSIC_IP, IPAddress);
            info.Add(Define.MUSIC_EXPIRE, expire.ToString(Define.DTM19));
            var strInfo = System.Text.Json.JsonSerializer.Serialize(info);
            return strInfo;
        }
    }
}

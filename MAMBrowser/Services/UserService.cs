using MAMBrowser.Controllers;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MAMBrowser.Services
{
    public interface IUserService
    {
        DTO_RESULT<DTO_USER_DETAIL> Authenticate(DTO_RESULT<DTO_USER_DETAIL> result, AuthenticateModel model);
        string RefreshToken(string userId);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly APIDAL _dal;

        public UserService(IOptions<AppSettings> appSettings, APIDAL dal)
        {
            _appSettings = appSettings.Value;
            _dal = dal;
        }

        public DTO_RESULT<DTO_USER_DETAIL> Authenticate(DTO_RESULT<DTO_USER_DETAIL> result, AuthenticateModel model)
        {
            DTO_USER_TOKEN userToken = _dal.Authenticate(model);

            if (userToken == null) return result;
            var jwtToken = generateJwtToken(userToken.ID);

            result.Token = jwtToken;

            return result;
        }

        public string RefreshToken(string userId)
        {
            var jwtToken = generateJwtToken(userId);
            return jwtToken;
        }

        private string generateJwtToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", userId),
                    //new Claim("authorCd", userToken.AuthorCD)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

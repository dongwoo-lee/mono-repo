using MAMBrowser.Controllers;
using MAMBrowser.DAL;
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
        string Authenticate(string id);
        DTO_RESULT<DTO_USER_DETAIL>  Reissue(DTO_RESULT<DTO_USER_DETAIL> result, AuthenticateModel model);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly APIDao _dao;

        public UserService(IOptions<AppSettings> appSettings, APIDao dao)
        {
            _appSettings = appSettings.Value;
            _dao = dao;
        }

        public string Authenticate(string id)
        {
            //DTO_USER_TOKEN userToken = _dal.Authenticate(model);

            //if (userToken == null) return result;
            //var jwtToken = GenerateJwtToken(userToken.ID);

            //result.Token = jwtToken;

            //return result;
            return GenerateJwtToken(id);
        }
        public DTO_RESULT<DTO_USER_DETAIL> Reissue(DTO_RESULT<DTO_USER_DETAIL> result, AuthenticateModel model)
        {
            var jwtToken = GenerateJwtToken(model.PERSONID);
            result.Token = jwtToken;
            return result;
        }
        

        private string GenerateJwtToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.TokenSignature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", userId),
                    //new Claim("authorCd", userToken.AuthorCD)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

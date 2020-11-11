using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private IUserService _userService;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            //var token = context.Request.Headers["X-Csrf-Token"].FirstOrDefault()?.Split(" ").Last();

            //if (token != null && token!="null")
            //    attachUserToContext(context, userService, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var signatureKey = Encoding.UTF8.GetBytes(_appSettings.TokenSignature);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = _appSettings.TokenIssuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signatureKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken); ;

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                context.Items["UserId"] = userId;
            }
            catch(Exception ex)
            {
                context.Items["UserId"] = null;
            }
        }
    }
}

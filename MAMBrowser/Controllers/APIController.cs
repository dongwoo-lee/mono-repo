using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {
        public APIController()
        {
        }

        [HttpPost("Authenticate")]
        public DTO_RESULT Authenticate([FromBody] AuthenticateModel account)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                APIBLL bll = new APIBLL();
                if (!bll.ExistUser(account.UserID))
                {
                    result.ErrorMsg = "User id not found";
                    result.ResultCode = RESUlT_CODES.INVALID_DATA;
                }
                else
                {
                    if (!bll.Authenticate(account.UserID, account.Pass))
                    {
                        result.ErrorMsg = "Password is incorrect";
                        result.ResultCode = RESUlT_CODES.INVALID_DATA;
                    }
                    else
                    {
                        var userDetail = bll.GetUserDetail(account.UserID);
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(SystemConfig.AppSettings.Signature);
                        var now = DateTime.Now;
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Issuer = "MAM",
                            Expires = now.AddHours(2),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                            IssuedAt = now,
                        };
                        tokenDescriptor.Claims = new Dictionary<string, object>();
                        tokenDescriptor.Claims.Add("UserInfo", userDetail);
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenString = tokenHandler.WriteToken(token);
                        result.Token = tokenString;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        /// <summary>
        /// 사용자 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_USER_DETAIL>> GetUserDetailList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_USER_DETAIL>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_USER_DETAIL>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.GetUserDetailList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 사용자 정보 업데이트
        /// </summary>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        [HttpPut("users")]
        public DTO_RESULT<DTO_RESULT_OBJECT<int>> UpdateUserDetail([FromBody] List<DTO_USER_DETAIL> dtoList)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<int>> result = new DTO_RESULT<DTO_RESULT_OBJECT<int>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject.Data = bll.UpdateUserDetail(dtoList);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 특정 사용자 조회
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>      
        [HttpGet("users/{id}")]
        public DTO_RESULT<DTO_USER_DETAIL> GetUserDetail(string id)
        {
            DTO_RESULT<DTO_USER_DETAIL> result = new DTO_RESULT<DTO_USER_DETAIL>();
            try
            {
                APIBLL bll = new APIBLL();
                string authorization = Request.Headers["Authorization"];
                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string token = authorization.Substring("Bearer ".Length).Trim();

                    result.ResultObject = bll.GetUserDetail(id);
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }
                
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>      
        [HttpGet("users/{id}/menu")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>> GetMenu(string id)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject.Data = bll.GetMenu(id);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>      
        [HttpGet("menugrp")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>> GetMenuGrpList(string id)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject.Data = bll.GetMenuGrpList();
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>      
        [HttpGet("users/{id}/menu")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>> GetBehavior(string authorCd)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject.Data = bll.GetBehavior(authorCd);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>      
        [HttpGet("authorlist")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>> GetAuthorList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject.Data = bll.GetAuthorList();
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 역할 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_ROLE_DETAIL>> GetRoleList()
        {
            
             DTO_RESULT<DTO_RESULT_LIST<DTO_ROLE_DETAIL>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_ROLE_DETAIL>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.GetRoleDetailList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 역할 정보 업데이트
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        [HttpPut("roles")]
        public DTO_RESULT UpdateRole([FromBody] List<DTO_ROLE_DETAIL> dtoList)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        /// <summary>
        /// ?
        /// </summary>
        /// <returns></returns>
        [HttpGet("config")]
        public DTO_RESULT Getconfig()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// ?
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("config")]
        public DTO_RESULT UpdateConfig([FromBody] string dto)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

    }
}

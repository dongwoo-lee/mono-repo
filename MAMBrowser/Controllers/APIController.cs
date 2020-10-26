﻿using MAMBrowser.DTO;
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
        private readonly AppSettings _appSesstings;
        public APIController(IOptions<AppSettings> appSesstings)
        {
            _appSesstings = appSesstings.Value;
        }

        /// <summary>
        /// 인증 서비스
        /// </summary>
        /// <param name="account">인증 정보</param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        public DTO_RESULT<DTO_USER_DETAIL> Authenticate([FromBody] AuthenticateModel account)
        {
            DTO_RESULT<DTO_USER_DETAIL> result = new DTO_RESULT<DTO_USER_DETAIL>();
            try
            {
                APIBLL bll = new APIBLL();
                if (!bll.ExistUser(account))
                {
                    result.ErrorMsg = "ID 를 찾을 수 없습니다.";
                    result.ResultCode = RESUlT_CODES.DENY_ACCESS;
                }
                else
                {
                    DTO_USER_TOKEN userToken = bll.Authenticate(account);
                    if (userToken == null)
                    {
                        result.ErrorMsg = "비밀번호가 틀립니다.";
                        result.ResultCode = RESUlT_CODES.DENY_ACCESS;
                    }
                    else
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var Signature = Encoding.ASCII.GetBytes(_appSesstings.TokenSignature);
                        var now = DateTime.UtcNow;
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Issuer = _appSesstings.TokenIssuer,
                            Expires = now.AddHours(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Signature), SecurityAlgorithms.HmacSha256Signature),
                            IssuedAt = now,
                            NotBefore = now,
                        };
                        tokenDescriptor.Claims = new Dictionary<string, object>();
                        tokenDescriptor.Claims.Add("accessInfo", userToken);
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenString = tokenHandler.WriteToken(token);
                        result.Token = tokenString;

                        result.ResultObject = GetUserDetail(account.PERSONID).ResultObject;
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL>> GetUserDetailList()
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL>>();
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
        public DTO_RESULT UpdateUserDetail([FromBody] List<UserExtModel> dtoList)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                APIBLL bll = new APIBLL();
                var updateCount = bll.UpdateUserDetail(dtoList);
                if (updateCount > 0)
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                else
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 특정 사용자 조회(개요)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>      
        [HttpGet("users/summary/{id}")]
        public DTO_RESULT<DTO_USER_DETAIL> GetUserSummary(string id)
        {
            DTO_RESULT<DTO_USER_DETAIL> result = new DTO_RESULT<DTO_USER_DETAIL>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.GetUserSummary(id);
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
        /// 특정 사용자 조회(상세)
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
                var user = bll.GetUserSummary(id);
                var menuList = bll.GetMenu(id);
                var lookup = menuList.ToLookup(menu => menu.ParentID);
                menuList.ForEach(menu =>
                {
                    if (menu.ParentID == "S01G01")
                    {
                        user.MenuList.Add(menu);//최상위 메뉴
                    }

                    if (lookup.Contains(menu.ID))
                    {
                        //자식이있으면 자식추가.
                        menu.Children = lookup[menu.ID].ToList();
                    }
                });

                user.BehaviorList = bll.GetBehavior(user.AuthorCD);

                result.ResultObject = user;
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
        /// 사용자별 메뉴목록 조회
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
                result.ResultObject = new DTO_RESULT_LIST<DTO_MENU>();
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
        /// 메뉴그룹별 메뉴목록 조회
        /// </summary>
        /// <param name="grpId">메뉴그룹 ID </param>
        /// <returns></returns>      
        [HttpGet("menugrp/{grpId}")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>> GetMenuByGrpId(string grpId)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = new DTO_RESULT_LIST<DTO_MENU>();
                result.ResultObject.Data = bll.GetMenuByGrpId(grpId);
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
        /// 메뉴그룹 목록조회
        /// </summary>
        /// <returns></returns>      
        [HttpGet("menugrp")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>> GetMenuGrpList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.GetMenuGrpList();
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
        /// 권한별 행위목록 조회
        /// </summary>
        /// <param name="authorCd">권한코드 : </param>
        /// <returns></returns>      
        [HttpGet("behaviors/{authorCd}")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>> GetBehavior(string authorCd)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MENU>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = new DTO_RESULT_LIST<DTO_MENU>();
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
        /// 권한목록 조회
        /// </summary>
        /// <returns></returns>      
        [HttpGet("authority-list")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>> GetAuthorList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_COMMON_CODE>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.GetAuthorList();
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
        /// <param name="dtoList"></param>
        /// <returns></returns>
        [HttpPut("roles")]
        public DTO_RESULT UpdateRole([FromBody] List<RoleExtModel> dtoList)
        {

            DTO_RESULT result = new DTO_RESULT();
            try
            {
                APIBLL bll = new APIBLL();
                var updateCount = bll.UpdateRole(dtoList);
                if (updateCount > 0)
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
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


        /// <summary>
        /// Authorization 헤더 토큰 테스트용 API : ex) Bearer token(jwt)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("accesssecurity")]
        public DTO_RESULT AccessSecurity()
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start_dt">검색 시작일</param>
        /// <param name="end_dt">검색 종료일</param>
        /// <param name="logLevel">로그 유형 : ex) DEBUG, INFO, WARN,ERROR</param>
        /// <param name="userName">사용자 이름</param>
        /// <param name="description">내용</param>
        /// <returns></returns>
        [HttpGet("Logs")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_LOG>> FindLogs([FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string logLevel, [FromQuery] string userName, [FromQuery] string description)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_LOG>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_LOG>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.FindLogs(start_dt, end_dt, logLevel, userName, description);
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
        /// 디스크 할당 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("disk-scope")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_NAMEVALUE>> GetAssignDiskList()
        {

            DTO_RESULT<DTO_RESULT_LIST<DTO_NAMEVALUE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_NAMEVALUE>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = new DTO_RESULT_LIST<DTO_NAMEVALUE>();
                result.ResultObject.Data = _appSesstings.DiskScope;
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
    }
}

﻿using MAMBrowser.BLL;
using M30.AudioFile.Common;
using M30.AudioFile.DAL;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Models;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {
        private readonly AppSettings _appSesstings;
        private readonly APIBll _bll;
        private IUserService _userService;

        public APIController(IOptions<AppSettings> appSesstings, APIBll bll, IUserService userService)
        {
            _appSesstings = appSesstings.Value;
            _bll = bll;
            _userService = userService;
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
                var clientIp = HttpContext.Connection.RemoteIpAddress;
                if (!_bll.ExistUser(account))
                {
                    result.ErrorMsg = "ID 를 찾을 수 없습니다.";
                    result.ResultCode = RESUlT_CODES.DENY_ACCESS;
                }
                else
                {
                    
                    if (!_bll.Authenticate(account))
                    {
                        result.ErrorMsg = "비밀번호가 틀립니다.";
                        result.ResultCode = RESUlT_CODES.DENY_ACCESS;
                    }
                    else
                    {
                        result.Token = _userService.Authenticate(account.PERSONID);
                        result.ResultObject = GetUserDetail(account.PERSONID, clientIp).ResultObject;
                        result.ResultCode = RESUlT_CODES.SUCCESS;
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
        /// 인증 연장
        /// </summary>
        /// <param name="account">인증 연장</param>
        /// <returns></returns>
        [HttpPost("Renewal")]
        public DTO_RESULT<DTO_USER_DETAIL> Renewal([FromBody] AuthenticateModel account)    //id만 가지고 인증 연장 (새로고침?), 토큰 + 프론트에서 권한메뉴 모두 새로고침.
        {
            DTO_RESULT<DTO_USER_DETAIL> result = new DTO_RESULT<DTO_USER_DETAIL>();
            try
            {
                //if (HttpContext.Items["User"] as string == account.PERSONID)  //인증작업 완료 이후 주석 풀 예정
                //{

                var clientIp = HttpContext.Connection.RemoteIpAddress;
                result = _userService.Reissue(result, account);
                result.ResultObject = GetUserDetail(account.PERSONID, clientIp).ResultObject;
                result.ResultCode = RESUlT_CODES.SUCCESS;
                //}
                //else
                //{
                //    result.ErrorMsg = "잘못된 요청입니다.";
                //    result.ResultCode = RESUlT_CODES.SUCCESS;
                //}
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        /// <summary>
        /// 토큰 재발급
        /// </summary>
        /// <param name="account">토큰 재발급</param>
        /// <returns></returns>
        [HttpPost("Reissue")]
        public DTO_RESULT<DTO_USER_DETAIL> Reissue([FromBody] AuthenticateModel account)    //토큰이 유효하다면 신규토큰을 재발급(로그인 연장버튼), 토큰만 갱신처리.
        {
            DTO_RESULT<DTO_USER_DETAIL> result = new DTO_RESULT<DTO_USER_DETAIL>();
            try
            {
                //if (HttpContext.Items["User"] as string == account.PERSONID)  //인증작업 완료 이후 주석 풀 예정
                //{
                result = _userService.Reissue(result, account);
                result.ResultCode = RESUlT_CODES.SUCCESS;
                //}
                //else
                //{
                //    result.ErrorMsg = "잘못된 요청입니다.";
                //    result.ResultCode = RESUlT_CODES.SUCCESS;
                //}
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
                result.ResultObject = _bll.GetUserDetailList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 사용자 정보 업데이트
        /// </summary>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        [HttpPut("users")]
        public DTO_RESULT UpdateUserDetail([FromBody] List<M30_COMM_USER_EXT> dtoList)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                var updateCount = _bll.UpdateUserDetail(dtoList);
                if (updateCount > 0)
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                else
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                result.ResultObject = _bll.GetUserSummary(id);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
            var clientIp = HttpContext.Connection.RemoteIpAddress;
            return GetUserDetail(id, clientIp);
        }
        private DTO_RESULT<DTO_USER_DETAIL> GetUserDetail(string id, IPAddress clientIp)
        {
            DTO_RESULT<DTO_USER_DETAIL> result = new DTO_RESULT<DTO_USER_DETAIL>();
            try
            {
                var user = _bll.GetUserSummary(id);
                var menuList = _bll.GetMenu(id);
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

                user.BehaviorList = _bll.GetBehavior(user.AuthorCD);
                user.ConDBName = _appSesstings.DBName;
                user.ConNetworkName = MAMUtility.NetworkName(_appSesstings.BroadcastStartNetwork, _appSesstings.BroadcastEndNetwork, clientIp);
                result.ResultObject = user;
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                result.ResultObject = new DTO_RESULT_LIST<DTO_MENU>();
                result.ResultObject.Data = _bll.GetMenu(id);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                result.ResultObject = new DTO_RESULT_LIST<DTO_MENU>();
                var resultMenuGrpList = _bll.GetMenuByGrpId(grpId);
                var parentMenuList = resultMenuGrpList.FindAll(menugrp => menugrp.ParentID == "S01G01");
                resultMenuGrpList.GroupBy(menu => menu.ParentID).ToList().ForEach(data =>
                {
                    var pMenu = parentMenuList.Find(pMenu => pMenu.ID == data.Key);
                    if (pMenu != null)
                        pMenu.Children.AddRange(data.ToList());
                });

                result.ResultObject.Data = parentMenuList;
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                result.ResultObject = _bll.GetMenuGrpList();
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                result.ResultObject = new DTO_RESULT_LIST<DTO_MENU>();
                result.ResultObject.Data = _bll.GetBehavior(authorCd);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                result.ResultObject = _bll.GetAuthorList();
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                result.ResultObject = _bll.GetRoleDetailList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 역할 정보 업데이트
        /// </summary>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        [HttpPut("roles")]
        public DTO_RESULT UpdateRole([FromBody] List<M30_COMM_ROLE_EXT> dtoList)
        {

            DTO_RESULT result = new DTO_RESULT();
            try
            {
                var updateCount = _bll.UpdateRole(dtoList);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_LOG>> FindLogs([FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string logLevel, [FromQuery] string userName, [FromQuery] string description, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue, [FromServices]LogBll logBll )
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_LOG>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_LOG>>();
            try
            {
                result.ResultObject = logBll.SearchLog(start_dt, end_dt, logLevel, userName, description, rowPerPage, selectPage, sortKey, sortValue);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                result.ResultObject = new DTO_RESULT_LIST<DTO_NAMEVALUE>();
                result.ResultObject.Data = _appSesstings.DiskScope;
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionGrpCd">옵션그룹코드</param>
        /// <returns></returns>
        [HttpGet("options/{optionGrpCd}")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_NAMEVALUE>> GetOptions(string optionGrpCd)
        {
            //string systemCode = systemcd.ToUpper();
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("options")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_NAMEVALUE>> SetOptions([FromBody] M30_COMM_OPTION_GRP optionGrp)
        {
            //string systemCode = systemcd.ToUpper();
            return null;
        }
    }
}

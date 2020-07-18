using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {
        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="body"></param>
        /// <returns>토큰 반환</returns>
        [HttpPost("login")]
        public DTO_RESULT Login([FromBody] string body)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        /// <summary>
        /// 사용자 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("userlist")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> GetUserList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_USER>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.GetUserList();
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
        /// 사용자 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("userlist/detaill")]
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
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("userlist/detail")]
        public DTO_RESULT<int> UpdateUserDetail([FromBody] List<DTO_USER_DETAIL> dtoList)
        {
            DTO_RESULT<int> result = new DTO_RESULT<int>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.UpdateUserDetail(dtoList);
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
        [HttpGet("userlist/detail/{id}")]
        public DTO_RESULT<DTO_USER_DETAIL> GetUserDetail(string id)
        {
            DTO_RESULT<DTO_USER_DETAIL> result = new DTO_RESULT<DTO_USER_DETAIL>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.GetUserDetail(id);
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
        [HttpGet("roles/detail")]
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
        [HttpPut("roles/detail")]
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

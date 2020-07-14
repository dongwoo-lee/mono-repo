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
        public DTO_RESULT GetUserList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        /// <summary>
        /// 사용자 정보 업데이트
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("user")]
        public DTO_RESULT UpdateUser([FromBody] List<DTO_USER_EXT> dtoList)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        /// <summary>
        /// 특정 사용자 조회
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>      
        [HttpGet("user")]
        public DTO_RESULT GetUser([FromQuery] string id)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 역할 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public DTO_RESULT GetRoleList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 역할 정보 업데이트
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        [HttpPut("roles")]
        public DTO_RESULT UpdateRole([FromQuery] string id, [FromBody] DTO_ROLE_EXT dtoList)
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

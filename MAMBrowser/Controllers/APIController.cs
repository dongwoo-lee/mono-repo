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
        [HttpPost("login")]
        public DTO_RESULT Login([FromBody] string body)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        [HttpGet("userlist")]
        public DTO_RESULT GetUserList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpPut("user")]
        public DTO_RESULT UpdateUser([FromQuery] string id, [FromBody] DTO_USER_EXT body)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("user")]
        public DTO_RESULT GetUser([FromQuery] string id)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("roles")]
        public DTO_RESULT GetRoleList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpPut("roles")]
        public DTO_RESULT UpdateRole([FromQuery] string id, [FromBody] DTO_ROLE_EXT body)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        [HttpGet("config")]
        public DTO_RESULT Getconfig()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpPut("config")]
        public DTO_RESULT UpdateConfig([FromBody] string body)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

    }
}

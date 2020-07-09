using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIController : ControllerBase
    {
        [HttpPost("login")]
        public DTO_RESULT Login()
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
        public DTO_RESULT UpdateUser(string id)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("user")]
        public DTO_RESULT GetUser(string id)
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
        public DTO_RESULT UpdateRole(string id)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
       
        [HttpGet("roles")]
        public DTO_RESULT Getconfig()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpPut("roles")]
        public DTO_RESULT UpdateConfig()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        
    }
}

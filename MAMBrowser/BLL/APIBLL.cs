using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    public class APIBLL 
    {
        public DTO_RESULT Login([FromBody] string body)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        public DTO_RESULT GetUserList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        public DTO_RESULT UpdateUser([FromBody] List<DTO_USER_EXT> dtoList)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        public DTO_RESULT GetUser(string id)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        public DTO_RESULT GetRoleList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        public DTO_RESULT UpdateRole(string id, [FromBody] DTO_ROLE_EXT dtoList)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        public DTO_RESULT Getconfig()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        public DTO_RESULT UpdateConfig([FromBody] string dto)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

    }
}

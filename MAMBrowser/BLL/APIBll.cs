using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class APIBll 
    {
        private readonly APIDao _dao;
        public APIBll(APIDao dao)
        {
            _dao = dao;
        }
        public DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL> GetUserDetailList()
        {
            return _dao.GetUserDetailList();
        }
        public int UpdateUserDetail(List<M30_COMM_USER_EXT> updateDtoList)
        {
            return _dao.UpdateUserDetail(updateDtoList);
        }

        public DTO_USER_DETAIL GetUserSummary(string id)
        {
            return _dao.GetUserSummary(id);
        }
        public List<DTO_MENU> GetMenu(string id)
        {
            return _dao.GetMenu(id);
        }
        public List<DTO_MENU> GetMenuByGrpId(string grpId)
        {
            return _dao.GetMenuByGrpId(grpId);
        }
        public List<DTO_MENU> GetBehavior(string authorCd)
        {
            return _dao.GetBehavior(authorCd);
        }
        public DTO_RESULT_LIST<DTO_COMMON_CODE> GetAuthorList()
        {
            return _dao.GetAuthorList();
        }  
        public DTO_RESULT_LIST<DTO_COMMON_CODE> GetMenuGrpList()
        {
            return _dao.GetMenuGrpList();
        }
        public bool ExistUser(AuthenticateModel user)
        {
            return _dao.ExistUser(user);
        }
        public bool Authenticate(AuthenticateModel user)
        {
            return _dao.Authenticate(user);
        }
        private DTO_USER_TOKEN GetToken(string id)
        {
            var userDetail = GetUserSummary(id);
            DTO_USER_TOKEN token = new DTO_USER_TOKEN(userDetail);
            return token;
        }
        public DTO_RESULT_LIST<DTO_ROLE_DETAIL> GetRoleDetailList()
        {
            return _dao.GetRoleDetailList();
        }
       
        public int UpdateRole(List<M30_COMM_ROLE_EXT> updateDtoList)
        {
            return _dao.UpdateRole(updateDtoList);
        }
    }
}

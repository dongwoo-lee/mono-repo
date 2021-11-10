using Dapper;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Models;
using M30.AudioFile.DAL;
using M30.AudioFile.DAL.Dao;
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

        public void SetOptions(string optionGrpCd, List<DTO_NAMEVALUE> options)
        {
            var selectOptions = _dao.GetOptions(optionGrpCd);
            if (selectOptions == null || selectOptions.Count == 0)
                _dao.InsertOptions(optionGrpCd, options);
            else if (selectOptions.Count!= options.Count)
            {
                _dao.DeleteOptions(optionGrpCd);
                _dao.InsertOptions(optionGrpCd, options);
            }
            else
                _dao.UpdateOptions(optionGrpCd, options);
        }
        public IList<DTO_NAMEVALUE> GetOptions(string optionGrpCd)
        {
            return _dao.GetOptions(optionGrpCd);
        }
        public IList<DTO_MASTERING_INFO> GetMasteringStatus(String startDt, string endDt, string workStatus)
        {
            return _dao.GetMasteringStatus(startDt, endDt, workStatus);
        }
    }
}

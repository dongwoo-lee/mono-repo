using Dapper;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Models;
using M30.AudioFile.DAL;
using M30.AudioFile.DAL.Dao;
using M30.AudioFile.DAL.Dto;
using MAMBrowser.MAMDto;
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
        public int UpdateUserOption(M30_COMM_USER_EXT dto)
        {
            return _dao.UpdateUserOption(dto);
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

        public void SetOptions(string optionGrpCd, List<Dto_MasteringOptions> options)
        {
            var selectOptions = _dao.GetOptions(optionGrpCd);
            if (selectOptions == null || selectOptions.Count == 0)
                _dao.InsertOptions(optionGrpCd, options);
            else
                _dao.UpdateOptions(optionGrpCd, options);
        }
        public IList<Dto_MasteringOptions> GetOptions(string optionGrpCd)
        {
            return _dao.GetOptions(optionGrpCd);
        }
        public IList<DTO_MASTERING_INFO> GetMasteringStatus(string userId)
        {
            List<string> workStatus = new List<string>();
            workStatus.Add("0");
            workStatus.Add("1");
            workStatus.Add("2");
            workStatus.Add("3");
            workStatus.Add("4");
            return _dao.GetMasteringStatus(null, null, userId, workStatus);
        }
        public IList<DTO_MASTERING_INFO> GetMasteringLogs(string startDt, string endDt, string userId)
        {
            List<string> workStatus = new List<string>();
            workStatus.Add("5");
            workStatus.Add("6");
            return _dao.GetMasteringStatus(startDt, endDt, userId, workStatus);
        }
        public IList<DTO_MENU> GetMasteringAuthority(string menuGrpCd)
        {
            return _dao.GetMasteringAuthority(menuGrpCd);
        }
        public IList<DTO_MENU> GetMasteringCategories(string menuGrpCd)
        {
            return _dao.GetMasteringCategories(menuGrpCd);
        }
    }
}

using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Models;
using System;

namespace MAMBrowser.BLL
{
    public class CategoriesBll
    {
        private readonly CategoriesDao _dao;
        public CategoriesBll(CategoriesDao dao)
        {
            _dao = dao;
        }
        public DTO_RESULT_LIST<DTO_USER> GetUserList()
        {
            return _dao.GetUserList();
        }
        public DTO_RESULT_LIST<DTO_USER> GetPDUserList()
        {
            return _dao.GetPDUserList();
        }
        public DTO_RESULT_LIST<DTO_USER> GetReportUserList()
        {
            return _dao.GetReportUserList();
        }
        public DTO_RESULT_LIST<DTO_USER> GetMDUserList()
        {
            return _dao.GetMDUserList();
        }

        public DTO_RESULT_LIST<DTO_CATEGORY> GetMedia()
        {
            return _dao.GetMedia();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetMcrSpotMedia()
        {
            return _dao.GetMcrSpotMedia();
        }
        
        public DTO_RESULT_LIST<DTO_CATEGORY> GetReport()
        {
            return _dao.GetReport();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetPro()
        {
            return _dao.GetPro();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetCM()
        {
            return _dao.GetCM();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetMcrSpot(string media)
        {
            return _dao.GetMcrSpot(media);
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerPr()
        {
            return _dao.GetFillerPr();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerGeneral()
        {
            return _dao.GetFillerGeneral();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerTimetone()
        {
            return _dao.GetFillerTimetone();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerETC()
        {
            return _dao.GetFillerETC();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetPgmCodes(string brd_dt, string media)
        {
            return _dao.GetPgmCodes(brd_dt, media);
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetPublicPrimary()
        {
            return _dao.GetPublicPrimary();
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetReqStatus()
        {
            return _dao.GetReqStatus();
        }

        public DTO_RESULT_LIST<DTO_CATEGORY> GetPublicSecond(string primaryCode, string userId)
        {
            return _dao.GetPublicSecond(primaryCode, userId);
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetDLDeviceList()
        {
            return _dao.GetDLDeviceList();
        }

        public void InsertPublicCategory(M30_COMM_CODE model)
        {
            _dao.InsertPublicCategory(model);
        }
        public void UpdatePublicCategory(M30_COMM_CODE model)
        {
            _dao.UpdatePublicCategory(model);
        }
        public void DeletePublicCategory(string key)
        {
            _dao.DeletePublicCategory(key);
        }
        public void InsertUserToPublicCategory()
        {
            _dao.InsertUserToPublicCategory();
        }

    }
}

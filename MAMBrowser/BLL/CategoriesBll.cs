using Dapper;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Models;
using M30.AudioFile.DAL;
using M30.AudioFile.DAL.Dao;
using System;
using System.Collections.Generic;

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
        public DTO_RESULT_LIST<DTO_CATEGORY> GetScrSpot()
        {
            return _dao.GetScrSpot();
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
            //_dao.InsertUserToPublicCategory();
        }

        /// <summary>
        /// 프로그램 목록 반환(PGM)
        /// </summary>
        /// <param name="media">매체ID</param>
        /// <param name="date">시작일자</param>
        /// <returns></returns>
        public IList<DTO_BrdSch> GetPgmSch(string media, string date)
        {
            return _dao.GetPgmSch(media, date, "P", "PM");
        }
        /// <summary>
        /// 프로그램 목록 반환(주조SPOT, 변동소재, 고정소재)
        /// </summary>
        /// <param name="media">매체ID</param>
        /// <param name="date">시작일자</param>
        /// <param name="spotType">MS : 주조SPOT , TS : 변동소재, TT : 고정소재</param>
        /// <returns></returns>
        public IList<DTO_BrdSpot> GetSpotSch(string media, string date, string spotType)
        {
            return _dao.GetSpotSch(media, date, spotType);
        }

        /// <summary>
        /// 특정 사용자가 담당하는 프로그램 목록 반환
        /// </summary>
        /// <param name="media">매체ID</param>
        /// <param name="userId">유저ID</param>
        /// <returns></returns>
        public IList<string> GetPgmCodeByUser(string media, string userId)
        {
            return _dao.GetPgmCodeByUser(media, userId);
        }

        /// <summary>
        /// 특정 사용자가 담당하는 오디오코드 목록 반환((구)프로소재 목록)
        /// </summary>
        /// <param name="userId">유저ID</param>
        /// <returns></returns>
        public IList<DTO_CATEGORY> GetAudioCodeByUser(string userId)
        {
            return _dao.GetAudioCodeByUser(userId);
        }

        /// <summary>
        /// 부조SPOT 소재 목록 조회
        /// </summary>
        /// <returns></returns>
        public IList<Dto_ScrSpot> GetScrSpotList(string spotName, string codeId, string cmOwner, string startDate, string endDate)
        {
            return _dao.GetScrSpotList(spotName, codeId, cmOwner, startDate, endDate);
        }
    }
}

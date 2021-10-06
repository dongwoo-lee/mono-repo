using Dapper;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.DAL;
using M30.AudioFile.DAL.Dao;
using System;
using System.IO;

namespace MAMBrowser.BLL
{
    public class ProductsBll
    {
        private readonly ProductsDao _dao;
        public ProductsBll(ProductsDao dao)
        {
            _dao = dao;
        }
        public DTO_RESULT_PAGE_LIST<DTO_PGM_INFO> FindPGM(string media, string brd_dt, string pgm, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FindPGM(media, brd_dt, pgm, editor, rowPerPage, selectPage, sortKey, sortValue);
        }
        public DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT> FindSCRSpot(string media, string start_dt, string end_dt, string pgmName, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FindSCRSpot(media, start_dt, end_dt, pgmName, editor, name, rowPerPage, selectPage, sortKey, sortValue);
        }
        public DTO_RESULT_PAGE_LIST<DTO_REPORT> FindReport(string cate, string start_dt, string end_dt, string isMastering, string pgmName, string editor, string reporterName, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FindReport(cate, start_dt, end_dt, isMastering, pgmName, editor, reporterName, name, rowPerPage, selectPage, sortKey, sortValue);
        }
        public DTO_RESULT_PAGE_LIST<DTO_PRO> FindOldPro(string media, string cate, string start_dt, string end_dt, string type, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FindOldPro(media, cate, start_dt, end_dt, type, editor, name, rowPerPage, selectPage, sortKey, sortValue);
        }
        public DTO_RESULT_PAGE_LIST<DTO_SB> FindSB(string viewName, string media, string brd_dt, string pgm)
        {
            return _dao.FindSB(viewName, media, brd_dt, pgm);
        }
        public DTO_RESULT_PAGE_LIST<DTO_SB_CONTENT> FindSBContents(string brd_dt, string sbID)
        {
            return _dao.FindSBContents(brd_dt, sbID);
        }
        public DTO_RESULT_PAGE_LIST<DTO_CM> FindCM(string media, string brd_dt, string cate, string pgmName)
        {
            return _dao.FindCM(media, brd_dt, cate, pgmName);
        }
        public DTO_RESULT_PAGE_LIST<DTO_CM_CONTENT> FindCMContents(string brd_dt, string cmGrpID)
        {
            return _dao.FindCMContents(brd_dt, cmGrpID);
        }
        public DTO_RESULT_PAGE_LIST<DTO_MCR_SPOT> FindMcrSpot(string media, string start_dt, string end_dt, string spotId, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FindMcrSpot(media, start_dt, end_dt, spotId, editor, rowPerPage, selectPage, sortKey, sortValue);
        }
        public DTO_RESULT_PAGE_LIST<DTO_FILLER> FindFiller(string viewName, string brd_dt, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FindFiller(viewName, brd_dt, cate, editor, name, rowPerPage, selectPage, sortKey, sortValue);
        }
        public DTO_RESULT_PAGE_LIST<DTO_FILLER_TIME> FindFillerTime(string media, string start_dt, string end_dt, string cate, string status, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FindFillerTime(media, start_dt, end_dt, cate, status, editor, name, rowPerPage, selectPage, sortKey, sortValue);
        }
        public DTO_RESULT_PAGE_LIST<DTO_DL30> FineDLArchive(string media, string brd_dt, long? dlDeviceSeq, string name, string sortKey, string sortValue)
        {
            return _dao.FineDLArchive(media, brd_dt, dlDeviceSeq, name, sortKey, sortValue);
        }
        public DTO_DL30 GetDLArchive(long seq)
        {
            return _dao.GetDLArchive(seq);
        }
    }
}

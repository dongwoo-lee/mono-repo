using MAMBrowser.DAL;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class LogBll
    {
        private readonly LogDao _dao;
        public LogBll(LogDao dao)
        {
            _dao = dao;
        }
        public DTO_RESULT_PAGE_LIST<DTO_LOG> SearchLog(string start_dt, string end_dt, string logLevel, string userName, string description, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.SearchLog(start_dt, end_dt, logLevel, userName, description, rowPerPage, selectPage, sortKey, sortValue);
        }
    }
}

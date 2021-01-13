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
        private const string DEBUG = "DEBUG";
        private const string INFO = "INFO";
        private const string WARN = "WARN";
        private const string ERROR = "ERROR";
       
        private readonly LogDao _dao;
        public LogBll(LogDao dao)
        {
            _dao = dao;
        }
        private void Log(string logLevel, string category, string userId, string userName, string description, string note)
        {
            _dao.AddLog(logLevel, category, userId, userName, description, note);
        }
        public async Task DebugAsync(string category, string userId, string userName, string description, string note)
        {
            Log(DEBUG, category, userId, userName, description, note);
        }
        public async Task InfoAsync(string category, string userId, string userName, string description, string note)
        {
            Log(INFO, category, userId, userName, description, note);
        }
        public async Task WarnAsync(string category, string userId, string userName, string description, string note)
        {
            Log(WARN, category, userId, userName, description, note);
        }
        public async Task ErrorAsync(string category, string userId, string userName, string description, string note)
        {
            Log(ERROR, category, userId, userName, description, note);
        }
        public DTO_RESULT_PAGE_LIST<DTO_LOG> SearchLog(string start_dt, string end_dt, string logLevel, string userName, string description, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.SearchLog(start_dt, end_dt, logLevel, userName, description, rowPerPage, selectPage, sortKey, sortValue);
        }
    }
}

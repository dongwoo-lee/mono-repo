using M30.AudioFile.Common.DTO;
using M30.AudioFile.DAL;
using M30.AudioFile.DAL.Dao;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class HttpContextDBLogger
    {
        private const string DEBUG = "DEBUG";
        private const string INFO = "INFO";
        private const string WARN = "WARN";
        private const string ERROR = "ERROR";
        private readonly LogDao _dao;
        public HttpContextDBLogger(LogDao dao)
        {
            _dao = dao;
        }

        private void Log(HttpContext httpContext, string logLevel, string userId, string description, string note)
        {
            try
            {
                string remoteIp = httpContext.Connection.RemoteIpAddress.ToString();
                _dao.AddLog(logLevel, remoteIp, userId, description, note);
            }
            catch(Exception ex)
            {
                // 파일로그 처리
                // DB로그는 누락될 수 있음.
            }
        }
        private void Log(string systemCode, string logLevel, string userId, string description, string note)
        {
            try
            {
                _dao.AddLog(systemCode, logLevel, "", userId, description, note);
            }
            catch (Exception ex)
            {
                // 파일로그 처리
                // DB로그는 누락될 수 있음.
            }
        }

        public async Task DebugAsync(HttpContext httpContext, string userId, string description, string note)
        {
            Log(httpContext, DEBUG, userId, description, note);
        }
        public async Task InfoAsync(HttpContext httpContext, string userId, string description, string note)
        {
            Log(httpContext, INFO, userId, description, note);
        }
        public async Task WarnAsync(HttpContext httpContext, string userId, string description, string note)
        {
            Log(httpContext, WARN, userId, description, note);
        }
        public async Task ErrorAsync(HttpContext httpContext, string userId, string description, string note)
        {
            Log(httpContext, ERROR, userId, description, note);
        }
        public void DebugAsync(string systemCode, string userId, string description, string note)
        {
            Log(systemCode,DEBUG, userId, description, note);
        }
        public void InfoAsync(string systemCode, string userId, string description, string note)
        {
            Log( systemCode,INFO, userId, description, note);
        }
        public void WarnAsync( string systemCode, string userId, string description, string note)
        {
            Log( systemCode, WARN, userId, description, note);
        }
        public void ErrorAsync( string systemCode, string userId, string description, string note)
        {
            Log( systemCode, ERROR, userId, description, note);
        }
        public DTO_RESULT_PAGE_LIST<DTO_LOG> GetLogList(string systemCode, string start_dt, string end_dt, string logLevel, string userName, string description, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            var result = _dao.SearchLog(systemCode, start_dt, end_dt, logLevel, userName, description, rowPerPage, selectPage, sortKey, sortValue);
            return result;
        }
    }
}

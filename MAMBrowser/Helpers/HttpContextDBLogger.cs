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
    }
}

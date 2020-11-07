using MAMBrowser.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAMBrowser.Services
{
    public class LogService
    {
        private const string DEBUG = "DEBUG";
        private const string INFO = "INFO";
        private const string WARN = "WARN";
        private const string ERROR = "ERROR";

        private readonly APIDAL _dal;
        public LogService(APIDAL dal)
        {
            _dal = dal;
        }
        private void Log(string logLevel, string clientIp, string userId, string userName, string description, string note)
        {
            _dal.AddLog(logLevel, clientIp, userId, userName, description, note);
        }
        public async Task DebugAsync(string clientIp, string userId, string userName, string description, string note)
        {
            Log(DEBUG, clientIp, userId, userName, description, note);
        }
        public async Task InfoAsync(string clientIp, string userId, string userName, string description, string note)
        {
            Log(INFO, clientIp, userId, userName, description, note);
        }
        public async Task WarnAsync(string clientIp, string userId, string userName, string description, string note)
        {
            Log(WARN, clientIp, userId, userName, description, note);
        }
        public async Task ErrorAsync(string clientIp, string userId, string userName, string description, string note)
        {
            Log(ERROR, clientIp, userId, userName, description, note);
        }

    }
}

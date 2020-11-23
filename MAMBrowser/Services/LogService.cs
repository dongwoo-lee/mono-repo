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
        private void Log(string logLevel, string category, string userId, string userName, string description, string note)
        {
            _dal.AddLog(logLevel, category, userId, userName, description, note);
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
    }
}

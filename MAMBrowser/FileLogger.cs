using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace MAMBrowser
{
    public class FileLogger
    {
        public static void Debug(string title, string msg)
        {

        }
        public static void Info(string title, string msg)
        {
            var logger =log4net.LogManager.GetLogger(typeof(FileLogger));
            logger.Info(title);
        }
        public static void Warn(string title, string msg)
        {

        }
        public static void Error(string title, string msg)
        {

        }
    }
}

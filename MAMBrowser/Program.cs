using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.Foundation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MAMBrowser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            // ConnectNetDrive.AllDisConnect();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true).UseSetting("detailedErrors", "true").UseStartup<Startup>();
                });
                //.ConfigureLogging((hostingContext, logging) =>
                //{
                //    logging.AddLog4Net();
                //    logging.SetMinimumLevel(LogLevel.Debug);
                //});
    }
}

using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MAMBrowser
{
    public class MyLoopWorker : BackgroundService
    {
        public MyLoopWorker()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //MyLogger.Debug($"starting.");
            
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    MyLogger.Debug($"process - {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            //    await Task.Delay(5000, stoppingToken);
            //}
            //MyLogger.Debug($"stoped.");
        }
    }
}

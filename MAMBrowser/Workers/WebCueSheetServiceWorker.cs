using M30.AudioFile.Common;
using MAMBrowser.BLL;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MAMBrowser.Workers
{
    internal class WebCueSheetServiceWorker : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly APIBll _apiBll;

        public WebCueSheetServiceWorker(ILogger<WebCueSheetServiceWorker> logger, APIBll apiBll)
        {
            _logger = logger;
            _apiBll= apiBll;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("WebCueSheet Background Service Worker is Started.");
            _timer = new Timer(DeleteTempFile, cancellationToken, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DeleteTempFile(object state)
        {
            DateTime before_time_wav_zip = DateTime.Now.AddHours(-2);
            DateTime before_time_attach = DateTime.Now.AddHours(-6);

            var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
            var wcsPathRoot = option.Find(dt => dt.Name == "WCS_ATTACH_PATH").Value.ToString();
            var rootdir_attach = Path.Combine(wcsPathRoot, "tmp");
            var rootdir_wav_zip = Startup.AppSetting.TempExportPath;

            try
            {
                //zip, wav
                DeleteOldTempFiles(rootdir_wav_zip, before_time_wav_zip);
                //attachments
                DeleteOldTempFiles(rootdir_attach, before_time_attach);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("WebCueSheetServiceWorker : File Delete Fail.");
                _logger.LogWarning($"WebCueSheetServiceWorker : {ex.Message}");
            }
        }

        /// <summary>
        /// 특정 시간 경과 파일, 폴더 삭제
        /// Delete all files and folders created before a specific time
        /// </summary>
        /// <param name="before_time">Deletion base time</param>
        /// <param name="dir_path">Deletion directory path</param>
        private void DeleteOldTempFiles(string dir_path, DateTime before_time)
        {
            DirectoryInfo di = new DirectoryInfo(dir_path);
            if (di.Exists)
            {
                FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories);
                DirectoryInfo[] dirs = di.GetDirectories("*", SearchOption.AllDirectories);

                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < before_time)
                    {
                        file.Delete();
                        _logger.LogInformation("WebCueSheetServiceWorker : File Delete Complete.");
                    }
                }

                foreach (DirectoryInfo dir in dirs.Reverse())
                {
                    if (!dir.GetFiles("*", SearchOption.AllDirectories).Any())
                    {
                        if (dir.LastWriteTime < before_time)
                        {
                            dir.Delete();
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("WebCueSheet Background Service Worker is Stoped.");
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}

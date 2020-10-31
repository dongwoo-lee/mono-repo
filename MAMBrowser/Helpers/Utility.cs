using MAMBrowser.Processor;
using Microsoft.AspNetCore.StaticFiles;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    
    public static class Utility
    {
        public const string DTM8 = "yyyyMMdd";
        public const string DTM10 = "yyyy-MM-dd";
        public const string DTM19 = "yyyy-MM-dd HH:mm:ss";

        public static string DownloadFile(IFileService fileService, string filePath, string newFileName, out string contentType)
        {
            var fileExtProvider = new FileExtensionContentTypeProvider();
            if (!fileExtProvider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }

            var targetPath = @$"c:\tmpwork\{newFileName}";
            fileService.DownloadFile(filePath, targetPath);
            return targetPath;
        }
        public static List<float> GetWaveform(IFileService fileService, string filePath)
        {
            string waveFileName = Path.GetFileName(filePath);
            string waveformFileName = $"{Path.GetFileNameWithoutExtension(filePath)}.egy";
            string waveformFilePath = filePath.Replace(waveFileName, waveformFileName);

            if (fileService.ExistFile(waveformFilePath))
            {
                var downloadStream = fileService.GetFileStream(waveformFilePath, 0);
                return AudioEngine.GetDecibelFromEgy(downloadStream);
            }
            else
            {

                var inputStream = fileService.GetFileStream(filePath, 0);
                MemoryStream ms = new MemoryStream();
                inputStream.CopyTo(ms);
                ms.Position = 0;

                WaveFileReader reader = new WaveFileReader(ms);
                var data = AudioEngine.GetDecibelFromWav(ms, 2);
                inputStream.Close();
                ms.Dispose();
                return data;
            }
        }

    }
}

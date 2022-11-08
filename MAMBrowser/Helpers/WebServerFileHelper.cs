using M30.AudioFile.Common;
using M30.AudioFile.Common.Foundation;
using MAMBrowser.DTO;
using MAMBrowser.Foundation;
using MAMBrowser.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MAMBrowser.Helper
{

    public class WebServerFileHelper
    {
        public WebServerFileHelper()
        {
        }

        public FileStreamResult Download(string downloadName, string token, HttpResponse response, IFileProtocol fileProtocol, string inline)
        {
            string filePath = "";
            if (TokenGenerator.ValidateFileToken(token, ref filePath))
            {
                //string fileName = Path.GetFileName(filePath);
                var fileExtProvider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!fileExtProvider.TryGetContentType(filePath, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                var stream = fileProtocol.GetFileStream(filePath, 0);
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = Uri.EscapeDataString(downloadName),
                    Inline = inline == "Y" ? true : false
                };
                response.Headers.Add("Content-Disposition", cd.ToString());
                return new FileStreamResult(stream, contentType);
            }
            else
                throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
        }
        public void TempDownload(string token, string userId, string remoteIp, IFileProtocol fileProtocol)
        {
            string filePath = "";
            if (TokenGenerator.ValidateFileToken(token, ref filePath))
            {
                string fileName = Path.GetFileName(filePath);

                var egyFileName = Path.GetFileNameWithoutExtension(filePath) + Define.EGY;
                var egyFilePath = filePath.Replace(fileName, egyFileName);

                MAMUtility.TempDownloadToLocal(userId, remoteIp, fileProtocol, filePath);       //임시파일 다운로드
                MAMUtility.TempDownloadToLocal(userId, remoteIp, fileProtocol, egyFilePath);    //파형임시파일 다운로드
            }
            else
                throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
        }
        public List<float> GetWaveform(string token, string userId, string remoteIp)
        {
            string filePath = "";
            if (TokenGenerator.ValidateFileToken(token, ref filePath))
            {
                string fileName = Path.GetFileName(filePath);
                //파형검색시 mp2파일은 wav로 치환됨.
                fileName = Path.GetExtension(fileName).ToUpper() == Define.MP2 ? fileName.ToUpper().Replace(Define.MP2, Define.WAV) : fileName;
                string tempSoundPath = CommonUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, fileName);
                return MAMUtility.GetWaveformCore(tempSoundPath);
            }
            else
                throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
        }
        public IActionResult Streaming(string token, string userId, string remoteIp)
        {
            string filePath = "";
            if (TokenGenerator.ValidateFileToken(token, ref filePath))
            {
                return StreamingFromFilePath(filePath, userId, remoteIp);
            }
            else
                throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
        }
        public IActionResult StreamingFromFilePath(string filePath, string userId, string remoteIp)
        {
            string fileName = Path.GetFileName(filePath);
            fileName = Path.GetExtension(fileName).ToUpper() == Define.MP2 ? fileName.ToUpper().Replace(Define.MP2, Define.WAV) : fileName;

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            string tempDownloadedPath = CommonUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, fileName);
            var result = new PhysicalFileResult(tempDownloadedPath, contentType);
            result.EnableRangeProcessing = true;
            return result;
        }

        public FileStreamResult DownloadFromPath(string downloadName, string filePath, HttpResponse response, IFileProtocol fileProtocol, string inline)
        {

            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!fileExtProvider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var stream = fileProtocol.GetFileStream(filePath, 0);
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                //FileName = WebUtility.UrlEncode(fileName),
                FileName = Uri.EscapeDataString(downloadName),
                Inline = inline == "Y" ? true : false
            };
            response.Headers.Add("Content-Disposition", cd.ToString());
            return new FileStreamResult(stream, contentType);
        }
        public void TempDownloadFromPath(string filePath, string userId, string remoteIp, IFileProtocol fileProtocol)
        {
            string fileName = Path.GetFileName(filePath);
            var egyFilePath = Path.ChangeExtension(filePath, Define.EGY);

            MAMUtility.TempDownloadToLocal(userId, remoteIp, fileProtocol, filePath);       //임시파일 다운로드
            MAMUtility.TempDownloadToLocal(userId, remoteIp, fileProtocol, egyFilePath);    //파형임시파일 다운로드
        }
        public List<float> GetWaveformFromPath(string filePath, string userId, string remoteIp)
        {
            string fileName = Path.GetFileName(filePath);
            //파형검색시 mp2파일은 wav로 치환됨.
            fileName = Path.GetExtension(fileName).ToUpper() == Define.MP2 ? fileName.ToUpper().Replace(Define.MP2, Define.WAV) : fileName;
            string tempSoundPath = CommonUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, fileName);
            return MAMUtility.GetWaveformCore(tempSoundPath);
        }
        public IActionResult StreamingFromPath(string filePath, string userId, string remoteIp)
        {
            string relativePath = CommonUtility.GetRelativePath(filePath);
            string fileName = Path.GetFileName(filePath);
            fileName = Path.GetExtension(fileName).ToUpper() == Define.MP2 ? fileName.ToUpper().Replace(Define.MP2, Define.WAV) : fileName;
            //if (direct.ToUpper() == "Y")
            //{
            //    int fileSize = 0;
            //    return new PushStreamResult(relativePath, fileName, fileSize, fileProtocol);
            //}
            //else
            //{
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            string tempDownloadedPath = CommonUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, fileName);
            var result = new PhysicalFileResult(tempDownloadedPath, contentType);
            result.EnableRangeProcessing = true;
            return result;
            //}
        }
        public IActionResult StreamingFromFileName(string fileName, string userId, string remoteIp)
        {
            fileName = Path.GetExtension(fileName).ToUpper() == Define.MP2 ? fileName.ToUpper().Replace(Define.MP2, Define.WAV) : fileName;
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            string tempDownloadedPath = CommonUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, fileName);
            var result = new PhysicalFileResult(tempDownloadedPath, contentType);
            result.EnableRangeProcessing = true;
            return result;
        }
        public void CheckFileExtensionValid(string[] extensionsRuls, string fileName)
        {
            fileName = fileName.ToLower();

            var isValidExtenstion = extensionsRuls.Any(ext =>
            {
                return fileName.LastIndexOf(ext) > -1;
            });
            if (!isValidExtenstion)
                throw new Exception("Not allowed file extension");
        }

        public void CheckMaxFileSize(FileStream stream, long maxSize)
        {
            if (stream.Length > maxSize)
                throw new Exception("File is too large");
        }

        public void AppendContentToFile(string path, IFormFile content, long maxSize)
        {
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                content.CopyTo(stream);
                CheckMaxFileSize(stream, maxSize);
            }
        }

        public string GetTempFileName(ChunkMetadata meta)
        {
            string date = DateTime.Now.ToString(Define.DTM8);
            //return $"{date}_{meta.FileGuid}_{Path.GetFileNameWithoutExtension(meta.FileName)}.tmp";
            return $"{date}_{meta.FileGuid}_{meta.FileName}";
        }

        public void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {
            using (WaveFileReader reader = new WaveFileReader(inPath))
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
                {
                    double bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000d;

                    int startPos = (int)(cutFromStart.TotalMilliseconds * bytesPerMillisecond);
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)(cutFromEnd.TotalMilliseconds * bytesPerMillisecond);
                    int endPos = endBytes - endBytes % reader.WaveFormat.BlockAlign;

                    TrimWavFile(reader, writer, startPos, endPos);
                }
            }
        }

        private void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.WriteData(buffer, 0, bytesRead);
                    }
                }
            }

        }

        //public AudioFileReader FadeInOutWavFile(string filePath, bool fadeIn, bool fadeOut)
        //{
        //    var audio = new AudioFileReader(filePath);
        //    var fade = new DelayFadeOutSampleProvider(audio);
        //        //fadeOut.BeginFadeOut(ele.ENDPOSITION - ele.STARTPOSITION - 2000, 2000);
        //        //WaveFileWriter.CreateWaveFile16(fadeOutFilePath, fadeOut);
        //    if (fadeIn)
        //    {
        //        var fadeInPath = Path.Combine(Path.GetDirectoryName(filePath), "_fadeIn_" + Path.GetFileName(filePath));
        //        fade.BeginFadeIn(2000);
        //        audio = new AudioFileReader(fadeInPath);
        //        WaveFileWriter.CreateWaveFile16(fadeInPath, audio);

        //    }
        //    if (fadeOut)
        //    {
        //        var fadeOutPath = Path.Combine(Path.GetDirectoryName(filePath), "_fadeOut_" + Path.GetFileName(filePath));
        //        fade.BeginFadeOut()

        //    }
        //    return null;
        //}




    }
}

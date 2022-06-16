using M30.AudioFile.Common;
using M30.AudioFile.Common.Foundation;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MAMBrowser.Helper
{
    public class WebServerFileHelper
    {
        public WebServerFileHelper(IOptions<AppSettings> options)
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

    }
}

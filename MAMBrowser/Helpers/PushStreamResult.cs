using MAMBrowser.Common;
using MAMBrowser.Foundation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class PushStreamResult : IActionResult
    {
        private readonly Action<ActionContext,Stream> _onStreamAvailabe;
        private readonly string _contentType;
        private readonly string _filePath;
        private readonly string _newFileName;
        private readonly long _fileSize;
        private readonly IFileDownloadProtocol _fileService;

        public PushStreamResult(string filePath, string newFileName, long fileSize, IFileDownloadProtocol fileService)
        {
            _onStreamAvailabe = OnStreamAvailable;
            var fileExtProvider = new FileExtensionContentTypeProvider();
            if (!fileExtProvider.TryGetContentType(filePath, out _contentType))
            {
                _contentType = "application/octet-stream";
            }
            _filePath = filePath;
            _newFileName = newFileName;
            _fileSize = fileSize;
            _fileService = fileService;
        }
        
        public Task ExecuteResultAsync(ActionContext context)
        {
            var outStream = context.HttpContext.Response.Body;
            context.HttpContext.Response.ContentType = _contentType;
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = Uri.EscapeDataString(_newFileName),
                Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            context.HttpContext.Response.Headers.Add("Accept-Ranges", "bytes");
            context.HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());

            _onStreamAvailabe(context, outStream);
            return Task.CompletedTask;
        }
        public void OnStreamAvailable(ActionContext context, Stream stream /*, CancellationToken requestAborted*/)
        {
            var rangeData = context.HttpContext.Request.GetTypedHeaders().Range;
            var fileExt = Path.GetExtension(_filePath).ToUpper();
            if (rangeData == null)
            {
                var inputStream = _fileService.GetFileStream(_filePath, 0);
                context.HttpContext.Response.ContentLength = _fileSize;
                if (fileExt != Define.MP2)
                {
                    inputStream.CopyTo(stream);
                }
            }
            else
            {
                var range = rangeData.Ranges.First();
                if (range.To == null)
                {
                }
                var contentSize = _fileSize - range.From;

                var downloadStream = _fileService.GetFileStream(_filePath, (long)range.From);
                context.HttpContext.Response.GetTypedHeaders().ContentRange = new Microsoft.Net.Http.Headers.ContentRangeHeaderValue((long)range.From, _fileSize - 1, _fileSize);
                context.HttpContext.Response.GetTypedHeaders().ContentLength = (long)contentSize;
                context.HttpContext.Response.StatusCode = 206;

                if (fileExt != Define.MP2)
                {
                    downloadStream.CopyTo(stream);
                }
            }

        }
    }
}

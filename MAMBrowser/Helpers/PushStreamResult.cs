using MAMBrowser.Processor;
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
        private readonly IFileDownloadService _fileService;

        public PushStreamResult(string filePath, string newFileName, long fileSize, IFileDownloadService fileService)
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
                FileName = WebUtility.UrlEncode(_newFileName),
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
                if (fileExt != MAMUtility.MP2)
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

                if (fileExt != MAMUtility.MP2)
                {
                    downloadStream.CopyTo(stream);
                }
            }

        }
        //private static bool IsMultipartRequest(RangeHeaderValue range)
        //{
        //    return range != null && range.Ranges != null && range.Ranges.Count > 1;
        //}
        //private static bool IsRangeRequest(RangeHeaderValue range)
        //{
        //    return range != null && range.Ranges != null && range.Ranges.Count > 0;
        //}
        //public static RangeHeaderValue GetRanges(this HttpContext context, long contentSize)
        //{
        //    RangeHeaderValue rangesResult = null;

        //    string rangeHeader = context.Request.Headers["Range"];

        //    if (!string.IsNullOrEmpty(rangeHeader))
        //    {
        //        // rangeHeader contains the value of the Range HTTP Header and can have values like:
        //        //      Range: bytes=0-1            * Get bytes 0 and 1, inclusive
        //        //      Range: bytes=0-500          * Get bytes 0 to 500 (the first 501 bytes), inclusive
        //        //      Range: bytes=400-1000       * Get bytes 500 to 1000 (501 bytes in total), inclusive
        //        //      Range: bytes=-200           * Get the last 200 bytes
        //        //      Range: bytes=500-           * Get all bytes from byte 500 to the end
        //        //
        //        // Can also have multiple ranges delimited by commas, as in:
        //        //      Range: bytes=0-500,600-1000 * Get bytes 0-500 (the first 501 bytes), inclusive plus bytes 600-1000 (401 bytes) inclusive

        //        // Remove "Ranges" and break up the ranges
        //        string[] ranges = rangeHeader.Replace("bytes=", string.Empty).Split(",".ToCharArray());

        //        rangesResult = new RangeHeaderValue();

        //        for (int i = 0; i < ranges.Length; i++)
        //        {
        //            const int START = 0, END = 1;

        //            long endByte, startByte;

        //            long parsedValue;

        //            string[] currentRange = ranges[i].Split("-".ToCharArray());

        //            if (long.TryParse(currentRange[END], out parsedValue))
        //                endByte = parsedValue;
        //            else
        //                endByte = contentSize - 1;


        //            if (long.TryParse(currentRange[START], out parsedValue))
        //                startByte = parsedValue;
        //            else
        //            {
        //                // No beginning specified, get last n bytes of file
        //                // We already parsed end, so subtract from total and
        //                // make end the actual size of the file
        //                startByte = contentSize - endByte;
        //                endByte = contentSize - 1;
        //            }

        //            rangesResult.Ranges.Add(new RangeItemHeaderValue(startByte, endByte));
        //        }
        //    }

        //    return rangesResult;
        //}

    }
}

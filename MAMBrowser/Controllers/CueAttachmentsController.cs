using DAP3.CueSheetCommon.DTO.Param;
using DAP3.CueSheetCommon.DTO.Result;
using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CueAttachmentsController : ControllerBase
    {
        private readonly IFileProtocol _fileService;
        public CueAttachmentsController(ServiceResolver sr)
        {
            _fileService = sr("MirosConnection");
        }
        //zip파일 내보내기
        [HttpPost("exportZipFile")]
        public bool ExportZipFile([FromBody] List<ViewCueSheetConDTO> pram)
        {
            try
            {
                var filePath = @"C:\Users\kimeunbee\Desktop\알집_테스트\20211022\MetaData.xml";
                DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(filePath));
                if (!di.Exists)
                {
                    di.Create();
                }
                foreach (ViewCueSheetConDTO ele in pram)
                {
                    ele.FilePath = @"\\test_svr\MBCDATA\FILLER\FC00005956.wav";
                    var outFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileName(ele.FilePath));
                    using (FileStream outFileStream = new FileStream(outFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (var inStream = _fileService.GetFileStream(ele.FilePath, 0))
                        {
                            inStream.CopyTo(outFileStream);
                        }
                    }

                }
                using (var FileStream = new StreamWriter(filePath))
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<ViewCueSheetConDTO>));
                    serialiser.Serialize(FileStream, pram);
                }

                var zipFilePath = @"C:\Users\kimeunbee\Desktop\알집_테스트\20211022.zip";
                ZipFileManager.Instance.CreateZIPFile(Path.GetDirectoryName(filePath), zipFilePath);
                return true;

                //string fileName = "20211022.zip";
                //byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
                //return File(fileBytes, "application/octet-stream", fileName);
                //const string contentType = "application/octet*";
                //HttpContext.Response.ContentType = contentType;
                //var result = new FileContentResult(System.IO.File.ReadAllBytes(zipFilePath), contentType)
                //{
                //   FileDownloadName = fileName
                //};
                //var provider = new FileExtensionContentTypeProvider();
                //string contentType;
                //if (!provider.TryGetContentType(zipFilePath, out contentType))
                //{
                //    contentType = "application/octet-stream";
                //}

                //return PhysicalFile(zipFilePath, contentType);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet("exportZipFileDownload")]
        public FileResult ExportZipFileDownload()
        {
            var zipFilePath = @"C:\Users\kimeunbee\Desktop\알집_테스트\20211022.zip";
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(zipFilePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return PhysicalFile(zipFilePath, contentType);

            //string fileName = "20211022.zip";
            //const string contentType = "application/zip";
            //HttpContext.Response.ContentType = contentType;
            //var result = new FileContentResult(System.IO.File.ReadAllBytes(zipFilePath), contentType)
            //{
            //    FileDownloadName = fileName
            //};

            //return result;
        }
    }
    public class ViewCueSheetCollectionDTO
    {
        public List<ViewCueSheetConDTO> ViewCueSheetConDTO { get; set; } = new List<ViewCueSheetConDTO>();
    }

    public class ViewCueSheetConDTO
    {
        public int RowNum { get; set; }
        public string ProductType { get; set; }
        public string CartCode { get; set; }
        public string ChannelType { get; set; }
        public string CartId { get; set; }
        public string OnairDate { get; set; }
        public string Duration { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
        public int FadeInTime { get; set; }
        public int FadeOutTime { get; set; }
        public string MainTitle { get; set; }
        public string SubTitle { get; set; }
        public string Memo { get; set; }
        public string TransType { get; set; }
        public string UseFlag { get; set; }
        //public List<string> FilePath { get; set; } = new List<string>();
        public string FilePath { get; set; }
    }
}
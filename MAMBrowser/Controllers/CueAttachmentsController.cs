using MAMBrowser.BLL;
using M30.AudioFile.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.DTO;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CueAttachmentsController : Controller
    {
        private readonly ProductsBll _bll;
        private readonly IFileProtocol _fileService;
        public CueAttachmentsController(ServiceResolver sr, ProductsBll bll)
        {
            _fileService = sr(MAMDefine.MirosConnection).FileSystem;
            _bll = bll;
        }

        //zip파일 내보내기
        [HttpPost("exportZipFile")]
        public bool ExportZipFile([FromBody] List<CueSheetConDTO> pram)
        {
            try
            {
                var filePath = @"C:\Users\kimeunbee\Desktop\알집_테스트\20211022\MetaData.xml";
                var filePathJson = @"C:\Users\kimeunbee\Desktop\알집_테스트\20211022\MetaData.json";
                DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(filePath));
                if (!di.Exists)
                {
                    di.Create();
                }
                else
                {
                    di.Delete(true);
                    di.Create();
                }
                foreach (CueSheetConDTO ele in pram)
                {
                    if (ele.FILEPATH != null && ele.FILEPATH != "")
                    {
                        //ele.FILEPATH = @"\\test_svr\MBCDATA\FILLER\FC00005956.wav";
                        var outFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileName(ele.FILEPATH));
                        var audioData = new CueSheetConAudioDTO();
                        ele.AUDIOS = new List<CueSheetConAudioDTO>();

                        using (FileStream outFileStream = new FileStream(outFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            try
                            {
                                using (var inStream = _fileService.GetFileStream(ele.FILEPATH, 0))
                                {
                                    inStream.CopyTo(outFileStream);

                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                ele.FILEPATH = "";
                            }
                        }
                        ele.FILEPATH = outFilePath;
                        audioData.P_TYPE = ele.CARTTYPE;
                        audioData.P_SEQNUM = 1;
                        audioData.P_CLIPID = ele.CARTID;
                        audioData.P_MAINTITLE = ele.MAINTITLE;
                        audioData.P_SUBTITLE = ele.SUBTITLE;
                        audioData.P_DURATION = ele.DURATION;
                        audioData.P_MASTERFILE = outFilePath;
                        ele.AUDIOS.Add(audioData);
                    }
                    else
                    {
                        // 그룹소재 아이템 가져오기
                        if (ele.ONAIRDATE != "")
                        {
                            ele.AUDIOS = new List<CueSheetConAudioDTO>();
                            if (ele.CARTTYPE == "SB")
                            {
                                var collection = _bll.FindSBContents(ele.ONAIRDATE, ele.CARTID);
                                if (collection.Data.Any())
                                {
                                    foreach (var item in collection.Data)
                                    {
                                        var result = new CueSheetConAudioDTO();
                                        result.P_SEQNUM = item.RowNO;
                                        result.P_CLIPID = item.ID;
                                        result.P_MAINTITLE = item.Name;
                                        result.P_DURATION = item.IntDuration;
                                        result.P_MASTERFILE = item.FilePath;
                                        result.P_CODEID = item.PgmCODE ?? "";
                                        ele.AUDIOS.Add(result);
                                    }

                                }
                            }
                            if (ele.CARTTYPE == "CM")
                            {
                                var collection = _bll.FindCMContents(ele.ONAIRDATE, ele.CARTID);
                                if (collection.Data.Any())
                                {
                                    foreach (var item in collection.Data)
                                    {
                                        var result = new CueSheetConAudioDTO();
                                        result.P_SEQNUM = item.RowNO;
                                        result.P_CLIPID = item.ID;
                                        result.P_MAINTITLE = item.Name;
                                        result.P_DURATION = item.IntDuration;
                                        result.P_MASTERFILE = item.FilePath;
                                        result.P_CODEID = item.PgmCODE ?? "";
                                        ele.AUDIOS.Add(result);
                                    }

                                }
                            }
                        }
                        if (ele.AUDIOS!=null&&ele.AUDIOS.Any())
                        {
                            foreach (var item in ele.AUDIOS)
                            {
                                var outFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileName(item.P_MASTERFILE));

                                using (FileStream outFileStream = new FileStream(outFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                                {
                                    try
                                    {
                                        using (var inStream = _fileService.GetFileStream(item.P_MASTERFILE, 0))
                                        {
                                            inStream.CopyTo(outFileStream);
                                            item.P_MASTERFILE = outFilePath;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                        item.P_MASTERFILE = "";
                                    }
                                }
                            }

                        }
                    }


                }
                using (var FileStream = new StreamWriter(filePath))
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<CueSheetConDTO>));
                    serialiser.Serialize(FileStream, pram);

                }

                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(filePathJson))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, pram);
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
    //public class ViewCueSheetCollectionDTO
    //{
    //    public List<ViewCueSheetConDTO> ViewCueSheetConDTO { get; set; } = new List<ViewCueSheetConDTO>();
    //}

    //public class ViewCueSheetConDTO
    //{
    //    public int RowNum { get; set; }
    //    public string ProductType { get; set; }
    //    public string CartCode { get; set; }
    //    public string ChannelType { get; set; }
    //    public string CartId { get; set; }
    //    public string OnairDate { get; set; }
    //    public string Duration { get; set; }
    //    public int StartPosition { get; set; }
    //    public int EndPosition { get; set; }
    //    public int FadeInTime { get; set; }
    //    public int FadeOutTime { get; set; }
    //    public string MainTitle { get; set; }
    //    public string SubTitle { get; set; }
    //    public string Memo { get; set; }
    //    public string TransType { get; set; }
    //    public string UseFlag { get; set; }
    //    //public List<string> FilePath { get; set; } = new List<string>();
    //    public string FilePath { get; set; }
    //}
}

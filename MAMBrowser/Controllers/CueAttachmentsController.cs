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
        public ActionResult<string> ExportZipFile([FromBody] List<CueSheetConDTO> pram)
        {
            try
            {
                var rootFolder = Startup.AppSetting.TempExportPath;
                if (!Directory.Exists(rootFolder))
                    Directory.CreateDirectory(rootFolder);

                var guid = Guid.NewGuid().ToString();
                string xmlFileName= $"{guid}_MetaData.xml";
                string jsonFileName = $"{guid}_MetaData.json"; 
                string xmlFileFullPath = Path.Combine(rootFolder, xmlFileName);
                string jsonFileFullPath = Path.Combine(rootFolder, jsonFileName);

                DirectoryInfo di = new DirectoryInfo(rootFolder);
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
                        var outFilePath = Path.Combine(rootFolder, Path.GetFileName(ele.FILEPATH));
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
                        if (ele.AUDIOS == null)
                        {
                            var audioData = new CueSheetConAudioDTO();
                            ele.AUDIOS = new List<CueSheetConAudioDTO>();
                            audioData.P_TYPE = ele.CARTTYPE;
                            audioData.P_SEQNUM = 1;
                            audioData.P_CLIPID = ele.CARTID;
                            audioData.P_MAINTITLE = ele.MAINTITLE;
                            audioData.P_SUBTITLE = ele.SUBTITLE;
                            audioData.P_DURATION = ele.DURATION;
                            audioData.P_MASTERFILE = "";
                            ele.AUDIOS.Add(audioData);
                        }
                        else
                        {
                            foreach (var item in ele.AUDIOS)
                            {
                                var outFilePath = Path.Combine(Path.GetDirectoryName(rootFolder), Path.GetFileName(item.P_MASTERFILE));

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
                using (var FileStream = new StreamWriter(xmlFileFullPath))
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<CueSheetConDTO>));
                    serialiser.Serialize(FileStream, pram);

                }

                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(jsonFileFullPath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, pram);
                }
                string zipFileName = $"{guid}.zip";
                var zipFilePath = Path.Combine(Path.GetDirectoryName(rootFolder), zipFileName);

                ZipFileManager.Instance.CreateZIPFile(Path.GetDirectoryName(rootFolder), zipFilePath);
                return Path.Combine(Path.GetDirectoryName(rootFolder), zipFileName);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet("exportZipFileDownload")]
        public FileResult ExportZipFileDownload([FromQuery] string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return PhysicalFile(fileName, contentType);


        }
    }
  
}

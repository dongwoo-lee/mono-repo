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
using Microsoft.AspNetCore.Http;
using M30.AudioFile.Common;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CueAttachmentsController : Controller
    {
        private readonly CueAttachmentsBll _bll;

        public CueAttachmentsController(CueAttachmentsBll bll)
        {
            _bll = bll;
        }

        //zip파일 내보내기
        [HttpPost("exportZipFile")]
        public ActionResult<string> ExportZipFile([FromQuery] string userid, [FromBody] List<CueSheetConDTO> pram)
        {
            ActionResult<string> result;
            try
            {
                result = _bll.ExportToZipFile(userid, pram);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        [HttpPost("exportWavFile")]
        public ActionResult<string> ExportWavFile([FromQuery]string userid,[FromBody]List<CueSheetConDTO> pram)
        {
            ActionResult<string> result;
            try
            {
                result = _bll.MergeAudioFilesIntoOneWav(userid, pram);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;

        }
        // 다운로드 링크
        [HttpGet("exportFileDownload")]
        public FileResult ExportFileDownload([FromQuery] Pram queryPram)
        {
            FileResult result;
            var rootFolder = Path.Combine(Startup.AppSetting.TempExportPath, @$"{queryPram.userid}\{queryPram.guid}");
            var FilePath = Path.Combine(Path.GetDirectoryName(rootFolder), queryPram.guid);
            var extension = Path.GetExtension(queryPram.guid);
            var provider = new FileExtensionContentTypeProvider();

            string contentType;
            if (!provider.TryGetContentType(FilePath, out contentType))
            {
                contentType = "application/octet-stream";
            }

            //zip file & attach(txt,pdf,docx,hwp)따라 다르게 해줘야할듯
            result = PhysicalFile(FilePath, contentType, $"{queryPram.downloadName + extension}");
            return result;
        }

        [RequestSizeLimit(int.MaxValue)]
        [HttpPost("chunkFileUpload")]
        public ActionResult<DTO_RESULT> ChunkFileUpload([FromForm] IFormFile file,[FromForm]string chunkMetadata, [FromForm] string serviceName,[FromForm] string brd_date)
        {
            var result = new DTO_RESULT();
            try
            {
                result.Token =_bll.UploadToTemp(file, chunkMetadata, serviceName, brd_date);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }

            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.FILE_NOT_FOUND;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }

        [HttpDelete("tempFileDelete")]
        public ActionResult<DTO_RESULT> TempAttachmentsFileDelete([FromQuery]string fileToken)
        {
            var result = new DTO_RESULT();
            try
            {
                _bll.DeleteTempAttachmentsFile(fileToken);
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
            }
            return result;

        }
    }
  
}

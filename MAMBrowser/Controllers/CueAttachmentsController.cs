using MAMBrowser.BLL;
using M30.AudioFile.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using Microsoft.AspNetCore.Http;
using M30.AudioFile.Common;
using M30.AudioFile.Common.Models;
using MAMBrowser.Hubs;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Threading;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CueAttachmentsController : Controller
    {
        private readonly CueAttachmentsBll _bll;
        PrivateFileBll _privateBll;

        public CueAttachmentsController(CueAttachmentsBll bll, PrivateFileBll privateBll)
        {
            _bll = bll;
            _privateBll = privateBll;
        }

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
        public Task<DTO_RESULT<ActionResult<string>>> ExportWavFile([FromQuery] string connectionId, [FromBody] List<CueSheetConDTO> pram, CancellationToken token)
        {
            Task<DTO_RESULT<ActionResult<string>>> result;
            
            try
            {
                string userId = HttpContext.Items[Define.USER_ID] as string;
                result = _bll.MergeAudioFilesIntoOneWav(userId, connectionId, pram, token);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        [HttpPost("WavCopyToMyspace")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> WavFileCopyToMyspace([FromQuery] string title, [FromBody] string file_path)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                if (System.IO.File.Exists(file_path))
                {
                    var metaData = new M30_MAM_PRIVATE_SPACE();
                    string userId = HttpContext.Items[Define.USER_ID] as string;

                    var fileName = Path.GetFileName(file_path);
                    using (var stream = System.IO.File.Open(file_path, FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite))
                    {
                        metaData.FILE_SIZE = stream.Length;
                        metaData.TITLE = title;
                        metaData.MEMO = title;
                        result = _privateBll.UploadFile(userId, stream, fileName, metaData);
                    }
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
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
            var provider = new FileExtensionContentTypeProvider();

            string contentType;
            if (!provider.TryGetContentType(FilePath, out contentType))
            {
                contentType = "application/octet-stream";
            }

            result = PhysicalFile(FilePath, contentType, $"{queryPram.downloadName}");
            return result;
        }

        [RequestSizeLimit(int.MaxValue)]
        [HttpPost("chunkFileTempUpload")]
        public ActionResult<DTO_RESULT> ChunkFileTempUpload([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string folder_date)
        {
            var result = new DTO_RESULT();
            try
            {
                result.ResultObject = _bll.UploadToTemp(file, chunkMetadata, folder_date);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }

            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.FILE_NOT_FOUND;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }

        [HttpDelete("attachmentsDelete")]
        public ActionResult<DTO_RESULT> AttachmentsFileDelete([FromQuery] AttachmentDTO file)
        {
            var result = new DTO_RESULT();
            try
            {
                _bll.DeleteAttachmentsFile(file);
                var folder = new DirectoryInfo(Path.GetDirectoryName(file.FILEPATH));
                if (folder.GetFileSystemInfos().Length == 0 && folder.Exists)
                {
                    Directory.Delete(Path.GetDirectoryName(file.FILEPATH));
                }

                result.ResultCode = RESUlT_CODES.SUCCESS;
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

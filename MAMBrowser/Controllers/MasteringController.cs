using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using MAMBrowser.BLL;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using MAMBrowser.MAMDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAudio.Wave;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CustomAuthorize]
    public class MasteringController : ControllerBase
    {
        public MasteringController()
        {
        }
        public class ChunkMetadata
        {
            public int index { get; set; }
            public int TotalCount { get; set; }
            public int FileSize { get; set; }
            public string FileName { get; set; }
            public string FileType { get; set; }
            public string FileGuid { get; set; }
        }
        //fileHeader size = 1152
        [HttpPost("Validation")]
        public ActionResult<DTO_RESULT<AudioInfo>> Validation([FromForm] IFormFile file,[FromForm] string fileExt)
        {
            DTO_RESULT<AudioInfo> result = new DTO_RESULT<AudioInfo>();
            
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                try
                {
                    ms.Position = 0;
                    result.ResultObject = AudioEngine.GetAudioInfo(ms, fileExt);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
                }
            }
            return result;
        }
        [HttpPost("my-disk")]
        public ActionResult<DTO_RESULT> RegMyDisk([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string UserId, [FromForm] string title, [FromForm] string memo)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                //1. 임시파일 쓰기
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);
                    
                    string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    //파일 확장자
                    CheckFileExtensionValid(metaDataObject.FileName);

                    var tempPath = @"D:\FileUpload\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);

                    string tempFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    string newFileName = date + "_" + title + "_my-disk_" + metaDataObject.FileName;
                    
                    var tempFilePath = Path.Combine(tempPath, tempFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string newFilePath = ProcessUploadedFile(tempFilePath, newFileName, date);

                        MyDiskMeta MyDisk = new MyDiskMeta();
                        MyDisk.Title = title;
                        MyDisk.Memo = memo;
                        MyDisk.UserId = UserId;
                        MyDisk.FilePath = newFilePath;
                        MyDisk.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        MyDisk.SoundType = SoundDataTypes.MY_DISK;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        rb.Enqueue(MyDisk, (byte)userPriority);
                    }
                }
            //4. 작업결과 리턴하기

            result.ResultCode = RESUlT_CODES.SUCCESS;
            }
             catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPost("program")]
        public ActionResult<DTO_RESULT> RegProgram([FromForm] IFormFile file, [FromForm] string chunkMetadata,
            [FromForm] string UserId, [FromForm] string memo, [FromForm] string media, [FromForm] string productId ,[FromForm] string onairTime, [FromForm] string editor)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                //1. 임시파일 쓰기
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    //파일 확장자
                    CheckFileExtensionValid(metaDataObject.FileName);

                    var tempPath = @"D:\FileUpload\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);

                    string tempFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    string newFileName = date + "_" + memo + "_program_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, tempFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string newFilePath = ProcessUploadedFile(tempFilePath, newFileName, date);

                        ProgramMeta Program = new ProgramMeta();

                        Program.UserId = UserId;
                        Program.Memo = memo;
                        Program.Media = media;
                        Program.ProductId = productId;
                        Program.OnAirTime = onairTime;
                        Program.Editor = editor;
                        Program.FilePath = newFilePath;
                        Program.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        Program.SoundType = SoundDataTypes.PROGRAM;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);

                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        rb.Enqueue(Program, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPost("mcr-spot")]
        public ActionResult<DTO_RESULT> RegMcrSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata, 
            [FromForm] string UserId, [FromForm] string memo, [FromForm] string media, [FromForm] string productId, [FromForm] string onairTime, [FromForm] string editor)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                //1. 임시파일 쓰기
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    //파일 확장자
                    CheckFileExtensionValid(metaDataObject.FileName);

                    var tempPath = @"D:\FileUpload\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);

                    string tempFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    string newFileName = date + "_" + memo + "_mcr-spot_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, tempFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string newFilePath = ProcessUploadedFile(tempFilePath, newFileName, date);

                        McrMeta mcr = new McrMeta();

                        mcr.UserId = UserId;
                        mcr.Memo = memo;
                        mcr.Media = media;
                        mcr.ProductId = productId;
                        mcr.OnAirTime = onairTime;
                        mcr.Editor = editor;
                        mcr.FilePath = newFilePath;
                        mcr.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        mcr.SoundType = SoundDataTypes.MCR_SPOT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        rb.Enqueue(mcr, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }
        [HttpPost("scr-spot")]
        public ActionResult<DTO_RESULT> RegScrSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata, 
            [FromForm] string UserId, [FromForm] string title, [FromForm] string memo, [FromForm] string advertiser, [FromForm] string editor, 
            [FromForm] string media)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                //1. 임시파일 쓰기
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    //파일 확장자
                    CheckFileExtensionValid(metaDataObject.FileName);

                    var tempPath = @"D:\FileUpload\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);

                    string tempFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    string newFileName = date + "_" + title + "_scr-spot_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, tempFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string newFilePath = ProcessUploadedFile(tempFilePath, newFileName, date);

                        ScrMeta scr = new ScrMeta();

                        scr.UserId = UserId;
                        scr.Title = title;
                        scr.Memo = memo;
                        scr.Advertiser = advertiser;
                        scr.Media = media;
                        scr.Editor = editor;
                        scr.FilePath = newFilePath;
                        scr.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        scr.SoundType = SoundDataTypes.SCR_SPOT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        rb.Enqueue(scr, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }
        [HttpPost("static-spot")]
        public ActionResult<DTO_RESULT> RegStaticSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata,
            [FromForm] string UserId, [FromForm] string memo, [FromForm] string productId, [FromForm] string EDate, [FromForm] string SDate, [FromForm] string editor, 
            [FromForm] string media, [FromForm] string advertiser)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                //1. 임시파일 쓰기
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    //파일 확장자
                    CheckFileExtensionValid(metaDataObject.FileName);

                    var tempPath = @"D:\FileUpload\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);

                    string tempFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    string newFileName = date + "_" + memo + "_static_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, tempFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string newFilePath = ProcessUploadedFile(tempFilePath, newFileName, date);

                        StaticSpotMeta staticSpot = new StaticSpotMeta();

                        staticSpot.UserId = UserId;
                        staticSpot.SDate = SDate;
                        staticSpot.Memo = memo;
                        staticSpot.EDate = EDate;
                        staticSpot.Advertiser = advertiser;
                        staticSpot.Media = media;
                        staticSpot.ProductId = productId;
                        staticSpot.Editor = editor;
                        staticSpot.FilePath = newFilePath;
                        staticSpot.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        staticSpot.SoundType = SoundDataTypes.SCR_SPOT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        rb.Enqueue(staticSpot, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }
        [HttpPost("var-spot")]
        public ActionResult<DTO_RESULT> RegVarSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata,
            [FromForm] string UserId, [FromForm] string memo, [FromForm] string productId, [FromForm] string EDate, [FromForm] string SDate, [FromForm] string editor,
            [FromForm] string media, [FromForm] string advertiser)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                //1. 임시파일 쓰기
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    //파일 확장자
                    CheckFileExtensionValid(metaDataObject.FileName);

                    var tempPath = @"D:\FileUpload\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);

                    string tempFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    string newFileName = date + "_" + memo + "_var_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, tempFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string newFilePath = ProcessUploadedFile(tempFilePath, newFileName, date);

                        VarSpotMeta varSpot = new VarSpotMeta();

                        varSpot.UserId = UserId;
                        varSpot.SDate = SDate;
                        varSpot.Memo = memo;
                        varSpot.EDate = EDate;
                        varSpot.Advertiser = advertiser;
                        varSpot.Media = media;
                        varSpot.ProductId = productId;
                        varSpot.Editor = editor;
                        varSpot.FilePath = newFilePath;
                        varSpot.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        varSpot.SoundType = SoundDataTypes.SCR_SPOT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        rb.Enqueue(varSpot, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }
        [HttpPost("report")]
        public ActionResult<DTO_RESULT> RegReport([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string UserId, [FromForm] string memo, 
            [FromForm] string productId, [FromForm] string onairTime, [FromForm] string reporter, [FromForm] string editor, [FromForm] string media)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                //1. 임시파일 쓰기
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    //파일 확장자
                    CheckFileExtensionValid(metaDataObject.FileName);

                    var tempPath = @"D:\FileUpload\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);

                    string tempFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    string newFileName = date + "_" + memo + "_report_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, tempFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string newFilePath = ProcessUploadedFile(tempFilePath, newFileName, date);

                        ReportMeta report = new ReportMeta();

                        report.UserId = UserId;
                        report.OnAirTime = onairTime;
                        report.Memo = memo;
                        report.Reporter = reporter;
                        report.Media = media;
                        report.ProductId = productId;
                        report.Editor = editor;
                        report.FilePath = newFilePath;
                        report.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        report.SoundType = SoundDataTypes.SCR_SPOT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        rb.Enqueue(report, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }
        [HttpPost("filler")]
        public ActionResult<DTO_RESULT> RegFiller([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string UserId, [FromForm] string title,
            [FromForm] string memo, [FromForm] string onairTime, [FromForm] string editor, [FromForm] string media)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                //1. 임시파일 쓰기
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    //파일 확장자
                    CheckFileExtensionValid(metaDataObject.FileName);

                    var tempPath = @"D:\FileUpload\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);

                    string tempFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    string newFileName = date + "_" + title + "_filler_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, tempFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string newFilePath = ProcessUploadedFile(tempFilePath, newFileName, date);

                        FillerMeta Filler = new FillerMeta();

                        Filler.UserId = UserId;
                        Filler.Title = title;
                        Filler.Memo = memo;
                        Filler.Media = media;
                        Filler.Editor = editor;
                        Filler.OnAirTime = onairTime;
                        Filler.FilePath = newFilePath;
                        Filler.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        Filler.SoundType = SoundDataTypes.SCR_SPOT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        rb.Enqueue(Filler, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }      
        [HttpPost("pro")]
        public ActionResult<DTO_RESULT> RegPro([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }

        [HttpPatch("my-disk")]
        public ActionResult<DTO_RESULT> UpdateMyDisk([FromBody] AudioFileMetaBase jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if(jsonObject==null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }
        
        [HttpPatch("scr-spot")]
        public ActionResult<DTO_RESULT> UpdateScrSpot([FromBody] AudioFileMetaBase jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }
        
        [HttpPatch("report")]
        public ActionResult<DTO_RESULT> UpdateReport([FromBody] AudioFileMetaBase jsonObject)
        
        {
            DTO_RESULT result = new DTO_RESULT();
            
            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPatch("filler")]
        public ActionResult<DTO_RESULT> UpdateFiller([FromBody] AudioFileMetaBase jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPatch("pro")]
        public ActionResult<DTO_RESULT> UpdatePro([FromBody] AudioFileMetaBase jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("program/{id}")]
        public ActionResult<DTO_RESULT> DeleteProgram(string id)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("scr-spot/{id}")]
        public ActionResult<DTO_RESULT> DeleteScrSpot(string id)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("pro/{id}")]
        public ActionResult<DTO_RESULT> DeletePro(string id)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("report/{id}")]
        public ActionResult<DTO_RESULT> DeleteReport(string id)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("mcr-spot/{id}")]
        public ActionResult<DTO_RESULT> DeleteMcrSpot(string id)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("filler/{id}")]
        public ActionResult<DTO_RESULT> DeleteFiller(string id)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("filler-time/{id}")]
        public ActionResult<DTO_RESULT> DeleteFillerTime(string id)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("dl/{id}")]
        public ActionResult<DTO_RESULT> DeleteDL(string id)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bll"></param>
        /// <returns></returns>
        [HttpGet("mastering-status")]
        public ActionResult<DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>>> GetMasteringStatus([FromServices] APIBll bll, string user_id)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>>();
            result.ResultObject = new DTO_RESULT_LIST<DTO_MASTERING_INFO>();
            result.ResultObject.Data = bll.GetMasteringStatus(user_id);
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <param name="bll"></param>
        /// <returns></returns>
        [HttpGet("mastering-logs")]
        public ActionResult<DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>>> GetMasteringStatus2([FromQuery] string startDt, [FromQuery] string endDt, [FromQuery] string user_id, [FromServices] APIBll bll)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>>();
            result.ResultObject = new DTO_RESULT_LIST<DTO_MASTERING_INFO>();
            result.ResultObject.Data = bll.GetMasteringLogs(startDt, endDt, user_id);
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }


        void CheckFileExtensionValid(string fileName)
        {
            fileName = fileName.ToLower();
            string[] imageExtensions = { ".mp3", ".mp2", ".wav" };

            var isValidExtenstion = imageExtensions.Any(ext => {
                return fileName.LastIndexOf(ext) > -1;
            });
            if (!isValidExtenstion)
                throw new Exception("Not allowed file extension");
        }

        void CheckMaxFileSize(FileStream stream)
        {
            if (stream.Length > 4000000000)
                throw new Exception("File is too large");
        }

        void AppendContentToFile(string path, IFormFile content)
        {
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                content.CopyTo(stream);
                CheckMaxFileSize(stream);
            }
        }

        string ProcessUploadedFile(string tempFilePath, string fileName, string date)
        {
            var path = @"D:\FileUpload";
            string newFolder = Path.Combine(path, date);
            DirectoryInfo di = new DirectoryInfo(newFolder);

            if (di.Exists == false)
            {
                di.Create();
            }
            string newFilePath = Path.Combine(newFolder, fileName);
            // check if the uploaded file is a valid image
            System.IO.File.Move(tempFilePath, newFilePath);
            return newFilePath;
        }
    }
}

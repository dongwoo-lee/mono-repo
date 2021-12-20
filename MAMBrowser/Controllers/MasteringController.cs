using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Foundation;
using M30.AudioFile.DAL.DBParams;
using M30.AudioFile.DAL.Repositories;
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
        APIBll _apiBll;
        public MasteringController(APIBll apiBll)
        {
            _apiBll = apiBll;
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
        public ActionResult<DTO_RESULT> RegMyDisk([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string editor, [FromForm] string title, [FromForm] string memo)
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        MyDiskMeta MyDisk = new MyDiskMeta();
                        MyDisk.Title = title;
                        MyDisk.Memo = memo;
                        MyDisk.Editor = editor;
                        MyDisk.FilePath = tempFilePath;
                        MyDisk.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        MyDisk.SoundType = Define.AUDIO_FILE_TYPE_MYDISK;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)

                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(MyDisk, (byte)userPriority);
                        RequestMastering(MyDisk, (byte)userPriority);   //이동우
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
        public ActionResult<DTO_RESULT> RegProgram([FromForm] IFormFile file, [FromForm] string chunkMetadata,[FromForm] string title,
             [FromForm] string memo, [FromForm] string media, [FromForm] string productId ,[FromForm] string brdDTM, [FromForm] string editor)
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        

                        ProgramMeta Program = new ProgramMeta();

                        Program.Title = title;
                        Program.Memo = memo;
                        Program.Media = media;
                        Program.ProductId = productId;
                        Program.BrdDTM = brdDTM;
                        Program.Editor = editor;
                        Program.FilePath = tempFilePath;
                        Program.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        Program.SoundType = Define.AUDIO_FILE_TYPE_PGM;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);

                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(Program, (byte)userPriority);
                        RequestMastering(Program, (byte)userPriority);   //이동우
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
           [FromForm] string title, [FromForm] string memo, [FromForm] string media, [FromForm] string productId, [FromForm] string brdDT, [FromForm] string editor, [FromForm] string advertiser)
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);


                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        

                        McrMeta mcr = new McrMeta();

                        mcr.Title = title;
                        mcr.Memo = memo;
                        mcr.Media = media;
                        mcr.ProductId = productId;
                        mcr.BrdDT = brdDT;
                        mcr.Editor = editor;
                        mcr.Advertiser = advertiser;
                        mcr.FilePath = tempFilePath;
                        mcr.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        mcr.SoundType = Define.AUDIO_FILE_TYPE_MCR_SPOT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(mcr, (byte)userPriority);
                        RequestMastering(mcr, (byte)userPriority);   //이동우
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
             [FromForm] string title, [FromForm] string memo, [FromForm] string advertiser, [FromForm] string editor, 
            [FromForm] string category)
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        

                        ScrMeta scr = new ScrMeta();

                        scr.Title = title;
                        scr.Memo = memo;
                        scr.Advertiser = advertiser;
                        scr.Category = category;
                        scr.Editor = editor;
                        scr.FilePath = tempFilePath;
                        scr.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        scr.SoundType = Define.AUDIO_FILE_TYPE_SCR_SPOT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(scr, (byte)userPriority);
                        RequestMastering(scr, (byte)userPriority);   //이동우
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
            [FromForm] string title, [FromForm] string memo, [FromForm] string productId, [FromForm] string EDate, [FromForm] string SDate, [FromForm] string editor, 
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        

                        FillerTimeMeta staticSpot = new FillerTimeMeta();

                        staticSpot.Title = title;
                        staticSpot.SDate = SDate;
                        staticSpot.Memo = memo;
                        staticSpot.EDate = EDate;
                        staticSpot.Advertiser = advertiser;
                        staticSpot.Media = media;
                        staticSpot.ProductId = productId;
                        staticSpot.Editor = editor;
                        staticSpot.FilePath = tempFilePath;
                        staticSpot.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        staticSpot.SoundType = Define.AUDIO_FILE_TYPE_STATIC;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(staticSpot, (byte)userPriority);
                        RequestMastering(staticSpot, (byte)userPriority);   //이동우
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
            [FromForm] string title, [FromForm] string memo, [FromForm] string productId, [FromForm] string EDate, [FromForm] string SDate, [FromForm] string editor,
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        

                        FillerTimeMeta varSpot = new FillerTimeMeta();

                        varSpot.Title = title;
                        varSpot.SDate = SDate;
                        varSpot.Memo = memo;
                        varSpot.EDate = EDate;
                        varSpot.Advertiser = advertiser;
                        varSpot.Media = media;
                        varSpot.ProductId = productId;
                        varSpot.Editor = editor;
                        varSpot.FilePath = tempFilePath;
                        varSpot.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        varSpot.SoundType = Define.AUDIO_FILE_TYPE_VAR;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(varSpot, (byte)userPriority);
                        RequestMastering(varSpot, (byte)userPriority);   //이동우
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
        public ActionResult<DTO_RESULT> RegReport([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string title,  [FromForm] string memo, 
            [FromForm] string productId, [FromForm] string brdDT, [FromForm] string reporter, [FromForm] string editor, [FromForm] string category)
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        

                        ReportMeta report = new ReportMeta();

                        report.Title = title;
                        report.BrdDT = brdDT;
                        report.Memo = memo;
                        report.Reporter = reporter;
                        report.Category = category;
                        report.ProductId = productId;
                        report.Editor = editor;
                        report.FilePath = tempFilePath;
                        report.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        report.SoundType = Define.AUDIO_FILE_TYPE_REPORT;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(report, (byte)userPriority);
                        RequestMastering(report, (byte)userPriority);   //이동우
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
        public ActionResult<DTO_RESULT> RegFiller([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string title,
            [FromForm] string memo, [FromForm] string brdDT, [FromForm] string editor, [FromForm] string category)
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        

                        FillerMeta Filler = new FillerMeta();

                        Filler.Title = title;
                        Filler.Memo = memo;
                        Filler.Category = category;
                        Filler.Editor = editor;
                        Filler.BrdDT = brdDT;
                        Filler.FilePath = tempFilePath;
                        Filler.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        Filler.SoundType = Define.AUDIO_FILE_TYPE_FILLER;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(Filler, (byte)userPriority);
                        RequestMastering(Filler, (byte)userPriority);   //이동우
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
        public ActionResult<DTO_RESULT> RegPro([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string editor,
           [FromForm] string title, [FromForm] string memo, [FromForm] string typeName, [FromForm] string type, [FromForm] string category)
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

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                    var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                    var host = CommonUtility.GetHost(tempPath);
                    var userinfo = GetStorageUserInfo(option);
                    NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    //청크 파일 어펜드
                    AppendContentToFile(tempFilePath, file);

                    //파일 업로드
                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        

                        ProMeta pro = new ProMeta();

                        pro.Title = title;
                        pro.Memo = memo;
                        pro.Category = category;
                        pro.Editor = editor;
                        pro.Type = type;
                        pro.TypeName = typeName;
                        pro.FilePath = tempFilePath;
                        pro.RegDtm = DateTime.Now.ToString(Define.DTM19);
                        pro.SoundType = Define.AUDIO_FILE_TYPE_PRO;

                        //2. 우선순위 확인 (ip, user, brdDtm)           
                        //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
                        var users = Startup.AppSetting.MasteringPriorities["Users"] as Dictionary<string, int>;
                        var userPriority = Convert.ToInt32(users["S01G04C001"]);


                        //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기 임시파일패스
                        //(임시파일 삭제는 다른 백그라운드 워커에서 처리)
                        //RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
                        //rb.Enqueue(pro, (byte)userPriority);
                        RequestMastering(pro, (byte)userPriority);   //이동우
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

        [HttpPatch("mydisk")]
        public ActionResult<DTO_RESULT> UpdateMyDisk([FromBody] UpdateMyDiskMeta jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if(jsonObject==null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                PrivateSpaceRepository repo = new PrivateSpaceRepository(Startup.AppSetting.ConnectionString);
                repo.EditMeta(new U_PrivateSpaceParam(jsonObject));
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPatch("program")]
        public ActionResult<DTO_RESULT> UpdateProgram([FromBody] UpdateProgramMeta jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                PGMRepository repo = new PGMRepository(Startup.AppSetting.ConnectionString);
                repo.EditMeta(new U_ProgramParam(jsonObject));
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPatch("mcr-spot")]
        public ActionResult<DTO_RESULT> UpdateMcrSpot([FromBody] UpdateMcrSpotMeta jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                McrSpotRepository repo = new McrSpotRepository(Startup.AppSetting.ConnectionString);
                repo.EditMeta(new U_McrSpotParam(jsonObject));
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPatch("scr-spot")]
        public ActionResult<DTO_RESULT> UpdateScrSpot([FromBody] UpdateScrSpotMeta jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                ScrSpotRepoository repo = new ScrSpotRepoository(Startup.AppSetting.ConnectionString);
                repo.EditMeta(new U_ScrSpotParam(jsonObject));
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }
        
        [HttpPatch("report")]
        public ActionResult<DTO_RESULT> UpdateReport([FromBody] UpdateReportMeta jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();
            
            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                ReportRepository repo = new ReportRepository(Startup.AppSetting.ConnectionString);
                repo.EditMeta(new U_ReportParam(jsonObject));
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPatch("filler-time")]
        public ActionResult<DTO_RESULT> UpdateFillerTime([FromBody] UpdateFillerTimeMeta jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");
                
                FillerTimeRepository repo = new FillerTimeRepository(Startup.AppSetting.ConnectionString);
                repo.EditMeta(new U_FillerTimeParam(jsonObject));
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPatch("filler")]
        public ActionResult<DTO_RESULT> UpdateFiller([FromBody] UpdateFillerMeta jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                FillerRepository repo = new FillerRepository(Startup.AppSetting.ConnectionString);
                repo.EditMeta(new U_FillerParam(jsonObject));
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpPatch("pro")]
        public ActionResult<DTO_RESULT> UpdatePro([FromBody] UpdateProMeta jsonObject)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (jsonObject == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                ProRepository repo = new ProRepository(Startup.AppSetting.ConnectionString);
                repo.EditMeta(new U_ProParam(jsonObject));
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("program/{id}")]
        public ActionResult<DTO_RESULT> DeleteProgram(string id, [FromQuery] string fileToken)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                DeleteAudioFile(HttpContext.Items[Define.USER_ID] as string, id, fileToken);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("scr-spot")]
        public ActionResult<DTO_RESULT> DeleteScrSpot([FromQuery] string spotID, [FromQuery] string productID, [FromQuery] string brdDT, [FromQuery] string fileToken)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (spotID == null || productID  == null || brdDT == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                MoveRecycle(fileToken);
                ScrSpotOperRepository repo = new ScrSpotOperRepository(Startup.AppSetting.ConnectionString);
                repo.Delete(new D_ScrSpotOperParam { SpotID=spotID, ProductID=productID, OnAirDate=brdDT});
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("pro/{id}")]
        public ActionResult<DTO_RESULT> DeletePro(string id, [FromQuery] string fileToken)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                DeleteAudioFile(HttpContext.Items[Define.USER_ID] as string, id, fileToken);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("report/{id}")]
        public ActionResult<DTO_RESULT> DeleteReport(string id, [FromQuery] string fileToken)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                DeleteAudioFile(HttpContext.Items[Define.USER_ID] as string, id, fileToken);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("mcr-spot/{id}")]
        public ActionResult<DTO_RESULT> DeleteMcrSpot(string id, [FromQuery] string fileToken)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                DeleteAudioFile(HttpContext.Items[Define.USER_ID] as string, id, fileToken);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("filler/{id}")]
        public ActionResult<DTO_RESULT> DeleteFiller(string id, [FromQuery] string fileToken)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                DeleteAudioFile(HttpContext.Items[Define.USER_ID] as string, id, fileToken);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return result;
        }

        [HttpDelete("filler-time/{id}")]
        public ActionResult<DTO_RESULT> DeleteFillerTime(string id, [FromQuery] string fileToken)
        {
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                if (id == null)
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, "parameter is empty");

                DeleteAudioFile(HttpContext.Items[Define.USER_ID] as string, id, fileToken);
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

        string GetTempFileName(ChunkMetadata meta)
        {
            string date = DateTime.Now.ToString(Define.DTM8);
            return $"{date}_{meta.FileGuid}_{Path.GetFileNameWithoutExtension(meta.FileName)}.tmp";
        }
       
        Dictionary<string,string> GetStorageUserInfo(IList<DTO_NAMEVALUE> option)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("id", option.ToList().Find(dt => dt.Name == "STORAGE_ID").Value.ToString());
            dictionary.Add("pass", option.ToList().Find(dt => dt.Name == "STORAGE_PASS").Value.ToString());
            return dictionary;
        }
        void RequestMastering(MasteringMetaBase meta, byte priority)
        {
            var mstParam = new I_MSTHistoryParam(meta);
            var connectionString = Startup.AppSetting.ConnectionString;
            MSTHistoryRepository repository = new MSTHistoryRepository(connectionString);
            var mstSeq = repository.Add<long>(mstParam);

            meta.MstSeq = mstSeq;
            RabbitMQueue.RabbitMQ rb = new RabbitMQueue.RabbitMQ();
            rb.Enqueue(meta, (byte)priority);
        }


        void DeleteAudioFile(string userId, string id, string fileToken)
        {
            var movedFilePath = MoveRecycle(fileToken);
            AudioFileRepository repo = new AudioFileRepository(Startup.AppSetting.ConnectionString);
            repo.Delete(id, userId, movedFilePath);
        }
      
        string MoveRecycle(string fileToken)
        {
            string filePath = "";
            if (TokenGenerator.ValidateFileToken(fileToken, ref filePath))
            {
                if (string.IsNullOrEmpty(filePath))
                    return string.Empty;

                var recycleFoler = _apiBll.GetOptions("S01G06C001").ToList().Find(dt => dt.Name == "RECYCLE_PATH").Value.ToString();
                var id = _apiBll.GetOptions("S01G06C001").ToList().Find(dt => dt.Name == "STORAGE_ID").Value.ToString();
                var pass = _apiBll.GetOptions("S01G06C001").ToList().Find(dt => dt.Name == "STORAGE_PASS").Value.ToString();
                var host = CommonUtility.GetHost(recycleFoler);
                NetworkShareAccessor.Access(host, id, pass);

                if(!Directory.Exists(recycleFoler))
                    Directory.CreateDirectory(recycleFoler);

                var newFileName = Path.GetFileName(filePath);
                var newFileFullPath = Path.Combine(recycleFoler, newFileName);
                System.IO.File.Move(filePath, newFileFullPath);
                return newFileFullPath;
            }
            return String.Empty;
        }
    }
}

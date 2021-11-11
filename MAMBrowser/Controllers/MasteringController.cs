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
        public ActionResult<DTO_RESULT> MyDisk([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string UserId, [FromForm] string title, [FromForm] string memo)
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

                    var tempPath = @"D:\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);
                    string newFileName = date + "_" + title + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;

                    
                    var tempFilePath = Path.Combine(tempPath, newFileName + ".tmp");
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
                        RabbitMQ(MyDisk, (byte)userPriority);
                    }
                }
            //4. 작업결과 리턴하기

            result.ResultCode = RESUlT_CODES.SUCCESS;
            }
             catch (Exception ex)
            {
                return StatusCode(400, "파일 업로드 실패");
            }
            return result;
        }

        [HttpPost("program")]
        public ActionResult<DTO_RESULT> Program([FromForm] IFormFile file, [FromForm] string chunkMetadata,
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

                    var tempPath = @"D:\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);
                    string newFileName = date + "_" + productId + "_"+ metaDataObject.FileGuid + "_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, newFileName + ".tmp");
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
                        RabbitMQ(Program, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(400, "파일 업로드 실패");
            }
            return result;
        }

        [HttpPost("mcr-spot")]
        public ActionResult<DTO_RESULT> McrSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata, 
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

                    var tempPath = @"D:\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);
                    string newFileName = date + "_" + productId + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, newFileName + ".tmp");
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
                        RabbitMQ(mcr, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(400, "파일 업로드 실패");
            }
            return result;
        }
        [HttpPost("scr-spot")]
        public ActionResult<DTO_RESULT> ScrSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata, 
            [FromForm] string UserId, [FromForm] string title, [FromForm] string memo, [FromForm] string usage, [FromForm] string advertiser, [FromForm] string editor, 
            [FromForm] string media, [FromForm] string onairTime)
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

                    var tempPath = @"D:\Temp";

                    string date = DateTime.Now.ToString(Define.DTM8);
                    string newFileName = date + "_" + title + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;


                    var tempFilePath = Path.Combine(tempPath, newFileName + ".tmp");
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
                        scr.Usage = usage;
                        scr.Advertiser = advertiser;
                        scr.Media = media;
                        scr.OnAirTime = onairTime;
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
                        RabbitMQ(scr, (byte)userPriority);
                    }
                }
                //4. 작업결과 리턴하기

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                return StatusCode(400, "파일 업로드 실패");
            }
            return result;
        }
        [HttpPost("filler")]
        public ActionResult<DTO_RESULT> Filler([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }
        [HttpPost("report")]
        public ActionResult<DTO_RESULT> Report([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }
        [HttpPost("static-spot")]
        public ActionResult<DTO_RESULT> StaticSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }
        [HttpPost("var-spot")]
        public ActionResult<DTO_RESULT> VarSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }
        [HttpPost("pro")]
        public ActionResult<DTO_RESULT> Pro([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="bll"></param>
      /// <returns></returns>
        [HttpGet("mastering-status")]
        public ActionResult<DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>>> GetMasteringStatus([FromServices] APIBll bll)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>>();
            result.ResultObject = new DTO_RESULT_LIST<DTO_MASTERING_INFO>();
            result.ResultObject.Data = bll.GetMasteringStatus();
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
        public ActionResult<DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>>> GetMasteringStatus2([FromQuery] string startDt, [FromQuery] string endDt, [FromServices] APIBll bll)
        {
            
            DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MASTERING_INFO>>();
            result.ResultObject = new DTO_RESULT_LIST<DTO_MASTERING_INFO>();
            result.ResultObject.Data = bll.GetMasteringLogs(startDt, endDt);
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
            System.IO.File.Copy(tempFilePath, newFilePath);
            return newFilePath;
        }

        void RabbitMQ(object value, byte priority)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            Dictionary<string, object> props = new Dictionary<string, object>();
            props.Add("x-max-priority", 10);
            channel.QueueDeclare("message",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: props);

            var properties = channel.CreateBasicProperties();
            properties.Priority = priority;

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            channel.BasicPublish("", "message", properties, body);

        }
    }
}

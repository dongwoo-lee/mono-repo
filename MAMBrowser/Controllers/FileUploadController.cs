using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Models;
using M30.AudioFile.DAL.Dao;
using MAMBrowser.Foundation;
using MAMBrowser.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//MBC File Upload 작업 중
namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/fileupload")]
    public class FileUploadController : Controller
    {
        private readonly PrivateFileDao _dao;
        private readonly IHubContext<FileHubs> _hubContext;

        public class ChunkMetadata
        {
            public int index { get; set; }
            public int TotalCount { get; set; }
            public int FileSize { get; set; }
            public string FileName { get; set; }
            public string FileType { get; set; }
            public string FileGuid { get; set; }
        }

        public class FileInfo
        {
            public int step { get; set; }
            public int FileSize { get; set; }
            public string FileName { get; set; }
            public string FileType { get; set; }
            public string FileGuid { get; set; }
            public string user_id { get; set; }
            public string title { get; set; }
            public string memo { get; set; }
        }

        public class Validate 
        { 
            public string duration { get; set; }
            public string audioFormat { get; set; }
        }

        public FileUploadController(IHubContext<FileHubs> hubContext, PrivateFileDao dao)
        {
            _dao = dao;
            _hubContext = hubContext;
        }


        //header stream
        //[HttpPost("check")]
        //public ActionResult check([FromForm] IFormFile file)
        //{
        //    try
        //    {
        //        var stream = file.OpenReadStream();
        //        var buffer = new byte[1024];
        //        int count = 0;
        //        stream.Read(buffer, 0, buffer.Length);

        //        Console.WriteLine(buffer);
        //    }
        //     catch (Exception ex)
        //    {
        //        return StatusCode(400, ex);
        //    }
        //    return new EmptyResult();
        //}

        [HttpPost("UploadChunk")]
        public ActionResult<string> UploadChunk([FromForm] IFormFile file, [FromForm] string chunkMetadata, [FromForm] string user_id, [FromForm] string connectionId,
            [FromForm] string title, [FromForm] string memo, [FromForm] long fileSize, [FromForm] Object ProgramSelected, [FromForm] string mediaCD, [FromForm] string categoryCD)
        {
            try
            {
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    CheckFileExtensionValid(metaDataObject.FileName);
                    string sdate = DateTime.Now.ToString(Define.DTM8);

                    string newtmpFileName = sdate + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;
                    var tempPath = @"D:\Temp";

                    // Uncomment to save the file
                    var tempFilePath = Path.Combine(tempPath, newtmpFileName + ".tmp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    AppendContentToFile(tempFilePath, file);

                    if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                    {
                        string date = DateTime.Now.ToString(Define.DTM8);

                        string newFileName = date + "_" + metaDataObject.FileGuid + "_" + metaDataObject.FileName;

                        //Upload (비동기 Upload await)
                        ProcessUploadedFile(tempFilePath, newFileName, date, user_id, title, memo, fileSize);
                        //ProcessUploadedFile(tempFilePath, newFileName, date);

                        RemoveTempFilesAfterDelay(tempFilePath, 15);
                        FileInfo fi = new FileInfo();
                        fi.FileGuid = metaDataObject.FileGuid;
                        fi.FileName = metaDataObject.FileName;
                        fi.FileSize = metaDataObject.FileSize;
                        fi.FileType = metaDataObject.FileType;
                        fi.step = 0;
                        fi.user_id = user_id;
                        fi.title = title;
                        fi.memo = memo;

                        string json = JsonConvert.SerializeObject(fi);
                        _hubContext.Clients.Client(connectionId).SendAsync("send", 1, fi);

                        RabbitMQ(fi);

                        Thread.Sleep(5000);
                        fi.step = 1;
                        _hubContext.Clients.Client(connectionId).SendAsync("send", 2, fi);

                        Thread.Sleep(5000);
                        fi.step = 2;
                        _hubContext.Clients.Client(connectionId).SendAsync("send", 3, fi);

                        Thread.Sleep(5000);
                        fi.step = 3;
                        _hubContext.Clients.Client(connectionId).SendAsync("send", 4, fi);

                        Thread.Sleep(5000);
                        fi.step = 4;

                        _hubContext.Clients.Client(connectionId).SendAsync("send", 5, fi);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex);
            }
            return new ActionResult<string>("ㅋㅋㅋ");
        }

        [HttpPost("Validation")]
        public ActionResult<string> Validation([FromForm] string metaData)
        {

            //DTO_RESULT<string> result = new DTO_RESULT<string>();

           
            if (metaData == "0")   //성공
            {
                Validate v = new Validate();
                v.duration = "값";
                v.audioFormat = "포맷값";

                string json = JsonConvert.SerializeObject(v);
                return new ActionResult<string>(json);
            }
            else if (metaData == "1")   //유효성검사 실패
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("errorMsg", "에러문구");
                var returnData = System.Text.Json.JsonSerializer.Serialize(param);
                return new ActionResult<string>(returnData);
            }
            return new ActionResult<string>("ㅋㅋㅋ");
        }


        void RemoveTempFilesAfterDelay(string path, int time)
        {
            Thread.Sleep(time);
            System.IO.File.Delete(path);
        }
        void CheckFileExtensionValid(string fileName)
        {
            fileName = fileName.ToLower();
            string[] imageExtensions = { ".jpg", ".jpeg", ".gif", ".png", ".mp3", ".mpeg", ".wav" };

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
       void ProcessUploadedFile(string tempFilePath, string fileName, string date, string userId, string title, string memo, long fileSize)
        //private void ProcessUploadedFile(string tempFilePath, string fileName, string date)
        {
            try
            {
                M30_MAM_PRIVATE_SPACE metaData = new M30_MAM_PRIVATE_SPACE();
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

                //SQL
                long ID = _dao.GetID();
                string sqlDate = DateTime.Now.ToString(Define.DTM8);
                using (FileStream stream = new FileStream(newFilePath, FileMode.Open))
                {
                    var headerStream = AudioEngine.GetHeaderStream(stream);
                    headerStream.Position = 0;
                    var audioFormat = AudioEngine.GetAudioFormat(headerStream, newFilePath);
                    headerStream.Position = 0;
                    metaData.SEQ = ID;
                    metaData.USER_ID = userId;
                    metaData.AUDIO_FORMAT = audioFormat;
                    metaData.FILE_PATH = newFilePath;
                    metaData.TITLE = title;
                    metaData.MEMO = memo;
                    metaData.FILE_SIZE = fileSize;
                    _dao.Insert(metaData);
                }
            }
            catch
            {
                throw;
            }
        }

        void AppendContentToFile(string path, IFormFile content)
        {
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                content.CopyTo(stream);
                CheckMaxFileSize(stream);
            }
        }

        void RabbitMQ(object value)
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
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            channel.BasicPublish("", "message", properties, body);

        }



    }
}
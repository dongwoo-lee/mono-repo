using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using MAMBrowser.MAMDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

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

        //fileHeader size = 1152
        [HttpPost("Validation")]
        public ActionResult<DTO_RESULT<AudioInfo>> Validation(byte[] fileHeaders, string fileExt)
        {
            DTO_RESULT<AudioInfo> result = new DTO_RESULT<AudioInfo>();
            using (MemoryStream ms = new MemoryStream(fileHeaders))
            {
                try
                {
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
        public ActionResult<DTO_RESULT> MyDisk([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            string clientIp = HttpContext.Connection.RemoteIpAddress.ToString();

            //1. 임시파일 쓰기


            //2. 우선순위 확인 (ip, user, brdDtm)           
            //id로 유저 권한을 가져와서 권한에 해당하는 우선순위를 가져온다.
            var users = Startup.AppSetting.MasteringPriorities["Users"] as NameValueCollection;
            var userPriority = Convert.ToInt32(users["S01G04C001"]);


            //3. 완료되면 RabbitMQ로 메타데이터, 우선순위 포함해서 보내기
            //(임시파일 삭제는 다른 백그라운드 워커에서 처리)


            //4. 작업결과 리턴하기
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

        [HttpPost("Program")]
        public ActionResult<DTO_RESULT> Program([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }
       
        [HttpPost("mcr-spot")]
        public ActionResult<DTO_RESULT> McrSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }
        [HttpPost("scr-spot")]
        public ActionResult<DTO_RESULT> ScrSpot([FromForm] IFormFile file, [FromForm] string chunkMetadata, [ModelBinder(BinderType = typeof(JsonModelBinder))] MyDiskMeta meta)
        {
            DTO_RESULT result = new DTO_RESULT();
            result.ResultCode = RESUlT_CODES.SUCCESS;
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
    }
}

using MAMBrowser.BLL;
using Microsoft.AspNetCore.Mvc;
using System;
using MAMBrowser.DTO;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common;
using M30_CueSheetDAO.ParamEntity;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.IO;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramInfomationController : ControllerBase
    {
        private readonly ProgramInfomationBll _bll;
        public ProgramInfomationController(ProgramInfomationBll bll)
        {
            _bll = bll;
        }

        [HttpPost("GetProgramInfo")]
        public DTO_RESULT<ProgramInfomationDTO> GetProgramInfoList([FromBody] ProgramInfoParamDTO dto)
        {
            var result = new DTO_RESULT<ProgramInfomationDTO>();
            try
            {
                result.ResultObject = _bll.GetProgramInfomationList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpGet("GetPgmImgFile")]
        public DTO_RESULT<string> GetPgmImgFile(string imgPath)
        {
            var result = new DTO_RESULT<string>();
            try
            {
                if (System.IO.File.Exists(imgPath))
                {
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
                    string fileExtension = Path.GetExtension(imgPath).ToLower();
                    string base64Image = Convert.ToBase64String(imageBytes);
                    result.ResultObject = base64Image;
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.FILE_NOT_FOUND;
            }
            return result;
        }

        [HttpPost("UpdatePgmImgFile")]
        public DTO_RESULT<IActionResult> UpdatePgmImgFile()
        {
            var result = new DTO_RESULT<IActionResult>();
            try
            {
                var file = Request.Form.Files.Count>0 ? Request.Form.Files[0] : null;
                var pgmcode = Request.Form["pgmcode"];
                _bll.UpdatePgmImgFile(file, pgmcode);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.FILE_NOT_FOUND;
            }
            return result;
        }
    }

}

using MAMBrowser.BLL;
using Microsoft.AspNetCore.Mvc;
using System;
using MAMBrowser.DTO;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common;
using M30_CueSheetDAO.ParamEntity;

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
    }

}

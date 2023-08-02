using MAMBrowser.BLL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MAMBrowser.DTO;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common;
using static MAMBrowser.DTO.ManagementSystemDTO;
using M30_ManagementControlDAO.Entity;
using M30_ManagementControlDAO.WebService;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudioInfomationController : ControllerBase
    {
        private readonly StudioInfomationBll _bll;

        public StudioInfomationController(StudioInfomationBll bll)
        {
            _bll= bll;
        }

        [HttpGet("GetSudioInfoMenu")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetStudioInfoMenu()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetStudioInfoMenu();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpGet("GetSudioAssignList")]
        public DTO_RESULT<StudioSchedulerDTO> GetStudioAssignList([FromQuery] string as_from, [FromQuery] string as_to, [FromQuery] string as_stid)
        {
            var result = new DTO_RESULT<StudioSchedulerDTO>();
            try
            {
                result.ResultObject = _bll.GetStudioAssignList(as_from, as_to, as_stid);
                result.ResultCode= RESUlT_CODES.SUCCESS;
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

using MAMBrowser.BLL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MAMBrowser.DTO;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common;
using static MAMBrowser.DTO.ManagementSystemDTO;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransMissionListController : ControllerBase
    {
        private readonly TransMissionListBll _bll;
        public TransMissionListController(TransMissionListBll bll)
        {
            _bll = bll;
        }

        [HttpPost("GetTransMissionList")]
        public DTO_RESULT<PageListCollectionDTO<TransMissionListItemDTO>> GetTransMissionList([FromBody] TransMissionListParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<TransMissionListItemDTO>> result = new DTO_RESULT<PageListCollectionDTO<TransMissionListItemDTO>>();
            try
            {
                result.ResultObject = _bll.GetTransMissionList(dto);
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

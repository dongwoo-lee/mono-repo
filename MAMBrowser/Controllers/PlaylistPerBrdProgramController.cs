using MAMBrowser.BLL;
using Microsoft.AspNetCore.Mvc;
using System;
using MAMBrowser.DTO;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistPerBrdProgramController : ControllerBase
    {
        private readonly PlaylistPerBrdProgramBll _bll;
        public PlaylistPerBrdProgramController(PlaylistPerBrdProgramBll bll)
        {
            _bll = bll;
        }

        [HttpPost("GetPlaylistProgram")]
        public DTO_RESULT<PageListCollectionDTO<PlaylistPerBrdProgramDTO>> GetPlaylistProgram([FromBody] PlaylistPerBrdProgramParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<PlaylistPerBrdProgramDTO>> result = new DTO_RESULT<PageListCollectionDTO<PlaylistPerBrdProgramDTO>>();
            try
            {
                result.ResultObject = _bll.GetPlaylistBrdProgramList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        [HttpPost("GetPlaylistStatistics")]
        public DTO_RESULT<PageListCollectionDTO<PlaylistStatisticsDTO>> GetPlaylistStatistics([FromBody] PlaylistStatisticsParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<PlaylistStatisticsDTO>> result = new DTO_RESULT<PageListCollectionDTO<PlaylistStatisticsDTO>>();
            try
            {
                result.ResultObject = _bll.GetPlaylistStatisticsList(dto);
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

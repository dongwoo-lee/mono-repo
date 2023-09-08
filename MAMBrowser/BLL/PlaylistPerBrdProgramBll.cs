using DevExpress.Xpo.Logger;
using M30_ManagementControlDAO.Interfaces;
using M30_ManagementControlDAO.ParamEntity;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using Microsoft.Extensions.Logging;

namespace MAMBrowser.BLL
{
    public class PlaylistPerBrdProgramBll
    {
        private readonly IPlaylistPerBrdProgramDAO _dao;
        private readonly ILogger<PlaylistPerBrdProgramBll> _logger;
        public PlaylistPerBrdProgramBll(IPlaylistPerBrdProgramDAO dao,ILogger<PlaylistPerBrdProgramBll> logger)
        {
            _dao = dao;
            _logger= logger;
        }
        public PageListCollectionDTO<PlaylistPerBrdProgramDTO> GetPlaylistBrdProgramList(PlaylistPerBrdProgramParamDTO dto)
        {
            var result = new PageListCollectionDTO<PlaylistPerBrdProgramDTO>();
            var param = new PlaylistPerBrdProgramParamBuilder()
                .SetBrdDate(dto.brddate)
                .SetProductid(dto.productid)
                .SetPeriod(dto.period)
                .SetUserid(dto.userid)
                .SetEnddate(dto.enddate)
                .SetAudioclipid(dto.audioclipid)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();

            var data = _dao.GetPlaylistPerProgram(param);

            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }
        public PageListCollectionDTO<PlaylistStatisticsDTO> GetPlaylistStatisticsList(PlaylistStatisticsParamDTO dto)
        {
            var result = new PageListCollectionDTO<PlaylistStatisticsDTO>();
            var param = new PlaylistStatisticsParamBuilder()
                .SetEndDate(dto.enddate)
                .SetMedia(dto.media)
                .SetPeriod(dto.period)
                .SetPersonId(dto.personid)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();

            var data = _dao.GetPlaylistStatistics(param);

            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }

    }
}

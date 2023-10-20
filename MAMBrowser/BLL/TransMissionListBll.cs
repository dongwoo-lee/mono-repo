using DevExpress.Xpo.Logger;
using M30.AudioFile.Common;
using M30_ManagementControlDAO.DAO;
using M30_ManagementControlDAO.Interfaces;
using M30_ManagementControlDAO.ParamEntity;
using M30_ManagementControlDAO.WebService;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace MAMBrowser.BLL
{
    public class TransMissionListBll
    {
        private readonly ITransMissionListDAO _dao;
        private readonly IStudioWebService _studioService;
        private readonly ILogger<TransMissionListBll> _logger;

        public TransMissionListBll(ITransMissionListDAO dao, StudioWebService studioService,ILogger<TransMissionListBll> logger)
        {
            _dao = dao;
            _studioService = new StudioSystemMockup(studioService);
            //_studioService = studioService;
            _logger = logger;
        }
        public PageListCollectionDTO<TransMissionListItemDTO> GetTransMissionList(TransMissionListParamDTO dto)
        {
            var result = new PageListCollectionDTO<TransMissionListItemDTO>();
            var param = new TransMissionListParamBuilder()
                .SetBrdDate(dto.brddate)
                .SetMedia(dto.media)
                .SetProductType(dto.producttype)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();

            var data = _dao.GetTransMissionList(param);
            var studioAssign = _studioService.GetStudioAssign(dto.brddate, dto.brddate, "", "");
            _logger.LogInformation($"SelectAssignedStudio_transmissionList : {studioAssign}");

            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting(studioAssign);
            return result;
        }
    }
}

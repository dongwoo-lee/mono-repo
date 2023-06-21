using M30_ManagementControlDAO.DAO;
using M30_ManagementControlDAO.Interfaces;
using M30_ManagementControlDAO.ParamEntity;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using System.Collections.Generic;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace MAMBrowser.BLL
{
    public class TransMissionListBll
    {
        private readonly ITransMissionListDAO _dao;
        public TransMissionListBll(ITransMissionListDAO dao)
        {
            _dao = dao;
        }
        public PageListCollectionDTO<TransMissionListItemDTO> GetTransMissionList(TransMissionListParamDTO dto)
        {
            var result = new PageListCollectionDTO<TransMissionListItemDTO>();
            var param = new TransMissionListParamBuilder()
                .SetBrdDate(dto.brddate)
                .SetMedia(dto.media)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();

            var data = _dao.GetTransMissionList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;

        }
    }
}

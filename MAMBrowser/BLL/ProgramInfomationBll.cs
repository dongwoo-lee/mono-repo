using M30_ManagementControlDAO.Interfaces;
using M30_ManagementControlDAO.ParamEntity;
using M30_ManagementControlDAO.WebService;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using System;
using System.Collections.Generic;

namespace MAMBrowser.BLL
{
    public class ProgramInfomationBll
    {
        private readonly IProgramInfomationDAO _dao;
        private readonly IStudioWebService _studioService;
        public ProgramInfomationBll(IProgramInfomationDAO dao, StudioWebService studioService)
        {
            _dao = dao;
            //_studioService = new StudioSystemMockup(studioService);
            _studioService = studioService;
        }
        public ProgramInfomationDTO GetProgramInfomationList(ProgramInfoParamDTO dto)
        {
            var result = new ProgramInfomationDTO();
            result.productIdDetails = new List<ProductIdDetail>();
            var param = new ProgramInfomationParamBuilder()
                .SetBrdDate(dto.brddate)
                .SetMedia(dto.media)
                .SetPgmCode(dto.pgmcode)
                .Build();
            var entity = _dao.GetProgramInfomationList(param);
            if (entity.Count > 0 && DateTime.ParseExact(entity[0].STARTDATE, "yyyyMMdd", null)<= DateTime.ParseExact(dto.brddate, "yyyyMMdd", null))
            {
                var studioAssign = _studioService.GetStudioAssign(dto.brddate, dto.brddate, null, null);
                result = _dao.GetProgramInfomationList(param).Converting(studioAssign);
            }
            return result;
        }
    }
}

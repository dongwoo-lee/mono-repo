using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using M30_ManagementControlDAO.DAO;
using M30_ManagementControlDAO.Interfaces;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using System.Collections.Generic;
using System.Linq;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using M30_ManagementControlDAO.WebService;
using Microsoft.Extensions.Logging;

namespace MAMBrowser.BLL
{
    public class StudioInfomationBll
    {
        private readonly IStudioInfomationDAO _dao;
        private readonly IStudioWebService _studioService;

        private readonly ILogger<StudioInfomationBll> _logger;

        public StudioInfomationBll(IStudioInfomationDAO dao, StudioWebService studioService,ILogger<StudioInfomationBll> logger)
        {
            _dao = dao;
            _studioService = new StudioSystemMockup(studioService);
            //_studioService = studioService;
            _logger = logger;
        }

        public DTO_RESULT_LIST<DTO_CATEGORY> GetStudioInfoMenu()
        {
            var result =  new DTO_RESULT_LIST<DTO_CATEGORY>();
            result.Data = new List<DTO_CATEGORY>();
            var api_studio_infos = _studioService.GetStudioInfo();
            var miros_studio_maps = _dao.GetMirosStudioMaps();
            _logger.LogInformation($"SelectStudioInfo : {api_studio_infos}");
            _logger.LogInformation($"GetMirosIfStudioMaps : {miros_studio_maps}");

            var query = from miros_studio in miros_studio_maps 
                        join api_studio in api_studio_infos.RstudioList on miros_studio.MAPI_STNAME equals api_studio.STNAME
                        select new
                        {
                            api_studio.STID,
                            api_studio.STNAME,
                            miros_studio.MIROS_ORDER
                        };

            _logger.LogInformation($"JoinStudioInfo : {query}");

            foreach (var info in query)
            {
                DTO_CATEGORY item = new DTO_CATEGORY();
                item.ID = info.STID;
                item.Name = info.STNAME;
                result.Data.Add(item);
            }

            _logger.LogInformation($"GetStudioInfo_result : {result}");

            return result;
        }
        public StudioSchedulerDTO GetStudioAssignList(string as_from, string as_to, string as_stid)
        {
            _logger.LogInformation($"SelectAssignList_params : as_from -> {as_from}, as_to -> {as_to}, as_stid -> {as_stid}");
            var result = new StudioSchedulerDTO();
            var api_assigns = _studioService.GetStudioAssign(as_from, as_to, as_stid, "");
            _logger.LogInformation($"SelectAssignedStudio : {api_assigns}");

            result.studioAssigns = api_assigns.RstudioAssignedList.SetAssigns();
            result.schedulerResources = api_assigns.RstudioAssignedList.SetResources();

            _logger.LogInformation($"SelectAssignedStudio_result : {result}");
            return result;
        }
    }
}

using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using M30_ManagementControlDAO.DAO;
using M30_ManagementControlDAO.Interfaces;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using System.Collections.Generic;
using M30_ManagementControlDAO.WebService;
using M30_ManagementControlDAO.Entity;
using System.Linq;
using MAMBrowser.DTO;
using MAMBrowser.Utils;

namespace MAMBrowser.BLL
{
    public class StudioBll
    {
        private readonly IStudioInfomationDAO _dao;
        private readonly IStudioWebService _studioService;

        public StudioBll(IStudioInfomationDAO dao, StudioWebService studioService)
        {
            _dao = dao;
            _studioService = new StudioSystemMockup(studioService);
        }

        public DTO_RESULT_LIST<DTO_CATEGORY> GetStudioInfoMenu()
        {
            var result =  new DTO_RESULT_LIST<DTO_CATEGORY>();
            result.Data = new List<DTO_CATEGORY>();
            var api_studio_infos = _studioService.GetStudioInfo();
            var miros_studio_maps = _dao.GetMirosStudioMaps();
            var query = from miros_studio in miros_studio_maps 
                        join api_studio in api_studio_infos.RstudioList on miros_studio.MAPI_STNAME equals api_studio.STNAME
                        select new
                        {
                            api_studio.STID,
                            api_studio.STNAME,
                            miros_studio.MIROS_ORDER
                        };

            foreach (var info in query)
            {
                DTO_CATEGORY item = new DTO_CATEGORY();
                item.ID = info.STID;
                item.Name = info.STNAME;
                result.Data.Add(item);
            }
            return result;
        }
        public StudioSchedulerDTO GetStudioAssignList(string as_from, string as_to, string as_stid)
        {
            var result = new StudioSchedulerDTO();
            var api_assigns = _studioService.GetStudioAssign(as_from, as_to, as_stid, null);
            result.studioAssigns = api_assigns.RstudioAssignedList.SetAssigns();
            result.schedulerResources = api_assigns.RstudioAssignedList.SetResources();
            return result;
        }
    }
}

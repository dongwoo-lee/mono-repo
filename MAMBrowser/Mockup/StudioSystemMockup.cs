using M30_ManagementControlDAO.Entity;
using M30_ManagementControlDAO.WebService;
using MAMBrowser.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MAMBrowser
{
    public class StudioSystemMockup : IStudioWebService
    {
        IStudioWebService _originService;
        public StudioSystemMockup(IStudioWebService originService)
        {
            _originService = originService;
        }

        public StudioInfoListEntity GetStudioInfo()
        {
            string filePath = @"Mockup\studio_info.txt";
            string jsonContent = File.ReadAllText(filePath, Encoding.UTF8);
            StudioInfoListEntity data = JsonConvert.DeserializeObject<StudioInfoListEntity>(jsonContent);

            return data;
        }

        public StudioAssignListEntity GetStudioAssign(string as_from, string as_to, string as_stid, string as_pgmid)
        {
            string filePath = @"Mockup\studio_assign.txt";
            string jsonContent = File.ReadAllText(filePath, Encoding.UTF8);
            StudioAssignListEntity data = JsonConvert.DeserializeObject<StudioAssignListEntity>(jsonContent);

            return data;
        }
    }
}

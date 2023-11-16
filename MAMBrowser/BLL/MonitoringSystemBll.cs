using MAMBrowser.DTO;
using MAMBrowser.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MAMBrowser.BLL
{
    public class MonitoringSystemBll
    {
        private readonly IHubContext<ProgressHub> _hubContext;
        public MonitoringSystemBll(IHubContext<ProgressHub> hubContext)
        {
            _hubContext = hubContext;
        }
       public MonitoringItemDTO<SLAPInfo> monitorObjectSLAPInfo(string name)
        {
            //var result = new MonitoringItemDTO<SLAPInfo>();
            //var info_agent = new SLAPInfo();
            //info_agent.Status = ConnectionStatus.Online;
            //info_agent.Version = "v1.0.230727";
            //info_agent.LoginId = "120120";
            //info_agent.LoginName = "남윤석";
            //info_agent.ProductId = "PMSSJ2NA";
            //info_agent.DeviceName = "Slap 1";
            //info_agent.DeviceType = "Main";
            //info_agent.StudioName = "RS_1";
            //info_agent.EventName = "시선집중 2부";
            //info_agent.FloorInfo = 9;
            //info_agent.PgmStartTime = new System.DateTime();
            //info_agent.PgmEndTime = new System.DateTime();
            //info_agent.TD = "김은비";

            //var info_pc = new PcInfo();
            //info_pc.SystemInfo = "Microsoft Windows 11 Pro x64";
            //info_pc.Processor = "Intel(R) Core(TM) i7-10700 CPU @ 2.90GHz";
            //info_pc.Version = "10.0.22621 빌드 22621";
            //info_pc.RAM = "x64";
            //info_pc.DiskInfo = "256GB";
            //info_pc.HostName = "DESKTOP-0QKT1O1";
            //info_pc.DomainName = "DESKTOP-0QKT1O1";
            //info_pc.IP = "192.168.1.231";
            //info_pc.MAC = "00-1A-7D-DA-71-0B";
            //info_pc.MemoryUsage = "12gb / 16 76%";
            //info_pc.DiskIO = "200 kb/s (10%)";
            //info_pc.NetworkIO = "24kbps";

            //result.agentMetrics = info_agent;
            return null;
        }
    }
}

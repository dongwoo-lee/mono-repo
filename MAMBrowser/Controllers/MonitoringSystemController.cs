using M30.AudioEngine;
using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using MAMBrowser.DTO;
using MAMBrowser.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitoringSystemController : Controller
    {
        private readonly IHubContext<ProgressHub> _hubContext;
        public MonitoringSystemController(IHubContext<ProgressHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("GetSlapDevice")]
        public DTO_RESULT<List<MonitoringItemDTO<SLAPInfo>>> GetSlapDevice()
        {
                var result = new DTO_RESULT<List<MonitoringItemDTO<SLAPInfo>>>();
            try
            {
                result.ResultObject = monitorObjectSLAPInfo();
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;

            }
            return result;
        }
        [HttpPost("UpdatingSlapDevice")]
        public async Task UpdatingSlapDevice([FromQuery] string connectionId)
        {
            while (true)
            {
                List<MonitoringItemDTO<SLAPInfo>> info = monitorObjectSLAPInfo();
                string serializedStatus = JsonConvert.SerializeObject(info);
                //int byteSize = Encoding.UTF8.GetByteCount(serializedStatus);
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveDeviceStatus", serializedStatus);
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private List<MonitoringItemDTO<SLAPInfo>> monitorObjectSLAPInfo()
        {
            var result = new List<MonitoringItemDTO<SLAPInfo>>();
            Random random = new Random();
            int randomNumber = random.Next(1, 80);
            for (int i = 1; i < 81; i++)
            {
                result.Add(SetItem(i));
            }
            return result;
        }
        public MonitoringItemDTO<SLAPInfo> SetItem(int index)
        {
            var result = new MonitoringItemDTO<SLAPInfo>();
            var info_agent = new SLAPInfo();
            string[] statusNames = Enum.GetNames(typeof(ConnectionStatus));
            string[] typeNames = Enum.GetNames(typeof(DeviceType));
            Random random= new Random();
            info_agent.Status = statusNames[random.Next(0, statusNames.Length)];
            info_agent.SoftwareVersion = "v1.0.230727";
            info_agent.LoginId = "120120";
            info_agent.LoginName = "남윤석";
            info_agent.ProductId = "PMSSJ2NA";
            info_agent.DeviceName = "Slap 1";
            info_agent.Type = typeNames[random.Next(0, typeNames.Length)];
            info_agent.StudioName = "RS_1";
            info_agent.EventName = $"시선집중 {index}부";
            info_agent.FloorInfo = 9;
            info_agent.PgmStartTime = new System.DateTime();
            info_agent.PgmEndTime = new System.DateTime();
            info_agent.TD = "김은비";

            var info_pc = new PcInfo();
            info_pc.SystemInfo = "Microsoft Windows 11 Pro x64";
            info_pc.Processor = "Intel(R) Core(TM) i7-10700 CPU @ 2.90GHz";
            info_pc.SystemVersion = "10.0.22621 빌드 22621";
            info_pc.RAM = "x64";
            info_pc.DiskInfo = "256GB";
            info_pc.HostName = "DESKTOP-0QKT1O1";
            info_pc.DomainName = "DESKTOP-0QKT1O1";
            info_pc.IP = "192.168.1.231";
            info_pc.MAC = "00-1A-7D-DA-71-0B";
            info_pc.MemoryUsage = "12gb / 16 76%";
            info_pc.DiskIO = "200 kb/s (10%)";
            info_pc.NetworkIO = "24kbps";

            result.agentMetrics = info_agent;
            result.systemMetrics = info_pc;
            return result;
        }
    }
}

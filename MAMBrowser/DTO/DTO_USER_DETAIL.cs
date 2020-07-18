using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_USER_DETAIL
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int DiskMax { get; set; }
        public int DiskAvailable { get; set; }
        public int DiskUsed { get; set; }
        public string Used { get; set; }
        public string MenuGrpID { get; set; }
        //메뉴
        //디스크 공간
    }
}

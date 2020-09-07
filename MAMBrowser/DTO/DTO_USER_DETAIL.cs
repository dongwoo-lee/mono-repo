﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_USER_DETAIL
    {
        public long UserExtID { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }
        public string AuthorCD { get; set; }
        public string AuthorName { get; set; }
        public int DiskMax { get; set; }
        public long DiskAvailable { get; set; }
        public long DiskUsed { get; set; }
        public string MenuGrpID { get; set; }
        public string MenuGrpName { get; set; }
        public string Used { get; set; }

        public List<DTO_MENU> MenuList { get; set; } = new List<DTO_MENU>();
        public List<DTO_MENU> BehaviorList { get; set; } = new List<DTO_MENU>();

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_LOG
    {
        public long Seq { get; set; }
        public string SystemCode { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string LogLevel { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string RegDtm { get; set; }
    }
}

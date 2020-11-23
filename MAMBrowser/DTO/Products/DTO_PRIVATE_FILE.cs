﻿using MAMBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_PRIVATE_FILE : DTO_FILEBASE
    {
        public long Seq { get; set; }
        public string Title { get; set; }
        public string Memo { get; set; }
        public string AudioFormat { get; set; }
        public string Used { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public long FileSize { get; set; }
        public string FileExt { get; set; }
        public string EditedDtm { get; set; }
        public string DeletedDtm { get; set; }
    }
}

using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.Common.Expand.Menus
{
    public class FillerEtcMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_USER> MDUserList { get; set; }
        public DTO_RESULT_LIST<DTO_CATEGORY> FillerETC { get; set; }
    }
}

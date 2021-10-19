using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.Common.Expand.Menus
{
    public class CMMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_CATEGORY> Medias { get; set; }
        public DTO_RESULT_LIST<DTO_CATEGORY> CM { get; set; }
    }
}

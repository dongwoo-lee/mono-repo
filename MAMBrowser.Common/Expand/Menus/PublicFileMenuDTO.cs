using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using MAMBrowser.Common.Expand.Menus;

namespace MAMBrowser.Common.Expand.Menus
{
    public class PublicFileMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_CATEGORY> Medias { get; set; }
        public DTO_RESULT_LIST<DTO_CATEGORY> PublicSecond { get; set; }
        public DTO_RESULT_LIST<DTO_USER> UserList { get; set; }
    }
}

using MAMBrowser.DTO;

namespace MAMBrowser.Common.Expand.Menus
{
    public class OldProMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_USER> UserList { get; set; }
        public DTO_RESULT_LIST<DTO_CATEGORY> Pro { get; set; }
    }
}

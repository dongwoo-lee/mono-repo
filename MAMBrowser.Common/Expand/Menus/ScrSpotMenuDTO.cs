using MAMBrowser.DTO;

namespace MAMBrowser.Common.Expand.Menus
{
    public class ScrSpotMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_CATEGORY> Medias { get; set; }
        public DTO_RESULT_LIST<DTO_USER> UserList { get; set; }
    }
}


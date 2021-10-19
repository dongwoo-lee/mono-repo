using MAMBrowser.DTO;

namespace MAMBrowser.Common.Expand.Menus
{
    public class McrSpotMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_CATEGORY> Medias { get; set; }
        public DTO_RESULT_LIST<DTO_USER> MDUserList { get; set; }
        public DTO_RESULT_LIST<DTO_CATEGORY> McrSpot { get; set; }
    }
}

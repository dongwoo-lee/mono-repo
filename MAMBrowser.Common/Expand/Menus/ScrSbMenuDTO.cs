using MAMBrowser.DTO;

namespace MAMBrowser.Common.Expand.Menus
{
    public class ScrSBMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_CATEGORY> Medias { get; set; }
        public DTO_RESULT_LIST<DTO_CATEGORY> PgmCodes { get; set; }
    }
}

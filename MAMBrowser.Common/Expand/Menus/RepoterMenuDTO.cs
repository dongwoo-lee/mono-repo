using MAMBrowser.DTO;

namespace MAMBrowser.Common.Expand.Menus
{
    public class RepoterMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_CATEGORY> Report { get; set; }
        public DTO_RESULT_LIST<DTO_USER> ReportUserList { get; set; }
    }
}

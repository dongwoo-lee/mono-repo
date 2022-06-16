using MAMBrowser.DTO;

namespace MAMBrowser.Common.Expand.Menus
{
    public class FillerTimeMenuDTO : MenuDTO
    {
        public DTO_RESULT_LIST<DTO_CATEGORY> Medias { get; set; }
        public DTO_RESULT_LIST<DTO_CATEGORY> FillerTimetone { get; set; }
        public DTO_RESULT_LIST<DTO_CATEGORY> ReqStatus { get; set; }
        public DTO_RESULT_LIST<DTO_USER> MDUserList { get; set; }
    }
}

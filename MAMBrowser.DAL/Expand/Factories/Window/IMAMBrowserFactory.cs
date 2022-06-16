using MAMBrowser.Common.Expand.CommonType;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.SearchOptions;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.DAL.Expand.Factories.Window
{
    public interface IMAMBrowserFactory
    {
        MenuDTO GetMenu(PageType type, MenuParamDTO paramter = null);
        MenuDTO GetSubMenu(PageType type, MenuParamDTO paramter = null);
        T Search<T>(SearchOptionDTO menu);
        bool Authenticate(string user, string pass);
        DTO_USER_DETAIL GetUserSummary(string user);

        DTO_RESULT_LIST<DTO_USER> GetPDList();
        DTO_RESULT_LIST<DTO_CATEGORY> GetMedia();
    }
}

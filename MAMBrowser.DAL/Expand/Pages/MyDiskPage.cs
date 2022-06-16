using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class MyDiskPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param = null)
        {
            return null;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            return null;
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            MyDiskSearchOptionDTO option = param as MyDiskSearchOptionDTO;
            MyDiskResultDTO result = new MyDiskResultDTO();
            result.Result = DalFactory.PrivateFileDao.FindData("Y", option.StartDate, option.EndDate, option.UserID, option.Title, option.Memo, option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

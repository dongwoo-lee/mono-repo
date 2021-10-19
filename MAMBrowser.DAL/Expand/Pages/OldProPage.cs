using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class OldProPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            OldProMenuDTO menuDTO = new OldProMenuDTO();
            menuDTO.Pro = DalFactory.CategoriesDao.GetPro();
            menuDTO.UserList = DalFactory.CategoriesDao.GetUserList();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            OldProSearchOptionDTO option = param as OldProSearchOptionDTO;
            OldProResultDTO result = new OldProResultDTO();
            result.Result = DalFactory.ProductsDao.FindOldPro("A", option.Cate, option.StartDate, option.EndDate,
               option.Type, option.Editor, option.Name, option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class ScrSpotPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            ScrSpotMenuDTO menuDTO = new ScrSpotMenuDTO();
            menuDTO.Medias = DalFactory.CategoriesDao.GetMedia();
            menuDTO.UserList = DalFactory.CategoriesDao.GetUserList();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            ScrSpotSearchOptionDTO option = param as ScrSpotSearchOptionDTO;
            ScrSpotResultDTO result = new ScrSpotResultDTO();
            result.Result = DalFactory.ProductsDao.FindSCRSpot(option.Media, option.StartDate, option.EndDate, option.PgmName,
                option.Editor, option.Name, option.RowPerPage, option.SelectPage, option.SortKey, "DESC");
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

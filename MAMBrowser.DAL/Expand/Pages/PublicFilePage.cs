using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class PublicFilePage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            PublicFileMenuDTO menuDTO = new PublicFileMenuDTO();
            menuDTO.Medias = DalFactory.CategoriesDao.GetPublicPrimary();
            menuDTO.UserList = DalFactory.CategoriesDao.GetUserList();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            PublicFileMenuDTO menuDTO = new PublicFileMenuDTO();
            menuDTO.PublicSecond = DalFactory.CategoriesDao.GetPublicSecond(
                (param == null) ? "A" :
                ((string.IsNullOrEmpty(param.Media) ? "A" : param.Media)), null);
            return menuDTO;
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            PublicFileSearchOptionDTO option = param as PublicFileSearchOptionDTO;
            PublicFileResultDTO result = new PublicFileResultDTO();
            result.Result = DalFactory.PublicFileDao.FineData(option.Media, option.Cate, option.StartDate, option.EndDate,
                option.Editor, option.Title, option.Memo, option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

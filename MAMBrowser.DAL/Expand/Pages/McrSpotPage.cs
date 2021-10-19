using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class McrSpotPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            McrSpotMenuDTO menuDTO = new McrSpotMenuDTO();
            menuDTO.McrSpot = DalFactory.CategoriesDao.GetMcrSpot(
                (param == null) ? "A" :
                (string.IsNullOrEmpty(param.Media)) ? "A" : param.Media);
            menuDTO.MDUserList = DalFactory.CategoriesDao.GetMDUserList();
            menuDTO.Medias = DalFactory.CategoriesDao.GetMedia();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            McrSpotSearchOptionDTO option = param as McrSpotSearchOptionDTO;
            McrSpotResultDTO result = new McrSpotResultDTO();
            result.Result = DalFactory.ProductsDao.FindMcrSpot(option.Media, option.StartDate, option.EndDate,
               option.SpotID, option.Editor, option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

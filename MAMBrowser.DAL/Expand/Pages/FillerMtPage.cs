using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class FillerMtPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            FillerMtMenuDTO menuDTO = new FillerMtMenuDTO();
            menuDTO.MDUserList = DalFactory.CategoriesDao.GetMDUserList();
            menuDTO.FillerGeneral = DalFactory.CategoriesDao.GetFillerGeneral();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            FillerMtSearchOptionDTO option = param as FillerMtSearchOptionDTO;
            FillerMtResultDTO result = new FillerMtResultDTO();
            result.Result = DalFactory.ProductsDao.FindFiller("M30_VW_FILLER_MATERIAL", option.BrdDate, option.Cate, option.Editor,
                option.Name, option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

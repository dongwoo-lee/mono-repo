using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class FillerPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            FillerPrMenuDTO menuDTO = new FillerPrMenuDTO();
            menuDTO.MDUserList = DalFactory.CategoriesDao.GetMDUserList();
            menuDTO.FillerPr = DalFactory.CategoriesDao.GetFillerPr();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            FillerPrSearchOptionDTO option = param as FillerPrSearchOptionDTO;
            FillerResultDTO result = new FillerResultDTO();
            result.Result = DalFactory.ProductsDao.FindFiller("M30_VW_FILLER_PR", option.BrdDate, option.Cate, option.Editor,
                option.Name, option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

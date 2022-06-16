using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;
using System;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class FillerEtcPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            FillerEtcMenuDTO menuDTO = new FillerEtcMenuDTO();
            menuDTO.MDUserList = DalFactory.CategoriesDao.GetMDUserList();
            menuDTO.FillerETC = DalFactory.CategoriesDao.GetFillerETC();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            FillerEtcSearchOptionDTO option = param as FillerEtcSearchOptionDTO;
            FillerEtcResultDTO result = new FillerEtcResultDTO();
            result.Result = DalFactory.ProductsDao.FindFiller("M30_VW_FILLER_ETC", option.BrdDate, option.Cate, option.Editor,
                option.Name, option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

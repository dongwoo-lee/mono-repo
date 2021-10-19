using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class ScrSBPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            ScrSBMenuDTO menuDTO = new ScrSBMenuDTO();
            menuDTO.Medias = DalFactory.CategoriesDao.GetMedia();
            menuDTO.PgmCodes = null;
            //menuDTO.PgmCodes = DalFactory.CategoriesDao.GetPgmCodes(
            //    (param == null) ? DateTime.Today.ToString("yyyyMMdd") :
            //    (string.IsNullOrEmpty(param.BrdDate)) ? DateTime.Today.ToString("yyyyMMdd") : param.BrdDate,
            //    (param == null) ? "A" :
            //    (string.IsNullOrEmpty(param.Media)) ? "A" : param.Media);

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            ScrSBMenuDTO menuDTO = new ScrSBMenuDTO();
            menuDTO.PgmCodes = DalFactory.CategoriesDao.GetPgmCodes(
                (param == null) ? DateTime.Today.ToString("yyyyMMdd") :
                (string.IsNullOrEmpty(param.BrdDate)) ? DateTime.Today.ToString("yyyyMMdd") : param.BrdDate,
                (param == null) ? "A" :
                (string.IsNullOrEmpty(param.Media)) ? "A" : param.Media);
            return menuDTO;
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            ScrSBSearchOptionDTO option = param as ScrSBSearchOptionDTO;
            ScrSBResultDTO result = new ScrSBResultDTO();
            result.Result = DalFactory.ProductsDao.FindSB("SUB", option.Media, option.BrdDate, option.Pgm);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

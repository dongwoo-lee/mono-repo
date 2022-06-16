using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class McrSBPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            McrSBMenuDTO menuDTO = new McrSBMenuDTO();
            menuDTO.Medias = DalFactory.CategoriesDao.GetMedia();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            McrSBSearchOptionDTO option = param as McrSBSearchOptionDTO;
            McrSBResultDTO result = new McrSBResultDTO();
            result.Result = DalFactory.ProductsDao.FindSB("MAIN", option.Media, option.BrdDate, null);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

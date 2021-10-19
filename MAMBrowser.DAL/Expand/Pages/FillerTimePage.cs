using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class FillerTimePage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            FillerTimeMenuDTO menuDTO = new FillerTimeMenuDTO();
            menuDTO.Medias = DalFactory.CategoriesDao.GetMedia();
            menuDTO.FillerTimetone = DalFactory.CategoriesDao.GetFillerTimetone();
            menuDTO.ReqStatus = DalFactory.CategoriesDao.GetReqStatus();
            menuDTO.MDUserList = DalFactory.CategoriesDao.GetMDUserList();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            FillerTimeSearchOptionDTO option = param as FillerTimeSearchOptionDTO;
            FillerTimeResultDTO result = new FillerTimeResultDTO();
            result.Result = DalFactory.ProductsDao.FindFillerTime(option.Media, option.StartDate, option.EndDate,
                option.Cate, option.Status, option.Editor, null, option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

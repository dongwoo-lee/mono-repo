using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class ReportPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            RepoterMenuDTO menuDTO = new RepoterMenuDTO();
            menuDTO.Report = DalFactory.CategoriesDao.GetReport();
            menuDTO.ReportUserList = DalFactory.CategoriesDao.GetReportUserList();
            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            ReportSearchOptionDTO option = param as ReportSearchOptionDTO;
            ReportResultDTO result = new ReportResultDTO();
            result.Result = DalFactory.ProductsDao.FindReport(option.Cate, option.StartDate, option.EndDate, option.IsMastering,
                option.PgmName, option.Editor, option.ReporterName, option.Name,
                option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

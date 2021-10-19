using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class CMContentsPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param = null)
        {
            return null;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            return null;
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            CMContentsSearchOptionDTO option = param as CMContentsSearchOptionDTO;
            CMContentsResultDTO result = new CMContentsResultDTO();
            result.Result = DalFactory.ProductsDao.FindCMContents(option.BrdDate, option.CmGrpID);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

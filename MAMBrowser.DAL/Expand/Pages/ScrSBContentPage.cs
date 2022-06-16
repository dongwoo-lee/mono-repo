using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class ScrSBContentPage : Page
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
            ScrSBContentsSearchOptionDTO option = param as ScrSBContentsSearchOptionDTO;
            ScrSBContentsResultDTO result = new ScrSBContentsResultDTO();
            result.Result = DalFactory.ProductsDao.FindSBContents(option.BrdDate, option.SBId);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

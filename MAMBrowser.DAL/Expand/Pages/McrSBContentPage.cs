using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class McrSBContentPage : Page
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
            McrSBContentsSearchOptionDTO option = param as McrSBContentsSearchOptionDTO;
            McrSBContentsResultDTO result = new McrSBContentsResultDTO();
            result.Result = DalFactory.ProductsDao.FindSBContents(option.BrdDate, option.SB_ID);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

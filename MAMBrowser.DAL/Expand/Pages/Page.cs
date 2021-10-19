using MAMBrowser.DAL.Expand.Factories;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.SearchOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal abstract class Page
    {
        public MAMDALFactory DalFactory { get; private set; }
        public Page()
        {
            this.DalFactory = MAMDALFactory.Instance;
        }

        public abstract MenuDTO GetMenu(MenuParamDTO param = null);
        public abstract MenuDTO GetSubMenu(MenuParamDTO param = null);
        public abstract T Search<T>(SearchOptionDTO param);
    }
}

using MAMBrowser.Common.Expand.CommonType;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.SearchOptions;
using MAMBrowser.DAL.Expand.Pages;
using System;

namespace MAMBrowser.DAL.Expand.Factories.Web
{
    public class MAMWebFactory
    {
        #region Initalize
        private static readonly Lazy<MAMWebFactory> lazy = new Lazy<MAMWebFactory>(() => new MAMWebFactory());
        public static MAMWebFactory Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        
        public void Setting(string ConnectionString, int ExpireHour, string IPAddress, string TokenIssuer, string TokenSignature)
        {
            MAMDALFactory.Instance.Setting(ConnectionString, ExpireHour, IPAddress, TokenIssuer, TokenSignature);
        }
        #endregion

        public MenuDTO GetMenus(PageType type, MenuParamDTO paramter = null)
        {
            Page page = GetPage(type);
            return page.GetMenu(paramter);
        }

        public MenuDTO GetSubMenu(PageType type, MenuParamDTO paramter = null)
        {
            Page page = GetPage(type);
            return page.GetSubMenu(paramter);
        }

        public T Search<T>(SearchOptionDTO menu)
        {
            Page page = GetPage(menu);
            return page.Search<T>(menu);
        }
        private Page GetPage(SearchOptionDTO menu)
        {
            if (menu.GetType() == typeof(PgmSearchOptionDTO))
                return new PgmPage();
            else if (menu.GetType() == typeof(CMSearchOptionDTO))
                return new CMPage();
            else if (menu.GetType() == typeof(FillerEtcSearchOptionDTO))
                return new FillerEtcPage();
            else if (menu.GetType() == typeof(FillerMtSearchOptionDTO))
                return new FillerMtPage();
            else if (menu.GetType() == typeof(FillerPrSearchOptionDTO))
                return new FillerPage();
            else if (menu.GetType() == typeof(FillerTimeSearchOptionDTO))
                return new FillerTimePage();
            else if (menu.GetType() == typeof(McrSBSearchOptionDTO))
                return new McrSBPage();
            else if (menu.GetType() == typeof(McrSpotSearchOptionDTO))
                return new McrSpotPage();
            else if (menu.GetType() == typeof(OldProSearchOptionDTO))
                return new OldProPage();
            else if (menu.GetType() == typeof(PgmCMSearchOptionDTO))
                return new PgmCMPage();
            else if (menu.GetType() == typeof(PublicFileSearchOptionDTO))
                return new PublicFilePage();
            else if (menu.GetType() == typeof(ReportSearchOptionDTO))
                return new ReportPage();
            else if (menu.GetType() == typeof(ScrSBSearchOptionDTO))
                return new ScrSBPage();
            else if (menu.GetType() == typeof(ScrSpotSearchOptionDTO))
                return new ScrSpotPage();
            else
                return null;
        }
        private Page GetPage(PageType type)
        {
            switch (type)
            {
                // 공유소재
                case PageType.PUBLIC_FILE:
                    return new PublicFilePage();
                // 프로그램
                case PageType.PGM:
                    return new PgmPage();
                // 부조 Spot
                case PageType.SCR_SPOT:
                    return new ScrSpotPage();
                // 취재물
                case PageType.REPOTE:
                    return new ReportPage();
                // 프로소재
                case PageType.OLD_PRO:
                    return new OldProPage();
                // 주조SB
                case PageType.MCR_SB:
                    return new McrSBPage();
                // 부조SB
                case PageType.SCR_SB:
                    return new ScrSBPage();
                // 프로그램 CM
                case PageType.PGM_CM:
                    return new PgmCMPage();
                // CM
                case PageType.CM:
                    return new CMPage();
                // 주조SPOT
                case PageType.MCR_SPOT:
                    return new McrSpotPage();
                // FILLER(pr)
                case PageType.FILLER_PR:
                    return new FillerPage();
                case PageType.FILLER_MT:
                    return new FillerMtPage();
                case PageType.FILLER_TIME:
                    return new FillerTimePage();
                case PageType.FILLER_ETC:
                    return new FillerEtcPage();
                default:
                    return null;
            }
        }
    }
}

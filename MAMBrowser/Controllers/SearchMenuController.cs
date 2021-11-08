using MAMBrowser.Common.Expand.Builder;
using MAMBrowser.Common.Expand.CommonType;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.DAL.Expand.Factories.Web;
using Microsoft.AspNetCore.Mvc;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchMenuController : ControllerBase
    {
        public class Pram
        {
            public string startDate { get; set; }
            public string endDate { get; set; }
            public string media { get; set; }
            public string cate { get; set; }
            public string editor { get; set; }
            public string title { get; set; }
            public string memo { get; set; }
            public string brddate { get; set; }
            public string pgmname { get; set; }
            public string pgm { get; set; }
            public string editorname { get; set; }
            public string reporterName { get; set; }
            public string name { get; set; }
            public string ismastering { get; set; }
            public string type { get; set; }
            public string spotid { get; set; }
            public string status { get; set; }

            public int rowperpage { get; set; }
            public int selectpage { get; set; }
            public string sortKey { get; set; }
            public string sortValue { get; set; }
        }

        #region 소재검색 옵션
        [HttpGet("GetSearchOption")]
        public MenuDTO GetSearchOption([FromQuery] string type)
        {
            switch (type)
            {
                //프로그램
                case "PGM":
                    return MAMWebFactory.Instance.GetMenus(PageType.PGM);
                //부조 SPOT
                case "SCR_SPOT":
                    return MAMWebFactory.Instance.GetMenus(PageType.SCR_SPOT);
                //공유소재
                case "PUBLIC_FILE":
                    return MAMWebFactory.Instance.GetMenus(PageType.PUBLIC_FILE);
                //취재물
                case "REPOTE":
                    return MAMWebFactory.Instance.GetMenus(PageType.REPOTE);
                //(구)프로소재
                case "OLD_PRO":
                    return MAMWebFactory.Instance.GetMenus(PageType.OLD_PRO);
                //주조SB
                case "MCR_SB":
                    return MAMWebFactory.Instance.GetMenus(PageType.MCR_SB);
                //부조SB
                case "SCR_SB":
                    return MAMWebFactory.Instance.GetMenus(PageType.SCR_SB);
                //프로그램CM
                case "PGM_CM":
                    return MAMWebFactory.Instance.GetMenus(PageType.PGM_CM);
                //CM
                case "CM":
                    return MAMWebFactory.Instance.GetMenus(PageType.CM);
                //주조SPOT
                case "MCR_SPOT":
                    return MAMWebFactory.Instance.GetMenus(PageType.MCR_SPOT);
                //Filler(PR)
                case "FILLER_PR":
                    return MAMWebFactory.Instance.GetMenus(PageType.FILLER_PR);
                //Filler(소재)
                case "FILLER_MT":
                    return MAMWebFactory.Instance.GetMenus(PageType.FILLER_MT);
                //Filler(시간)
                case "FILLER_TIME":
                    return MAMWebFactory.Instance.GetMenus(PageType.FILLER_TIME);
                //Filler(기타)
                case "FILLER_ETC":
                    return MAMWebFactory.Instance.GetMenus(PageType.FILLER_ETC);

                default:
                    return null;
            }
        }
        #endregion

        #region 소재검색 테이블
        //프로그램
        [HttpGet("GetSearchTable/PGM")]
        public PgmResultDTO GetProgram([FromQuery] Pram pram)
        {
            var a = new PgmSearchOptionBuilder()
                .SetBrdDate(pram.brddate)
                .SetMedia(pram.media)
                .SetPgmName(pram.pgmname)
                .SetEditorName(pram.editorname)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)

                .Build();

            return MAMWebFactory.Instance.Search<PgmResultDTO>(a);

        }
        //부조 SPOT
        [HttpGet("GetSearchTable/SCR_SPOT")]
        public ScrSpotResultDTO GetScrSpot([FromQuery] Pram pram)
        {
            var a = new ScrSpotSearchOptionBuilder()
                .SetStartDate(pram.startDate)
                .SetEndDate(pram.endDate)
                .SetMedia(pram.media)
                .SetPgmName(pram.pgmname)
                .SetEditor(pram.editor)
                .SetName(pram.name)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<ScrSpotResultDTO>(a);
        }
        //공유소재
        [HttpGet("GetSearchTable/PUBLIC_FILE")]
        public PublicFileResultDTO GetPublic([FromQuery] Pram pram)
        {
            var a = new PublicSearchOptionBuilder()
                .SetStartDate(pram.startDate)
                .SetEndDate(pram.endDate)
                .SetMedia(pram.media)
                .SetCate(pram.cate)
                .SetEditor(pram.editor)
                .SetTitle(pram.title)
                .SetMemo(pram.memo)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<PublicFileResultDTO>(a);
        }
        //취재물
        [HttpGet("GetSearchTable/REPOTE")]
        public ReportResultDTO GetRepoter([FromQuery] Pram pram)
        {
            var a = new ReportSearchOptionBuilder()
                .SetStartDate(pram.startDate)
                .SetEndDate(pram.endDate)
                .SetCate(pram.cate)
                .SetPgmName(pram.pgmname)
                .SetReporterName(pram.reporterName)
                .SetEditor(pram.editor)
                .SetName(pram.name)
                .SetIsMastering(pram.ismastering)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<ReportResultDTO>(a);
        }
        //(구)프로소재
        [HttpGet("GetSearchTable/OLD_PRO")]
        public OldProResultDTO GetOldPro([FromQuery] Pram pram)
        {
            var a = new OldProSearchOptionBuilder()
                .SetStartDate(pram.startDate)
                .SetEndDate(pram.endDate)
                .SetType(pram.type)
                .SetCate(pram.cate)
                .SetEditor(pram.editor)
                .SetName(pram.name)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<OldProResultDTO>(a);
        }
        //주조SB
        [HttpGet("GetSearchTable/MCR_SB")]
        public McrSBResultDTO GetMcrSb([FromQuery] Pram pram)
        {
            var a = new McrSBearchOptionBuilder()
                .SetBrdDate(pram.brddate)
                .SetMedia(pram.media)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
            .Build();


            return MAMWebFactory.Instance.Search<McrSBResultDTO>(a);
        }
        //부조SB
        [HttpGet("GetSearchTable/SCR_SB")]
        public ScrSBResultDTO GetScrSb([FromQuery] Pram pram)
        {
            var a = new ScrSBSearchOptionBuilder()
                .SetBrdDate(pram.brddate)
                .SetMedia(pram.media)
                .SetPgm(pram.pgm)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<ScrSBResultDTO>(a);
        }
        //프로그램CM
        [HttpGet("GetSearchTable/PGM_CM")]
        public PgmCMResultDTO GetPgmCm([FromQuery] Pram pram)
        {
            var a = new PgmCMSearchOptionBuilder()
                .SetBrdDate(pram.brddate)
                .SetMedia(pram.media)
                .SetPgmName(pram.pgmname)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<PgmCMResultDTO>(a);
        }
        //CM
        [HttpGet("GetSearchTable/CM")]
        public CMResultDTO GetCm([FromQuery] Pram pram)
        {
            var a = new CMSearchOptionBuilder()
                .SetBrdDate(pram.brddate)
                .SetMedia(pram.media)
                .SetCate(pram.cate)
                .SetPgmName(pram.pgmname)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<CMResultDTO>(a);
        }
        //주조SPOT
        [HttpGet("GetSearchTable/MCR_SPOT")]
        public McrSpotResultDTO GetMcrSpot([FromQuery] Pram pram)
        {
            var a = new McrSpotSearchOptionBuilder()
                .SetStartDate(pram.startDate)
                .SetEndDate(pram.endDate)
                .SetMedia(pram.media)
                .SetSpotID(pram.spotid)
                .SetEditor(pram.editor)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<McrSpotResultDTO>(a);
        }
        //Filler(PR)
        [HttpGet("GetSearchTable/FILLER_PR")]
        public FillerResultDTO GetFiller([FromQuery] Pram pram)
        {
            var a = new FillerSearchOptionBuilder()
                .SetBrdDate(pram.brddate)
                .SetCate(pram.cate)
                .SetEditor(pram.editor)
                .SetName(pram.name)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<FillerResultDTO>(a);
        }
        //Filler(소재)
        [HttpGet("GetSearchTable/FILLER_MT")]
        public FillerMtResultDTO GetFillerMt([FromQuery] Pram pram)
        {
            var a = new FillerMtSearchOptionBuilder()
                .SetBrdDate(pram.brddate)
                .SetCate(pram.cate)
                .SetEditor(pram.editor)
                .SetName(pram.name)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<FillerMtResultDTO>(a);
        }
        //Filler(시간)
        [HttpGet("GetSearchTable/FILLER_TIME")]
        public FillerTimeResultDTO GetFillerTime([FromQuery] Pram pram)
        {
            var a = new FillerTimeSearchOptionBuilder()
                .SetStartDate(pram.startDate)
                .SetEndDate(pram.endDate)
                .SetMedia(pram.media)
                .SetCate(pram.cate)
                .SetStatus(pram.status)
                .SetEditor(pram.editor)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<FillerTimeResultDTO>(a);
        }
        //Filler(기타)
        [HttpGet("GetSearchTable/FILLER_ETC")]
        public FillerEtcResultDTO GetFillerEtc([FromQuery] Pram pram)
        {
            var a = new FillerEtcSearchOptionBuilder()
                .SetBrdDate(pram.brddate)
                .SetCate(pram.cate)
                .SetEditor(pram.editor)
                .SetName(pram.name)
                .SetRowPerPage(pram.rowperpage)
                .SetSelectPage(pram.selectpage)
                .SetSortKey(pram.sortKey)
                .SetSortValue(pram.sortValue)
                .Build();

            return MAMWebFactory.Instance.Search<FillerEtcResultDTO>(a);
        }
        #endregion

        //공유소재 > 분류 옵션
        [HttpGet("GetPublicSecond")]
        public MenuDTO GetPublicSecond([FromQuery] string media)
        {
            return MAMWebFactory.Instance.GetSubMenu(PageType.PUBLIC_FILE, new MenuParamDTO() { Media = media });
        }
    }
}
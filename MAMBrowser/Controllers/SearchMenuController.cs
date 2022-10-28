using M30.AudioEngine;
using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.DTO.Products;
using M30.AudioFile.Common.Expand.Builder;
using M30.AudioFile.Common.Expand.CommonType;
using M30.AudioFile.Common.Expand.Menus;
using M30.AudioFile.Common.Expand.Result;
using M30.AudioFile.Common.Expand.SearchOptions;
using M30.AudioFile.DAL.Dao;
using M30.AudioFile.DAL.Dto;
using M30.AudioFile.DAL.Expand.Factories.Web;
using M30.AudioFile.DAL.WebService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchMenuController : ControllerBase
    {
        private readonly IMusicService _fileService;
        private readonly APIDao _apiDao;

        public SearchMenuController(MusicWebService fileService, APIDao apiDao)
        {
            _fileService = fileService;
            //_fileService = new MusicSystemMockup();
            _apiDao = apiDao;
        }
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
            public string userid { get; set; }
            public long? deviceSeq { get; set; }
            public int rowperpage { get; set; }
            public int selectpage { get; set; }
            public string sortKey { get; set; }
            public string sortValue { get; set; }

            public int searchType1 { get; set; }
            public string searchType2 { get; set; }
            public int gradeType { get; set; }
            public string searchText { get; set; }
            public string albumName { get; set; }
            public string artistName { get; set; }

        }
        public class MusicResultDTO
        {
            public DTO_RESULT_PAGE_LIST<DTO_MUSIC> Result { get; set; }
        }
        public class EFFECTResultDTO
        {
            public DTO_RESULT_PAGE_LIST<DTO_EFFECT> Result { get; set; }
        }

        #region 소재검색 옵션
        [HttpGet("GetSearchOption")]
        public DTO_RESULT<object> GetSearchOption([FromQuery] string type)
        {
            var result = new DTO_RESULT<object>();
            try
            {
                switch (type)
                {
                    //프로그램();
                    case "PGM":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.PGM);
                        break;
                    //DL30();
                    case "DL30":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.DL30);
                        break;
                    //부조 SPOT();
                    case "SCR_SPOT":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.SCR_SPOT);
                        break;
                    //공유소재();
                    case "PUBLIC_FILE":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.PUBLIC_FILE);
                        break;
                    //취재물();
                    case "REPOTE":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.REPOTE);
                        break;
                    //프로소재();
                    case "OLD_PRO":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.OLD_PRO);
                        break;
                    //주조SB();
                    case "MCR_SB":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.MCR_SB);
                        break;
                    //부조SB();
                    case "SCR_SB":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.SCR_SB);
                        break;
                    //프로그램CM();
                    case "PGM_CM":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.PGM_CM);
                        break;
                    //CM();
                    case "CM":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.CM);
                        break;
                    //주조SPOT();
                    case "MCR_SPOT":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.MCR_SPOT);
                        break;
                    //Filler(PR)();
                    case "FILLER_PR":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.FILLER_PR);
                        break;
                    //Filler(소재)();
                    case "FILLER_MT":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.FILLER_MT);
                        break;
                    //Filler(시간)
                    case "FILLER_TIME":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.FILLER_TIME);
                        break;
                    //Filler(기타)
                    case "FILLER_ETC":
                        result.ResultObject = MAMWebFactory.Instance.GetMenus(PageType.FILLER_ETC);
                        break;

                    default:
                        break;
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        #endregion

        #region 소재검색 테이블
        //Song
        [HttpGet("GetSearchTable/SONG")]
        public DTO_RESULT<SongResultDTO> GetSONG([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<SongResultDTO>();
            try
            {
                if (string.IsNullOrEmpty(pram.title) && string.IsNullOrEmpty(pram.albumName) && string.IsNullOrEmpty(pram.artistName))
                {
                    result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                    result.ResultObject = new SongResultDTO();
                    result.ResultObject.Result = new DTO_RESULT_PAGE_LIST<DTO_SONG>();
                }
                else
                {
                    var dto = new SongSearchOptionDTO()
                    {
                        AlbumName = pram.albumName,
                        ArtistName = pram.artistName,
                        SongName = pram.title,
                        RowPerPage = pram.rowperpage,
                        SelectPage = pram.selectpage,
                    };
                    result.ResultObject = MAMWebFactory.Instance.Search<SongResultDTO>(dto);
                    result.ResultCode = RESUlT_CODES.SUCCESS;

                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //MY 공간
        [HttpGet("GetSearchTable/MyDisk")]
        public DTO_RESULT<MyDiskResultDTO> GetMyDisk([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<MyDiskResultDTO>();
            try
            {
                var dto = new MyDiskSearchOptionDTO()
                {
                    UserID = pram.userid,
                    StartDate = pram.startDate,
                    EndDate = pram.endDate,
                    Title = pram.title,
                    Memo = pram.memo,
                    RowPerPage = pram.rowperpage,
                    SelectPage = pram.selectpage,
                };
                result.ResultObject = MAMWebFactory.Instance.Search<MyDiskResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //DL 3.0
        [HttpGet("GetSearchTable/DL30")]
        public DTO_RESULT<DL30ResultDTO> GetDL30([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<DL30ResultDTO>();
            try
            {
                var dto = new DL30SearchOptionDTO()
                {
                    brd_dt = pram.brddate,
                    deviceSeq = pram.deviceSeq,
                    media = pram.media,
                    name = pram.name,
                    RowPerPage = pram.rowperpage,
                    SelectPage = pram.selectpage,

                };

                result.ResultObject = MAMWebFactory.Instance.Search<DL30ResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }

            return result;
        }

        //MUSIC
        [HttpGet("GetSearchTable/MUSIC")]
        public DTO_RESULT<MusicResultDTO> GetMUSIC([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<MusicResultDTO>();
            try
            {
                result.ResultObject = new MusicResultDTO();
                result.ResultObject.Result = new DTO_RESULT_PAGE_LIST<DTO_MUSIC>();
                long totalCount = 0;
                if (string.IsNullOrEmpty(pram.searchText))
                    result.ResultObject.Result.Data = new List<DTO_MUSIC>();
                else
                    result.ResultObject.Result.Data = _fileService.SearchSong((MusicSearchTypes1)pram.searchType1, pram.searchType2, (GradeTypes)pram.gradeType, pram.searchText, pram.rowperpage, pram.selectpage, out totalCount);

                result.ResultObject.Result.TotalRowCount = totalCount;
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //EFFECT
        [HttpGet("GetSearchTable/EFFECT")]
        public DTO_RESULT<EFFECTResultDTO> GetEFFECT([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<EFFECTResultDTO>();
            try
            {
                result.ResultObject = new EFFECTResultDTO();
                result.ResultObject.Result = new DTO_RESULT_PAGE_LIST<DTO_EFFECT>();
                long totalCount = 0;
                if (string.IsNullOrEmpty(pram.searchText))
                    result.ResultObject.Result.Data = new List<DTO_EFFECT>();
                else
                    result.ResultObject.Result.Data = _fileService.SearchEffect(pram.searchText, pram.rowperpage, pram.selectpage, out totalCount);
                result.ResultObject.Result.TotalRowCount = totalCount;
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //프로그램
        [HttpGet("GetSearchTable/PGM")]
        public DTO_RESULT<PgmResultDTO> GetProgram([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<PgmResultDTO>();
            try
            {
                var dto = new PgmSearchOptionBuilder()
                    .SetBrdDate(pram.brddate)
                    .SetMedia(pram.media)
                    .SetPgmName(pram.pgmname)
                    .SetEditorName(pram.editorname)
                    .SetRowPerPage(pram.rowperpage)
                    .SetSelectPage(pram.selectpage)
                    .SetSortKey(pram.sortKey)
                    .SetSortValue(pram.sortValue)
                    .Build();

                result.ResultObject = MAMWebFactory.Instance.Search<PgmResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //부조 SPOT
        [HttpGet("GetSearchTable/SCR_SPOT")]
        public DTO_RESULT<ScrSpotResultDTO> GetScrSpot([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<ScrSpotResultDTO>();
            try
            {
                var dto = new ScrSpotSearchOptionBuilder()
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

                result.ResultObject = MAMWebFactory.Instance.Search<ScrSpotResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //공유소재
        [HttpGet("GetSearchTable/PUBLIC_FILE")]
        public DTO_RESULT<PublicFileResultDTO> GetPublic([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<PublicFileResultDTO>();
            try
            {
                var dto = new PublicSearchOptionBuilder()
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

                result.ResultObject = MAMWebFactory.Instance.Search<PublicFileResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //취재물
        [HttpGet("GetSearchTable/REPOTE")]
        public DTO_RESULT<ReportResultDTO> GetRepoter([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<ReportResultDTO>();
            try
            {
                var dto = new ReportSearchOptionBuilder()
                    .SetBrdDate(pram.brddate)
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

                result.ResultObject = MAMWebFactory.Instance.Search<ReportResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //프로소재
        [HttpGet("GetSearchTable/OLD_PRO")]
        public DTO_RESULT<OldProResultDTO> GetOldPro([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<OldProResultDTO>();
            try
            {
                var dto = new OldProSearchOptionBuilder()
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

                result.ResultObject = MAMWebFactory.Instance.Search<OldProResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //주조SB
        [HttpGet("GetSearchTable/MCR_SB")]
        public DTO_RESULT<McrSBResultDTO> GetMcrSb([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<McrSBResultDTO>();
            try
            {
                var dto = new McrSBearchOptionBuilder()
                    .SetBrdDate(pram.brddate)
                    .SetMedia(pram.media)
                    .SetRowPerPage(pram.rowperpage)
                    .SetSelectPage(pram.selectpage)
                    .SetSortKey(pram.sortKey)
                    .SetSortValue(pram.sortValue)
                .Build();
                result.ResultObject = MAMWebFactory.Instance.Search<McrSBResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //부조SB
        [HttpGet("GetSearchTable/SCR_SB")]
        public DTO_RESULT<ScrSBResultDTO> GetScrSb([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<ScrSBResultDTO>();
            try
            {
                var dto = new ScrSBSearchOptionBuilder()
                    .SetBrdDate(pram.brddate)
                    .SetMedia(pram.media)
                    .SetPgm(pram.pgm)
                    .SetRowPerPage(pram.rowperpage)
                    .SetSelectPage(pram.selectpage)
                    .SetSortKey(pram.sortKey)
                    .SetSortValue(pram.sortValue)
                    .Build();

                result.ResultObject = MAMWebFactory.Instance.Search<ScrSBResultDTO>(dto);

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //프로그램CM
        [HttpGet("GetSearchTable/PGM_CM")]
        public DTO_RESULT<PgmCMResultDTO> GetPgmCm([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<PgmCMResultDTO>();
            try
            {
                var dto = new PgmCMSearchOptionBuilder()
              .SetBrdDate(pram.brddate)
              .SetMedia(pram.media)
              .SetPgmName(pram.pgmname)
              .SetRowPerPage(pram.rowperpage)
              .SetSelectPage(pram.selectpage)
              .SetSortKey(pram.sortKey)
              .SetSortValue(pram.sortValue)
              .Build();

                result.ResultObject = MAMWebFactory.Instance.Search<PgmCMResultDTO>(dto);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //CM
        [HttpGet("GetSearchTable/CM")]
        public DTO_RESULT<CMResultDTO> GetCm([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<CMResultDTO>();
            try
            {
                var dto = new CMSearchOptionBuilder()
                    .SetBrdDate(pram.brddate)
                    .SetMedia(pram.media)
                    .SetCate(pram.cate)
                    .SetPgmName(pram.pgmname)
                    .SetRowPerPage(pram.rowperpage)
                    .SetSelectPage(pram.selectpage)
                    .SetSortKey(pram.sortKey)
                    .SetSortValue(pram.sortValue)
                    .Build();

                result.ResultObject = MAMWebFactory.Instance.Search<CMResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //주조SPOT
        [HttpGet("GetSearchTable/MCR_SPOT")]
        public DTO_RESULT<McrSpotResultDTO> GetMcrSpot([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<McrSpotResultDTO>();
            try
            {
                var dto = new McrSpotSearchOptionBuilder()
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

                result.ResultObject = MAMWebFactory.Instance.Search<McrSpotResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //Filler(PR)
        [HttpGet("GetSearchTable/FILLER_PR")]
        public DTO_RESULT<FillerResultDTO> GetFiller([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<FillerResultDTO>();
            try
            {
                var dto = new FillerSearchOptionBuilder()
                    .SetBrdDate(pram.brddate)
                    .SetCate(pram.cate)
                    .SetEditor(pram.editor)
                    .SetName(pram.name)
                    .SetRowPerPage(pram.rowperpage)
                    .SetSelectPage(pram.selectpage)
                    .SetSortKey(pram.sortKey)
                    .SetSortValue(pram.sortValue)
                    .Build();

                result.ResultObject = MAMWebFactory.Instance.Search<FillerResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //Filler(소재)
        [HttpGet("GetSearchTable/FILLER_MT")]
        public DTO_RESULT<FillerMtResultDTO> GetFillerMt([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<FillerMtResultDTO>();
            try
            {
                var dto = new FillerMtSearchOptionBuilder()
                    .SetBrdDate(pram.brddate)
                    .SetCate(pram.cate)
                    .SetEditor(pram.editor)
                    .SetName(pram.name)
                    .SetRowPerPage(pram.rowperpage)
                    .SetSelectPage(pram.selectpage)
                    .SetSortKey(pram.sortKey)
                    .SetSortValue(pram.sortValue)
                    .Build();

                result.ResultObject = MAMWebFactory.Instance.Search<FillerMtResultDTO>(dto);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //Filler(시간)
        [HttpGet("GetSearchTable/FILLER_TIME")]
        public DTO_RESULT<FillerTimeResultDTO> GetFillerTime([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<FillerTimeResultDTO>();
            try
            {
                var dto = new FillerTimeSearchOptionBuilder()
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

                result.ResultObject = MAMWebFactory.Instance.Search<FillerTimeResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //Filler(기타)
        [HttpGet("GetSearchTable/FILLER_ETC")]
        public DTO_RESULT<FillerEtcResultDTO> GetFillerEtc([FromQuery] Pram pram)
        {
            var result = new DTO_RESULT<FillerEtcResultDTO>();
            try
            {
                var dto = new FillerEtcSearchOptionBuilder()
                    .SetBrdDate(pram.brddate)
                    .SetCate(pram.cate)
                    .SetEditor(pram.editor)
                    .SetName(pram.name)
                    .SetRowPerPage(pram.rowperpage)
                    .SetSelectPage(pram.selectpage)
                    .SetSortKey(pram.sortKey)
                    .SetSortValue(pram.sortValue)
                    .Build();

                result.ResultObject = MAMWebFactory.Instance.Search<FillerEtcResultDTO>(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        #endregion

        //공유소재 > 분류 옵션 (웹 큐시트에서 제외 됨, 추후 추가될 시 테스트 필요)
        [HttpGet("GetPublicSecond")]
        public DTO_RESULT<object> GetPublicSecond([FromQuery] string media)
        {
            var result = new DTO_RESULT<object>();
            try
            {
                result.ResultObject = MAMWebFactory.Instance.GetSubMenu(PageType.PUBLIC_FILE, new MenuParamDTO() { Media = media });
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //부조SB > pgmcode (웹 큐시트에서 제외 됨, 추후 추가될 시 테스트 필요)
        [HttpGet("GetPgmcodes")]
        public DTO_RESULT<object> GetPgmcodes([FromQuery] string media, string brd_dt)
        {
            var result = new DTO_RESULT<object>();
            try
            {
                result.ResultObject = MAMWebFactory.Instance.GetSubMenu(PageType.SCR_SB, new MenuParamDTO() { Media = media, BrdDate = brd_dt });
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //음반 기록실 rowData 가져오기
        [HttpPost("GetSongItem")]
        public DTO_RESULT<ActionResult<DTO_SONG>> GetSongMastering([FromBody] DTO_MUSIC pram)
        {
            var result = new DTO_RESULT<ActionResult<DTO_SONG>>();
            try
            {
                var jsonMusicInfo = CommonUtility.ParseToJsonRequestContent(pram.FileToken);
                var musicInfo = CommonUtility.ParseToRequestContent(pram.FileToken);
                var requestInfo = _fileService.GetRequestInfo(musicInfo);
                long fileSize;
                var stream = _fileService.GetFileStream(requestInfo[0] as string, Convert.ToInt32(requestInfo[1]), jsonMusicInfo, out fileSize);

                var options = GetMasteringOptions(Startup.AppSetting.ConnectionString);
                string storageId = options.Find(dt => dt.Name == "STORAGE_ID").Value.ToString();
                string storagePass = options.Find(dt => dt.Name == "STORAGE_PASS").Value.ToString();
                int sampleRate = Convert.ToInt32(options.Find(dt => dt.Name == "SAMPLE_RATE").Value);
                int bitDepth = Convert.ToInt32(options.Find(dt => dt.Name == "BIT_DEPTH").Value);
                int channel = Convert.ToInt32(options.Find(dt => dt.Name == "CHANNEL").Value);

                string workFolder = options.Find(dt => dt.Name == "MST_UPLOAD_PATH").Value.ToString();
                string targetFolder = options.Find(dt => dt.Name == "SONG_PATH").Value.ToString();

                string fileExt = Path.GetExtension(pram.FilePath);
                SongMastering sm = new SongMastering(Startup.AppSetting.ConnectionString, storageId, storagePass, sampleRate, bitDepth, channel);
                result.ResultObject = sm.MasteringSong(pram, stream, fileExt, HttpContext.Items[Define.USER_ID] as string, workFolder, targetFolder, null, null);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                //StatusCode(500, ex.Message);
            }
            return result;
        }

        //효과음 rowData 가져오기
        [HttpPost("GetEffectItem")]
        public DTO_RESULT<DTO_PRO> GetEffectMastering([FromBody] DTO_EFFECT pram)
        {
            var result = new DTO_RESULT<DTO_PRO>();
            try
            {
                var jsonMusicInfo = CommonUtility.ParseToJsonRequestContent(pram.FileToken);
                var musicInfo = CommonUtility.ParseToRequestContent(pram.FileToken);
                var requestInfo = _fileService.GetRequestInfo(musicInfo);
                long fileSize;
                var stream = _fileService.GetFileStream(requestInfo[0] as string, Convert.ToInt32(requestInfo[1]), jsonMusicInfo, out fileSize);

                var options = GetMasteringOptions(Startup.AppSetting.ConnectionString);
                string storageId = options.Find(dt => dt.Name == "STORAGE_ID").Value.ToString();
                string storagePass = options.Find(dt => dt.Name == "STORAGE_PASS").Value.ToString();
                int sampleRate = Convert.ToInt32(options.Find(dt => dt.Name == "SAMPLE_RATE").Value);
                int bitDepth = Convert.ToInt32(options.Find(dt => dt.Name == "BIT_DEPTH").Value);
                int channel = Convert.ToInt32(options.Find(dt => dt.Name == "CHANNEL").Value);

                string workFolder = options.Find(dt => dt.Name == "MST_UPLOAD_PATH").Value.ToString(); ;
                string targetFolder = options.Find(dt => dt.Name == "SONG_PATH").Value.ToString(); ;

                string fileExt = Path.GetExtension(pram.FilePath);
                SongMastering sm = new SongMastering(Startup.AppSetting.ConnectionString, storageId, storagePass, sampleRate, bitDepth, channel);
                result.ResultObject = sm.MasteringEffect(pram, stream, fileExt, HttpContext.Items[Define.USER_ID] as string, workFolder, targetFolder, null, null);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        private List<Dto_MasteringOptions> GetMasteringOptions(string connectionString)
        {
            var masteringOptions = _apiDao.GetOptions("S01G06C001");
            var options = masteringOptions.ToList();
            return options;
        }
    }
}
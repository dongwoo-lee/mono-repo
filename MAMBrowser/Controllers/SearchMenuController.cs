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
using M30.AudioFile.DAL.Expand.Factories.Web;
using M30.AudioFile.DAL.WebService;
using MAMBrowser.DTO;
using MAMBrowser.Foundation;
using MAMBrowser.Mockup;
using MAMBrowser.Services;
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
            public long? deviceSeq {get; set;}
            public int rowperpage { get; set; }
            public int selectpage { get; set; }
            public string sortKey { get; set; }
            public string sortValue { get; set; }

            public int searchType1 { get; set; }
            public string searchType2 { get; set; }
            public int gradeType { get; set; }
            public string searchText { get; set; }

        }
        public class MusicResultDTO
        {
            public DTO_RESULT_PAGE_LIST<DTO_SONG> Result { get; set; }
        }
        public class EFFECTResultDTO
        {
            public DTO_RESULT_PAGE_LIST<DTO_EFFECT> Result { get; set; }
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
                //DL30
                case "DL30":
                    return MAMWebFactory.Instance.GetMenus(PageType.DL30);
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
        //MY 공간
        [HttpGet("GetSearchTable/MyDisk")]
        public MyDiskResultDTO GetMyDisk([FromQuery] Pram pram)
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

            return MAMWebFactory.Instance.Search<MyDiskResultDTO>(dto);
        }
        //DL 3.0
        [HttpGet("GetSearchTable/DL30")]
        public DL30ResultDTO GetDL30([FromQuery] Pram pram)
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

            return MAMWebFactory.Instance.Search<DL30ResultDTO>(dto);
        }

        //MUSIC
        [HttpGet("GetSearchTable/MUSIC")]
        public MusicResultDTO GetMUSIC([FromQuery] Pram pram)
        {
            var result = new MusicResultDTO();
            result.Result = new DTO_RESULT_PAGE_LIST<DTO_SONG>();
            long totalCount = 0;
            result.Result.Data = _fileService.SearchSong((MusicSearchTypes1)pram.searchType1, pram.searchType2, (GradeTypes)pram.gradeType, pram.searchText, pram.rowperpage, pram.selectpage, out totalCount);
            result.Result.TotalRowCount = totalCount;
            return result;
        }

        //EFFECT
        [HttpGet("GetSearchTable/EFFECT")]
        public EFFECTResultDTO GetEFFECT([FromQuery] Pram pram)
        {
            var result = new EFFECTResultDTO();
            result.Result = new DTO_RESULT_PAGE_LIST<DTO_EFFECT>();
            long totalCount = 0;
            result.Result.Data = _fileService.SearchEffect(pram.searchText, pram.rowperpage, pram.selectpage, out totalCount);
            result.Result.TotalRowCount = totalCount;
            return result;
        }

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

            var result =  MAMWebFactory.Instance.Search<OldProResultDTO>(a);
            return result;
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


            var result = MAMWebFactory.Instance.Search<McrSBResultDTO>(a);
            return result;
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

            var result =  MAMWebFactory.Instance.Search<ScrSBResultDTO>(a);
            return result;
        }
        //프로그램CM
        [HttpGet("GetSearchTable/PGM_CM")]
        public PgmCMResultDTO GetPgmCm([FromQuery] Pram pram)
        {
            try
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

                var result = MAMWebFactory.Instance.Search<PgmCMResultDTO>(a);
                return result;
            }
            catch
            {
                throw;
            }
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
        //부조SB > pgmcode
        [HttpGet("GetPgmcodes")]
        public MenuDTO GetPgmcodes([FromQuery] string media, string brd_dt)
        {
            return MAMWebFactory.Instance.GetSubMenu(PageType.SCR_SB, new MenuParamDTO() { Media = media, BrdDate= brd_dt});
        }

        //음반 기록실 rowData 가져오기
        [HttpPost("GetSongItem")]
        public ActionResult<DTO_SONG_CACHE>GetSongMastering([FromBody] DTO_SONG pram)
        {
            try
            {
                //목업데이터
                //MasteringMockup mockup = new MasteringMockup();
                //return mockup.SongMastering(pram);

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
                var result = sm.MasteringSong(pram, stream, fileExt, HttpContext.Items[Define.USER_ID] as string, workFolder, targetFolder, null, null);

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //효과음 rowData 가져오기
        [HttpPost("GetEffectItem")]
        public DTO_SONG_CACHE GetEffectMastering([FromBody] DTO_EFFECT pram)
        {
            //목업데이터
            //MasteringMockup mockup = new MasteringMockup();
            //return mockup.EffectMastering(pram);

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
            var result = sm.MasteringEffect(pram, stream, fileExt, HttpContext.Items[Define.USER_ID] as string, workFolder, targetFolder, null, null);
            return result;
        }

        private List<DTO_NAMEVALUE> GetMasteringOptions(string connectionString)
        {
            var masteringOptions = _apiDao.GetOptions("S01G06C001");
            var options = masteringOptions.ToList();
            return options;
        }
    }
}
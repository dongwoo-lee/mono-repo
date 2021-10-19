using MAMBrowser.Common.Expand.SearchOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.Common.Expand.Builder
{
    public class CMSearchOptionBuilder
    {
        private readonly CMSearchOptionDTO option = new CMSearchOptionDTO();
        public CMSearchOptionBuilder SetBrdDate(string brddate) { option.BrdDate = brddate; return this; }
        public CMSearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public CMSearchOptionBuilder SetCate(string cate) { option.Cate = cate; return this; }
        public CMSearchOptionBuilder SetPgmName(string pgmname) { option.PgmName = pgmname; return this; }
        public CMSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public CMSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public CMSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public CMSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class FillerEtcSearchOptionBuilder
    {
        private readonly FillerEtcSearchOptionDTO option = new FillerEtcSearchOptionDTO();
        public FillerEtcSearchOptionBuilder SetBrdDate(string brdDate) { option.BrdDate = brdDate; return this; }
        public FillerEtcSearchOptionBuilder SetCate(string cate) { option.Cate = cate; return this; }
        public FillerEtcSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public FillerEtcSearchOptionBuilder SetName(string name) { option.Name = name; return this; }
        public FillerEtcSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public FillerEtcSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public FillerEtcSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public FillerEtcSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class FillerMtSearchOptionBuilder
    {
        private readonly FillerMtSearchOptionDTO option = new FillerMtSearchOptionDTO();
        public FillerMtSearchOptionBuilder SetBrdDate(string brdDate) { option.BrdDate = brdDate; return this; }
        public FillerMtSearchOptionBuilder SetCate(string cate) { option.Cate = cate; return this; }
        public FillerMtSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public FillerMtSearchOptionBuilder SetName(string name) { option.Name = name; return this; }
        public FillerMtSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public FillerMtSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public FillerMtSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public FillerMtSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class FillerSearchOptionBuilder
    {
        private readonly FillerPrSearchOptionDTO option = new FillerPrSearchOptionDTO();
        public FillerSearchOptionBuilder SetBrdDate(string brdDate) { option.BrdDate = brdDate; return this; }
        public FillerSearchOptionBuilder SetCate(string cate) { option.Cate = cate; return this; }
        public FillerSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public FillerSearchOptionBuilder SetName(string name) { option.Name = name; return this; }
        public FillerSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public FillerSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public FillerSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public FillerSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class FillerTimeSearchOptionBuilder
    {
        private readonly FillerTimeSearchOptionDTO option = new FillerTimeSearchOptionDTO();
        public FillerTimeSearchOptionBuilder SetStartDate(string startDate) { option.StartDate = startDate; return this; }
        public FillerTimeSearchOptionBuilder SetEndDate(string endDate) { option.EndDate = endDate; return this; }
        public FillerTimeSearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public FillerTimeSearchOptionBuilder SetCate(string cate) { option.Cate = cate; return this; }
        public FillerTimeSearchOptionBuilder SetStatus(string status) { option.Status = status; return this; }
        public FillerTimeSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public FillerTimeSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public FillerTimeSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public FillerTimeSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public FillerTimeSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class McrSBearchOptionBuilder
    {
        private readonly McrSBSearchOptionDTO option = new McrSBSearchOptionDTO();
        public McrSBearchOptionBuilder SetBrdDate(string brddate) { option.BrdDate = brddate; return this; }
        public McrSBearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public McrSBearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public McrSBearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public McrSBearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public McrSBearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class McrSpotSearchOptionBuilder
    {
        private readonly McrSpotSearchOptionDTO option = new McrSpotSearchOptionDTO();
        public McrSpotSearchOptionBuilder SetStartDate(string startDate) { option.StartDate = startDate; return this; }
        public McrSpotSearchOptionBuilder SetEndDate(string endDate) { option.EndDate = endDate; return this; }
        public McrSpotSearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public McrSpotSearchOptionBuilder SetSpotID(string spotid) { option.SpotID = spotid; return this; }
        public McrSpotSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public McrSpotSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public McrSpotSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public McrSpotSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public McrSpotSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class OldProSearchOptionBuilder
    {
        private readonly OldProSearchOptionDTO option = new OldProSearchOptionDTO();
        public OldProSearchOptionBuilder SetStartDate(string startDate) { option.StartDate = startDate; return this; }
        public OldProSearchOptionBuilder SetEndDate(string endDate) { option.EndDate = endDate; return this; }
        public OldProSearchOptionBuilder SetType(string type) { option.Type = type; return this; }
        public OldProSearchOptionBuilder SetCate(string cate) { option.Cate = cate; return this; }
        public OldProSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public OldProSearchOptionBuilder SetName(string name) { option.Name = name; return this; }
        public OldProSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public OldProSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public OldProSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public OldProSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class PgmCMSearchOptionBuilder
    {
        private readonly PgmCMSearchOptionDTO option = new PgmCMSearchOptionDTO();
        public PgmCMSearchOptionBuilder SetBrdDate(string brddate) { option.BrdDate = brddate; return this; }
        public PgmCMSearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public PgmCMSearchOptionBuilder SetPgmName(string pgmname) { option.PgmName = pgmname; return this; }
        public PgmCMSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public PgmCMSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public PgmCMSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public PgmCMSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class PgmSearchOptionBuilder
    {
        private readonly PgmSearchOptionDTO option = new PgmSearchOptionDTO();
        public PgmSearchOptionBuilder SetBrdDate(string brddate) { option.BrdDate = brddate; return this; }
        public PgmSearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public PgmSearchOptionBuilder SetPgmName(string pgmname) { option.PgmName = pgmname; return this; }
        public PgmSearchOptionBuilder SetEditorName(string editorname) { option.EditorName = editorname; return this; }
        public PgmSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public PgmSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public PgmSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public PgmSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class PublicSearchOptionBuilder
    {
        private readonly PublicFileSearchOptionDTO option = new PublicFileSearchOptionDTO();
        public PublicSearchOptionBuilder SetStartDate(string startDate) { option.StartDate = startDate; return this; }
        public PublicSearchOptionBuilder SetEndDate(string endDate) { option.EndDate = endDate; return this; }
        public PublicSearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public PublicSearchOptionBuilder SetCate(string cate) { option.Cate = cate; return this; }
        public PublicSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public PublicSearchOptionBuilder SetTitle(string title) { option.Title = title; return this; }
        public PublicSearchOptionBuilder SetMemo(string memo) { option.Memo = memo; return this; }
        public PublicSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public PublicSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public PublicSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public PublicSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class ReportSearchOptionBuilder
    {
        private readonly ReportSearchOptionDTO option = new ReportSearchOptionDTO();
        public ReportSearchOptionBuilder SetStartDate(string startDate) { option.StartDate = startDate; return this; }
        public ReportSearchOptionBuilder SetEndDate(string endDate) { option.EndDate = endDate; return this; }
        public ReportSearchOptionBuilder SetCate(string cate) { option.Cate = cate; return this; }
        public ReportSearchOptionBuilder SetPgmName(string pgmName) { option.PgmName = pgmName; return this; }
        public ReportSearchOptionBuilder SetReporterName(string reporterName) { option.ReporterName = reporterName; return this; }
        public ReportSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public ReportSearchOptionBuilder SetName(string name) { option.Name = name; return this; }
        public ReportSearchOptionBuilder SetIsMastering(string ismastering) { option.IsMastering = ismastering; return this; }
        public ReportSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public ReportSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public ReportSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public ReportSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class ScrSBSearchOptionBuilder
    {
        private readonly ScrSBSearchOptionDTO option = new ScrSBSearchOptionDTO();
        public ScrSBSearchOptionBuilder SetBrdDate(string brddate) { option.BrdDate = brddate; return this; }
        public ScrSBSearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public ScrSBSearchOptionBuilder SetPgm(string pgm) { option.Pgm = pgm; return this; }
        public ScrSBSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public ScrSBSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public ScrSBSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public ScrSBSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }

    public class ScrSpotSearchOptionBuilder
    {
        private readonly ScrSpotSearchOptionDTO option = new ScrSpotSearchOptionDTO();
        public ScrSpotSearchOptionBuilder SetStartDate(string startDate) { option.StartDate = startDate; return this; }
        public ScrSpotSearchOptionBuilder SetEndDate(string endDate) { option.EndDate = endDate; return this; }
        public ScrSpotSearchOptionBuilder SetMedia(string media) { option.Media = media; return this; }
        public ScrSpotSearchOptionBuilder SetPgmName(string pgmName) { option.PgmName = pgmName; return this; }
        public ScrSpotSearchOptionBuilder SetEditor(string editor) { option.Editor = editor; return this; }
        public ScrSpotSearchOptionBuilder SetName(string name) { option.Name = name; return this; }
        public ScrSpotSearchOptionBuilder SetRowPerPage(int rowperpage) { option.RowPerPage = rowperpage; return this; }
        public ScrSpotSearchOptionBuilder SetSelectPage(int selectpage) { option.SelectPage = selectpage; return this; }
        public ScrSpotSearchOptionBuilder SetSortKey(string sortKey) { option.SortKey = sortKey; return this; }
        public ScrSpotSearchOptionBuilder SetSortValue(string sortValue) { option.SortValue = sortValue; return this; }
        public SearchOptionDTO Build() => option;
    }
}

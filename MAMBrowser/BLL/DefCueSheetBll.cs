using M30_CueSheetDAO;
using M30_CueSheetDAO.DAO;
using M30_CueSheetDAO.Entity;
using M30_CueSheetDAO.Interfaces;
using M30_CueSheetDAO.ParamEntity;
using M30.AudioFile.Common.DTO;
using MAMBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.DTO;

namespace MAMBrowser.BLL
{
    public class DefCueSheetBll
    {
        private readonly ICueSheetDAO _dao;
        private readonly ICommonDAO _common_dao;


        public DefCueSheetBll(ICueSheetDAO dao, ICommonDAO common_dao)
        {
            _dao = dao;
            _common_dao = common_dao;
        }
        // 기본큐시트 목록 가져오기
        public DefCueList_Page GetDefCueList(List<string> productids, int row_per_page, int select_page)
        {
            var result = new DefCueList_Page();
            DefCueSheetListParam param = new DefCueSheetListParamBuilder()
                .SetProductIDs(productids)
                .SetRowPage(row_per_page)
                .SetSelectPage(select_page)
                .Build();

            var data = _dao.GetDefCueSheetList(param);
            result.RowPerPage = row_per_page;
            result.SelectPage = select_page;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DefCueSheetListEntity?.Converting();
            return result;

        }
        // 기본큐시트 상세내용 가져오기
        public CueSheetCollectionDTO GetDefCue(string productid, List<string> week, string pgmcode, string brd_dt)
        {
            DefCueSheetInfoParam param = new DefCueSheetInfoParamBuilder()
                .SetWeeks(week)
                .SetProductID(productid)
                .SetRequestType(RequestType.Web)
                .Build();
            var result = _dao.GetDefCueSheet(param);
            var toDate = DateTime.Today;
            if (brd_dt == null)
            {
                brd_dt = toDate.ToString("yyyyMMdd");
            }
            if (pgmcode != null && brd_dt != null)
            {
                SponsorParam spon_param = new SponsorParam();
                spon_param.BrdDate = brd_dt;
                spon_param.PgmCode = pgmcode;
                //var result2 = _common_dao.GetSponsor(spon_param);
                result.CueSheetConEntities = _common_dao.GetSponsor(spon_param).SetSponsorToEntity(_dao.GetDefCueSheet(param).CueSheetConEntities);
            }

            return result?.DefConverting();
        }
        //기본큐시트 생성 & 업데이트
        public int SaveDefaultCueSheet(CueSheetCollectionDTO pram)
        {
            var paramData = pram.DefToEntity();
            DefCueSheetCreateParam param = new DefCueSheetCreateParamBuilder()
                .SetCueSheetConParams(paramData.CueSheetConParams)
                .SetDefCueSheetParam(paramData.DefCueSheetParam)
                .SetPrintParams(paramData.PrintParams)
                .Build();

            param.DelDefCueParams = new List<DefDeleteParam>();
            param.DelDefCueParams = paramData.DelDefCueParams;

            return _dao.CreateDefCueSheet(param);
        }

        //기본큐시트 삭제
        public bool DeleteDefaultCueSheet(int[] delParams)
        {
            var param = new List<DefDeleteParam>();
            for (int i = 0; i < delParams.Length; i++)
            {
                var delParam = new DefDeleteParam();
                delParam.p_del_cueid = delParams[i];
                param.Add(delParam);
            }
            return _dao.DeleteDefCueSheet(param);
        }

    }
}

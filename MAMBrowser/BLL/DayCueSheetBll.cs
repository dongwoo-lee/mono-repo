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
    public class DayCueSheetBll
    {
        private readonly ICueSheetDAO _dao;
        private readonly ICommonDAO _common_dao;

        public DayCueSheetBll(ICueSheetDAO dao, ICommonDAO common_dao)
        {
            _dao = dao;
            _common_dao = common_dao;
        }

        // 일일큐시트 목록 가져오기
        public DayCueList_Page GetDayCueSheetList(List<string> products, List<string> dates, int row_per_page, int select_page)
        {
            var result = new DayCueList_Page();
            DayCueSheetListParam param = new DayCueSheetListParamBuilder()
                .SetDates(dates)
                .SetProductIDs(products)
                .SetRowPage(row_per_page)
                .SetSelectPage(select_page)
                .Build();

            var data = _dao.GetDayCueSheetList(param);
            result.RowPerPage = row_per_page;
            result.SelectPage = select_page;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DayCueSheetListEntity?.Converting();
            return result;
        }

        // 일일큐시트 상세내용 가져오기 
        public CueSheetCollectionDTO GetDayCueSheet(string productid, int cueid, string pgmcode, string brd_dt)
        {
            DayCueSheetInfoParam param = new DayCueSheetInfoParamBuilder()
                .SetCueID(cueid)
                .SetProductID(productid)
                .SetRequestType(RequestType.Web)
                .Build();

            var result = _dao.GetDayCueSheet(param);
            if (pgmcode!=null&& brd_dt != null)
            {
            SponsorParam spon_param = new SponsorParam();
            spon_param.BrdDate = brd_dt;
            spon_param.PgmCode = pgmcode;
            result.CueSheetConEntities = _common_dao.GetSponsor(spon_param).SetSponsorToEntity(_dao.GetDayCueSheet(param).CueSheetConEntities);
            }

            return result?.DayConverting();


        }
        public List<CueSheetConDTO> GetAddSponsorList(string pgmcode, string brd_dt)
        {
            SponsorParam param = new SponsorParam();
            param.BrdDate = brd_dt;
            param.PgmCode = pgmcode;

            return _common_dao.GetSponsor(param).AddSponsorListToDTO(pgmcode);
        }

        //일일큐시트 저장
        public int SaveDayCue(CueSheetCollectionDTO pram)
        {
            var paramData = pram.DayToEntity();
            DayCueSheetCreateParam param = new DayCueSheetCreateParamBuilder()
                .SetCueSheetConParams(paramData.CueSheetConParams)
                .SetDayCueSheetParam(paramData.DayCueSheetParam)
                .SetPrintParams(paramData.PrintParams)
                .Build();

            var result = _dao.CreateDayCueSheet(param);
            return result;
        }
        //구 DAP 삭제
        public int DelOldCue(CueSheetCollectionDTO pram)
        {
            DeletePDPQS(pram.CueSheetDTO, 'I');
            DeletePDPQS(pram.CueSheetDTO, 'N');
            return 1;
        }
        //구 DAP 저장
        public int SaveOldCue(CueSheetCollectionDTO pram)
        {
            // 구 DB
            // 기존 구 DB 데이터 삭제
            //DeletePDPQS(pram.CueSheetDTO, 'I');
            //DeletePDPQS(pram.CueSheetDTO, 'N');

            var instanceCon = pram.PDPQSToEntity('I');
            var normalCon = pram.PDPQSToEntity('N');
            if (instanceCon.PDPQSConParam?.Any() == true)
                _dao.CreatePDPQS(instanceCon);
            if (normalCon.PDPQSConParam?.Any() == true)
                _dao.CreatePDPQS(normalCon);
            return 0;
        }

        // 구DB 삭제
        private void DeletePDPQS(CueSheetDTO cueshset, char type)
        {
            _dao.DeletePDPQS(new PDPQSDeleteCollectionParam()
            {
                DeleteParam = new PDPQSDeleteParam()
                {
                    ProductID_in = cueshset.PRODUCTID,
                    PQSType_in = type,
                    OnAirDate_in = cueshset.BRDDATE
                },
                DeleteConParam = new PDPQSConDeleteParam()
                {
                    OnAirDate_in = cueshset.BRDDATE,
                    PqsType_in = type,
                    ProductID_in = cueshset.PRODUCTID
                }
            });
        }
    }
}

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
        public DayCueList_Page GetDayCueSheetList(List<string> products, List<string> dates, int row_per_page, int select_page, string media)
        {
            var result = new DayCueList_Page();
            DayCueSheetListParam param = new DayCueSheetListParamBuilder()
                .SetDates(dates)
                .SetProductIDs(products)
                .SetRowPage(row_per_page)
                .SetSelectPage(select_page)
                .Build();
            //media 추가
            param.Media = media;

            var data = _dao.GetDayCueSheetListV2(param);
            result.RowPerPage = row_per_page;
            result.SelectPage = select_page;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DayCueSheetListEntity?.Converting();
            return result;
        }

        // 일일큐시트 상세내용 가져오기 
        public CueSheetCollectionDTO GetDayCueSheet(string productid, string pgmcode, string brd_dt)
        {
            DayCueSheetInfoParam param = new DayCueSheetInfoParamBuilder()
                .SetCueID(-1)
                .SetProductID(productid)
                .SetRequestType(RequestType.Web)
                .Build();
            param.BrdDate = brd_dt;
            var result = _dao.GetDayCueSheet(param);
            if (result.CueSheetEntity == null)
            {
                return null;
            }
            else
            {
                if (pgmcode != null && brd_dt != null)
                {
                    SponsorParam spon_param = new SponsorParam();
                    spon_param.BrdDate = brd_dt;
                    spon_param.PgmCode = pgmcode;
                    result.CueSheetConEntities = _common_dao.GetSponsor(spon_param).SetSponsorToEntity(_dao.GetDayCueSheet(param).CueSheetConEntities);
                }

                return result?.DayConverting();

            }
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

        /// <summary>
        /// (구)DAP 저장
        /// </summary>
        /// <param name="pram"></param>
        /// <returns> 유효성검사(MY,DL3 소재 불가능) 실패시 : False </returns>
        public bool SaveOldCueSheet(CueSheetCollectionDTO pram)
        {
            // 1. 저장가능한지 유효성 검사
            if (ValidateForOldSave(pram))
            {
                SaveOldCueSheetChannel(pram, 'I');
                SaveOldCueSheetChannel(pram, 'N');
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 1. pdpqs 인서트
        /// 2. pdpqscon 삭제
        /// 3. pdpqscon 인서트
        /// </summary>
        /// <param name="cueSheetChannel">  I : C채널, N : A/B채널</param>
        private void SaveOldCueSheetChannel(CueSheetCollectionDTO pram, char cueSheetChannel)
        {
            var pqsCon = pram.PDPQSToEntity(cueSheetChannel);
            _dao.CreatePDPQS(pqsCon);
            DeletePDPQSCon(pram.CueSheetDTO, cueSheetChannel);
            _dao.CreatePDPQSCon(pqsCon);
        }

        /// <summary>
        /// (구)DAP 큐시트 저장에 대한 유효성 검사
        /// </summary>
        /// <remarks>
        /// MY디스크와 DL3는 (구)DAP 에 저장 할 수 없다.
        /// MY디스크와 DL3는 카트타입이 빈 문자열 또는 null 이다.
        /// </remarks>
        /// <param name="pram"></param>
        /// <returns></returns>
        private bool ValidateForOldSave(CueSheetCollectionDTO pram)
        {
            //A,B채널  유효성 검사
            var normalCon = pram.PDPQSToEntity('N');
            foreach(var pqsCon in normalCon.PDPQSConParam)
            {
                 if(string.IsNullOrEmpty(pqsCon.CartType_in))
                    return false;
            }

            //C채널 유효성 검사
            var instanceCon = pram.PDPQSToEntity('I');
            foreach (var pqsCon in instanceCon.PDPQSConParam)
            {
                if (string.IsNullOrEmpty(pqsCon.CartType_in))
                    return false;
            }
            return true;
        }

        // 구DB 삭제
        private void DeletePDPQSCon(CueSheetDTO cueshset, char type)
        {
            _dao.DeletePDPQSCon(new PDPQSDeleteCollectionParam()
            {
                DeleteConParam = new PDPQSConDeleteParam()
                {
                    OnAirDate_in = cueshset.BRDDATE,
                    PqsType_in = type,
                    ProductID_in = cueshset.PRODUCTID,
                }
            });
        }
    }
}

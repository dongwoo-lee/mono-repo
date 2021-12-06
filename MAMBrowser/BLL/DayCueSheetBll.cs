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
            result.CueSheetConEntities = _common_dao.GetSponsor(spon_param).SetSponsor(_dao.GetDayCueSheet(param).CueSheetConEntities);
            }

            return result?.DayConverting();

        }

        //일일큐시트 저장
        public int SaveDayCue(CueSheetCollectionDTO pram)
        {
            // 신 DB
            var paramData = pram.DayToEntity();
            DayCueSheetCreateParam param = new DayCueSheetCreateParamBuilder()
                .SetCueSheetConParams(paramData.CueSheetConParams)
                .SetDayCueSheetParam(paramData.DayCueSheetParam)
                .SetPrintParams(paramData.PrintParams)
                .Build();

            // 구 DB
            // 기존 구 DB 데이터 삭제
            DeletePDPQS(pram.CueSheetDTO, 'I');
            DeletePDPQS(pram.CueSheetDTO, 'N');

            var instanceCon = pram.PDPQSToEntity('I');
            var normalCon = pram.PDPQSToEntity('N');
            if (instanceCon.PDPQSConParam?.Any() == true)
                _dao.CreatePDPQS(instanceCon);
            if (normalCon.PDPQSConParam?.Any() == true)
                _dao.CreatePDPQS(normalCon);

            return _dao.CreateDayCueSheet(param);
            //var instacneValue = param.CueSheetConParams.Where(x => x.p_channeltype.Equals('I') && x.p_seqnum < 33).ToList();
            //if (instacneValue?.Any() == true)
            //{
            //    var list = new List<PDPQSConParam>();
            //    foreach (var item in instacneValue)
            //    {
            //        var resultItem = new PDPQSConParam();
            //        resultItem.Description_in = item.p_memo;
            //        resultItem.LLevel_in = 0;
            //        resultItem.RLevel_in = 0;
            //        resultItem.FadeInTime_in = item.p_fadeintime;
            //        resultItem.FadeOutTime_in = item.p_fadeouttime;
            //        resultItem.ExtroTime_in = 0;
            //        resultItem.IntroTime_in = 0;
            //        resultItem.MainTitle_in = item.p_maintitle;
            //        resultItem.TransType_in = item.p_transtype;
            //        resultItem.SOM_in = item.p_startposition;
            //        resultItem.CartType_in = setCartType(item.p_cartid);
            //        resultItem.CartID_in = item.p_cartid;
            //        resultItem.SeqNum_in = item.p_seqnum;
            //        resultItem.PqsType_in = 'I';
            //        resultItem.OnAirDate_in = param.DayCueSheetParam.p_brddate;
            //        resultItem.ProductID_in = param.DayCueSheetParam.p_productid;
            //        resultItem.EOM_in = item.p_endposition;
            //        resultItem.SubTitle_in = item.p_subtitle;
            //        list.Add(resultItem);
            //    }
            //    var pqsData = pram.CueSheetDTO.PDPQSToEntity('I');
            //    _dao.CreatePDPQS(new PDPQSCreateCollectionParam()
            //    {
            //        PDPQSParam = pqsData,
            //        PDPQSConParam = new List<PDPQSConParam>(list)
            //    });
            //}

            //var normalValue = param.CueSheetConParams.Where(x => 
            //x.p_channeltype.Equals('N') && x.p_cartid !=""&&
            //(!string.IsNullOrEmpty(x.p_memo) || !string.IsNullOrEmpty(x.p_maintitle))).ToList();
            //if (normalValue?.Any() == true)
            //{
            //    var list = new List<PDPQSConParam>();
            //    foreach (var item in normalValue)
            //    {
            //        var resultItem = new PDPQSConParam();
            //        resultItem.Description_in = item.p_memo;
            //        resultItem.LLevel_in = 0;
            //        resultItem.RLevel_in = 0;
            //        resultItem.FadeInTime_in = item.p_fadeintime;
            //        resultItem.FadeOutTime_in = item.p_fadeouttime;
            //        resultItem.ExtroTime_in = 0;
            //        resultItem.IntroTime_in = 0;
            //        resultItem.MainTitle_in = item.p_maintitle;
            //        resultItem.TransType_in = item.p_transtype;
            //        resultItem.SOM_in = item.p_startposition;
            //        resultItem.CartType_in = setCartType(item.p_cartid);
            //        resultItem.CartID_in = item.p_cartid;
            //        resultItem.SeqNum_in = item.p_seqnum;
            //        resultItem.PqsType_in = 'N';
            //        resultItem.OnAirDate_in = param.DayCueSheetParam.p_brddate;
            //        resultItem.ProductID_in = param.DayCueSheetParam.p_productid;
            //        resultItem.EOM_in = item.p_endposition;
            //        resultItem.SubTitle_in = item.p_subtitle;
            //        list.Add(resultItem);
            //    }
            //    var pqsData = pram.CueSheetDTO.PDPQSToEntity('N');
            //    _dao.CreatePDPQS(new PDPQSCreateCollectionParam()
            //    {
            //        PDPQSParam = pqsData,
            //        PDPQSConParam = list
            //    });


            //}


        }


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

        public static string setCartType(string cartid)
        {
            var front = cartid.Substring(0, 2);
            var back = cartid[cartid.Length - 2];
            var result = "";
            switch (front)
            {
                case "TS":
                    result = "TS";
                    break;
                case "SS":
                    result = "SS";
                    break;
                case "AC":
                    result = "AC";
                    break;
                case "EC":
                    result = "EC";
                    break;
                case "RC":
                    result = "RC";
                    break;
                case "AS":
                    result = "SB";
                    break;
                case "FC":
                    result = "FC";
                    break;
                case "ST":
                    result = "ST";
                    break;
                case "PM":
                    if (back.ToString() == "NA")
                    {
                        result = "AR";
                    }
                    else
                    {
                        result = "PM";
                    }
                    break;
                case "MS":
                    result = "MS";
                    break;
                case "TT":
                    result = "TT";
                    break;
                case "AR":
                    result = "AR";
                    break;
                case "MB":
                    result = "MB";
                    break;
                case "JG":
                    result = "JG";
                    break;
                default:
                    result = "CM";
                    break;
            }
            return result;
        }
    }
}

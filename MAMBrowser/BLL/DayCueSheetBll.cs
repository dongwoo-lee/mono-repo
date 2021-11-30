﻿using M30_CueSheetDAO;
using M30_CueSheetDAO.DAO;
using M30_CueSheetDAO.Entity;
using M30_CueSheetDAO.Interfaces;
using M30_CueSheetDAO.ParamEntity;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class DayCueSheetBll
    {
        private readonly ICueSheetDAO _dao;

        public DayCueSheetBll(ICueSheetDAO dao)
        {
            _dao = dao;
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
        public CueSheetCollectionDTO GetDayCueSheet(string productid, int cueid)
        {
            DayCueSheetInfoParam param = new DayCueSheetInfoParamBuilder()
                .SetCueID(cueid)
                .SetProductID(productid)
                .SetRequestType(RequestType.Web)
                .Build();

            return _dao.GetDayCueSheet(param)?.DayConverting();
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
            
            var instacneValue = param.CueSheetConParams.Where(x => x.p_channeltype.Equals('I') && x.p_seqnum < 33).ToList();
            if (instacneValue?.Any() == true)
            {
                var list = new List<PDPQSConParam>();
                foreach (var item in instacneValue)
                {
                    var resultItem = new PDPQSConParam();
                    resultItem.Description_in = item.p_memo;
                    resultItem.LLevel_in = 0;
                    resultItem.RLevel_in = 0;
                    resultItem.FadeInTime_in = item.p_fadeintime;
                    resultItem.FadeOutTime_in = item.p_fadeouttime;
                    resultItem.ExtroTime_in = 0;
                    resultItem.IntroTime_in = 0;
                    resultItem.MainTitle_in = item.p_maintitle;
                    resultItem.TransType_in = Char.ToLower(item.p_transtype);
                    resultItem.SOM_in = item.p_startposition;
                    resultItem.CartType_in = setCartType(item.p_cartid);
                    resultItem.CartID_in = item.p_cartid;
                    resultItem.SeqNum_in = item.p_seqnum;
                    resultItem.PqsType_in = 'I';
                    resultItem.OnAirDate_in = param.DayCueSheetParam.p_brddate;
                    resultItem.ProductID_in = param.DayCueSheetParam.p_productid;
                    resultItem.EOM_in = item.p_endposition;
                    resultItem.SubTitle_in = item.p_subtitle;
                    list.Add(resultItem);
                }
                var pqsData = pram.CueSheetDTO.PDPQSToEntity('I');
                _dao.CreatePDPQS(new PDPQSCreateCollectionParam()
                {
                    PDPQSParam = pqsData,
                    PDPQSConParam = new List<PDPQSConParam>(list)
                });
            }

            var normalValue = param.CueSheetConParams.Where(x => 
            x.p_channeltype.Equals('N') && x.p_cartid !=""&&
            (!string.IsNullOrEmpty(x.p_memo) || !string.IsNullOrEmpty(x.p_maintitle))).ToList();
            if (normalValue?.Any() == true)
            {
                var list = new List<PDPQSConParam>();
                foreach (var item in normalValue)
                {
                    var resultItem = new PDPQSConParam();
                    resultItem.Description_in = item.p_memo;
                    resultItem.LLevel_in = 0;
                    resultItem.RLevel_in = 0;
                    resultItem.FadeInTime_in = item.p_fadeintime;
                    resultItem.FadeOutTime_in = item.p_fadeouttime;
                    resultItem.ExtroTime_in = 0;
                    resultItem.IntroTime_in = 0;
                    resultItem.MainTitle_in = item.p_maintitle;
                    resultItem.TransType_in = Char.ToLower(item.p_transtype);
                    resultItem.SOM_in = item.p_startposition;
                    resultItem.CartType_in = setCartType(item.p_cartid);
                    resultItem.CartID_in = item.p_cartid;
                    resultItem.SeqNum_in = item.p_seqnum;
                    resultItem.PqsType_in = 'N';
                    resultItem.OnAirDate_in = param.DayCueSheetParam.p_brddate;
                    resultItem.ProductID_in = param.DayCueSheetParam.p_productid;
                    resultItem.EOM_in = item.p_endposition;
                    resultItem.SubTitle_in = item.p_subtitle;
                    list.Add(resultItem);
                }
                var pqsData = pram.CueSheetDTO.PDPQSToEntity('N');
                _dao.CreatePDPQS(new PDPQSCreateCollectionParam()
                {
                    PDPQSParam = pqsData,
                    PDPQSConParam = list
                });


            }
            return _dao.CreateDayCueSheet(param);
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
            var back = cartid.Substring(8, 2);
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
                    if (back == "NA")
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

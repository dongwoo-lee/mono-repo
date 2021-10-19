using DAP3.CueSheetCommon.DTO.Param;
using DAP3.CueSheetCommon.DTO.Result;
using DAP3.CueSheetDAL.Factories.Web;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MAMBrowser.BLL
{
    public class DayCueSheetBll
    {
        private readonly WebCueSheetFactory _factory;
        public DayCueSheetBll(WebCueSheetFactory factory) 
        {
            _factory = factory;
        }

        // 일일큐시트 목록 가져오기 (특정날짜)
        public List<DayCueSheetListDTO> GetDayCueList(string[] products, string[] dates)
        {
            try
            {
                var result = _factory.CueSheetRepository.GetDayCueSheetList(products, dates).ToList();
                return result;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        // 일일큐시트 상세내용 가져오기 
        public CueSheetCollectionDTO GetDayCue(string productid, int cueid)
        {
            var result = _factory.CueSheetRepository.GetDayCueSheet(productid, cueid, null);
            return result;
        }

        // 일일큐시트 저장
        public SaveResultDTO SaveDayCue(CueSheetParamDTO cueParam, DayCueSheetParamDTO dayParam, IEnumerable<CueSheetConParamDTO> conParams, IEnumerable<string> tagParams, IEnumerable<PrintParamDTO> printParams, IEnumerable<AttachmentParamDTO> attParams)
        {
            var result = _factory.CueSheetRepository.SaveDayCueSheet(cueParam, dayParam, conParams, tagParams, printParams, attParams);
            return result;
        }

        // 일일큐시트 삭제
        public bool DelDayCue(int cueid)
        {
            return _factory.CueSheetRepository.DeleteDayCueSheet(cueid);
        }
    }
}

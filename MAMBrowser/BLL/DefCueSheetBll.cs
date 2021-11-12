using DAP3.CueSheetCommon.DTO.Param;
using DAP3.CueSheetCommon.DTO.Result;
using DAP3.CueSheetDAL.Factories.Web;
using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MAMBrowser.BLL
{
    public class DefCueSheetBll
    {
        private readonly WebCueSheetFactory _factory;
        public DefCueSheetBll(WebCueSheetFactory factory)
        {
            _factory = factory;
        }
        // 기본큐시트 목록 가져오기
        public List<DefaultCueSheetListDTO> GetDefCueList(string[] productids)
        {
            var result = _factory.CueSheetRepository.GetDefaultCueSheetList(productids).ToList();
            return result;
        }
        // 기본큐시트 상세내용 가져오기
        public CueSheetCollectionDTO GetDefCue(string productid, int[] cueid)
        {
            var result = _factory.CueSheetRepository.GetDefultCueSheet(productid, cueid);
            return result;
        }
        //기본큐시트 생성 & 업데이트
        public bool SaveDefaultCueSheet(CueSheetParamDTO cueParam, IEnumerable<string> defParams, IEnumerable<CueSheetConParamDTO> conParams, IEnumerable<string> tagParams, IEnumerable<PrintParamDTO> printParams, IEnumerable<AttachmentParamDTO> attParams, int[] delParams)
        {
            //int[] delid = { 797, 798 };
            var result = _factory.CueSheetRepository.SaveDefaultCueSheet(cueParam, defParams, conParams, tagParams, printParams, attParams, delParams);
            return result.IsSuccess;
        }
        //기본큐시트 삭제
        public bool DeleteDefaultCueSheet(int[] delParams)
        {
            var result = _factory.CueSheetRepository.DeleteDefaultCueSheet(delParams);
            return result;
        }
    }
}

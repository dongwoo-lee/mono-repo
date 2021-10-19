using DAP3.CueSheetCommon.DTO.Param;
using DAP3.CueSheetCommon.DTO.Result;
using DAP3.CueSheetDAL.Factories.Web;
using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAMBrowser.BLL
{
    public class TemplateBll
    {
        private readonly WebCueSheetFactory _factory;
        public TemplateBll(WebCueSheetFactory factory)
        {
            _factory = factory;
        }
        // 템플릿 목록 가져오기
        public List<TemplateListDTO> GetTemplateList(string personid)
        {
            return _factory.TemplateRepository.GetTemplateList(personid).ToList();
        }

        // 템플릿 상세내용 가져오기 
        public TemplateCollectionDTO GetTemplate(int cueid)
        {
            return _factory.TemplateRepository.GetTemplate(cueid);
        }

        // 템플릿 저장
        public bool SaveTemplate(TemplateParamDTO temParam, IEnumerable<CueSheetConParamDTO> conParams, IEnumerable<string> tagParams, IEnumerable<PrintParamDTO> printParams, IEnumerable<AttachmentParamDTO> attParams)
        {
            var result = _factory.TemplateRepository.SaveTemplate(temParam, conParams, tagParams, printParams, attParams);
            return result.IsSuccess;
        }

        // 템플릿 삭제
        public bool DeleteTemplate(int[] tempids)
        {
            return _factory.TemplateRepository.DeleteTemplate(tempids);
        }
    }
}

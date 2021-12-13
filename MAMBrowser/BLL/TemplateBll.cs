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
    public class TemplateBll
    {
        private readonly ITemplateDAO _dao;
        private readonly ICommonDAO _common_dao;

        public TemplateBll(ITemplateDAO dao, ICommonDAO common_dao)
        {
            _dao = dao;
            _common_dao = common_dao;
        }
        // 템플릿 목록 가져오기
        public TempCueList_Page GetPersonIDWithTitleTemplateList(string personid, string title, int row_per_page, int select_page)
        {
            var result = new TempCueList_Page();
            TemplateListParam param = new TemplateListParamBuilder()
                .SetPersonID(personid)
                .SetTitle(title)
                .Build();

            var data = _dao.GetTemplateList(param);
            result.RowPerPage = row_per_page;
            result.SelectPage = select_page;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.TemplateListEntity?.Converting();
            return result;

        }
        // 템플릿 상세내용 가져오기
        public CueSheetCollectionDTO GetTemplate(int cueid, string pgmcode, string brd_dt)
        {
            TemplateInfoParam param = new TemplateInfoParam();
            param.TemplateID = cueid;
            var toDate = DateTime.Today;
            if (brd_dt == null)
            {
                brd_dt = toDate.ToString("yyyyMMdd");
            }
            var result = _dao.GetTemplate(param);
            if (pgmcode != null && brd_dt != null)
            {
                SponsorParam spon_param = new SponsorParam();
                spon_param.BrdDate = brd_dt;
                spon_param.PgmCode = pgmcode;
                result.CueSheetConEntities = _common_dao.GetSponsor(spon_param).SetSponsorToEntity(_dao.GetTemplate(param).CueSheetConEntities);
            }
            return result?.TemConverting();
        }
        //템플릿 생성 & 업데이트 (해야함)
        public int SaveTemplate(CueSheetCollectionDTO pram)
        {
            var paramData = pram.TempToEntity();
            TemplateCreateParam param = new TemplateCreateParam();
            param.TemplateParam = paramData.TemplateParam;
            param.CueSheetConParams = paramData.CueSheetConParams;
            param.PrintParams = paramData.PrintParams;

            return _dao.CreateTemplate(param);
        }

        //템플릿 삭제
        public bool DeleteTemplate(int[] tempids)
        {
            List<TemplateDeleteParam> delParmas = new List<TemplateDeleteParam>();

            for(int i = 0; i < tempids.Length; i++)
            {
                delParmas.Add(new TemplateDeleteParamBuilder()
                    .SetDelTempID(tempids[i])
                    .Build());
            }
            
            return _dao.DeleteTemplate(delParmas);
        }
    }
}

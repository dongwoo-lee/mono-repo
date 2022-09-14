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
    public class ArchiveCueSheetBll
    {
        private readonly ICueSheetDAO _dao;

        public ArchiveCueSheetBll(ICueSheetDAO dao)
        {
            _dao = dao;
        }

        // 이전큐시트 목록 가져오기
        public ArchiveCueList_Page GetArchiveCueSheetList(List<string> products, string start_dt, string end_dt, int row_per_page, int select_page, List<string> tag)
        {
            var result = new ArchiveCueList_Page();
            ArchiveCueSheetListParam param = new ArchiveCueSheetListParamBuilder()
                .SetProductIDs(products)
                .SetStartDate(start_dt)
                .SetEndDate(end_dt)
                .SetRowPage(row_per_page)
                .SetSelectPage(select_page)
                .SetTags(tag)
                .Build();

            var data = _dao.GetArchiveCueSheetList(param);
            result.RowPerPage = row_per_page;
            result.SelectPage = select_page;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.ArchiveCueSheetListEntity?.Converting();
            return result;
        }

        // 이전큐시트 상세내용 가져오기 
        public CueSheetCollectionDTO GetArchiveCueSheet(int cueid)
        {
            ArchiveCueSheetInfoParam param = new ArchiveCueSheetInfoParamBuilder()
                .SetCueID(cueid)
                .Build();

            return _dao.GetArchiveCueSheet(param)?.ArchiveConverting();
        }

    }
}

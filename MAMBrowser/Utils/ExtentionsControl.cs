using M30_ManagementControlDAO.Entity;
using static MAMBrowser.DTO.ManagementSystemDTO;
using System.Collections.Generic;
using System;
using MAMBrowser.DTO;

namespace MAMBrowser.Utils
{
    public static class ExtentionsControl
    {
        public static List<TransMissionListItemDTO> Converting(this List<TransMissionEntity> entitys)
        {
            var result = new List<TransMissionListItemDTO>();
            foreach (var entity in entitys)
            {
                var item = new TransMissionListItemDTO();
                item.MAINMACHINE = entity.MAINMACHINE;
                item.SEQNUM = entity.SEQNUM;
                item.PRODUCTID = entity.PRODUCTID;
                item.SOURCEID = entity.SOURCEID;
                item.ONAIRTIME = entity.ONAIRTIME;
                item.DURATION = entity.DURATION;
                item.EVENTNAME = entity.EVENTNAME;
                item.DLFILEPATH = entity.DLFILEPATH;
                item.PGMFILEPATH = entity.PGMFILEPATH;
                item.CUEID = entity.CUEID;
                result.Add(item);
            }
            return result;
        }
    }
}

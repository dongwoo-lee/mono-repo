using System;
using System.Collections.Generic;

namespace MAMBrowser.DTO
{
    public class StudioSchedulerDTO
    {
        public List<StudioAssign> studioAssigns { get; set; }
        public List<SchedulerResources> schedulerResources { get; set; }

    }

    public class StudioAssign
    {
        public string TDNAME { get; set; }
        public string TDID { get; set; }
        public string GUBUN { get; set; }
        public DateTime STARTDATE { get; set; }
        public DateTime ENDDATE { get; set; }
        public string DESCRIPTION { get; set; }
    }

    public class SchedulerResources
    {
        public string ID { get; set; }
        public string COLOR { get; set; }
    }
}

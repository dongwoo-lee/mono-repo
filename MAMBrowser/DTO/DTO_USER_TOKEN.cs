using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_USER_TOKEN
    {
        public long UserSeq { get; set; }
        public string ID { get; set; }
        public string RoleID { get; set; }
        public string AuthorCD { get; set; }
        //public string AuthorName { get; set; }
        public string MenuGrpID { get; set; }
        public DTO_USER_TOKEN()
        {

        }
        public DTO_USER_TOKEN(DTO_USER_DETAIL userDetail)
        {
            this.ID = userDetail.ID;
            this.RoleID = userDetail.RoleID;
            this.AuthorCD = userDetail.AuthorCD;
            //this.AuthorName = userDetail.AuthorName;
            this.MenuGrpID = userDetail.MenuGrpID;
        }
    }
}

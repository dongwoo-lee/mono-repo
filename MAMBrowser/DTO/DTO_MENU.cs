using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_MENU
    {
        public string ParentID { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Visible { get; set; }
        public string Enable { get; set; }
        public List<DTO_MENU> Children { get; set; }
    }
}

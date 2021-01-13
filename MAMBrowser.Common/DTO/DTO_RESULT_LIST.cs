using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_RESULT_LIST<T>
    {
        public IList<T> Data { get; set; }
        public int TotalRowCount
        {
            get
            {
                if (Data == null)
                    return 0;

                return Data.Count();
            }
        }
    }
}

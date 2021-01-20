using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Common.Foundation
{
    public class SortNameAttribute : Attribute
    {
        public string Name { get; private set; }
        public SortNameAttribute(string name) :  base()
        {
            Name = name;
        }
    }
}

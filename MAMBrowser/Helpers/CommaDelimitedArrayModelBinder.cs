using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    [TypeConverter(typeof(LongListConverter))]
    public class LongList : List<long>
    {
        public LongList(List<string> collection) : base(Array.ConvertAll(collection.ToArray(), item => Convert.ToInt64(item)))
        {
        }
    }

    public class LongListConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                return null;

            if (value is string s)
            {
                if (string.IsNullOrEmpty(s))
                    return null;
                return new LongList(s.Split(',').ToList());
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}

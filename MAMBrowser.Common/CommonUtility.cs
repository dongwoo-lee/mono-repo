using MAMBrowser.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MAMBrowser.Common
{
    public class CommonUtility
    {
        public static string GetSortString(Type dtoType, string sortKey, string sortValue)
        {
            string sortFiled = "";
            string sortDirection = "";
            Dictionary<string, string> sortMap = new Dictionary<string, string>();
            foreach (var prop in dtoType.GetProperties())
            {
                var attrList = prop.GetCustomAttributes(true);
                foreach (var attr in attrList)
                {
                    if (attr.GetType() == typeof(SortNameAttribute))
                    {
                        sortMap.Add(prop.Name.ToUpper(), ((SortNameAttribute)attr).Name);
                        break;
                    }

                }
            }
            sortFiled = sortMap[sortKey.ToUpper()];
            if (string.IsNullOrEmpty(sortFiled))
                return "";

            switch (sortValue.ToUpper())
            {
                case "ASC":
                    sortDirection = "ASC";
                    break;
                case "DESC":
                    sortDirection = "DESC";
                    break;
                default:
                    sortDirection = "ASC";
                    break;
            }
            return $"ORDER BY {sortFiled} {sortDirection}";
        }
    }
}

using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class CategoriesBLL
    {
        
        public DTO_RESULT_LIST<DTO_CATEGORY> GetMedia()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'CH' ORDER BY NUM");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.DataList = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetReport()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'RP' ORDER BY CODENAME");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.DataList = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        //public DTO_RESULT_LIST<DTO_CATEGORY> GetPro()
        //{

        //}
        //public DTO_RESULT_LIST<DTO_CATEGORY> GetCM()
        //{

        //}
        //public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerPr()
        //{

        //}
        //public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerGeneral()
        //{

        //}
        //public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerTimetone()
        //{

        //}
        //public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerETC()
        //{

        //}

    }
}

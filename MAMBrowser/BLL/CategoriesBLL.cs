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
            var queryTemplate = builder.AddTemplate("SELECT * FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'CH' ORDER BY NUM");
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
            var queryTemplate = builder.AddTemplate("SELECT * FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'RP' ORDER BY CODENAME");
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
        public DTO_RESULT_LIST<DTO_CATEGORY> GetPro()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT * FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'PA' AND SUBCODEID='Y'");
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
        public DTO_RESULT_LIST<DTO_CATEGORY> GetCM()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODENAME, CODEID FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'PT'");
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
        public DTO_RESULT_LIST<DTO_CATEGORY> GetMcrSpot(string media)
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODENAME FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'SP' AND MEDIA = :MEDIA ORDER BY CODENAME");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.DataList = repository.Select(queryTemplate.RawSql, new { MEDIA=media }, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerPr(string media)
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'FP' AND MEDIA = :MEDIA");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.DataList = repository.Select(queryTemplate.RawSql, new { MEDIA=media}, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerGeneral()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'FF'");
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
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerTimetone()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("");
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
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerETC()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("");
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

    }
}

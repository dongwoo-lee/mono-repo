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

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
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

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
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

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
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

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
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

            returnData.Data = repository.Select(queryTemplate.RawSql, new { MEDIA=media }, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerPr()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'FP'");
            Repository <DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
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

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerTimetone()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'FT' ORDER BY NUM");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerETC()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'FE'");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetPgmCodes(string brd_dt)
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT CODEID,CODENAME FROM MEM_CATEGORY_VIEW
WHERE CODETYPE = 'UP'
AND SUBCODEID = (SELECT MAX(SUBCODEID) FROM MEM_CATEGORY_VIEW /**where**/)
ORDER BY NUM");
            builder.Where("SUBCODEID <= 'BRD_DT'");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, new {BRD_DT= brd_dt }, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetPublicPrimary()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT * FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'PC'");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetReqStatus()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'SE' ORDER BY NUM");
            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }


        public DTO_RESULT_LIST<DTO_CATEGORY> GetPublicSecond(string primaryCode)
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_CODE.CODE, M30_CODE.NAME NAME FROM M30_CODE_MAP
LEFT JOIN M30_CODE ON M30_CODE.CODE = M30_CODE_MAP.CODE /**where**/");
            builder.Where("(SYSTEM_CD = 'S01' AND MAP_CD = 'S00G01C005')");
            builder.Where("GRP_CD = :GRP_CD");
            DynamicParameters param = new DynamicParameters();
            param.Add("GRP_CD", primaryCode);

            Repository<DTO_CATEGORY> repository = new Repository<DTO_CATEGORY>();
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODE,
                    Name = row.NAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, param, resultMapping);
            return returnData;
        }

    }
}

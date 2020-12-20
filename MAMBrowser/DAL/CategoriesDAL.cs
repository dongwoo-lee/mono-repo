using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class CategoriesDAL
    {
        public DTO_RESULT_LIST<DTO_USER> GetUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PERSONNAME FROM MIROS_USER ORDER BY CONVERT(PERSONNAME, 'US8ICL'), PERSONNAME ASC");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_USER> GetPDUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME FROM MIROS_USER 
                                                      WHERE ROLE IN('R_PD_PD', 'R_PD_ADMIN') OR ROLE LIKE 'R_SYS_%'
                                                      ORDER BY CONVERT(PERSONNAME, 'US8ICL'), PERSONNAME ASC");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_USER> GetReportUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME FROM MIROS_USER 
                                                      WHERE ROLE IN('R_PD_PD', 'R_PD_ADMIN', 'R_PD_JOURNALIST') OR ROLE LIKE 'R_SYS_%'
                                                      ORDER BY CONVERT(PERSONNAME, 'US8ICL'), PERSONNAME ASC");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_USER> GetMDUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME FROM MIROS_USER 
                                                      WHERE ROLE IN('R_PD_ADMIN', 'R_MD_ADMIN', 'R_MD_MD') OR ROLE LIKE 'R_SYS_%'
                                                      ORDER BY CONVERT(PERSONNAME, 'US8ICL'), PERSONNAME ASC");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }

        

        public DTO_RESULT_LIST<DTO_CATEGORY> GetMedia()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'CH' ORDER BY NUM");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'RP' ORDER BY CODENAME");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'PA' AND SUBCODEID='Y'");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT CODENAME, CODEID FROM M30_VW_CATEGORY WHERE CODETYPE = 'PT'");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'SP' AND MEDIA = :MEDIA ORDER BY CODENAME");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'FP'");
            Repository  repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'FF'");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'FT' ORDER BY NUM");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'FE'");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate(@"SELECT CODEID,CODENAME FROM M30_VW_CATEGORY
WHERE CODETYPE = 'UP'
AND SUBCODEID = (SELECT MAX(SUBCODEID) FROM M30_VW_CATEGORY /**where**/)
ORDER BY NUM");
            builder.Where("SUBCODEID <= 'BRD_DT'");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'PC'");
            Repository repository = new Repository();
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
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'SE' ORDER BY NUM");
            Repository repository = new Repository();
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


        public DTO_RESULT_LIST<DTO_CATEGORY> GetPublicSecond(string primaryCode, string userId)
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.Add("GRP_CD", primaryCode);
            param.Add("USER_ID", userId);

            var queryTemplate = builder.AddTemplate(@"SELECT M30_CODE.CODE, M30_CODE.NAME NAME FROM M30_CODE_MAP
LEFT JOIN M30_CODE ON M30_CODE.CODE = M30_CODE_MAP.CODE /**where**/");
            builder.Where("(SYSTEM_CD = 'S01' AND MAP_CD = 'S00G01C005')");
            builder.Where("GRP_CD = :GRP_CD");
            if (!string.IsNullOrEmpty(userId))
            {
                builder.Where("M30_CODE.CODE IN (SELECT CODE FROM M30_CODE_MAP WHERE MAP_CD = 'S00G01C003' AND GRP_CD = :USER_ID)");
            }
           
            Repository repository = new Repository();
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



        public void InsertPublicCategory(M30_CODE model)
        {
            string query = @"INSERT INTO M30_CODE VALUES(CODE=:CODE, PARENT_CODE=:PARENT_CODE, NAME=:NAME)";
            Repository repository = new Repository();
            repository.Insert(query, model);
        }
        public void UpdatePublicCategory(M30_CODE model)
        {
            string query = @"UPDATE M30_CODE SET 
                             NAME=:NAME
                             WHERE CODE=:CODE)";

            Repository repository = new Repository();
            repository.Update(query, model);
        }
        public void DeletePublicCategory(string key)
        {
            string query = @"DELETE M30_CODE 
                           WHERE CODE=:CODE)";

            Repository repository = new Repository();
            repository.Delete(query, key);
        }
        public void InsertUserToPublicCategory()
        {
            string query = @"INSERT INTO M30_CODE VALUES(CODE=:CODE, PARENT_CODE=:PARENT_CODE, NAME=:NAME)";
            Repository repository = new Repository();
            //repository.Insert(query, model);
        }

    }
}

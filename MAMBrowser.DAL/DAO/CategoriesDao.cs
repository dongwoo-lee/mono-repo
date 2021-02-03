using Dapper;
using MAMBrowser.DTO;
using MAMBrowser.Models;
using System;

namespace MAMBrowser.DAL
{
    public class CategoriesDao
    {
        private readonly Repository _repository;
        public CategoriesDao(Repository repository)
        {
            _repository = repository;
        }
        public DTO_RESULT_LIST<DTO_USER> GetUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PERSONNAME FROM MIROS_USER ORDER BY CONVERT(PERSONNAME, 'US8ICL'), PERSONNAME ASC");
            
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_USER> GetPDUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME FROM MIROS_USER 
                                                      WHERE ROLE IN('R_PD_PD', 'R_PD_ADMIN') OR ROLE LIKE 'R_SYS_%'
                                                      ORDER BY CONVERT(PERSONNAME, 'US8ICL'), PERSONNAME ASC");
            
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_USER> GetReportUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME FROM MIROS_USER 
                                                      WHERE ROLE IN('R_PD_PD', 'R_PD_ADMIN', 'R_PD_JOURNALIST') OR ROLE LIKE 'R_SYS_%'
                                                      ORDER BY CONVERT(PERSONNAME, 'US8ICL'), PERSONNAME ASC");
            
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_USER> GetMDUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME FROM MIROS_USER 
                                                      WHERE ROLE IN('R_PD_ADMIN', 'R_MD_ADMIN', 'R_MD_MD') OR ROLE LIKE 'R_SYS_%'
                                                      ORDER BY CONVERT(PERSONNAME, 'US8ICL'), PERSONNAME ASC");
            
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }

        

        public DTO_RESULT_LIST<DTO_CATEGORY> GetMedia()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'CH' ORDER BY NUM");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetReport()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'RP' ORDER BY CODENAME");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetPro()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'PA' AND SUBCODEID='Y' ORDER BY CODENAME ASC");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetCM()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODENAME, CODEID FROM M30_VW_CATEGORY WHERE CODETYPE = 'PT'");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetMcrSpot(string media)
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'SP' AND MEDIA = :MEDIA ORDER BY CODENAME");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, new { MEDIA=media }, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerPr()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'FP'");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerGeneral()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'FF'");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerTimetone()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'FT' ORDER BY NUM");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetFillerETC()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'FE'");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
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
            builder.Where("SUBCODEID <= :BRD_DT");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, new {BRD_DT= brd_dt }, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetPublicPrimary()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'PC'");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetReqStatus()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT CODEID, CODENAME FROM M30_VW_CATEGORY WHERE CODETYPE = 'SE' ORDER BY NUM");
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODEID,
                    Name = row.CODENAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }


        public DTO_RESULT_LIST<DTO_CATEGORY> GetPublicSecond(string primaryCode, string userId)
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.Add("GRP_CD", primaryCode);
            param.Add("USER_ID", userId);

            var queryTemplate = builder.AddTemplate(@"SELECT M30_COMM_CODE.CODE, M30_COMM_CODE.NAME NAME FROM M30_COMM_CODE_MAP
LEFT JOIN M30_COMM_CODE ON M30_COMM_CODE.CODE = M30_COMM_CODE_MAP.CODE /**where**/ ORDER BY NAME ASC");
            builder.Where("(SYSTEM_CD = 'S01' AND MAP_CD = 'S00G01C005')");
            builder.Where("GRP_CD = :GRP_CD");
            //if (!string.IsNullOrEmpty(userId))
            //{
            //    builder.Where("M30_COMM_CODE.CODE IN (SELECT CODE FROM M30_COMM_CODE_MAP WHERE MAP_CD = 'S00G01C003' AND GRP_CD = :USER_ID)");
            //}
           
            
            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = row.CODE,
                    Name = row.NAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, param, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_CATEGORY> GetDLDeviceList()
        {
            DTO_RESULT_LIST<DTO_CATEGORY> returnData = new DTO_RESULT_LIST<DTO_CATEGORY>();
            var builder = new SqlBuilder();

            var queryTemplate = builder.AddTemplate(@"SELECT SEQ, DEVICE_NAME FROM M30_DL_DEVICE");

            var resultMapping = new Func<dynamic, DTO_CATEGORY>((row) =>
            {
                return new DTO_CATEGORY
                {
                    ID = Convert.ToInt64(row.SEQ).ToString(),
                    Name = row.DEVICE_NAME
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }

        
        public void InsertPublicCategory(M30_COMM_CODE model)
        {
            string query = @"INSERT INTO M30_COMM_CODE VALUES(CODE=:CODE, PARENT_CODE=:PARENT_CODE, NAME=:NAME)";
            
            _repository.Insert(query, model);
        }
        public void UpdatePublicCategory(M30_COMM_CODE model)
        {
            string query = @"UPDATE M30_COMM_CODE SET 
                             NAME=:NAME
                             WHERE CODE=:CODE)";

            
            _repository.Update(query, model);
        }
        public void DeletePublicCategory(string key)
        {
            string query = @"DELETE M30_COMM_CODE 
                           WHERE CODE=:CODE)";

            
            _repository.Delete(query, key);
        }
        public void InsertUserToPublicCategory()
        {
            string query = @"INSERT INTO M30_COMM_CODE VALUES(CODE=:CODE, PARENT_CODE=:PARENT_CODE, NAME=:NAME)";
            
            //_repository.Insert(query, model);
        }

    }
}

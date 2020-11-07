using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAMBrowser.Controllers
{
    public class APIDAL 
    {
        
        public DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL> GetUserDetailList()
        {
            DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL> returnData = new DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME, MIROS_USER.ROLE, ROLE_NAME, AUTHOR_CD, A.NAME AS AUTHOR_NAME, USER_EXT_ID, DISK_MAX, DISK_USED, ((DISK_MAX*1000000000)-DISK_USED) AS DISK_AVLB, MENU_GRP_CD, B.NAME AS MENU_GRP_NAME, USED 
FROM MIROS_USER
INNER JOIN MIROS_ROLE ON MIROS_ROLE.ROLE=MIROS_USER.ROLE
INNER JOIN M30_USER_EXT ON MIROS_USER.PERSONID = M30_USER_EXT.USER_ID 
INNER JOIN M30_ROLE_EXT ON MIROS_USER.ROLE = M30_ROLE_EXT.ROLE_ID
INNER JOIN M30_CODE A ON A.CODE = M30_ROLE_EXT.AUTHOR_CD
LEFT JOIN M30_CODE B ON B.CODE = M30_USER_EXT.MENU_GRP_CD");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_USER_DETAIL>((row) =>
            {
                return new DTO_USER_DETAIL
                {
                    UserExtID = Convert.ToInt64(row.USER_EXT_ID),
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                    RoleID = row.ROLE,
                    RoleName = row.ROLE_NAME,
                    AuthorCD = row.AUTHOR_CD,
                    AuthorName = row.AUTHOR_NAME,
                    DiskMax = Convert.ToInt32(row.DISK_MAX),
                    DiskUsed = Convert.ToInt64(row.DISK_USED),
                    DiskAvailable = Convert.ToInt64(row.DISK_AVLB),
                    MenuGrpID = row.MENU_GRP_CD,
                    MenuGrpName = row.MENU_GRP_NAME,
                    Used = row.USED,
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }
        public int UpdateUserDetail(List<UserExtModel> updateDtoList)
        {
            var builder = new SqlBuilder();
            //USER_EXT_ID로 업데이트 되게끔... 향후수정.
            var queryTemplate = builder.AddTemplate("UPDATE M30_USER_EXT SET DISK_MAX=:DISK_MAX, DISK_USED=:DISK_USED, MENU_GRP_CD=:MENU_GRP_CD,USED=:USED WHERE USER_ID=:USER_ID");
            builder.AddParameters(updateDtoList);
            Repository repository = new Repository();
            //var paramMap = updateDtoList.Select((entity) =>
            //{
            //    return new
            //    {
            //        USER_ID = entity.USER_ID,
            //        DISK_MAX = entity.DISK_MAX,
            //        DISK_USED = entity.DISK_USED,

            //        USED = entity.USED
            //    };
            //});

            return repository.Update(queryTemplate.RawSql, updateDtoList);
        }
        public DTO_USER_DETAIL GetUserSummary(string id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME, MIROS_USER.ROLE, ROLE_NAME, AUTHOR_CD, A.NAME AS AUTHOR_NAME, USER_EXT_ID, DISK_MAX, DISK_USED, ((DISK_MAX*1000000000)-DISK_USED) AS DISK_AVLB, MENU_GRP_CD, B.NAME AS MENU_GRP_NAME, USED 
FROM MIROS_USER
INNER JOIN MIROS_ROLE ON MIROS_ROLE.ROLE=MIROS_USER.ROLE
INNER JOIN M30_USER_EXT ON MIROS_USER.PERSONID = M30_USER_EXT.USER_ID 
INNER JOIN M30_ROLE_EXT ON MIROS_USER.ROLE = M30_ROLE_EXT.ROLE_ID
INNER JOIN M30_CODE A ON A.CODE = M30_ROLE_EXT.AUTHOR_CD
LEFT JOIN M30_CODE B ON B.CODE = M30_USER_EXT.MENU_GRP_CD
/**where**/");
            builder.Where("PERSONID=:PERSONID");
            DynamicParameters param = new DynamicParameters();
            param.Add("PERSONID", id);

            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_USER_DETAIL>((row) =>
            {
                return new DTO_USER_DETAIL
                {
                    UserExtID = Convert.ToInt64(row.USER_EXT_ID),
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                    RoleID = row.ROLE,
                    RoleName = row.ROLE_NAME,
                    AuthorCD = row.AUTHOR_CD,
                    AuthorName = row.AUTHOR_NAME,
                    DiskMax = Convert.ToInt32(row.DISK_MAX),
                    DiskUsed = Convert.ToInt64(row.DISK_USED),
                    DiskAvailable = Convert.ToInt64(row.DISK_AVLB),
                    MenuGrpID = row.MENU_GRP_CD,
                    MenuGrpName = row.MENU_GRP_NAME,
                    Used = row.USED,
                };
            });

            return repository.Get(queryTemplate.RawSql, param, resultMapping);
        }
        public List<DTO_MENU> GetMenu(string id)
        {
            List<DTO_MENU> returnData = new List<DTO_MENU>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_CODE.PARENT_CODE, M30_CODE.CODE, M30_CODE.NAME, VISIBLE, ENABLE FROM M30_USER_EXT
INNER JOIN M30_MENU_MAP ON M30_MENU_MAP.GRP_CD=M30_USER_EXT.MENU_GRP_CD
INNER JOIN M30_CODE ON M30_CODE.CODE = M30_MENU_MAP.CODE /**where**/");
            DynamicParameters param = new DynamicParameters();
            param.Add("USER_ID", id);
            builder.Where("USER_ID = :USER_ID");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_MENU>((row) =>
            {
                return new DTO_MENU
                {
                    ParentID = row.PARENT_CODE,
                    ID = row.CODE,
                    Name = row.NAME,
                    Visible = row.VISIBLE,
                    Enable = row.ENABLE,
                };
            });

            return repository.Select(queryTemplate.RawSql, param, resultMapping).ToList();
        }
        public List<DTO_MENU> GetMenuByGrpId(string grpId)
        {
            List<DTO_MENU> returnData = new List<DTO_MENU>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_MENU_MAP.*, M30_CODE.NAME FROM M30_MENU_MAP
LEFT JOIN M30_CODE ON M30_CODE.CODE= M30_MENU_MAP.CODE /**where**/");
            DynamicParameters param = new DynamicParameters();
            builder.Where("MAP_CD='S00G01C001'");
            param.Add("GRP_CD", grpId);
            builder.Where("GRP_CD = :GRP_CD");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_MENU>((row) =>
            {
                return new DTO_MENU
                {
                    ParentID = row.PARENT_CODE,
                    ID = row.CODE,
                    Name = row.NAME,
                    Visible = row.VISIBLE,
                    Enable = row.ENABLE,
                };
            });

            return repository.Select(queryTemplate.RawSql, param, resultMapping).ToList();
        }
        public List<DTO_MENU> GetBehavior(string authorCd)
        {
            List<DTO_MENU> returnData = new List<DTO_MENU>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_CODE.PARENT_CODE, M30_CODE.CODE, M30_CODE.NAME, VISIBLE, ENABLE FROM (SELECT * FROM M30_MENU_MAP
/**where**/) A
LEFT JOIN M30_CODE ON M30_CODE.CODE = A.CODE");
            builder.Where("(MAP_CD='S00G01C002' AND GRP_CD=:AUTHOR_CD)");
            DynamicParameters param = new DynamicParameters();
            param.Add("AUTHOR_CD", authorCd);

            Repository  repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_MENU>((row) =>
            {
                return new DTO_MENU
                {
                    ParentID = row.PARENT_CODE,
                    ID = row.CODE,
                    Name = row.NAME,
                    Visible = row.VISIBLE,
                    Enable = row.ENABLE,
                };
            });

            return repository.Select(queryTemplate.RawSql, param, resultMapping).ToList();
        }
        public DTO_RESULT_LIST<DTO_COMMON_CODE> GetAuthorList()
        {
            DTO_RESULT_LIST<DTO_COMMON_CODE> returnData = new DTO_RESULT_LIST<DTO_COMMON_CODE>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_CODE WHERE PARENT_CODE='S01G03'");

            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_COMMON_CODE>((row) =>
            {
                return new DTO_COMMON_CODE
                {
                    Code = row.CODE,
                    ParentCode = row.PARENT_CODE,
                    Name = row.NAME,
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }  
        public DTO_RESULT_LIST<DTO_COMMON_CODE> GetMenuGrpList()
        {
            DTO_RESULT_LIST<DTO_COMMON_CODE> returnData = new DTO_RESULT_LIST<DTO_COMMON_CODE>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_CODE WHERE PARENT_CODE='S01G04'");

            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_COMMON_CODE>((row) =>
            {
                return new DTO_COMMON_CODE
                {
                    Code = row.CODE,
                    ParentCode = row.PARENT_CODE,
                    Name = row.NAME,
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public bool ExistUser(AuthenticateModel user)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PERSONNAME FROM MIROS_USER /**where**/");
            builder.Where("PERSONID=:PERSONID");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                };
            });

            var result = repository.Select(queryTemplate.RawSql, user, resultMapping);
            if (result.Count() > 0)
                return true;
            else
                return false;

        }
        public DTO_USER_TOKEN Authenticate(AuthenticateModel user)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PASSWD FROM MIROS_USER /**where**/");
            builder.Where("PERSONID=:PERSONID AND PASSWD=:PASSWD");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                };
            });

            var result = repository.Select(queryTemplate.RawSql, user, resultMapping);
            if (result.Count() > 0)
                return GetToken(user.PERSONID);
            else
                return null;
        }
        private DTO_USER_TOKEN GetToken(string id)
        {
            var userDetail = GetUserSummary(id);
            DTO_USER_TOKEN token = new DTO_USER_TOKEN(userDetail);
            return token;
        }
        public DTO_RESULT_LIST<DTO_ROLE_DETAIL> GetRoleDetailList()
        {
            DTO_RESULT_LIST<DTO_ROLE_DETAIL> returnData = new DTO_RESULT_LIST<DTO_ROLE_DETAIL>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT ROLE_ID, ROLE_NAME, AUTHOR_CD, AUTHOR.NAME AS AUTHOR_NAME FROM MIROS_ROLE
LEFT JOIN M30_ROLE_EXT ON M30_ROLE_EXT.ROLE_ID = MIROS_ROLE.ROLE
LEFT JOIN(SELECT * FROM M30_CODE WHERE PARENT_CODE = 'S01G03') AUTHOR ON AUTHOR.CODE = M30_ROLE_EXT.AUTHOR_CD");
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_ROLE_DETAIL>((row) =>
            {
                return new DTO_ROLE_DETAIL
                {
                    ID = row.ROLE_ID,
                    Name = row.ROLE_NAME,
                    AuthorCode = row.AUTHOR_CD,
                    AuthorName = row.AUTHOR_NAME
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }



        public void AddLog(string logLevel, string clientIp, string userId, string userName, string description, string note)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("INSERT INTO FROM M30_LOG VALUES(:SYSTEM_CD, :LOG_LEVEL, :CLIENT_IP, :USER_ID, :USER_NAME, :DESCRIPTION, :NOTE, SYSDATE)");
            DynamicParameters param = new DynamicParameters();
            param.Add("SYSTEM_CD", "S01");
            param.Add("LOG_LEVEL", logLevel);
            param.Add("CLIENT_IP", clientIp);
            param.Add("USER_ID", userId);
            param.Add("USER_NAME", userName);
            param.Add("DESCRIPTION", description);
            param.Add("NOTE", note);
            
            Repository repository = new Repository();
            repository.Insert(queryTemplate.RawSql, param);
        }
        public DTO_RESULT_LIST<DTO_LOG> SearchLog(string start_dt, string end_dt, string logLevel, string userName, string description)
        {
            DTO_RESULT_LIST<DTO_LOG> returnData = new DTO_RESULT_LIST<DTO_LOG>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT * FROM M30_LOG /**where**/");
            DynamicParameters param = new DynamicParameters();
            param.Add("START_DT", start_dt);
            param.Add("END_DT", end_dt);
            param.Add("LOG_LEVEL", logLevel);
            param.Add("USER_NAME", userName);
            param.Add("DESCRIPTION", description);
            builder.Where("(TO_DATE(:START_DT,'YYYYMMDD') <= REG_DTM AND REG_DTM < TO_DATE(:END_DT,'YYYYMMDD')+1)");
            if (!string.IsNullOrEmpty(logLevel))
            {
                builder.Where("LOG_LEVEL=:LOG_LEVEL");
            }
            if (!string.IsNullOrEmpty(userName))
            {
                string[] wordArray = userName.Split(' ');
                foreach (var word in wordArray)
                {
                    builder.Where($"LOWER(USER_NAME) LIKE LOWER('%{word}%')");
                }
            }
            if (!string.IsNullOrEmpty(description))
            {
                string[] wordArray = description.Split(' ');
                foreach (var word in wordArray)
                {
                    builder.Where($"LOWER(DESCRIPTION) LIKE LOWER('%{word}%')");
                }
            }
            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_LOG>((row) =>
            {
                return new DTO_LOG
                {
                    Seq = Convert.ToInt64(row.SEQ),
                    SystemCode= row.SYSTEM_CD,
                    LogLevel = row.LOG_LEVEL,
                    ClientIp = row.CLIENT_IP,
                    UserID = row.USER_ID,
                    UserName = row.USER_NAME,
                    Description = row.DESCRIPTION,
                    Note = row.NOTE,
                    RegDtm = ((DateTime)row.REG_DTM).ToString(MAMUtility.DTM19),
                };
            });
            returnData.Data = repository.Select(queryTemplate.RawSql, param, resultMapping);
            return returnData;
        }
        public int UpdateRole(List<RoleExtModel> updateDtoList)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("UPDATE M30_ROLE_EXT SET AUTHOR_CD=:AUTHOR_CD /**where**/");
            builder.AddParameters(updateDtoList);
            builder.Where("ROLE_ID=:ROLE_ID");
            Repository repository = new Repository();
            var paramMap = updateDtoList.Select((entity) =>
            {
                return new
                {
                    ROLE_ID = entity.ROLE_ID,
                    AUTHOR_CD = entity.AUTHOR_CD,
                };
            });

            return repository.Update(queryTemplate.RawSql, paramMap);
        }

        //public int Getconfig()
        //{
        //    DTO_RESULT result = new DTO_RESULT();
        //    return result;
        //}
        //public int UpdateConfig()
        //{
        //    DTO_RESULT result = new DTO_RESULT();
        //    return result;
        //}
    }
}

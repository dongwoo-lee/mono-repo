using Dapper;
using MAMBrowser.Common;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAMBrowser.DAL
{
    public class APIDao 
    {
        private readonly Repository _repository;
        public APIDao(Repository repository)
        {
            _repository = repository;
        }

        public DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL> GetUserDetailList()
        {
            DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL> returnData = new DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(
                @"SELECT A.*, ((DISK_MAX*1000000000)-DISK_USED) AS DISK_AVLB FROM 
                (SELECT  A.*, NVL(C.DISK_USED, 0) AS DISK_USED FROM 
                    (SELECT PERSONID, PERSONNAME, MIROS_USER.ROLE, ROLE_NAME, AUTHOR_CD, A.NAME AS AUTHOR_NAME, USER_EXT_ID, DISK_MAX, 
                    MENU_GRP_CD, B.NAME AS MENU_GRP_NAME, USED 
                    FROM MIROS_USER
                    INNER JOIN MIROS_ROLE ON MIROS_ROLE.ROLE=MIROS_USER.ROLE
                    INNER JOIN M30_COMM_USER_EXT ON MIROS_USER.PERSONID = M30_COMM_USER_EXT.USER_ID 
                    INNER JOIN M30_COMM_ROLE_EXT ON MIROS_USER.ROLE = M30_COMM_ROLE_EXT.ROLE_ID
                    INNER JOIN M30_COMM_CODE A ON A.CODE = M30_COMM_ROLE_EXT.AUTHOR_CD
                    LEFT JOIN M30_COMM_CODE B ON B.CODE = M30_COMM_USER_EXT.MENU_GRP_CD) A
                    LEFT JOIN (SELECT USER_ID, SUM(FILE_SIZE) AS DISK_USED FROM M30_MAM_PRIVATE_SPACE GROUP BY USER_ID) C ON A.PERSONID=C.USER_ID) A");
            
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

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }
        public int UpdateUserDetail(List<M30_COMM_USER_EXT> updateDtoList)
        {
            var builder = new SqlBuilder();
            // disk_used필드는 사용자설정에서 변경되지 않으니 업데이트 하지 않는다.

            var queryTemplate = builder.AddTemplate("UPDATE M30_COMM_USER_EXT SET DISK_MAX=:DISK_MAX, MENU_GRP_CD=:MENU_GRP_CD,USED=:USED WHERE USER_ID=:USER_ID");
            builder.AddParameters(updateDtoList);
            
            return _repository.Update(queryTemplate.RawSql, updateDtoList);
        }

        public DTO_USER_DETAIL GetUserSummary(string id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(
                @"SELECT A.*, ((DISK_MAX*1000000000)-DISK_USED) AS DISK_AVLB FROM 
                (SELECT A.*, NVL(C.DISK_USED, 0) AS DISK_USED FROM 
                (SELECT PERSONID, PERSONNAME, MIROS_USER.ROLE, ROLE_NAME, AUTHOR_CD, A.NAME AS AUTHOR_NAME, USER_EXT_ID, DISK_MAX, 
                MENU_GRP_CD, B.NAME AS MENU_GRP_NAME, USED 
                FROM MIROS_USER
                INNER JOIN MIROS_ROLE ON MIROS_ROLE.ROLE=MIROS_USER.ROLE
                INNER JOIN M30_COMM_USER_EXT ON MIROS_USER.PERSONID = M30_COMM_USER_EXT.USER_ID 
                INNER JOIN M30_COMM_ROLE_EXT ON MIROS_USER.ROLE = M30_COMM_ROLE_EXT.ROLE_ID
                INNER JOIN M30_COMM_CODE A ON A.CODE = M30_COMM_ROLE_EXT.AUTHOR_CD
                LEFT JOIN M30_COMM_CODE B ON B.CODE = M30_COMM_USER_EXT.MENU_GRP_CD
                WHERE PERSONID=:PERSONID) A
                LEFT JOIN (SELECT USER_ID, SUM(FILE_SIZE) AS DISK_USED FROM M30_MAM_PRIVATE_SPACE WHERE USER_ID=:PERSONID GROUP BY USER_ID) C ON A.PERSONID=C.USER_ID) A");
            DynamicParameters param = new DynamicParameters();
            param.Add("PERSONID", id);

            
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

            return _repository.Get(queryTemplate.RawSql, param, resultMapping);
        }
        public List<DTO_MENU> GetMenu(string id)
        {
            List<DTO_MENU> returnData = new List<DTO_MENU>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_COMM_CODE.PARENT_CODE, M30_COMM_CODE.CODE, M30_COMM_CODE.NAME, VISIBLE, ENABLE FROM M30_COMM_USER_EXT
INNER JOIN M30_COMM_MENU_MAP ON M30_COMM_MENU_MAP.GRP_CD=M30_COMM_USER_EXT.MENU_GRP_CD
INNER JOIN M30_COMM_CODE ON M30_COMM_CODE.CODE = M30_COMM_MENU_MAP.CODE /**where**/");
            DynamicParameters param = new DynamicParameters();
            param.Add("USER_ID", id);
            builder.Where("USER_ID = :USER_ID");
            
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

            return _repository.Select(queryTemplate.RawSql, param, resultMapping).ToList();
        }
        public List<DTO_MENU> GetMenuByGrpId(string grpId)
        {
            List<DTO_MENU> returnData = new List<DTO_MENU>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_COMM_MENU_MAP.*, M30_COMM_CODE.NAME, M30_COMM_CODE.PARENT_CODE FROM M30_COMM_MENU_MAP
LEFT JOIN M30_COMM_CODE ON M30_COMM_CODE.CODE= M30_COMM_MENU_MAP.CODE /**where**/");
            DynamicParameters param = new DynamicParameters();
            builder.Where("MAP_CD='S00G01C001'");
            param.Add("GRP_CD", grpId);
            builder.Where("GRP_CD = :GRP_CD");
            
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

            return _repository.Select(queryTemplate.RawSql, param, resultMapping).ToList();
        }
        public List<DTO_MENU> GetBehavior(string authorCd)
        {
            List<DTO_MENU> returnData = new List<DTO_MENU>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_COMM_CODE.PARENT_CODE, M30_COMM_CODE.CODE, M30_COMM_CODE.NAME, VISIBLE, ENABLE FROM (SELECT * FROM M30_COMM_MENU_MAP
/**where**/) A
LEFT JOIN M30_COMM_CODE ON M30_COMM_CODE.CODE = A.CODE");
            builder.Where("(MAP_CD='S00G01C002' AND GRP_CD=:AUTHOR_CD)");
            DynamicParameters param = new DynamicParameters();
            param.Add("AUTHOR_CD", authorCd);

            
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

            return _repository.Select(queryTemplate.RawSql, param, resultMapping).ToList();
        }
        public DTO_RESULT_LIST<DTO_COMMON_CODE> GetAuthorList()
        {
            DTO_RESULT_LIST<DTO_COMMON_CODE> returnData = new DTO_RESULT_LIST<DTO_COMMON_CODE>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_COMM_CODE WHERE PARENT_CODE='S01G03'");

            
            var resultMapping = new Func<dynamic, DTO_COMMON_CODE>((row) =>
            {
                return new DTO_COMMON_CODE
                {
                    Code = row.CODE,
                    ParentCode = row.PARENT_CODE,
                    Name = row.NAME,
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }  
        public DTO_RESULT_LIST<DTO_COMMON_CODE> GetMenuGrpList()
        {
            DTO_RESULT_LIST<DTO_COMMON_CODE> returnData = new DTO_RESULT_LIST<DTO_COMMON_CODE>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_COMM_CODE WHERE PARENT_CODE='S01G04'");

            
            var resultMapping = new Func<dynamic, DTO_COMMON_CODE>((row) =>
            {
                return new DTO_COMMON_CODE
                {
                    Code = row.CODE,
                    ParentCode = row.PARENT_CODE,
                    Name = row.NAME,
                };
            });

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }
        public bool ExistUser(AuthenticateModel user)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PERSONNAME FROM MIROS_USER /**where**/");
            builder.Where("PERSONID=:PERSONID");
            
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                };
            });

            var result = _repository.Select(queryTemplate.RawSql, user, resultMapping);
            if (result.Count() > 0)
                return true;
            else
                return false;

        }
        public bool Authenticate(AuthenticateModel user)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PASSWD FROM MIROS_USER /**where**/");
            builder.Where("PERSONID=:PERSONID AND PASSWD=:PASSWD");
            
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                };
            });

            var result = _repository.Select(queryTemplate.RawSql, user, resultMapping);
            if (result.Count() > 0)
                return true;
            else
                return false;
            //if (result.Count() > 0)
            //    return GetToken(user.PERSONID);
            //else
            //    return null;
        }

        public DTO_RESULT_LIST<DTO_ROLE_DETAIL> GetRoleDetailList()
        {
            DTO_RESULT_LIST<DTO_ROLE_DETAIL> returnData = new DTO_RESULT_LIST<DTO_ROLE_DETAIL>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT ROLE_ID, ROLE_NAME, AUTHOR_CD, AUTHOR.NAME AS AUTHOR_NAME FROM MIROS_ROLE
LEFT JOIN M30_COMM_ROLE_EXT ON M30_COMM_ROLE_EXT.ROLE_ID = MIROS_ROLE.ROLE
LEFT JOIN(SELECT * FROM M30_COMM_CODE WHERE PARENT_CODE = 'S01G03') AUTHOR ON AUTHOR.CODE = M30_COMM_ROLE_EXT.AUTHOR_CD");
            
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

            returnData.Data = _repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }

        public int UpdateRole(List<M30_COMM_ROLE_EXT> updateDtoList)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("UPDATE M30_COMM_ROLE_EXT SET AUTHOR_CD=:AUTHOR_CD /**where**/");
            builder.Where("ROLE_ID=:ROLE_ID");
            
            var paramMap = updateDtoList.Select((entity) =>
            {
                return new
                {
                    ROLE_ID = entity.ROLE_ID,
                    AUTHOR_CD = entity.AUTHOR_CD,
                };
            });

            return _repository.Update(queryTemplate.RawSql, paramMap);
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

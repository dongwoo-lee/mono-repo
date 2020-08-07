using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAMBrowser.Controllers
{
    public class APIBLL 
    {
        public DTO_RESULT_LIST<DTO_USER> GetUserList()
        {
            DTO_RESULT_LIST<DTO_USER> returnData = new DTO_RESULT_LIST<DTO_USER>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PERSONNAME FROM MIROS_USER");
            Repository<DTO_USER> repository = new Repository<DTO_USER>();
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
        public DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL> GetUserDetailList()
        {
            DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL> returnData = new DTO_RESULT_PAGE_LIST<DTO_USER_DETAIL>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME, MIROS_USER.ROLE, ROLE_NAME, AUTHOR_CD, A.NAME AS AUTHOR_NAME, DISK_MAX, DISK_USED, (DISK_MAX-DISK_USED) AS DISK_AVLB, MENU_GRP_ID, B.NAME AS MENU_GRP_NAME, USED 
FROM MIROS_USER
INNER JOIN MIROS_ROLE ON MIROS_ROLE.ROLE=MIROS_USER.ROLE
INNER JOIN M30_USER_EXT ON MIROS_USER.PERSONID = M30_USER_EXT.USER_ID 
INNER JOIN M30_ROLE_EXT ON MIROS_USER.ROLE = M30_ROLE_EXT.ROLE_ID
INNER JOIN M30_CODE A ON A.CODE = M30_ROLE_EXT.AUTHOR_CD
LEFT JOIN M30_CODE B ON B.CODE = M30_USER_EXT.MENU_GRP_ID");
            Repository<DTO_USER_DETAIL> repository = new Repository<DTO_USER_DETAIL>();
            var resultMapping = new Func<dynamic, DTO_USER_DETAIL>((row) =>
            {
                return new DTO_USER_DETAIL
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                    RoleID = row.ROLE,
                    RoleName = row.ROLE_NAME,
                    AuthorCD = row.AUTHOR_CD,
                    AuthorName = row.AUTHOR_NAME,
                    DiskMax = Convert.ToInt32(row.DISK_MAX),
                    DiskUsed = Convert.ToInt32(row.DISK_USED),
                    DiskAvailable = Convert.ToInt32(row.DISK_AVLB),
                    MenuGrpID = row.MENU_GRP_ID,
                    MenuGrpName = row.MENU_GRP_NAME,
                    Used = row.USED,
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }
        public int UpdateUserDetail(List<DTO_USER_DETAIL> updateDtoList)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("UPDATE M30_USER_EXT SET DISK_MAX=:DISK_MAX, DISK_USED=:DISK_USED, USED=:USED WHERE USER_ID=:USER_ID");
            builder.AddParameters(updateDtoList);
            Repository<DTO_USER_DETAIL> repository = new Repository<DTO_USER_DETAIL>();
            var paramMap = updateDtoList.Select((entity) =>
            {
                return new
                {
                    USER_ID = entity.ID,
                    DISK_MAX = entity.DiskMax,
                    DISK_USED = entity.DiskUsed,
                    USED = entity.Used
                };
            });

            return repository.Update(queryTemplate.RawSql, paramMap);
        }
        public DTO_USER_DETAIL GetUserDetail(string id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT PERSONID, PERSONNAME, MIROS_USER.ROLE, ROLE_NAME, AUTHOR_CD, M30_CODE.NAME AS AUTHOR_NAME, DISK_MAX, DISK_USED, (DISK_MAX-DISK_USED) AS DISK_AVLB, MENU_GRP_CD, B.NAME AS MENU_GRP_NAME, USED 
FROM MIROS_USER 
INNER JOIN MIROS_ROLE ON MIROS_ROLE.ROLE=MIROS_USER.ROLE
INNER JOIN M30_USER_EXT ON MIROS_USER.PERSONID = M30_USER_EXT.USER_ID 
INNER JOIN M30_ROLE_EXT ON MIROS_USER.ROLE = M30_ROLE_EXT.ROLE_ID
INNER JOIN M30_CODE ON M30_CODE.CODE = M30_ROLE_EXT.AUTHOR_CD
LEFT JOIN M30_CODE B ON B.CODE = M30_USER_EXT.MENU_GRP_CD
WHERE PERSONID='"+id +"'");
            Repository<DTO_USER_DETAIL> repository = new Repository<DTO_USER_DETAIL>();
            var resultMapping = new Func<dynamic, DTO_USER_DETAIL>((row) =>
            {
                return new DTO_USER_DETAIL
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                    RoleID = row.ROLE,
                    RoleName = row.ROLE_NAME,
                    AuthorCD = row.AUTHOR_CD,
                    AuthorName = row.AUTHOR_NAME,
                    DiskMax = Convert.ToInt32(row.DISK_MAX),
                    DiskUsed = Convert.ToInt32(row.DISK_USED),
                    DiskAvailable = Convert.ToInt32(row.DISK_AVLB),
                    MenuGrpID = row.MENU_GRP_CD,
                    MenuGrpName = row.MENU_GRP_NAME,
                    Used = row.USED,
                };
            });

            return repository.Get(queryTemplate.RawSql, new {PERSONID=id}, resultMapping);
        }

        public DTO_RESULT_LIST<DTO_MENU> GetMenu(string id)
        {
            DTO_RESULT_LIST<DTO_MENU> returnData = new DTO_RESULT_LIST<DTO_MENU>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_CODE.CODE, M30_CODE.NAME, VISIBLE, ENABLE FROM M30_USER_EXT
INNER JOIN M30_MENU_MAP ON M30_MENU_MAP.GRP_CD=M30_USER_EXT.MENU_GRP_CD
INNER JOIN M30_CODE ON M30_CODE.CODE = M30_MENU_MAP.CODE /**where**/");
            DynamicParameters param = new DynamicParameters();
            param.Add("USER_ID", id);
            builder.Where("USER_ID = :USER_ID");
            Repository<DTO_MENU> repository = new Repository<DTO_MENU>();
            var resultMapping = new Func<dynamic, DTO_MENU>((row) =>
            {
                return new DTO_MENU
                {
                    ID = row.CODE,
                    Name = row.NAME,
                    Visible = row.VISIBLE,
                    Enable = row.ENABLE,
                };
            });

           returnData.Data = repository.Select(queryTemplate.RawSql, param, resultMapping);
           return returnData;
        }
        public DTO_RESULT_LIST<DTO_MENU> GetBehavior(string authorCd)
        {
            DTO_RESULT_LIST<DTO_MENU> returnData = new DTO_RESULT_LIST<DTO_MENU>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_CODE.CODE, M30_CODE.NAME, VISIBLE, ENABLE FROM (SELECT * FROM M30_MENU_MAP
/**where**/) A
LEFT JOIN M30_CODE ON M30_CODE.CODE = A.CODE");
            builder.Where("(MAP_CD='S00G01C002' AND GRP_CD=:AUTHOR_CD)");
            DynamicParameters param = new DynamicParameters();
            param.Add("AUTHOR_CD", authorCd);

            Repository <DTO_MENU> repository = new Repository<DTO_MENU>();
            var resultMapping = new Func<dynamic, DTO_MENU>((row) =>
            {
                return new DTO_MENU
                {
                    ID = row.CODE,
                    Name = row.NAME,
                    Visible = row.VISIBLE,
                    Enable = row.ENABLE,
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, param, resultMapping);
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_COMMON_CODE> GetAuthorList()
        {
            DTO_RESULT_LIST<DTO_COMMON_CODE> returnData = new DTO_RESULT_LIST<DTO_COMMON_CODE>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_CODE WHERE PARENT_CODE='S01G03'");

            Repository<DTO_COMMON_CODE> repository = new Repository<DTO_COMMON_CODE>();
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

            Repository<DTO_COMMON_CODE> repository = new Repository<DTO_COMMON_CODE>();
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
        public bool ExistUser(string id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PERSONNAME FROM MIROS_USER /**where**/");
            builder.Where("PERSONID=:PERSONID");
            Repository<DTO_USER> repository = new Repository<DTO_USER>();
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                };
            });

            var result = repository.Select(queryTemplate.RawSql, new { PERSONID = id }, resultMapping);
            if (result.Count() > 0)
                return true;
            else
                return false;

        }
        public DTO_USER_TOKEN Authenticate(string id, string pass)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PASSWD FROM MIROS_USER /**where**/");
            builder.Where("PERSONID=:PERSONID AND PASSWD=:PASSWD");
            Repository<DTO_USER> repository = new Repository<DTO_USER>();
            var resultMapping = new Func<dynamic, DTO_USER>((row) =>
            {
                return new DTO_USER
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                };
            });

            var result = repository.Select(queryTemplate.RawSql, new { PERSONID = id, PASSWD = pass }, resultMapping);
            if (result.Count() > 0)
                return GetToken(id);
            else
                return null;
        }
        private DTO_USER_TOKEN GetToken(string id)
        {
            var userDetail = GetUserDetail(id);
            DTO_USER_TOKEN token = new DTO_USER_TOKEN(userDetail);
            return token;
        }




        public DTO_RESULT_PAGE_LIST<DTO_ROLE_DETAIL> GetRoleDetailList()
        {
            DTO_RESULT_PAGE_LIST<DTO_ROLE_DETAIL> returnData = new DTO_RESULT_PAGE_LIST<DTO_ROLE_DETAIL>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT ROLE_ID, ROLE_NAME, AUTHOR_CD FROM MIROS_ROLE, M30_ROLE_EXT WHERE MIROS_ROLE.ROLE=M30_ROLE_EXT.ROLE_ID");
            Repository<DTO_ROLE_DETAIL> repository = new Repository<DTO_ROLE_DETAIL>();
            var resultMapping = new Func<dynamic, DTO_ROLE_DETAIL>((row) =>
            {
                return new DTO_ROLE_DETAIL
                {
                    ID = row.ROLE_ID,
                    Name = row.ROLE_NAME,
                    AuthorCode = row.AUTHOR_CD
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, null, resultMapping);
            return returnData;
        }




        //public DTO_RESULT UpdateRole()
        //{
        //    DTO_RESULT result = new DTO_RESULT();
        //    return result;
        //}

        //public DTO_RESULT Getconfig()
        //{
        //    DTO_RESULT result = new DTO_RESULT();
        //    return result;
        //}
        //public DTO_RESULT UpdateConfig()
        //{
        //    DTO_RESULT result = new DTO_RESULT();
        //    return result;
        //}

    }
}

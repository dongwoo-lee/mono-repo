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
        public DTO_RESULT Login()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

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
        public DTO_RESULT_LIST<DTO_USER_DETAIL> GetUserDetailList()
        {
            DTO_RESULT_LIST<DTO_USER_DETAIL> returnData = new DTO_RESULT_LIST<DTO_USER_DETAIL>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PERSONNAME, DISK_MAX, DISK_USED, (DISK_MAX-DISK_USED) AS DISK_AVLB, MENU_GRP_ID, USED FROM MIROS_USER INNER JOIN M30_USER_EXT ON MIROS_USER.PERSONID = M30_USER_EXT.USER_ID");
            Repository<DTO_USER_DETAIL> repository = new Repository<DTO_USER_DETAIL>();
            var resultMapping = new Func<dynamic, DTO_USER_DETAIL>((row) =>
            {
                return new DTO_USER_DETAIL
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                    DiskMax = Convert.ToInt32(row.DISK_MAX),
                    DiskUsed = Convert.ToInt32(row.DISK_USED),
                    DiskAvailable = Convert.ToInt32(row.DISK_AVLB),
                    MenuGrpID = row.MENU_GRP_ID,
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
            var queryTemplate = builder.AddTemplate("SELECT PERSONID, PERSONNAME, DISK_MAX, DISK_USED, (DISK_MAX-DISK_USED) AS DISK_AVLB, MENU_GRP_ID, USED FROM MIROS_USER INNER JOIN M30_USER_EXT ON MIROS_USER.PERSONID = M30_USER_EXT.USER_ID WHERE PERSONID=:PERSONID");
            Repository<DTO_USER_DETAIL> repository = new Repository<DTO_USER_DETAIL>();
            var resultMapping = new Func<dynamic, DTO_USER_DETAIL>((row) =>
            {
                return new DTO_USER_DETAIL
                {
                    ID = row.PERSONID,
                    Name = row.PERSONNAME,
                    DiskMax = Convert.ToInt32(row.DISK_MAX),
                    DiskUsed = Convert.ToInt32(row.DISK_USED),
                    DiskAvailable = Convert.ToInt32(row.DISK_AVLB),
                    MenuGrpID = row.MENU_GRP_ID,
                    Used = row.USED,
                };
            });

            return repository.Get(queryTemplate.RawSql, new {PERSONID=id}, resultMapping);
        }
        public DTO_RESULT_LIST<DTO_ROLE_DETAIL> GetRoleDetailList()
        {
            DTO_RESULT_LIST<DTO_ROLE_DETAIL> returnData = new DTO_RESULT_LIST<DTO_ROLE_DETAIL>();
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

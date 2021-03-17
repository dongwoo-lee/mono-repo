using Dapper;
using MAMBrowser.Common;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAMBrowser.DAL
{
    public class LogDao
    {
        private readonly Repository _repository;
        public LogDao(Repository repository)
        {
            _repository = repository;
        }

        public void AddLog(string logLevel, string category, string userId, string description, string note)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_COMM_LOG VALUES(M30_COMM_LOG_SEQ.NEXTVAL, :SYSTEM_CD, :LOG_LEVEL, :CATEGORY, :USER_ID, 
            (SELECT PERSONNAME FROM MIROS_USER WHERE PERSONID=:USER_ID), :DESCRIPTION, :NOTE, SYSDATE)");
            DynamicParameters param = new DynamicParameters();
            param.Add("SYSTEM_CD", "S01");
            param.Add("LOG_LEVEL", logLevel);
            param.Add("CATEGORY", category);
            param.Add("USER_ID", userId);
            param.Add("DESCRIPTION", description);
            param.Add("NOTE", note);


            _repository.Insert(queryTemplate.RawSql, param);
        }
        public DTO_RESULT_PAGE_LIST<DTO_LOG> SearchLog(string start_dt, string end_dt, string logLevel, string userName, string description, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST<DTO_LOG> returnData = new DTO_RESULT_PAGE_LIST<DTO_LOG>();
            var builder = new SqlBuilder();
            var querySource = builder.AddTemplate("SELECT * FROM M30_COMM_LOG /**where**/ ORDER BY REG_DTM DESC");
            DynamicParameters param = new DynamicParameters();
            param.Add("START_DT", start_dt);
            param.Add("END_DT", end_dt);
            param.Add("LOG_LEVEL", logLevel);
            param.Add("USER_NAME", userName);
            param.Add("DESCRIPTION", description);
            param.Add("START_NO", startNo);
            param.Add("LAST_NO", lastNo);
            builder.Where("SYSTEM_CD='S01'");
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


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");




            var resultMapping = new Func<dynamic, DTO_LOG>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_LOG
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Seq = Convert.ToInt64(row.SEQ),
                    SystemCode = row.SYSTEM_CD,
                    LogLevel = row.LOG_LEVEL,
                    Category = row.CATEGORY,
                    UserID = row.USER_ID,
                    UserName = row.USER_NAME,
                    Description = row.DESCRIPTION,
                    Note = row.NOTE,
                    RegDtm = ((DateTime)row.REG_DTM).ToString(Define.DTM19),
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
    }
}

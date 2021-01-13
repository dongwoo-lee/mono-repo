using Dapper;
using MAMBrowser.Common;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Models;
using System;
using System.IO;

namespace MAMBrowser.DAL
{
    public class PublicFileDao
    {
        private readonly Repository _repository;
        public PublicFileDao(Repository repository)
        {
            _repository = repository;
        }
      
        public DTO_PUBLIC_FILE Insert(M30_MAM_PUBLIC_SPACE metaData)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", metaData.SEQ);
            param.Add("USER_ID", metaData.USER_ID);
            param.Add("TITLE", metaData.TITLE);
            param.Add("MEDIA_CD", metaData.MEDIA_CD);
            param.Add("CATE_CD", metaData.CATE_CD);
            param.Add("MEMO", metaData.MEMO);
            param.Add("AUDIO_FORMAT", metaData.AUDIO_FORMAT);
            param.Add("FILE_SIZE", metaData.FILE_SIZE);
            param.Add("FILE_PATH", metaData.FILE_PATH);

            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_MAM_PUBLIC_SPACE 
            VALUES(:SEQ, :USER_ID, :TITLE, :MEDIA_CD, :CATE_CD, :MEMO, :AUDIO_FORMAT, :FILE_SIZE, :FILE_PATH, SYSDATE)");
            _repository.Insert(queryTemplate.RawSql, param);

            return Get(metaData.SEQ);
        }
        public bool Delete(long seq)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"DELETE M30_MAM_PUBLIC_SPACE WHERE SEQ=:SEQ");
            builder.Where("SEQ=:SEQ");
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seq);
            _repository.Delete(queryTemplate.RawSql, param);
            return true;
        }
        public int UpdateData(long seq, M30_MAM_PUBLIC_SPACE metaData)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_MAM_PUBLIC_SPACE SET TITLE=:TITLE, MEMO=:MEMO,MEDIA_CD=:MEDIA_CD, CATE_CD=:CATE_CD, EDITED_DTM = SYSDATE /**where**/");
            builder.Where("SEQ=:SEQ");
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(metaData);
            return _repository.Update(queryTemplate.RawSql, param);
        }
        public DTO_PUBLIC_FILE Get(long id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT A.*, B.CODENAME AS MEDIA_NAME, C.NAME AS CATE_NAME, D.PERSONNAME AS USER_NAME FROM M30_MAM_PUBLIC_SPACE A
            LEFT JOIN (SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'PC') B ON B.CODEID=A.MEDIA_CD
            LEFT JOIN (SELECT * FROM M30_COMM_CODE WHERE PARENT_CODE='S01G05') C ON C.CODE=A.CATE_CD
            LEFT JOIN MIROS_USER D ON D.PERSONID=A.USER_ID 
            WHERE SEQ=:SEQ");
            var resultMapping = new Func<dynamic, DTO_PUBLIC_FILE>((row) =>
            {
                return new DTO_PUBLIC_FILE
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Seq = Convert.ToInt64(row.SEQ),
                    MediaCD = row.MEDIA_CD,
                    MediaName = row.MEDIA_NAME,
                    CategoryCD = row.CATE_CD,
                    CategoryName = row.CATE_NAME,
                    UserId = row.USER_ID,
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Define.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    FileExt = Path.GetExtension(row.FILE_PATH)
                };
            });
            return _repository.Get(queryTemplate.RawSql, new { SEQ = id }, resultMapping);
        }
        public DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE> FineData(string mediaCd, string cateCd, string start_dt, string end_dt, string userId, string title, string memo, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE> returnData = new DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE>();
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;
            DynamicParameters param = new DynamicParameters();
            param.Add("MEDIA_CD", mediaCd);
            param.Add("CATE_CD", cateCd);
            param.Add("USER_ID", userId);
            param.Add("TITLE", title);
            param.Add("MEMO", memo);
            param.Add("START_NO", startNo);
            param.Add("LAST_NO", lastNo);
            param.Add("START_DT", start_dt);
            param.Add("END_DT", end_dt);


            var builder = new SqlBuilder();
            var querySource = builder.AddTemplate(@"SELECT A.*, B.CODENAME AS MEDIA_NAME, C.NAME AS CATE_NAME, D.PERSONNAME AS USER_NAME FROM M30_MAM_PUBLIC_SPACE A
LEFT JOIN (SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'PC') B ON B.CODEID=A.MEDIA_CD
LEFT JOIN (SELECT * FROM M30_COMM_CODE WHERE PARENT_CODE='S01G05') C ON C.CODE=A.CATE_CD
LEFT JOIN MIROS_USER D ON D.PERSONID=A.USER_ID 
/**where**/");

            if (!string.IsNullOrEmpty(mediaCd))
            {
                builder.Where("MEDIA_CD =:MEDIA_CD");
            }
            if (!string.IsNullOrEmpty(cateCd))
            {
                builder.Where("CATE_CD =:CATE_CD");
            }
            if (!string.IsNullOrEmpty(start_dt))
            {
                builder.Where("TO_DATE(:START_DT,'YYYYMMDD') <= EDITED_DTM");
            }
            if (!string.IsNullOrEmpty(end_dt))
            {
                builder.Where("EDITED_DTM < TO_DATE(:END_DT,'YYYYMMDD')+1");
            }

            if (userId != null)
            {
                builder.Where("USER_ID=:USER_ID");
            }
            if (!string.IsNullOrEmpty(title))
            {
                string[] nameArray = title.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(TITLE) LIKE LOWER('%{word}%')");
                }
            }
            if (!string.IsNullOrEmpty(memo))
            {
                string[] nameArray = memo.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(MEMO) LIKE LOWER('%{word}%')");
                }
            }

            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY EDITED_DTM, TITLE ASC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_PUBLIC_FILE), sortKey, sortValue);
            }


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");


            var resultMapping = new Func<dynamic, DTO_PUBLIC_FILE>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_PUBLIC_FILE
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Seq = Convert.ToInt64(row.SEQ),
                    MediaCD = row.MEDIA_CD,
                    MediaName = row.MEDIA_NAME,
                    CategoryCD = row.CATE_CD,
                    CategoryName = row.CATE_NAME,
                    UserId = row.USER_ID,
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Define.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    FileExt = Path.GetExtension(row.FILE_PATH)
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public long GetID()
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT M30_MAM_PUBLIC_SPACE_SEQ.NEXTVAL AS SEQ FROM DUAL");

            var resultMapping = new Func<dynamic, long>((row) =>
            {
                return Convert.ToInt64(row.SEQ);
            });

            return _repository.Get(queryTemplate.RawSql, null, resultMapping);
        }
        public DTO_PUBLIC_FILE Get(string title)
        {
            var dto = _repository.Get<DTO_PUBLIC_FILE>("SELECT * FROM M30_MAM_PUBLIC_SPACE WHERE LOWER(TITLE) =LOWER(:TITLE)", new { TITLE = title }, DTO_PUBLIC_FILE.ResultMapping());
            return dto;
        }
        public long CountPublicCategory(string cateCd)
        {
            var data = _repository.Get<dynamic>("SELECT COUNT(SEQ) AS RCOUNT FROM M30_MAM_PUBLIC_SPACE WHERE CATE_CD = :CATE_CD", new { CATE_CD = cateCd },
            new Func<dynamic, dynamic>((row) =>
            {
                return new
                {
                    count = Convert.ToInt64(row.RCOUNT)
                };
            }));
            return data.count;
        }
    }
}

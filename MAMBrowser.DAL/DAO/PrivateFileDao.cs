using Dapper;
using MAMBrowser.Common;
using MAMBrowser.DAL.Expand.Factories;
using MAMBrowser.DTO;
using MAMBrowser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAMBrowser.DAL
{
    public class PrivateFileDao
    {
        private readonly Repository _repository;
        public PrivateFileDao(Repository repository)
        {
            _repository = repository;
        }
        public void Insert(M30_MAM_PRIVATE_SPACE metaData)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", metaData.SEQ);
            param.Add("USER_ID", metaData.USER_ID);
            param.Add("TITLE", metaData.TITLE);
            param.Add("MEMO", metaData.MEMO);
            param.Add("AUDIO_FORMAT", metaData.AUDIO_FORMAT);
            param.Add("FILE_SIZE", metaData.FILE_SIZE);
            param.Add("FILE_PATH", metaData.FILE_PATH);
            
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_MAM_PRIVATE_SPACE 
            VALUES(:SEQ, :USER_ID, :TITLE, :MEMO, :AUDIO_FORMAT, :FILE_SIZE, :FILE_PATH, 'Y', SYSDATE, NULL)");

             _repository.Insert(queryTemplate.RawSql, param);
        }
        
        public int UpdateUsedN(string userId, List<long> seqList)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_MAM_PRIVATE_SPACE SET USED='N', DELETED_DTM=SYSDATE WHERE SEQ IN :SEQ");
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seqList);
            return _repository.Update(queryTemplate.RawSql, param);
        }
        public int Delete(string userId, List<long> seqList)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"DELETE M30_MAM_PRIVATE_SPACE WHERE USED='N' AND SEQ IN :SEQ");
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seqList);

            return _repository.Delete(queryTemplate.RawSql, param);
        }

        /// <summary>
        /// 휴지통 비우기
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<long> GetSeqAllByUsedN(string userId)
        {
            string getRecycleBin = @"SELECT * FROM M30_MAM_PRIVATE_SPACE WHERE USED='N'";
            var dtoList = _repository.Select<DTO_PRIVATE_FILE>(getRecycleBin, null, DTO_PRIVATE_FILE.ResultMapping());
            List<long> seqList = new List<long>();
            dtoList.ToList().ForEach(dto => seqList.Add(dto.Seq));
            return seqList;
        }
        public IList<DTO_PRIVATE_FILE> GetAllByUsedN(string userId)
        {
            string getRecycleBin = @"SELECT * FROM M30_MAM_PRIVATE_SPACE WHERE USED='N'";
            var dtoList = _repository.Select<DTO_PRIVATE_FILE>(getRecycleBin, null, DTO_PRIVATE_FILE.ResultMapping());
            return dtoList;
        }

        public int UpdateUsedY(string userId, List<long> seqList)    //복원
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_MAM_PRIVATE_SPACE SET USED='Y', DELETED_DTM=NULL WHERE SEQ IN :SEQ");
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seqList);
            return _repository.Update(queryTemplate.RawSql, param);
        }

        public int UpdateData(M30_MAM_PRIVATE_SPACE metaData)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_MAM_PRIVATE_SPACE SET TITLE=:TITLE, MEMO=:MEMO, EDITED_DTM = SYSDATE /**where**/");
            builder.Where("SEQ=:SEQ");
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(metaData);
            return _repository.Update(queryTemplate.RawSql, param);
        }

        public DTO_PRIVATE_FILE Get(long id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_MAM_PRIVATE_SPACE WHERE SEQ=:SEQ");
            var resultMapping = DTO_PRIVATE_FILE.ResultMapping();
            return _repository.Get(queryTemplate.RawSql, new { SEQ = id }, resultMapping);
        }
        public IList<DTO_PRIVATE_FILE> Get(List<long> idList)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_MAM_PRIVATE_SPACE WHERE SEQ IN :IDS");
            var resultMapping = DTO_PRIVATE_FILE.ResultMapping();
            return _repository.Select(queryTemplate.RawSql, new { IDS = idList }, resultMapping);
        }
        public DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE> FindData(string used, string start_dt, string end_dt, string userId, string title, string memo, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE> returnData = new DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>();
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;
            DynamicParameters param = new DynamicParameters();
            param.Add("USER_ID", userId);
            param.Add("TITLE", title);
            param.Add("MEMO", memo);
            param.Add("START_NO", startNo);
            param.Add("LAST_NO", lastNo);
            param.Add("USED", used);
            param.Add("START_DT", start_dt);
            param.Add("END_DT", end_dt);

            var builder = new SqlBuilder();
            var querySource = builder.AddTemplate(@"SELECT * FROM M30_MAM_PRIVATE_SPACE /**where**/");
            builder.Where("(USER_ID=:USER_ID AND USED=:USED)");
            if (!string.IsNullOrEmpty(start_dt))
            {
                builder.Where("TO_DATE(:START_DT,'YYYYMMDD') <= EDITED_DTM");
            }
            if (!string.IsNullOrEmpty(end_dt))
            {
                builder.Where("EDITED_DTM < TO_DATE(:END_DT,'YYYYMMDD')+1");
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
                orderBy = "ORDER BY EDITED_DTM DESC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_PRIVATE_FILE), sortKey, sortValue);
            }

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");


            var resultMapping = new Func<dynamic, DTO_PRIVATE_FILE>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_PRIVATE_FILE
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Seq = Convert.ToInt64(row.SEQ),
                    UserId = row.USER_ID,
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Define.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    DeletedDtm = row.DELETED_DTM == null ? "" : ((DateTime)row.DELETED_DTM).ToString(Define.DTM19),
                    Used = row.USED,
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
            var queryTemplate = builder.AddTemplate("SELECT M30_MAM_PRIVATE_SPACE_SEQ.NEXTVAL AS SEQ FROM DUAL");

            var resultMapping = new Func<dynamic, long>((row) =>
            {
                return Convert.ToInt64(row.SEQ);
            });

            return _repository.Get(queryTemplate.RawSql, null, resultMapping);
        }
        public DTO_PRIVATE_FILE Get (string title)
        {
            return _repository.Get<DTO_PRIVATE_FILE>("SELECT * FROM M30_MAM_PRIVATE_SPACE WHERE LOWER(TITLE) =LOWER(:TITLE)", new { TITLE = title }, DTO_PRIVATE_FILE.ResultMapping());
        }
    }
}

using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    public class PrivateFileBLL
    {
        public DTO_PRIVATE_FILE Upload(IFormFile file, PrivateFileModel metaData)
        {
            long ID = GetID();
            string date = DateTime.Now.ToString(Utility.DTM8);
            string fileName = $"{ ID.ToString() }_{ file.FileName}";
            var relativeSourceFolder = $"{SystemConfig.AppSettings.FtpTmpUploadFolder}";
            var relativeTargetFolder = $"{SystemConfig.AppSettings.FtpPrivateUploadFolder}/{metaData.UserExtID}/{date}";
            var relativeSourcePath = $"{relativeSourceFolder}/{fileName}";
            var relativeTargetPath = $"{relativeTargetFolder}/{fileName}";

            MyFtp.MakeFTPDir(relativeSourceFolder);
            if (MyFtp.Upload(file.OpenReadStream(), relativeSourcePath, file.Length))
            {
                MyFtp.MakeFTPDir(relativeTargetFolder);
                if (MyFtp.FtpRename(relativeSourcePath, relativeTargetPath))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("SEQ", metaData.Seq);
                    param.Add("USEREXTID", metaData.UserExtID);
                    param.Add("TITLE", metaData.Title);
                    param.Add("MEMO", metaData.Memo);
                    param.Add("AUDIO_FORMAT", "test format");
                    param.Add("FILE_SIZE", file.Length);
                    param.Add("FILE_PATH", metaData.FilePath);
                    //db에 데이터 등록
                    var builder = new SqlBuilder();
                    var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_PRIVATE_SPACE 
VALUES(:SEQ, :USEREXTID, :TITLE, :MEMO, :AUDIO_FORMAT, :FILE_SIZE, :FILE_PATH, 'Y', SYSDATE, NULL)");
                    Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
                    repository.Insert(queryTemplate.RawSql, param);
                    return Get(ID);
                }
            }
            return null;
        }
     
        public bool DeleteDB(long userextid, long seq)
        {
            //파일 실제 삭제 이후
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PRIVATE_SPACE SET USED='N' WHERE SEQ=:SEQ");
            builder.Where("SEQ=:SEQ");
            Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seq);
            return repository.Update(queryTemplate.RawSql, param) > 0 ? true : false;
        }
        public bool DeletePhysical(long userextid, long seq)
        {
            //파일 실제 삭제 이후

            //파일 실제 삭제 이후
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"DELETE M30_PRIVATE_SPACE WHERE SEQ=:SEQ");
            builder.Where("SEQ=:SEQ");
            Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seq);
            repository.Delete(queryTemplate.RawSql,seq);
            return true;
        }
        public bool DeleteAllPhysical(long userextid)
        {
            //파일 실제 삭제 이후

            //파일 실제 삭제 이후
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"DELETE M30_PRIVATE_SPACE WHERE USED='N'");
            Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
            DynamicParameters param = new DynamicParameters();
            repository.Delete(queryTemplate.RawSql, null);
            return true;
        }

        public bool Recycle(long userextid, long seq)   //복원
        { 
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PRIVATE_SPACE SET USED='Y' WHERE SEQ=:SEQ");
            builder.Where("SEQ=:SEQ");
            Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seq);
            return repository.Update(queryTemplate.RawSql, param) > 0 ? true : false;
        }
        public bool RecycleAll(long userextid, long[] seqList)    //모두 복원
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PRIVATE_SPACE SET USED='Y' WHERE SEQ=:SEQ");
            builder.Where("SEQ=:SEQ");
            Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seqList);
            return repository.Update(queryTemplate.RawSql, param) > 0 ? true : false;
        }

        public int UpdateData(PrivateFileModel metaData)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PRIVATE_SPACE SET TITLE=:TITLE, MEMO=:MEMO, EDITED_DTM = SYSDATE");
            Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(metaData);
            return repository.Update(queryTemplate.RawSql, param);
        }
        
        public DTO_PRIVATE_FILE Get(long id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_PRIVATE_SPACE WHERE SEQ=:SEQ");
            Repository<DTO_PRIVATE_FILE> repository = new Repository<DTO_PRIVATE_FILE>();
            var resultMapping = new Func<dynamic, DTO_PRIVATE_FILE>((row) =>
            {
                return new DTO_PRIVATE_FILE
                {
                    Seq = Convert.ToInt64(row.SEQ),
                    UserExtID = Convert.ToInt64(row.USER_EXT_ID),
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Utility.DTM19),
                    FilePath = row.FILE_PATH,
                    DeletedDtm = row.DELETED_DTM == null ? "" : ((DateTime)row.DELETED_DTM).ToString(Utility.DTM19),
                    Used = row.USED,
                };
            });
            return repository.Get(queryTemplate.RawSql, new { SEQ=id },resultMapping);
        }
        public DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE> FineData(string used, long userextid, string title, string memo, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE> returnData = new DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>();
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;
            DynamicParameters param = new DynamicParameters();
            param.Add("USER_EXT_ID", userextid);
            param.Add("TITLE", title);
            param.Add("MEMO", memo);
            param.Add("START_NO", startNo);
            param.Add("LAST_NO", lastNo);
            param.Add("USED", used);

            var builder = new SqlBuilder();
            var querySource = builder.AddTemplate(@"SELECT * FROM M30_PRIVATE_SPACE /**where**/");
            builder.Where("(USER_EXT_ID=:USER_EXT_ID AND USED=:USED)");
            if (!string.IsNullOrEmpty(title))
            {
                builder.Where("TITLE = :TITLE");
            }
            if (!string.IsNullOrEmpty(memo))
            {
                string[] nameArray = memo.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(AUDIONAME) LIKE LOWER('%{word}%')");
                }
            }


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");


            Repository<DTO_PRIVATE_FILE> repository = new Repository<DTO_PRIVATE_FILE>();
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
                    UserExtID = Convert.ToInt64(row.USER_EXT_ID),
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Utility.DTM19),
                    FilePath = row.FILE_PATH,
                    DeletedDtm = row.DELETED_DTM == null ? "" : ((DateTime)row.DELETED_DTM).ToString(Utility.DTM19),
                    Used = row.USED,
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        private long GetID()
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT M30_PRIVATE_SPACE_SEQ.NEXTVAL AS SEQ FROM DUAL");
            Repository<long> repository = new Repository<long>();

            var resultMapping = new Func<dynamic, long>((row) =>
            {
                return Convert.ToInt64(row.SEQ);
            });

            return repository.Get(queryTemplate.RawSql, null, resultMapping);
        }
    }
}

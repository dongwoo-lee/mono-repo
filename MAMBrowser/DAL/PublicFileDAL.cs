using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    public class PublicFileDAL
    {
        public PublicFileDAL()
        {

        }
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        public PublicFileDAL(IOptions<AppSettings> appSesstings, ServiceResolver sr)
        {
            _appSettings = appSesstings.Value;
            _fileService = sr("PublicWorkConnection");
        }
        public DTO_PUBLIC_FILE Insert(string userId, IFormFile file, PublicFileModel metaData, string host)
        {
            //업로드 권한 처리 필요.

            long ID = GetID();
            string date = DateTime.Now.ToString(MAMUtility.DTM8);
            string fileName = $"{ ID.ToString() }_{file.FileName}";
            var relativeSourceFolder = $"{_fileService.TmpUploadFolder}";
            var relativeTargetFolder = @$"{_fileService.UploadFolder}\{userId}\{date}";      //공유소재도 유저확장ID 사용?, 분류코드별로...필요해보임.
            var relativeSourcePath = @$"{relativeSourceFolder}\{fileName}";
            var relativeTargetPath = @$"{relativeTargetFolder}\{fileName}";

            var stream = file.OpenReadStream();
            
            var headerStream = AudioEngine.GetHeaderStream(stream);
            headerStream.Position = 0;
            var audioFormat = AudioEngine.GetAudioFormat(headerStream, relativeTargetPath);
            headerStream.Position = 0;

            _fileService.MakeDirectory(relativeSourceFolder);
            _fileService.Upload(headerStream, stream, relativeSourcePath, file.Length);
            _fileService.MakeDirectory(relativeTargetFolder);
            _fileService.Move(relativeSourcePath, relativeTargetPath);
            stream.Dispose();
            headerStream.Dispose();

            //metaData.SEQ = ID;
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", ID);
            param.Add("USER_ID", userId);
            param.Add("TITLE", metaData.TITLE);
            param.Add("MEDIA_CD", metaData.MEDIA_CD);
            param.Add("CATE_CD", metaData.CATE_CD);
            param.Add("MEMO", metaData.MEMO);
            param.Add("AUDIO_FORMAT", audioFormat);
            param.Add("FILE_SIZE", file.Length);
            param.Add("FILE_PATH", @$"\\{host}\{relativeTargetPath}");

            //db에 데이터 등록
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_MAM_PUBLIC_SPACE 
VALUES(:SEQ, :USER_ID, :TITLE, :MEDIA_CD, :CATE_CD, :MEMO, :AUDIO_FORMAT, :FILE_SIZE, :FILE_PATH, SYSDATE)");
            Repository repository = new Repository();
            repository.Insert(queryTemplate.RawSql, param);


            return Get(ID);
        }
        public bool DeletePhysical(long seq)
        {
            //파일 실제 삭제
            var fileData = Get(seq);
            _fileService.Delete(fileData.FilePath);

            //파일 실제 삭제 이후
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"DELETE M30_MAM_PUBLIC_SPACE WHERE SEQ=:SEQ");
            builder.Where("SEQ=:SEQ");
            Repository repository = new Repository();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seq);
            repository.Delete(queryTemplate.RawSql, param);
            return true;
        }
        public int UpdateData(long seq, PublicFileModel metaData)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_MAM_PUBLIC_SPACE SET TITLE=:TITLE, MEMO=:MEMO,MEDIA_CD=:MEDIA_CD, CATE_CD=:CATE_CD, EDITED_DTM = SYSDATE /**where**/");
            builder.Where("SEQ=:SEQ");
            Repository repository = new Repository();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(metaData);
            return repository.Update(queryTemplate.RawSql, param);
        }
        public DTO_PUBLIC_FILE Get(long id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT A.*, B.CODENAME AS MEDIA_NAME, C.NAME AS CATE_NAME, D.PERSONNAME AS USER_NAME FROM M30_MAM_PUBLIC_SPACE A
LEFT JOIN (SELECT * FROM M30_VW_CATEGORY WHERE CODETYPE = 'PC') B ON B.CODEID=A.MEDIA_CD
LEFT JOIN (SELECT * FROM M30_COMM_CODE WHERE PARENT_CODE='S01G05') C ON C.CODE=A.CATE_CD
LEFT JOIN MIROS_USER D ON D.PERSONID=A.USER_ID 
WHERE SEQ=:SEQ");
            Repository repository = new Repository();
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
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(MAMUtility.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    FileExt = Path.GetExtension(row.FILE_PATH)
                };
            });
            return repository.Get(queryTemplate.RawSql, new { SEQ = id }, resultMapping);
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
                orderBy = MAMUtility.GetSortString(typeof(DTO_PUBLIC_FILE), sortKey, sortValue);
            }


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");


            Repository repository = new Repository();
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
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(MAMUtility.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    FileExt = Path.GetExtension(row.FILE_PATH)
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        private long GetID()
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT M30_MAM_PUBLIC_SPACE_SEQ.NEXTVAL AS SEQ FROM DUAL");
            Repository repository = new Repository();

            var resultMapping = new Func<dynamic, long>((row) =>
            {
                return Convert.ToInt64(row.SEQ);
            });

            return repository.Get(queryTemplate.RawSql, null, resultMapping);
        }
        public bool IsExistTitle(string title)
        {
            Repository repository = new Repository();
            var dto = repository.Get<DTO_PRIVATE_FILE>("SELECT * FROM M30_MAM_PUBLIC_SPACE WHERE LOWER(TITLE) =LOWER(:TITLE)", new { TITLE = title }, DTO_PRIVATE_FILE.ResultMapping());
            if (dto == null)
                return false;
            else
                return true;
        }
        public long CountPublicCategory(string cateCd)
        {
            Repository repository = new Repository();
            var data = repository.Get<dynamic>("SELECT COUNT(SEQ) AS RCOUNT FROM M30_MAM_PUBLIC_SPACE WHERE CATE_CD = :CATE_CD", new { CATE_CD = cateCd },
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

using Dapper;
using DL_Service.DAL;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAMBrowser.Controllers
{
    public class PrivateFileDAL
    {
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        public PrivateFileDAL(IOptions<AppSettings> appSesstings, ServiceResolver sr)
        {
            _appSettings = appSesstings.Value;
            _fileService = sr("PrivateWorkConnection");
        }

        public DTO_PRIVATE_FILE Insert(string userId, IFormFile file, PrivateFileModel metaData, string host)
        {
            long ID = GetID();
            string date = DateTime.Now.ToString(MAMUtility.DTM8);
            string fileName = $"{ ID.ToString() }_{ file.FileName}";
            var relativeSourceFolder = $"{_fileService.TmpUploadFolder}";
            var relativeTargetFolder = @$"{_fileService.UploadFolder}\{userId}\{date}";
            var relativeSourcePath = @$"{relativeSourceFolder}\{fileName}";
            var relativeTargetPath = @$"{relativeTargetFolder}\{fileName}";
            
            _fileService.MakeDirectory(relativeSourceFolder);
            var stream = file.OpenReadStream();
            var audioFormat = AudioEngine.GetAudioFormat(stream, relativeTargetPath);
            stream.Position = 0;
            _fileService.Upload(file.OpenReadStream(), relativeSourcePath, file.Length);
            _fileService.MakeDirectory(relativeTargetFolder);
            _fileService.Move(relativeSourcePath, relativeTargetPath);
            

            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", ID);
            param.Add("USER_ID", userId);
            param.Add("TITLE", metaData.TITLE);
            param.Add("MEMO", metaData.MEMO);
            param.Add("AUDIO_FORMAT", audioFormat);
            param.Add("FILE_SIZE", file.Length);
            param.Add("FILE_PATH", @$"\\{host}\{relativeTargetPath}");
            //db에 데이터 등록
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_PRIVATE_SPACE 
VALUES(:SEQ, :USER_ID, :TITLE, :MEMO, :AUDIO_FORMAT, :FILE_SIZE, :FILE_PATH, 'Y', SYSDATE, NULL)");

            var builder2 = new SqlBuilder();
            var queryTemplate2 = builder2.AddTemplate("UPDATE M30_USER_EXT SET DISK_USED=(DISK_USED+:FILE_SIZE) WHERE USER_ID=:USER_ID");
            DynamicParameters param2 = new DynamicParameters();
            param2.Add("USER_ID", userId);
            param2.Add("FILE_SIZE", file.Length);

            TransactionRepository repository = new TransactionRepository();
            repository.BeginTransaction();
            try
            {
                repository.Insert(queryTemplate.RawSql, param);
                repository.Update(queryTemplate2.RawSql, param2);
                repository.CommitTransaction();
            }
            catch (Exception ex)
            {
                repository.RollbackTransaction();
                throw;
            }


            return Get(ID);
        }
        
        public bool DeleteDB(string userId, LongList seqList)
        {
            //파일 실제 삭제 이후
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PRIVATE_SPACE SET USED='N', DELETED_DTM=SYSDATE WHERE SEQ IN :SEQ");
            Repository repository = new Repository();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seqList);
            return repository.Update(queryTemplate.RawSql, param) > 0 ? true : false;
        }
        public bool DeleteRecycleBin(string userId, List<long> seqList)
        {
            if (seqList.Count < 1)
                return true;

            //파일 실제 삭제
            long totalDeleteSize = 0;
            foreach (var seq in seqList)
            {
                var fileData = Get(seq);
                totalDeleteSize += fileData.FileSize;
                _fileService.Delete(fileData.FilePath);
            }

            //파일 실제 삭제 이후
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"DELETE M30_PRIVATE_SPACE WHERE USED='N' AND SEQ IN :SEQ");
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seqList);

            var builder2 = new SqlBuilder();
            var queryTemplate2 = builder2.AddTemplate("UPDATE M30_USER_EXT SET DISK_USED=(DISK_USED+:FILE_SIZE) WHERE USER_ID=:USER_ID");
            DynamicParameters param2 = new DynamicParameters();
            param2.Add("USER_ID", userId);
            param2.Add("FILE_SIZE", -(totalDeleteSize));

            TransactionRepository repository = new TransactionRepository();
            repository.BeginTransaction();
            try
            {
                repository.Delete(queryTemplate.RawSql, param);
                repository.Update(queryTemplate2.RawSql, param2);
                repository.CommitTransaction();
            }
            catch (Exception ex)
            {
                repository.RollbackTransaction();
                throw;
            }

            return true;
        }

        /// <summary>
        /// 휴지통 비우기
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteAllRecycleBin(string userId)    
        {
            string getRecycleBin = @"SELECT * FROM M30_PRIVATE_SPACE WHERE USED='N'";
            Repository sRepository = new Repository();
            var dtoList = sRepository.Select<DTO_PRIVATE_FILE>(getRecycleBin, null, DTO_PRIVATE_FILE.ResultMapping());
            List<long> seqList = new List<long>();
            dtoList.ToList().ForEach(dto => seqList.Add(dto.Seq));
            return DeleteRecycleBin(userId, seqList);
        }     

        public bool RecycleAll(string userId, LongList seqList)    //복원
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PRIVATE_SPACE SET USED='Y', DELETED_DTM=NULL WHERE SEQ IN :SEQ");
            Repository repository = new Repository();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seqList);
            return repository.Update(queryTemplate.RawSql, param) > 0 ? true : false;
        }

        public int UpdateData(PrivateFileModel metaData)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PRIVATE_SPACE SET TITLE=:TITLE, MEMO=:MEMO, EDITED_DTM = SYSDATE /**where**/");
            builder.Where("SEQ=:SEQ");
            Repository repository = new Repository();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(metaData);
            return repository.Update(queryTemplate.RawSql, param);
        }

        public DTO_PRIVATE_FILE Get(long id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_PRIVATE_SPACE WHERE SEQ=:SEQ");
            Repository repository = new Repository();
            var resultMapping = DTO_PRIVATE_FILE.ResultMapping();
            return repository.Get(queryTemplate.RawSql, new { SEQ = id }, resultMapping);
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
            var querySource = builder.AddTemplate(@"SELECT * FROM M30_PRIVATE_SPACE /**where**/ /**orderby**/");
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

            builder.OrderBy("EDITED_DTM DESC");

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");


            Repository repository = new Repository();
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
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(MAMUtility.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    DeletedDtm = row.DELETED_DTM == null ? "" : ((DateTime)row.DELETED_DTM).ToString(MAMUtility.DTM19),
                    Used = row.USED,
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
            var queryTemplate = builder.AddTemplate("SELECT M30_PRIVATE_SPACE_SEQ.NEXTVAL AS SEQ FROM DUAL");
            Repository repository = new Repository();

            var resultMapping = new Func<dynamic, long>((row) =>
            {
                return Convert.ToInt64(row.SEQ);
            });

            return repository.Get(queryTemplate.RawSql, null, resultMapping);
        }
    }
}

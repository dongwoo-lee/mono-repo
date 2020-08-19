﻿using Dapper;
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
    public class PublicFileBLL 
    {
        public DTO_PRIVATE_FILE Upload(long userextid, IFormFile file, PublicFileModel metaData)
        {
            //업로드 권한 처리 필요.

            long ID = GetID();
            string date = DateTime.Now.ToString(Utility.DTM8);
            string fileName = $"{ ID.ToString() }_{ file.FileName}";
            var relativeSourceFolder = $"{SystemConfig.AppSettings.FtpTmpUploadFolder}";
            var relativeTargetFolder = $"{SystemConfig.AppSettings.FtpPublicUploadFolder}/{userextid}/{date}";      //공유소재도 유저확장ID 사용?
            var relativeSourcePath = $"{relativeSourceFolder}/{fileName}";
            var relativeTargetPath = $"{relativeTargetFolder}/{fileName}";

            MyFtp.MakeFTPDir(relativeSourceFolder);
            if (MyFtp.Upload(file.OpenReadStream(), relativeSourcePath, file.Length))
            {
                MyFtp.MakeFTPDir(relativeTargetFolder);
                if (MyFtp.FtpRename(relativeSourcePath, relativeTargetPath))
                {
                    //db에 데이터 등록
                    var builder = new SqlBuilder();
                    var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_PUBLIC_SPACE 
VALUES(:SEQ, :USER_EXT_ID, :TITLE, :MEMO, :AUDIO_FORMAT, :FILE_SIZE, :FILE_PATH, 'Y', SYSDATE, NULL)");
                    Repository<PublicFileModel> repository = new Repository<PublicFileModel>();
                    repository.Insert(queryTemplate.RawSql, metaData);


                    return Get(ID);
                }
            }
            return null;
        }
        public bool DeletePhysical(long userextid, long seq)
        {
            //파일 실제 삭제 이후

            //파일 실제 삭제 이후
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"DELETE M30_PUBLIC_SPACE WHERE SEQ=:SEQ");
            builder.Where("SEQ=:SEQ");
            Repository<PublicFileModel> repository = new Repository<PublicFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seq);
            repository.Delete(queryTemplate.RawSql, seq);
            return true;
        }
        public int UpdateData(PublicFileModel metaData)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PUBLIC_SPACE SET TITLE=:TITLE, MEMO=:MEMO, EDITED_DTM = SYSDATE /**where**/");
            builder.Where("SEQ=:SEQ");
            Repository<PublicFileModel> repository = new Repository<PublicFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(metaData);
            return repository.Update(queryTemplate.RawSql, param);
        }
        public DTO_PRIVATE_FILE Get(long id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT * FROM M30_PUBLIC_SPACE WHERE SEQ=:SEQ");
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
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    DeletedDtm = row.DELETED_DTM == null ? "" : ((DateTime)row.DELETED_DTM).ToString(Utility.DTM19),
                    Used = row.USED,
                };
            });
            return repository.Get(queryTemplate.RawSql, new { SEQ = id }, resultMapping);
        }
        public DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE> FineData(long? userextid, string title, string memo, int rowPerPage, int selectPage, string sortKey, string sortValue)
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

            var builder = new SqlBuilder();
            var querySource = builder.AddTemplate(@"SELECT * FROM M30_PUBLIC_SPACE /**where**/");
            if (userextid!=null)
            {
                builder.Where("USER_EXT_ID=:USER_EXT_ID");
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

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        private long GetID()
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate("SELECT M30_PUBLIC_SPACE_SEQ.NEXTVAL AS SEQ FROM DUAL");
            Repository<long> repository = new Repository<long>();

            var resultMapping = new Func<dynamic, long>((row) =>
            {
                return Convert.ToInt64(row.SEQ);
            });

            return repository.Get(queryTemplate.RawSql, null, resultMapping);
        }
    }
}

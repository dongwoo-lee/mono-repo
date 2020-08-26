﻿using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    public class PublicFileBLL 
    {
        public DTO_PUBLIC_FILE Upload(IFormFile file, PublicFileModel metaData)
        {
            //업로드 권한 처리 필요.

            long ID = GetID();
            string date = DateTime.Now.ToString(Utility.DTM8);
            string fileName = $"{ ID.ToString() }_{ file.FileName}";
            var relativeSourceFolder = $"{SystemConfig.AppSettings.FtpTmpUploadFolder}";
            var relativeTargetFolder = $"{SystemConfig.AppSettings.FtpPublicUploadFolder}/{metaData.USER_EXT_ID}/{date}";      //공유소재도 유저확장ID 사용?
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
                    //metaData.SEQ = ID;
                    DynamicParameters param = new DynamicParameters();
                    param.Add("SEQ", ID);
                    param.Add("USER_EXT_ID", metaData.USER_EXT_ID);
                    param.Add("TITLE", metaData.TITLE);
                    param.Add("MEDIA_CD", metaData.MEDIA_CD);
                    param.Add("CATE_CD", metaData.CATE_CD);
                    param.Add("MEMO", metaData.MEMO);
                    param.Add("AUDIO_FORMAT", "test format");
                    param.Add("FILE_SIZE", file.Length);
                    param.Add("FILE_PATH", relativeTargetPath);


                    var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_PUBLIC_SPACE 
VALUES(:SEQ, :USER_EXT_ID, :TITLE, :MEDIA_CD, :CATE_CD, :MEMO, :AUDIO_FORMAT, :FILE_SIZE, :FILE_PATH, SYSDATE)");
                    Repository<PublicFileModel> repository = new Repository<PublicFileModel>();
                    repository.Insert(queryTemplate.RawSql, param);


                    return Get(ID);
                }
            }
            return null;
        }
        public bool DeletePhysical(long seq)
        {
            //파일 실제 삭제 이후

            //파일 실제 삭제 이후
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"DELETE M30_PUBLIC_SPACE WHERE SEQ=:SEQ");
            builder.Where("SEQ=:SEQ");
            Repository<PublicFileModel> repository = new Repository<PublicFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("SEQ", seq);
            repository.Delete(queryTemplate.RawSql, param);
            return true;
        }
        public int UpdateData(long seq, PublicFileModel metaData)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"UPDATE M30_PUBLIC_SPACE SET TITLE=:TITLE, MEMO=:MEMO,MEDIA_CD=:MEDIA_CD, CATE_CD=:CATE_CD, EDITED_DTM = SYSDATE /**where**/");
            builder.Where("SEQ=:SEQ");
            Repository<PublicFileModel> repository = new Repository<PublicFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(metaData);
            return repository.Update(queryTemplate.RawSql, param);
        }
        public DTO_PUBLIC_FILE Get(long id)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT M30_PUBLIC_SPACE.*, MEDIA.CODENAME AS MEDIA_NAME, CATE.NAME AS CATE_NAME FROM M30_PUBLIC_SPACE 
LEFT JOIN (select * from m30_code_map
LEFT JOIN (SELECT * FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'PC' ORDER BY NUM) M ON M.CODEID=M30_CODE_MAP.GRP_CD
where map_cd='S00G01C005') MEDIA ON MEDIA.CODEID=M30_PUBLIC_SPACE.MEDIA_CD
LEFT JOIN (select * from m30_code WHERE PARENT_CODE='S01G05') CATE ON CATE.CODE=M30_PUBLIC_SPACE.CATE_CD WHERE SEQ=:SEQ");
            Repository<DTO_PUBLIC_FILE> repository = new Repository<DTO_PUBLIC_FILE>();
            var resultMapping = new Func<dynamic, DTO_PUBLIC_FILE>((row) =>
            {
                return new DTO_PUBLIC_FILE
                {
                    Seq = Convert.ToInt64(row.SEQ),
                    MediaCD = row.MEDIA_CD,
                    MediaName = row.MEDIA_NAME,
                    CategoryCD = row.CATE_CD,
                    CatetoryName = row.CATE_NAME,
                    UserExtID = Convert.ToInt64(row.USER_EXT_ID),
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Utility.DTM19),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = row.FILE_PATH,
                    FileExt = Path.GetExtension(row.FILE_PATH)
                };
            });
            return repository.Get(queryTemplate.RawSql, new { SEQ = id }, resultMapping);
        }
        public DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE> FineData(string start_dt, string end_dt, long? userextid, string title, string memo, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE> returnData = new DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE>();
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;
            DynamicParameters param = new DynamicParameters();
            param.Add("USER_EXT_ID", userextid);
            param.Add("TITLE", title);
            param.Add("MEMO", memo);
            param.Add("START_NO", startNo);
            param.Add("LAST_NO", lastNo);
            param.Add("START_DT", start_dt);
            param.Add("END_DT", end_dt);


            var builder = new SqlBuilder();
            var querySource = builder.AddTemplate(@"SELECT M30_PUBLIC_SPACE.*, MEDIA.CODENAME AS MEDIA_NAME, CATE.NAME AS CATE_NAME FROM M30_PUBLIC_SPACE 
LEFT JOIN (SELECT * FROM M30_CODE_MAP
LEFT JOIN (SELECT * FROM MEM_CATEGORY_VIEW WHERE CODETYPE = 'PC' ORDER BY NUM) M ON M.CODEID=M30_CODE_MAP.GRP_CD
WHERE MAP_CD='S00G01C005') MEDIA ON MEDIA.CODEID=M30_PUBLIC_SPACE.MEDIA_CD
LEFT JOIN (SELECT * FROM M30_CODE WHERE PARENT_CODE='S01G05') CATE ON CATE.CODE=M30_PUBLIC_SPACE.CATE_CD /**where**/");

            if (!string.IsNullOrEmpty(start_dt))
            {
                builder.Where("TO_DATE(:START_DT,'YYYYMMDD') <= EDITED_DTM");
            }
            if (!string.IsNullOrEmpty(end_dt))
            {
                builder.Where("EDITED_DTM < TO_DATE(:END_DT,'YYYYMMDD')+1");
            }

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


            Repository<DTO_PUBLIC_FILE> repository = new Repository<DTO_PUBLIC_FILE>();
            var resultMapping = new Func<dynamic, DTO_PUBLIC_FILE>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_PUBLIC_FILE
                {
                    Seq = Convert.ToInt64(row.SEQ),
                    MediaCD = row.MEDIA_CD,
                    MediaName = row.MEDIA_NAME,
                    CategoryCD = row.CATE_CD,
                    CatetoryName = row.CATE_NAME,
                    UserExtID = Convert.ToInt64(row.USER_EXT_ID),
                    UserName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditedDtm = ((DateTime)row.EDITED_DTM).ToString(Utility.DTM19),
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

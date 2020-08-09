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
        public DTO_PRIVATE_FILE UploadFile(IFormFile file, PrivateFileModel metaData)
        {
            long ID = GetID();
            string date = DateTime.Now.ToString(Utility.DTM8);
            string fileName = $"{ ID.ToString() }_{ file.FileName}";
            var relativeSourceFolder = $"{SystemConfig.AppSettings.FtpTmpUploadFolder}";
            var relativeTargetFolder = $"{SystemConfig.AppSettings.FtpPrivateUploadFolder}/{metaData.EditorID}/{date}";
            var relativeSourcePath = $"{relativeSourceFolder}/{fileName}";
            var relativeTargetPath = $"{relativeTargetFolder}/{fileName}";

            MyFtp.MakeFTPDir(relativeSourceFolder);
            if (MyFtp.Upload(file.OpenReadStream(), relativeSourcePath, file.Length))
            {
                MyFtp.MakeFTPDir(relativeTargetFolder);
                if (MyFtp.FtpRename(relativeSourcePath, relativeTargetPath))
                {
                    metaData.FileSize = file.Length;
                    //db에 데이터 등록
                    var builder = new SqlBuilder();
                    var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_PRIVATE_SPACE 
VALUES(:Seq, :EditorID, :Title, :Memo, :AudioFormat, :FileSize, :FilePath, 'Y', SYSDATE, NULL)");
                    Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
                    DynamicParameters param = new DynamicParameters();
                    param.AddDynamicParams(metaData);
                    repository.Insert(queryTemplate.RawSql, param);
                    return Get(ID);
                }
            }
        }
        public void UpdateData(PrivateFileModel metaData)
        {
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"INSERT INTO M30_PRIVATE_SPACE 
VALUES(:Seq, :EditorID, :Title, :Memo, :AudioFormat, :FileSize, :FilePath, 'Y', SYSDATE, NULL)");
            Repository<PrivateFileModel> repository = new Repository<PrivateFileModel>();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(metaData);
            repository.Update(queryTemplate.RawSql, param);
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
                    Seq = row.SEQ,
                    EditorID = row.USER_ID,
                    EditorName = row.USER_NAME,
                    Title = row.TITLE,
                    Memo = row.MEMO,
                    AudioFormat = row.AUDIO_FORMAT,
                    EditDtm = row.EDIT_DTM,
                    //Seq = row.FILE_SIZE,
                    //Seq = row.FILE_PATH,
                    //Seq = row.USED,
                    //Seq = row.DELETED_DTM,
                };
            });
            return repository.Get(queryTemplate.RawSql, new { SEQ=id },resultMapping);
        }
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> FineData(string filename, string title, string memo, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
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

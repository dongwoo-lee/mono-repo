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
        public bool UploadFile(IFormFile file, [FromBody] PrivateFileModel jsonMetaData)
        {
            string fileID = GetID().ToString();
            string date = DateTime.Now.ToString(Utility.DTM8);
            string fileName = $"{ fileID }_{ file.FileName}";
            var relativeSourceFolder = $"{SystemConfig.AppSettings.FtpTmpUploadFolder}";
            var relativeTargetFolder = $"{SystemConfig.AppSettings.FtpPrivateUploadFolder}/{jsonMetaData.EditorID}/{date}";
            var relativeSourcePath = $"{relativeSourceFolder}/{fileName}";
            var relativeTargetPath = $"{relativeTargetFolder}/{fileName}";

            MyFtp.MakeFTPDir(relativeSourceFolder);
            if (MyFtp.Upload(file.OpenReadStream(), relativeSourcePath, file.Length))
            {
                MyFtp.MakeFTPDir(relativeTargetFolder);
                return MyFtp.FtpRename(relativeSourcePath, relativeTargetPath);
            }
            return false;
        }
        public DTO_RESULT UpdateData(string id)
        {
            DTO_RESULT result = new DTO_RESULT();
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

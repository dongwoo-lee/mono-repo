using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class ProductsBLL
    {
        public DTO_RESULT_LIST<DTO_PGM_INFO> FindPGM(string media, string brd_dt)
        {
            DTO_RESULT_LIST<DTO_PGM_INFO> returnData = new DTO_RESULT_LIST<DTO_PGM_INFO>();
            var builder = new SqlBuilder();
            var queryTemplate = builder.AddTemplate(@"SELECT MEDIANAME, EVENTNAME, ONAIRDATE, ONAIRTIME, STATENAME, MILLISEC, EDITOR, EDITORNAME, EDITTIME, REQTIME, MASTERFILE
                     FROM
                     MEM_PROGRAM_VIEW /**where**/");
            var param = new { MEDIA = media, ONAIRDATE = brd_dt };
            builder.Where("MEDIA = :MEDIA", param);
            builder.Where("ONAIRDATE = :ONAIRDATE", param);

            Repository<DTO_PGM_INFO> repository = new Repository<DTO_PGM_INFO>();
            var resultMapping = new Func<dynamic, DTO_PGM_INFO>((row) =>
            {
                return new DTO_PGM_INFO
                {
                    MediaName = row.MEDIANAME,
                    Name = row.EVENTNAME,
                    BrdDT = row.ONAIRDATE,
                    BrdTime = row.ONAIRTIME,
                    Status = row.STATENAME,
                    Duration = row.MILLISEC,
                    UserID = row.EDITOR,
                    UserName = row.EDITORNAME,
                    EditDtm = row.EDITTIME,
                    CompleteDtm = row.REQTIME,
                    FilePath = row.MASTERFILE
                };
            });

            returnData.DataList = repository.Select(queryTemplate.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            if (firstObj != null)
            {
                returnData.TotalRowCount = firstObj.Count;
                returnData.RowPerPage = 200;
                returnData.SelectPage = 1;
            }
            return returnData;
        }

        public DTO_RESULT_LIST<DTO_SCR_SPOT> FindSCRSpot(string start_dt, string end_dt, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_LIST<DTO_SCR_SPOT> returnData = new DTO_RESULT_LIST<DTO_SCR_SPOT>();
            var builder = new SqlBuilder();
            
            var param = new 
            {
                START_DT = start_dt, END_DT = end_dt, USER = editor,
                START_NO = startNo, LAST_NO = lastNo, 
                SORTKEY = sortKey, SORTVALUE = sortValue }
            ;
            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_SPOT_SUB_VIEW /**where**/");
            builder.Select("SPOTNAME, CODENAME, MILLISEC, EDITFORMAT, ONAIRDATE, EVENTNAME, MASTERTIME, MASTERFILE, EDITOR, EDITORNAME, EDITTIME");
            builder.Where("(ONAIRDATE >= :START_DT AND ONAIRDATE <= :END_DT)");
            //if (!string.IsNullOrEmpty(name))
            //{
            //    string[] nameArray = name.Split(' ');
            //    foreach (var word in nameArray)
            //    {
            //        builder.Where($"LOWER(SPOTNAME) LIKE LOWER('%{word}%')");
            //    }
            //}

            if (!string.IsNullOrEmpty(param.USER))
            {
                builder.Where("EDITORNAME = :USER", param);
            }
            builder.OrderBy("EDITTIME DESC");

            
            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            Repository<DTO_SCR_SPOT> repository = new Repository<DTO_SCR_SPOT>();
            var resultMapping = new Func<dynamic, DTO_SCR_SPOT>((row) =>
            {
                return new DTO_SCR_SPOT
                {
                     Name= row.SPOTNAME,
                    Category = row.CODENAME,
                    Duration= row.MILLISEC,
                    Track= row.EDITFORMAT,
                    BrdDT= row.ONAIRDATE,
                    UserID= row.EDITOR,
                    PGMName= row.EVENTNAME,
                    MasteringDtm= row.MASTERTIME,
                    FilePath= row.MASTERFILE,
                    UserName = row.EDITORNAME,
                    EditDtm= row.EDITTIME,
                    Count = Convert.ToInt64(row.RESULT_COUNT)
                };
            });

            returnData.DataList = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            if (firstObj != null)
            {
                returnData.TotalRowCount = firstObj.Count;
                returnData.RowPerPage = rowPerPage;
                returnData.SelectPage = selectPage;
            }
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_REPORT> FindReport(string cate, string start_dt, string end_dt, string pgm, string editor, string reporter, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_LIST<DTO_REPORT> returnData = new DTO_RESULT_LIST<DTO_REPORT>();
            var builder = new SqlBuilder();
            var builder2 = new SqlBuilder();
            var builder3 = new SqlBuilder();
            var builder4 = new SqlBuilder();
            var param = new { CATE = cate, START_DT = start_dt, END_DT = end_dt, PGM = pgm, USER = editor, REPORTER = reporter,  START_NO = startNo, LAST_NO = lastNo, SORTKEY = sortKey, SORTVALUE = sortValue };
            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_REPORT_VIEW /**where**/");
            builder.Select("CODENAME, REPORTER, EVENTNAME, ONAIRDATE, MILLISEC, EDITFORMAT, EDITORNAME, EDITTIME, MASTERTIME, MASTERFILE");
            builder.Where("(ONAIRDATE >= :START_DT AND ONAIRDATE <= :END_DT)");
            if (!string.IsNullOrEmpty(param.CATE))
            {
                builder.Where("CODENAME=:CATE");
            }
            //if (!string.IsNullOrEmpty(name))
            //{
            //    string[] nameArray = name.Split(' ');
            //    foreach (var word in nameArray)
            //    {
            //        builder.Where($"LOWER(SPOTNAME) LIKE LOWER('%{word}%')");
            //    }
            //}
            if (!string.IsNullOrEmpty(param.PGM))
            {
                builder.Where("PRODUCTID = :PGM");
            }
            if (!string.IsNullOrEmpty(param.REPORTER))
            {
                builder.Where("REPORTER LIKE :REPORTER");
            }
            if (!string.IsNullOrEmpty(param.USER))
            {
                builder.Where("EDITOR = :USER");
            }
            builder.OrderBy("EDITTIME DESC");


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            Repository<DTO_REPORT> repository = new Repository<DTO_REPORT>();
            var resultMapping = new Func<dynamic, DTO_REPORT>((row) =>
            {
                return new DTO_REPORT
                {
                    Name = row.REPORTNAME,
                    Category = row.CODENAME,
                    Reporter = row.REPORTER,
                    PGMName = row.EVENTNAME,
                    BrdDtm = row.ONAIRDATE,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    UserID = row.EDITOR,
                    UserName = row.EDITORNAME,
                    EditDtm = row.EDITTIME,
                    MasteringDtm = row.MASTERTIME,
                    FilePath = row.MASTERFILE,
                    Count = Convert.ToInt64(row.RESULT_COUNT)
                };
            });

            returnData.DataList = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            if (firstObj != null)
            {
                returnData.TotalRowCount = firstObj.Count;
                returnData.RowPerPage = rowPerPage;
                returnData.SelectPage = selectPage;
            }
            return returnData;
        }
        //public DTO_RESULT_LIST<DTO_PRO> FindOldPro(string media, string type, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_SONG> FindMusic(int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_EFFECT> FindEffect(string searchWord, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_SB> FindMcrSB(string media, string brd_dt, string pgm) { }
        //public DTO_RESULT_LIST<DTO_SB> FindScrSB(string media, string brd_dt, string pgm) { }
        //public DTO_RESULT_LIST<DTO_CM> FindCM(string media, string brd_dt, string adcd, string pgm) { }
        //public DTO_RESULT_LIST<DTO_MCR_SPOT> FindMcrSpot(string media, string cate, string start_dt, string end_dt, string status, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_PUBLIC_FILE> FindProFiller(string media, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_PUBLIC_FILE> FindFeneralFiller(string media, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_PUBLIC_FILE> FindTimetoneFiller(string media, string start_dt, string end_dt, string status, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_PUBLIC_FILE> FindETCFiller(string media, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_DL30> FindNewDL(string media, string cate, string brd_dt) { }
    }
}

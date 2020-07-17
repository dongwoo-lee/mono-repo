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
            builder.OrderBy("ONAIRDATE, ONAIRTIME");
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
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = row.EDITTIME,
                    ReqCompleteDtm = row.REQTIME,
                    FilePath = row.MASTERFILE
                };
            });

            returnData.DataList = repository.Select(queryTemplate.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.DataList.Count;
            return returnData;
        }

        public DTO_RESULT_LIST<DTO_SCR_SPOT> FindSCRSpot(string start_dt, string end_dt, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_LIST<DTO_SCR_SPOT> returnData = new DTO_RESULT_LIST<DTO_SCR_SPOT>();
            var builder = new SqlBuilder();

            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                START_DT = start_dt,
                END_DT = end_dt,
                EDITOR = editor,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });
            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_SPOT_SUB_VIEW /**where**/");
            builder.Select("SPOTNAME, CODENAME, MILLISEC, EDITFORMAT, ONAIRDATE, EVENTNAME, MASTERTIME, MASTERFILE, EDITOR, EDITORNAME, EDITTIME");
            builder.Where("(ONAIRDATE >= :START_DT AND ONAIRDATE <= :END_DT)");
            if (!string.IsNullOrEmpty(name))
            {
                string[] nameArray = name.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(SPOTNAME) LIKE LOWER('%{word}%')");
                }
            }

            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR", param);
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
                    EditorID= row.EDITOR,
                    PGMName= row.EVENTNAME,
                    MasteringDtm= row.MASTERTIME,
                    FilePath= row.MASTERFILE,
                    EditorName = row.EDITORNAME,
                    EditDtm= row.EDITTIME,
                    Count = Convert.ToInt64(row.RESULT_COUNT)
                };
            });

            returnData.DataList = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            if (firstObj != null)
            {
                returnData.TotalRowCount = firstObj.Count;
            }
            else
            {
                returnData.TotalRowCount = 0;
            }
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_REPORT> FindReport(string cate, string start_dt, string end_dt, string pgm, string pgmName, string editor, string reporterName, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_LIST<DTO_REPORT> returnData = new DTO_RESULT_LIST<DTO_REPORT>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                CATE = cate,
                START_DT = start_dt,
                END_DT = end_dt,
                PGM = pgm,
                EDITOR = editor,
                REPORTER = reporterName,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_REPORT_VIEW /**where**/");
            builder.Select("CODENAME, REPORTER, PRODUCTID, EVENTNAME, ONAIRDATE, MILLISEC, EDITFORMAT, EDITORNAME, EDITTIME, MASTERTIME, MASTERFILE");
            builder.Where("(ONAIRDATE >= :START_DT AND ONAIRDATE <= :END_DT)");
            if (!string.IsNullOrEmpty(cate))
            {
                builder.Where("CODENAME=:CATE");
            }
            //if (!string.IsNullOrEmpty(name))
            //{
            //    string[] nameArray = name.Split(' ');
            //    foreach (var word in nameArray)
            //    {
            //        builder.Where($"LOWER(???) LIKE LOWER('%{word}%')");
            //    }
            //}
            if (!string.IsNullOrEmpty(pgm))
            {
                builder.Where("PRODUCTID = :PGM");
            }
            else
            {
                if (!string.IsNullOrEmpty(pgmName))
                {
                    string[] nameArray = pgmName.Split(' ');
                    foreach (var word in nameArray)
                    {
                        builder.Where($"LOWER(EVENTNAME) LIKE LOWER('%{word}%')");
                    }
                }
            }
            if (!string.IsNullOrEmpty(reporterName))
            {
                builder.Where($"LOWER(REPORTER) LIKE LOWER('%{reporterName}%')");   //리포터는 등록된 사용자가 아니므로 이름검색.
            }
            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR");
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
                    PGMID= row.PRODUCTID,
                    PGMName = row.EVENTNAME,
                    BrdDtm = row.ONAIRDATE,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = row.EDITTIME,
                    MasteringDtm = row.MASTERTIME,
                    FilePath = row.MASTERFILE,
                    Count = Convert.ToInt64(row.RESULT_COUNT)
                };
            });

            returnData.DataList = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            if (firstObj != null)
            {
                returnData.TotalRowCount = firstObj.Count;
            }
            else
            {
                returnData.TotalRowCount = 0;
            }
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_PRO> FindOldPro(string media, string cate, string type, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_LIST<DTO_PRO> returnData = new DTO_RESULT_LIST<DTO_PRO>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new 
            {
                MEDIA = media,
                CATE = cate,
                TYPE= type,
                EDITOR = editor,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_PROAUDIOFILE_VIEW /**where**/");
            builder.Select("AUDIOID, AUDIONAME, CODENAME, MILLISEC, EDITFORMAT, EDITOR, EDITORNAME, EDITTIME, MASTERTIME, TYPENAME, MASTERFILE");
            //if (!string.IsNullOrEmpty(media))
            //{
            //    builder.Where("?=:MEDIA");
            //}
            if (!string.IsNullOrEmpty(cate))
            {
                builder.Where("AUDIOID=:CATE");
            }
            //if (!string.IsNullOrEmpty(cate))
            //{
            //    builder.Where("?=:TYPE");   // 방송//폐지
            //}
            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR");
            }
            if (!string.IsNullOrEmpty(name))
            {
                string[] nameArray = name.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(AUDIONAME) LIKE LOWER('%{word}%')");
                }
            }

            builder.OrderBy("EDITTIME DESC");


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            Repository<DTO_PRO> repository = new Repository<DTO_PRO>();
            var resultMapping = new Func<dynamic, DTO_PRO>((row) =>
            {
                return new DTO_PRO
                {
                    CategoryID = row.AUDIOID,
                    Category = row.AUDIONAME,
                    Name = row.CODENAME,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = row.EDITTIME,
                    MasteringDtm = row.MASTERTIME,
                    ProType = row.TYPENAME,
                    FilePath = row.MASTERFILE,
                    Count = Convert.ToInt64(row.RESULT_COUNT)
                };
            });

            returnData.DataList = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            if (firstObj != null)
            {
                returnData.TotalRowCount = firstObj.Count;
            }
            else
            {
                returnData.TotalRowCount = 0;
            }
            return returnData;
        }
        //public DTO_RESULT_LIST<DTO_SONG> FindMusic(int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_EFFECT> FindEffect(string searchWord, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        public DTO_RESULT_LIST<DTO_SB> FindMcrSB(string viewName,string media, string brd_dt, string pgm, string pgmName) 
        {
            DTO_RESULT_LIST<DTO_SB> returnData = new DTO_RESULT_LIST<DTO_SB>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA = media,
                BRD_DT = brd_dt,
                PGM = pgm,
            });

            var querySource = builder.AddTemplate($"SELECT * FROM MEM_SB_{viewName}_VIEW /**where**/");
            if (!string.IsNullOrEmpty(media))
            {
                builder.Where("MEDIA=:MEDIA");
            }
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("ONAIRDATE = :BRD_DT");   // 방송//폐지
            }
            if (!string.IsNullOrEmpty(pgm))
            {
                builder.Where("PRODUCTID = :PGM");
            }
            else
            {
                if (!string.IsNullOrEmpty(pgmName))
                {
                    string[] nameArray = pgmName.Split(' ');
                    foreach (var word in nameArray)
                    {
                        builder.Where($"LOWER(EVENTNAME) LIKE LOWER('%{word}%')");
                    }
                }
            }
            builder.OrderBy("SBID ASC");

            Repository<DTO_SB> repository = new Repository<DTO_SB>();
            var resultMapping = new Func<dynamic, DTO_SB>((row) =>
            {
                return new DTO_SB
                {
                    BrdDT = row.ONEAIRDATE,
                    ID = row.SBID,
                    Name = row.SBNAME,
                    Length = row.DURSEC,
                    Capacity=row.CAPACITY,
                    Status = row.STATENAME,
                    PGMName =row.EVENTNAME ,
                    EditorID=row.EDITOR,
                    EditorName = row.EDITORNAME,
                };
            });

            returnData.DataList = repository.Select(querySource.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.DataList.Count;
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_SB_CONTENT> FindSBDetail(string brd_dt, string sbID)
        {
            DTO_RESULT_LIST<DTO_SB_CONTENT> returnData = new DTO_RESULT_LIST<DTO_SB_CONTENT>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                BRD_DT = brd_dt,
                SBID = sbID,
            });

            var querySource = builder.AddTemplate(@"SELECT * FROM MEM_SB_CLIP_VIEW /**where**/ /**orderby**/");
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("ONAIRDATE = :BRD_DT");   
            }
            if (!string.IsNullOrEmpty(sbID))
            {
                builder.Where("SBGROUPID = :SBID");
            }
            builder.OrderBy("NUM, CMSEQNUM");

            Repository<DTO_SB_CONTENT> repository = new Repository<DTO_SB_CONTENT>();
            var resultMapping = new Func<dynamic, DTO_SB_CONTENT>((row) =>
            {
                return new DTO_SB_CONTENT
                {
                    Order = row.NUM,
                    CategoryCode = row.CLIPTYPE,
                    CategoryName = row.OWNER,
                    ID = row.CLIPID,
                    Name = row.CLIPNAME,
                    Length = row.CLIPSEC,
                    Format = "",
                };
            });

            returnData.DataList = repository.Select(querySource.RawSql, param, resultMapping);
            var firstObj = returnData.DataList.FirstOrDefault();
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.DataList.Count;
            return returnData;
        }

        //public DTO_RESULT_LIST<DTO_CM> FindCM(string media, string brd_dt, string adcd, string pgm) { }
        //public DTO_RESULT_LIST<DTO_CM> FindCMDetail(string media, string brd_dt, string adcd, string pgm) { }
        //public DTO_RESULT_LIST<DTO_MCR_SPOT> FindMcrSpot(string media, string cate, string start_dt, string end_dt, string status, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_PUBLIC_FILE> FindProFiller(string media, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_PUBLIC_FILE> FindFeneralFiller(string media, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_PUBLIC_FILE> FindTimetoneFiller(string media, string start_dt, string end_dt, string status, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_PUBLIC_FILE> FindETCFiller(string media, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_DL30> FindNewDL(string media, string cate, string brd_dt) { }
    }
}

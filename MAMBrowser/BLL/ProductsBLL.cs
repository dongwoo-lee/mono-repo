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
            var queryTemplate = builder.AddTemplate(@"SELECT ROWNUM AS RNO, MEDIANAME, EVENTNAME, ONAIRDATE, ONAIRTIME, STATENAME, MILLISEC, EDITOR, EDITORNAME, EDITTIME, REQTIME, MASTERFILE
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
                    RowNO = Convert.ToInt32(row.RNO),
                    MediaName = row.MEDIANAME,
                    Name = row.EVENTNAME,
                    BrdDT = row.ONAIRDATE,
                    BrdTime = row.ONAIRTIME,
                    Status = row.STATENAME,
                    Duration = row.MILLISEC,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Utility.DTM19),
                    ReqCompleteDtm = ((DateTime)row.REQTIME).ToString(Utility.DTM19),
                    FilePath = row.MASTERFILE
                };
            });

            returnData.Data = repository.Select(queryTemplate.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
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
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_SCR_SPOT
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Name = row.SPOTNAME,
                    CategoryName = row.CODENAME,
                    Duration= row.MILLISEC,
                    Track= row.EDITFORMAT,
                    BrdDT= row.ONAIRDATE,
                    EditorID= row.EDITOR,
                    PGMName= row.EVENTNAME,
                    MasteringDtm= row.MASTERTIME,
                    FilePath= row.MASTERFILE,
                    EditorName = row.EDITORNAME,
                    EditDtm= row.EDITTIME,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
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
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_REPORT
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Name = row.REPORTNAME,
                    CategoryName = row.CODENAME,
                    Reporter = row.REPORTER,
                    PGMID= row.PRODUCTID,
                    PGMName = row.EVENTNAME,
                    BrdDtm = row.ONAIRDATE,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Utility.DTM19),
                    MasteringDtm = row.MASTERTIME,
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
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
            //    builder.Where("?=:MEDIA");        ???매체 정보 없음;
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
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_PRO
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    CategoryID = row.AUDIOID,
                    CategoryName = row.AUDIONAME,
                    Name = row.CODENAME,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Utility.DTM19),
                    MasteringDtm = row.MASTERTIME,
                    ProType = row.TYPENAME,
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        //public DTO_RESULT_LIST<DTO_SONG> FindMusic(int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        //public DTO_RESULT_LIST<DTO_EFFECT> FindEffect(string searchWord, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        public DTO_RESULT_LIST<DTO_SB> FindSB(string viewName,string media, string brd_dt, string pgm, string pgmName) 
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

            var querySource = builder.AddTemplate($"SELECT /**select**/ FROM MEM_SB_{viewName}_VIEW /**where**/");
            builder.Select("ROWNUM AS RNO, ONEAIRDATE,SBID,SBNAME,DURSEC,CAPACITY,STATENAME,EVENTNAME ,EDITOR,EDITORNAME");
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
                    RowNO = Convert.ToInt32(row.RNO),
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

            returnData.Data = repository.Select(querySource.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
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
                    RowNO = row.NUM,
                    CategoryID = row.CLIPTYPE,
                    CategoryName = row.OWNER,
                    ID = row.CLIPID,
                    Name = row.CLIPNAME,
                    Length = row.CLIPSEC,
                    Format = "",
                };
            });

            returnData.Data = repository.Select(querySource.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }

        public DTO_RESULT_LIST<DTO_CM> FindCM(string media, string brd_dt, string cate, string pgm)
        {
            DTO_RESULT_LIST<DTO_CM> returnData = new DTO_RESULT_LIST<DTO_CM>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA = media,
                BRD_DT = brd_dt,
                CATE= cate,
                PGM = pgm,
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_CM_GROUP_VIEW /**where**/ /**orderby**/");
            builder.Select("ROWNUM AS RNO, MEDIA, ONAIRDATE, CMGROUPNAME, CMGROUPID, PROID, DURSEC, CMCAPACITY, STATENAME, EDITOR, EDITORNAME, EDITTIME");
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
                builder.Where("PROID = :PGM");
            }
            builder.OrderBy("MEDIA, ONAIRTIME, CMGROUPNAME");

            Repository<DTO_CM> repository = new Repository<DTO_CM>();
            var resultMapping = new Func<dynamic, DTO_CM>((row) =>
            {
                return new DTO_CM
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    BrdDT = row.ONAIRDATE,
                    ID = row.CMGROUPID,
                    Name = row.CMGROUPNAME,
                    Length = row.DURSEC,
                    Capacity = row.CMCAPACITY,
                    Status = row.STATENAME,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Utility.DTM19),
                };
            });

            returnData.Data = repository.Select(querySource.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }

        public DTO_RESULT_LIST<DTO_CM_CONTENT> FindCMDetail(string brd_dt, string cmGrpID)
        {
            DTO_RESULT_LIST<DTO_CM_CONTENT> returnData = new DTO_RESULT_LIST<DTO_CM_CONTENT>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                BRD_DT = brd_dt,
                CM_GRP_ID = cmGrpID,
            });

            var querySource = builder.AddTemplate(@"SELECT * FROM MEM_CM_CLIP_VIEW /**where**/ /**orderby**/");
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("ONAIRDATE = :BRD_DT");
            }
            if (!string.IsNullOrEmpty(cmGrpID))
            {
                builder.Where("CMGROUPID = :CM_GRP_ID");
            }
            builder.OrderBy("NUM");

            Repository<DTO_CM_CONTENT> repository = new Repository<DTO_CM_CONTENT>();

            var resultMapping = new Func<dynamic, DTO_CM_CONTENT>((row) =>
            {
                return new DTO_CM_CONTENT
                {

                    RowNO = row.NUM,
                    Advertiser = row.CMOWNER,
                    Name = row.CMNAME,
                    ID = row.CMCLIPID,
                    Length = row.CLIPSEC,
                    CodingUserID = row.REGISTER,
                    CodingUserName = row.REGISTERNAME,
                    CodingDT = row.REGISTDATE,
                    Format = row.FORMAT,
                    FilePath = row.MASTERFILE
                };
            });

            returnData.Data = repository.Select(querySource.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_MCR_SPOT> FindMcrSpot(string media, string cate, string start_dt, string end_dt, string status, string editor, string editorName, int rowPerPage, int selectPage, string sortKey, string sortValue) 
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_LIST<DTO_MCR_SPOT> returnData = new DTO_RESULT_LIST<DTO_MCR_SPOT>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA = media,
                CATE = cate,
                START_DT = start_dt,
                END_DT = end_dt,
                STATUS = status,
                EDITOR = editor,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_SPOT_MAIN_VIEW /**where**/");
            builder.Select(@"MEDIANAME, SPOTID, SPOTNAME, ONAIRDATE, STATENAME, MILLISEC, EDITFORMAT, EDITOR, EDITORNAME, EDITTIME, REQTIME, MASTERFILE");
            builder.Where("(ONAIRDATE >= :START_DT AND ONAIRDATE <= :END_DT)");
            if (!string.IsNullOrEmpty(media))
            {
                builder.Where("MEDIA=:MEDIA");
            }
            if (!string.IsNullOrEmpty(cate))
            {
                builder.Where("SPOTID=:CATE");
            }
            if (!string.IsNullOrEmpty(cate))
            {
                builder.Where("STATUS=:STATUS");   
            }
            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR");
            }
            else if(!string.IsNullOrEmpty(editorName))
            {
                string[] nameArray = editorName.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(EDITORNAME) LIKE LOWER('%{word}%')");
                }
            }

            builder.OrderBy("EDITTIME DESC");


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            Repository<DTO_MCR_SPOT> repository = new Repository<DTO_MCR_SPOT>();
            var resultMapping = new Func<dynamic, DTO_MCR_SPOT>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_MCR_SPOT
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    MediaName = row.MEDIANAME,
                    ID = row.SPOTID,
                    Name = row.SPOTNAME,
                    BrdDT = row.ONAIRDATE,
                    Status = row.STATENAME,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Utility.DTM19),
                    ReqCompleteDtm = ((DateTime)row.REQTIME).ToString(Utility.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }

        public DTO_RESULT_LIST<DTO_FILLER> FindFiller(string viewName, string brd_dt, string cate, string editor, string editorName, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) 
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_LIST<DTO_FILLER> returnData = new DTO_RESULT_LIST<DTO_FILLER>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                CATE = cate,
                BRD_DT = brd_dt,
                EDITOR = editor,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });

            var querySource = builder.AddTemplate($"SELECT /**select**/ FROM {viewName} /**where**/ /**orderby**/");
            builder.Select(@"FILLERID, FILLERNAME, CODEID, CODENAME, ENDDATE, MILLISEC, EDITFORMAT, EDITOR, EDITORNAME, EDITTIME, MASTERTIME, MASTERFILE");
            builder.Where("ENDDATE >= :BRD_DT");
            if (!string.IsNullOrEmpty(cate))
            {
                builder.Where("CODEID=:CATE");
            }
            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR");
            }
            else if (!string.IsNullOrEmpty(editorName))
            {
                string[] nameArray = editorName.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(EDITORNAME) LIKE LOWER('%{word}%')");
                }
            }
            if (!string.IsNullOrEmpty(name))
            {
                string[] nameArray = name.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(FILLERNAME) LIKE LOWER('%{word}%')");
                }
            }

            builder.OrderBy("EDITTIME DESC");


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            Repository<DTO_FILLER> repository = new Repository<DTO_FILLER>();
            var resultMapping = new Func<dynamic, DTO_FILLER>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_FILLER
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    ID = row.FILLERID,
                    Name = row.FILLERNAME,
                    BrdDT = row.ENDDATE,
                    CategoryID = row.CODEID,
                    CategoryName = row.CODENAME,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Utility.DTM19),
                    MasteringDtm = ((DateTime)row.MASTERTIME).ToString(Utility.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public DTO_RESULT_LIST<DTO_FILLER_TIME> FindFillerTime(string media, string start_dt, string end_dt, string cate, string status, string editor, string editorName, string name, int rowPerPage, int selectPage, string sortKey, string sortValue) 
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_LIST<DTO_FILLER_TIME> returnData = new DTO_RESULT_LIST<DTO_FILLER_TIME>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA= media,
                CATE = cate,
                START_DT = start_dt,
                END_DT = end_dt,
                STATUS = status,
                EDITOR = editor,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });

            var querySource = builder.AddTemplate($"SELECT /**select**/ FROM MEM_FILLER_TIME_VIEW /**where**/ /**orderby**/");
            builder.Select(@"MEDIA, MEDIANAME, SPOTID, SPOTNAME, STARTDATE, ENDDATE, STATEID, STATENAME, MILLISEC, EDITFORMAT, EDITOR, EDITORNAME, EDITTIME, REQTIME, MASTERFILE");
            builder.Where("MEDIA=:MEDIA");
            builder.Where("(STARTDATE >= :START_DT AND ENDDATE <= :END_DT)");
            if (!string.IsNullOrEmpty(cate))
            {
                builder.Where("SPOTTYPE=:CATE");
            }
            if (!string.IsNullOrEmpty(status))
            {
                builder.Where("STATEID=:STATUS");
            }
            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR");
            }
            else if (!string.IsNullOrEmpty(editorName))
            {
                string[] nameArray = editorName.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(EDITORNAME) LIKE LOWER('%{word}%')");
                }
            }
            if (!string.IsNullOrEmpty(name))
            {
                string[] nameArray = name.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(SPOTNAME) LIKE LOWER('%{word}%')");
                }
            }

            builder.OrderBy("EDITTIME DESC");


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            Repository<DTO_FILLER_TIME> repository = new Repository<DTO_FILLER_TIME>();
            var resultMapping = new Func<dynamic, DTO_FILLER_TIME>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_FILLER_TIME
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    MediaName = row.MEDIANAME,
                    ID = row.SPOTID,
                    Name = row.SPOTNAME,
                    CategoryID = row.SPOTTYPE,
                    StartDT = row.STARTDATE,
                    EndDT = row.ENDDATE,
                    Status = row.STATENAME,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Utility.DTM19),
                    ReqCompleteDtm = ((DateTime)row.REQTIME).ToString(Utility.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        //public DTO_RESULT_LIST<DTO_DL30> FindNewDL(string media, string cate, string brd_dt) 
        //{
        //}
    }
}

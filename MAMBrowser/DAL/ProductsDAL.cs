using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class ProductsDAL
    {
        private readonly AppSettings _appSesstings;
        public ProductsDAL(IOptions<AppSettings> appSesstings)
        {
            _appSesstings = appSesstings.Value;
        }
        public DTO_RESULT_PAGE_LIST<DTO_PGM_INFO> FindPGM(string media, string brd_dt, string pgm, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST<DTO_PGM_INFO> returnData = new DTO_RESULT_PAGE_LIST<DTO_PGM_INFO>();
            var builder = new SqlBuilder();
            var querySource = builder.AddTemplate(@"SELECT MEDIANAME, EVENTNAME, ONAIRDATE, ONAIRTIME, STATENAME, MILLISEC, EDITOR, EDITORNAME, EDITTIME, REQTIME, MASTERFILE
                     FROM
                     MEM_PROGRAM_VIEW /**where**/");
            var param = new {
                MEDIA = media, 
                ONAIRDATE = brd_dt, 
                PDQ_PRODUCTID = pgm, 
                EDITOR = editor,
                START_NO = startNo,
                LAST_NO = lastNo,
            };
            builder.Where("MEDIA = :MEDIA");
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("ONAIRDATE = :ONAIRDATE");
            }
            if (!string.IsNullOrEmpty(pgm))
            {
                builder.Where("PDQ_PRODUCTID = :PDQ_PRODUCTID");
            }
            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR");
            }
            builder.OrderBy("ONAIRDATE, ONAIRTIME");

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");


            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_PGM_INFO>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(MAMUtility.DTM19),
                    ReqCompleteDtm = row.REQTIME != null ? ((DateTime)row.REQTIME).ToString(MAMUtility.DTM19) : null,
                    FilePath = row.MASTERFILE
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }

        public DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT> FindSCRSpot(string media, string start_dt, string end_dt, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT> returnData = new DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT>();
            var builder = new SqlBuilder();

            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA = media,
                START_DT = start_dt,
                END_DT = end_dt,
                EDITOR = editor,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });
            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_SPOT_SUB_VIEW /**where**/");
            builder.Select("MEDIA, MEDIANAME, SPOTNAME, CODENAME, MILLISEC, EDITFORMAT, ONAIRDATE, EVENTNAME, MASTERTIME, MASTERFILE, EDITOR, EDITORNAME, EDITTIME");
            builder.Where("MEDIA=:MEDIA");
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

            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_SCR_SPOT>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_SCR_SPOT
                {
                    MediaCD = row.MEDIA,
                    MediaName = row.MEDIANAME,
                    RowNO = Convert.ToInt32(row.RNO),
                    Name = row.SPOTNAME,
                    CategoryName = row.CODENAME,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    BrdDT = row.ONAIRDATE,
                    EditorID = row.EDITOR,
                    PGMName = row.EVENTNAME,
                    MasteringDtm = row.MASTERTIME == null ? "" : ((DateTime)row.MASTERTIME).ToString(MAMUtility.DTM19),
                    FilePath = row.MASTERFILE,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(MAMUtility.DTM19),
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public DTO_RESULT_PAGE_LIST<DTO_REPORT> FindReport(string cate, string start_dt, string end_dt, string pgmName, string editor, string reporterName, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST<DTO_REPORT> returnData = new DTO_RESULT_PAGE_LIST<DTO_REPORT>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                CATE = cate,
                START_DT = start_dt,
                END_DT = end_dt,
                PGM = pgmName,
                EDITOR = editor,
                REPORTER = reporterName,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_REPORT_VIEW /**where**/");
            builder.Select("REPORTNAME, CODENAME, REPORTER, PRODUCTID, EVENTNAME, ONAIRDATE, MILLISEC, EDITFORMAT, EDITORNAME, EDITTIME, MASTERTIME, MASTERFILE");
            builder.Where("(ONAIRDATE >= :START_DT AND ONAIRDATE <= :END_DT)");
            if (!string.IsNullOrEmpty(cate))
            {
                builder.Where("CODEID=:CATE");
            }
            if (!string.IsNullOrEmpty(pgmName))
            {
                string[] nameArray = pgmName.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(EVENTNAME) LIKE LOWER('%{word}%')");
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

            Repository repository = new Repository();
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
                    PGMID = row.PRODUCTID,
                    PGMName = row.EVENTNAME,
                    BrdDT = row.ONAIRDATE,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(MAMUtility.DTM19),
                    MasteringDtm = row.MASTERTIME == null ? "" : ((DateTime)row.MASTERTIME).ToString(MAMUtility.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public DTO_RESULT_PAGE_LIST<DTO_PRO> FindOldPro(string media, string cate, string type, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST<DTO_PRO> returnData = new DTO_RESULT_PAGE_LIST<DTO_PRO>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA = media,
                CATE = cate,
                TYPE = type,
                EDITOR = editor,
                START_NO = startNo,
                LAST_NO = lastNo,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM MEM_PROAUDIOFILE_VIEW /**where**/");
            builder.Select("AUDIOID, AUDIONAME, CODENAME, MILLISEC, EDITFORMAT, EDITOR, EDITORNAME, EDITTIME, MASTERTIME, TYPENAME, MASTERFILE");
            //if (!string.IsNullOrEmpty(media))  //DB단 매체 필드가 없어서 조건으로 넣을 수 없음.
            //{
            //    builder.Where("MEDIA=:MEDIA");      
            //}
            if (!string.IsNullOrEmpty(cate))
            {
                builder.Where("CODEID=:CATE");
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (type == "Y")
                {
                    builder.Where("CODENAME NOT LIKE '%(폐지)%'");   // 방송//폐지    -> LIKE문 말고 다른걸로...
                }
                else
                {
                    builder.Where("CODENAME LIKE '%(폐지)%'");   // 방송//폐지    -> LIKE문 말고 다른걸로...
                }
            }
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

            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_PRO>((row) =>
            {
                if (returnData.TotalRowCount == 0)
                {
                    returnData.TotalRowCount = Convert.ToInt32(row.RESULT_COUNT);
                }
                return new DTO_PRO
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    CategoryID = row.CODEID,
                    CategoryName = row.CODENAME,
                    Name = row.AUDIONAME,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(MAMUtility.DTM19),
                    MasteringDtm = row.MASTERTIME == null ? "" : ((DateTime)row.MASTERTIME).ToString(MAMUtility.DTM19),
                    ProType = row.TYPENAME,
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        //public DTO_RESULT_LIST<DTO_SONG> FindMusic(int rowPerPage, int selectPage, string sortKey, string sortValue) 
        //{
        //    //var data = netDrivePath.Split(Path.DirectorySeparatorChar);
        //    //var hostName = data[2];
        //    //var albumDomain = $@"http://{hostName}.{mbcDomain}";
        //}
        //public DTO_RESULT_LIST<DTO_SONG> GetAlbumImage(string directory, string fileName)
        //{
        //    var scraps = directory.Split(Path.DirectorySeparatorChar);
        //    var hostName = scraps[2];
        //    var albumUrl = $@"http://{hostName}.{_appSesstings.Value.MbcDomain}/{string.Join("/", scraps.Skip(3).ToList())}";
        //    HttpClient client = new HttpClient();

        //}
        //public DTO_RESULT_LIST<DTO_SONG> GetAlbumImagePathList(string directory, string fileName)
        //{
        //    var filePath = $@"{directory}\{fileName}";
        //    var hostName = scraps[2];
        //    var albumUrl = $@"http://{hostName}.{_appSesstings.Value.MbcDomain}/{string.Join("/", scraps.Skip(3).ToList())}";
        //    HttpClient client = new HttpClient();
        //    //client.
        //    return null;
        //}

        //public DTO_RESULT_LIST<DTO_EFFECT> FindEffect(string searchWord, int rowPerPage, int selectPage, string sortKey, string sortValue) { }
        public DTO_RESULT_PAGE_LIST<DTO_SB> FindSB(string viewName, string media, string brd_dt, string pgm)
        {
            DTO_RESULT_PAGE_LIST<DTO_SB> returnData = new DTO_RESULT_PAGE_LIST<DTO_SB>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA = media,
                BRD_DT = brd_dt,
                PGM = pgm,
            });

            var querySource = builder.AddTemplate($"SELECT /**select**/ FROM MEM_SB_{viewName}_VIEW /**where**/");
            builder.Select("ROWNUM AS RNO, ONAIRDATE,SBID,SBNAME,DURSEC,CAPACITY,STATENAME,EVENTNAME ,EDITOR,EDITORNAME");
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
            builder.OrderBy("SBID ASC");

            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_SB>((row) =>
            {
                return new DTO_SB
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    BrdDT = row.ONAIRDATE,
                    ID = row.SBID,
                    Name = row.SBNAME,
                    Length = row.DURSEC,
                    Capacity = row.CAPACITY,
                    Status = row.STATENAME,
                    PGMName = row.EVENTNAME,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                };
            });

            returnData.Data = repository.Select(querySource.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }
        public DTO_RESULT_PAGE_LIST<DTO_SB_CONTENT> FindSBContents(string brd_dt, string sbID)
        {
            DTO_RESULT_PAGE_LIST<DTO_SB_CONTENT> returnData = new DTO_RESULT_PAGE_LIST<DTO_SB_CONTENT>();
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

            Repository repository = new Repository();
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
                    FilePath = row.MASTERFILE,
                    Format = "",
                };
            });

            returnData.Data = repository.Select(querySource.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }

        public DTO_RESULT_PAGE_LIST<DTO_CM> FindCM(string media, string brd_dt, string cate, string pgmName)
        {
            DTO_RESULT_PAGE_LIST<DTO_CM> returnData = new DTO_RESULT_PAGE_LIST<DTO_CM>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA = media,
                BRD_DT = brd_dt,
                CATE = cate,
            });

            var querySource = builder.AddTemplate(@"SELECT ROWNUM AS RNO, A.* FROM (SELECT /**select**/ FROM MEM_CM_GROUP_VIEW /**where**/ /**orderby**/) A");
            builder.Select("MEDIA, ONAIRDATE, CMGROUPNAME, CMGROUPID, PROID, DURSEC, CMCAPACITY, STATENAME, EDITOR, EDITORNAME, EDITTIME");
            if (!string.IsNullOrEmpty(media))
            {
                builder.Where("MEDIA=:MEDIA");
            }
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("PROTYPE = :CATE");   // 방송//폐지
            }
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("ONAIRDATE = :BRD_DT");
            }
            if (!string.IsNullOrEmpty(pgmName))
            {
                string[] nameArray = pgmName.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(CMGROUPNAME) LIKE LOWER('%{word}%')");
                }
            }

            builder.OrderBy("MEDIA, ONAIRTIME, CMGROUPNAME");

            Repository repository = new Repository();
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(MAMUtility.DTM19),
                };
            });

            returnData.Data = repository.Select(querySource.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }

        public DTO_RESULT_PAGE_LIST<DTO_CM_CONTENT> FindCMContents(string brd_dt, string cmGrpID)
        {
            DTO_RESULT_PAGE_LIST<DTO_CM_CONTENT> returnData = new DTO_RESULT_PAGE_LIST<DTO_CM_CONTENT>();
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

            Repository repository = new Repository();

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
        public DTO_RESULT_PAGE_LIST<DTO_MCR_SPOT> FindMcrSpot(string media, string start_dt, string end_dt, string spotId, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST<DTO_MCR_SPOT> returnData = new DTO_RESULT_PAGE_LIST<DTO_MCR_SPOT>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA = media,
                START_DT = start_dt,
                END_DT = end_dt,
                SPOTID = spotId,
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
            if (!string.IsNullOrEmpty(spotId))
            {
                builder.Where("SPOTID=:SPOTID");
            }
            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR");
            }

            builder.OrderBy("EDITTIME DESC");


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            Repository repository = new Repository();
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(MAMUtility.DTM19),
                    ReqCompleteDtm = ((DateTime)row.REQTIME).ToString(MAMUtility.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }

        public DTO_RESULT_PAGE_LIST<DTO_FILLER> FindFiller(string viewName, string brd_dt, string cate, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST<DTO_FILLER> returnData = new DTO_RESULT_PAGE_LIST<DTO_FILLER>();
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
            if (!string.IsNullOrEmpty(name))
            {
                string[] nameArray = name.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(FILLERNAME) LIKE LOWER('%{word}%')");
                }
            }

            builder.OrderBy("MASTERTIME DESC");


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            Repository repository = new Repository();
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(MAMUtility.DTM19),
                    MasteringDtm = row.MASTERTIME == null ? "" : ((DateTime)row.MASTERTIME).ToString(MAMUtility.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public DTO_RESULT_PAGE_LIST<DTO_FILLER_TIME> FindFillerTime(string media, string start_dt, string end_dt, string cate, string status, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST<DTO_FILLER_TIME> returnData = new DTO_RESULT_PAGE_LIST<DTO_FILLER_TIME>();
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

            var querySource = builder.AddTemplate($"SELECT /**select**/ FROM MEM_FILLER_TIME_VIEW /**where**/ /**orderby**/");
            builder.Select(@"MEDIA, MEDIANAME, SPOTID, SPOTNAME, STARTDATE, ENDDATE, STATEID, STATENAME, MILLISEC, EDITFORMAT, EDITOR, EDITORNAME, EDITTIME, REQTIME, MASTERTIME, MASTERFILE");
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

            Repository repository = new Repository();
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(MAMUtility.DTM19),
                    MasteringDtm = row.MASTERTIME != null ? ((DateTime)row.MASTERTIME).ToString(MAMUtility.DTM19) : null,
                    FilePath = row.MASTERFILE,
                    FileName = Path.GetFileName(row.MASTERFILE)
                };
            });

            returnData.Data = repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public DTO_RESULT_PAGE_LIST<DTO_DL30> FineDLArchive(string media, string brd_dt, string name, string sortKey, string sortValue)
        {
            DTO_RESULT_PAGE_LIST<DTO_DL30> returnData = new DTO_RESULT_PAGE_LIST<DTO_DL30>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA_CD = media,
                SCH_DATE = brd_dt,
                REC_NAME = name,
                SORTKEY = sortKey,
                SORTVALUE = sortValue
            });

            var querySource = builder.AddTemplate(@$"SELECT ROWNUM AS RNO, D.* FROM (SELECT ROWNUM AS RNO, A.*, B.FILE_EXT, B.FILE_SIZE, B.REG_DTM, B.DAMS_ID, C.DEVICE_NAME FROM M30_DL_ARCHIVE A
INNER JOIN M30_DL_ARCHIVE_FILE B ON B.ARCHIVE_SEQ=A.SEQ AND B.FILE_EXT='WAV'
LEFT JOIN M30_DL_DEVICE C ON C.SEQ = A.DEVICE_SEQ
/**where**/
ORDER BY BRD_DTM) D");
            if (!string.IsNullOrEmpty(media))
            {
                builder.Where("MEDIA_CD=:MEDIA_CD");
            }
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("SCH_DATE=:SCH_DATE");
            }
            if (!string.IsNullOrEmpty(name))
            {
                string[] nameArray = name.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(REC_NAME) LIKE LOWER('%{word}%')");
                }
            }

            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_DL30>((row) =>
            {
                return new DTO_DL30
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Seq = Convert.ToInt64(row.SEQ),
                    DeviceSeq = Convert.ToInt64(row.DEVICE_SEQ),
                    MediaCD = row.MEDIA_CD,
                    MediaName = row.MEDIA_CD,
                    SchDate = row.SCH_DATE,
                    BrdDate = ((DateTime)row.BRD_DTM).ToString(MAMUtility.DTM19),
                    ProgramID = row.PRODUCT_ID,
                    SourceID = row.SOURCE_ID,
                    RecName = row.REC_NAME,
                    Duration = Convert.ToInt32(row.LENGTH),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = $"{row.FILE_PATH}/{row.SOURCE_ID}.{row.FILE_EXT}",
                    RegDtm = ((DateTime)row.REG_DTM).ToString(MAMUtility.DTM19),
                    DeviceName = row.DEVICE_NAME,
                };
            });

            returnData.Data = repository.Select(querySource.RawSql, param, resultMapping);
            returnData.RowPerPage = 200;
            returnData.SelectPage = 1;
            returnData.TotalRowCount = returnData.Data.Count;
            return returnData;
        }
        public DTO_DL30 GetDLArchive(long seq)
        {
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                SEQ = seq,
            });

            var querySource = builder.AddTemplate(@$"SELECT A.*, B.FILE_EXT, B.FILE_SIZE, B.REG_DTM, B.DAMS_ID, C.DEVICE_NAME FROM M30_DL_ARCHIVE A
INNER JOIN M30_DL_ARCHIVE_FILE B ON B.ARCHIVE_SEQ=A.SEQ AND B.FILE_EXT='WAV'
LEFT JOIN M30_DL_DEVICE C ON C.SEQ = A.DEVICE_SEQ
WHERE A.SEQ := SEQ
ORDER BY BRD_DTM");

            Repository repository = new Repository();
            var resultMapping = new Func<dynamic, DTO_DL30>((row) =>
            {
                return new DTO_DL30
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    Seq = Convert.ToInt64(row.SEQ),
                    DeviceSeq = Convert.ToInt64(row.DEVICE_SEQ),
                    MediaCD = row.MEDIA_CD,
                    MediaName = row.MEDIA_CD,
                    SchDate = row.SCH_DATE,
                    BrdDate = ((DateTime)row.BRD_DTM).ToString(MAMUtility.DTM19),
                    ProgramID = row.PRODUCT_ID,
                    SourceID = row.SOURCE_ID,
                    RecName = row.REC_NAME,
                    Duration = Convert.ToInt32(row.LENGTH),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = $"{row.FILE_PATH}/{row.SOURCE_ID}.{row.FILE_EXT}", 
                    RegDtm = ((DateTime)row.REG_DTM).ToString(MAMUtility.DTM19),
                    DeviceName = row.DEVICE_NAME,
                };
            });

           return repository.Get(querySource.RawSql, param, resultMapping);
        }
    }
}

using Dapper;
using MAMBrowser.Common;
using MAMBrowser.DTO;
using System;
using System.IO;

namespace MAMBrowser.DAL
{
    public class ProductsDao
    {
        private readonly Repository _repository;
        public ProductsDao(Repository repository)
        {
            _repository = repository;
        }
        public DTO_RESULT_PAGE_LIST<DTO_PGM_INFO> FindPGM(string media, string brd_dt, string pgm, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            int startNo = (rowPerPage * selectPage) - (rowPerPage - 1);
            int lastNo = startNo + rowPerPage;

            DTO_RESULT_PAGE_LIST <DTO_PGM_INFO> returnData = new DTO_RESULT_PAGE_LIST<DTO_PGM_INFO>();
            var builder = new SqlBuilder();
            var querySource = builder.AddTemplate(@"SELECT MEDIANAME, EVENTNAME, ONAIRDATE, ONAIRTIME, STATENAME, MILLISEC, EDITOR, EDITORNAME, EDITTIME, REQTIME, MASTERFILE
                     FROM
                     M30_VW_PROGRAM /**where**/");
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

            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY ONAIRDATE, ONAIRTIME ASC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_PGM_INFO), sortKey, sortValue);
            }

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");


            
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Define.DTM19),
                    ReqCompleteDtm = row.REQTIME != null ? ((DateTime)row.REQTIME).ToString(Define.DTM19) : null,
                    FilePath = row.MASTERFILE
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }

        public DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT> FindSCRSpot(string media, string start_dt, string end_dt, string pgmName, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
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
            });
            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM M30_VW_SPOT_SUB /**where**/");
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
            if (!string.IsNullOrEmpty(pgmName))
            {
                string[] nameArray = pgmName.Split(' ');
                foreach (var word in nameArray)
                {
                    builder.Where($"LOWER(EVENTNAME) LIKE LOWER('%{word}%')");
                }
            }
            if (!string.IsNullOrEmpty(editor))
            {
                builder.Where("EDITOR = :EDITOR", param);
            }

            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY SPOTNAME, ONAIRDATE ASC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_SCR_SPOT), sortKey, sortValue);
            }

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            
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
                    MasteringDtm = row.MASTERTIME == null ? "" : ((DateTime)row.MASTERTIME).ToString(Define.DTM19),
                    FilePath = row.MASTERFILE,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Define.DTM19),
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public DTO_RESULT_PAGE_LIST<DTO_REPORT> FindReport(string cate, string start_dt, string end_dt, string isMastering, string pgmName, string editor, string reporterName, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
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
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM M30_VW_REPORT /**where**/");
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
            if (isMastering == "Y")
            {
                builder.Where("MASTERFILE IS NOT NULL");
            }

            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY EVENTNAME ASC, ONAIRDATE DESC, REPORTNAME ASC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_REPORT), sortKey, sortValue);
            }

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Define.DTM19),
                    MasteringDtm = row.MASTERTIME == null ? "" : ((DateTime)row.MASTERTIME).ToString(Define.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public DTO_RESULT_PAGE_LIST<DTO_PRO> FindOldPro(string media, string cate, string start_dt, string end_dt, string type, string editor, string name, int rowPerPage, int selectPage, string sortKey, string sortValue)
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
                START_DT = start_dt,
                END_DT = end_dt,
                TYPE = type,
                EDITOR = editor,    
                START_NO = startNo,
                LAST_NO = lastNo,
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM M30_VW_PROAUDIOFILE /**where**/");
            builder.Select("AUDIOID, AUDIONAME, CODENAME, MILLISEC, EDITFORMAT, EDITOR, EDITORNAME, EDITTIME, MASTERTIME, TYPENAME, MASTERFILE");
            builder.Where("(MASTERTIME >= :START_DT AND MASTERTIME <=  TO_DATE(:END_DT,'YYYYMMDD')+1 )");
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

            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY MASTERTIME DESC, AUDIONAME ASC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_PRO), sortKey, sortValue);
            }

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Define.DTM19),
                    MasteringDtm = row.MASTERTIME == null ? "" : ((DateTime)row.MASTERTIME).ToString(Define.DTM19),
                    ProType = row.TYPENAME,
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        
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

            string orderBy = "ORDER BY SBID ASC";

            var querySource = builder.AddTemplate($"SELECT /**select**/ FROM M30_VW_SB_{viewName} /**where**/ {orderBy}");
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

            returnData.Data = _repository.Select(querySource.RawSql, param, resultMapping);
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

            string orderBy = "ORDER BY NUM, CMSEQNUM ASC";
            var querySource = builder.AddTemplate($@"SELECT * FROM M30_VW_SB_CLIP /**where**/ {orderBy}");
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("ONAIRDATE = :BRD_DT");
            }
            if (!string.IsNullOrEmpty(sbID))
            {
                builder.Where("SBGROUPID = :SBID");
            }

            
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

            returnData.Data = _repository.Select(querySource.RawSql, param, resultMapping);
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

            string orderBy = "ORDER BY MEDIA, ONAIRTIME, CMGROUPNAME ASC";
            var querySource = builder.AddTemplate($@"SELECT ROWNUM AS RNO, A.* FROM (SELECT /**select**/ FROM M30_VW_CM_GROUP /**where**/ {orderBy}) A");
            builder.Select("MEDIA, MEDIANAME, ONAIRDATE, CMGROUPNAME, CMGROUPID, PROID, DURSEC, CMCAPACITY, STATENAME, EDITOR, EDITORNAME, EDITTIME");
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

            
            var resultMapping = new Func<dynamic, DTO_CM>((row) =>
            {
                return new DTO_CM
                {
                    RowNO = Convert.ToInt32(row.RNO),
                    MediaName = row.MEDIANAME,
                    BrdDT = row.ONAIRDATE,
                    ID = row.CMGROUPID,
                    Name = row.CMGROUPNAME,
                    Length = row.DURSEC,
                    Capacity = row.CMCAPACITY,
                    Status = row.STATENAME,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Define.DTM19),
                };
            });

            returnData.Data = _repository.Select(querySource.RawSql, param, resultMapping);
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

            string orderBy = "ORDER BY NUM ASC";
            var querySource = builder.AddTemplate($@"SELECT * FROM M30_VW_CM_CLIP /**where**/ {orderBy}");
            if (!string.IsNullOrEmpty(brd_dt))
            {
                builder.Where("ONAIRDATE = :BRD_DT");
            }
            if (!string.IsNullOrEmpty(cmGrpID))
            {
                builder.Where("CMGROUPID = :CM_GRP_ID");
            }
            builder.OrderBy("NUM");

            

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

            returnData.Data = _repository.Select(querySource.RawSql, param, resultMapping);
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
            });

            var querySource = builder.AddTemplate(@"SELECT /**select**/ FROM M30_VW_SPOT_MAIN /**where**/");
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

            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY EDITTIME DESC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_MCR_SPOT), sortKey, sortValue);
            }

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Define.DTM19),
                    ReqCompleteDtm = ((DateTime)row.REQTIME).ToString(Define.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
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
            });

            var querySource = builder.AddTemplate($"SELECT /**select**/ FROM {viewName} /**where**/");
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

            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY MASTERTIME DESC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_FILLER), sortKey, sortValue);
            }


            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            
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
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Define.DTM19),
                    MasteringDtm = row.MASTERTIME == null ? "" : ((DateTime)row.MASTERTIME).ToString(Define.DTM19),
                    FilePath = row.MASTERFILE,
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
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
            });

            var querySource = builder.AddTemplate($"SELECT /**select**/ FROM M30_VW_FILLER_TIME /**where**/");
            builder.Select(@"MEDIA, MEDIANAME, SPOTID, SPOTNAME, ONAIRDATE, STARTDATE, ENDDATE, STATEID, STATENAME, MILLISEC, EDITFORMAT, EDITOR, EDITORNAME, EDITTIME, REQTIME, MASTERTIME, MASTERFILE");
            builder.Where("MEDIA=:MEDIA");
            builder.Where(":START_DT <= ONAIRDATE AND ONAIRDATE <= :END_DT");
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

            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY EDITTIME DESC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_FILLER_TIME), sortKey, sortValue);
            }

            var queryTemplate = builder.AddTemplate($"SELECT A.*, ROWNUM AS RNO, COUNT(*) OVER () RESULT_COUNT FROM ({querySource.RawSql} {orderBy}) A");
            var queryMaxPaging = builder.AddTemplate($"SELECT B.* FROM ({queryTemplate.RawSql}) B WHERE RNO <:LAST_NO");
            var queryMaxMinPaging = builder.AddTemplate($"SELECT C.* FROM ({queryMaxPaging.RawSql}) C WHERE RNO >=:START_NO");

            
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
                    BrdDate = row.ONAIRDATE,
                    StartDT = row.STARTDATE,
                    EndDT = row.ENDDATE,
                    Status = row.STATENAME,
                    Duration = row.MILLISEC,
                    Track = row.EDITFORMAT,
                    EditorID = row.EDITOR,
                    EditorName = row.EDITORNAME,
                    EditDtm = ((DateTime)row.EDITTIME).ToString(Define.DTM19),
                    MasteringDtm = row.MASTERTIME != null ? ((DateTime)row.MASTERTIME).ToString(Define.DTM19) : null,
                    FilePath = row.MASTERFILE,
                    FileName = Path.GetFileName(row.MASTERFILE)
                };
            });

            returnData.Data = _repository.Select(queryMaxMinPaging.RawSql, param, resultMapping);
            returnData.RowPerPage = rowPerPage;
            returnData.SelectPage = selectPage;
            return returnData;
        }
        public DTO_RESULT_PAGE_LIST<DTO_DL30> FineDLArchive(string media, string brd_dt, long? deviceSeq, string name, string sortKey, string sortValue)
        {
            DTO_RESULT_PAGE_LIST<DTO_DL30> returnData = new DTO_RESULT_PAGE_LIST<DTO_DL30>();
            var builder = new SqlBuilder();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                MEDIA_CD = media,
                SCH_DATE = brd_dt,
                REC_NAME = name,
                DEVICE_SEQ= deviceSeq,
            });
            string orderBy = "";
            if (string.IsNullOrEmpty(sortKey))
            {
                orderBy = "ORDER BY BRD_DTM ASC";
            }
            else
            {
                orderBy = CommonUtility.GetSortString(typeof(DTO_DL30), sortKey, sortValue);
            }

            var querySource = builder.AddTemplate(@$"SELECT ROWNUM AS RNO, D.* FROM (SELECT A.*, B.FILE_EXT, B.FILE_SIZE, B.REG_DTM, B.DAMS_ID, C.DEVICE_NAME FROM M30_DL_ARCHIVE A
                                                    INNER JOIN M30_DL_ARCHIVE_FILE B ON B.ARCHIVE_SEQ=A.SEQ AND B.FILE_EXT='WAV'
                                                    LEFT JOIN M30_DL_DEVICE C ON C.SEQ = A.DEVICE_SEQ
                                                    /**where**/
                                                    {orderBy}) D");
            
            if (deviceSeq!=null)
            {
                builder.Where("DEVICE_SEQ=:DEVICE_SEQ");
            }
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
                    BrdDate = ((DateTime)row.BRD_DTM).ToString(Define.DTM19),
                    ProgramID = row.PRODUCT_ID,
                    SourceID = row.SOURCE_ID,
                    RecName = row.REC_NAME,
                    Duration = TimeSpan.FromSeconds(Convert.ToInt32(row.LENGTH)).ToString(),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = string.IsNullOrEmpty((string)row.FILE_PATH) ? string.Empty : $"{row.FILE_PATH}\\{((DateTime)row.BRD_DTM).ToString(Define.DTM14)}_{row.SOURCE_ID}.{((string)row.FILE_EXT).ToLower()}",
                    RegDtm = ((DateTime)row.REG_DTM).ToString(Define.DTM19),
                    DeviceName = row.DEVICE_NAME,
                };
            });

            returnData.Data = _repository.Select(querySource.RawSql, param, resultMapping);
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
                                                    WHERE A.SEQ =:SEQ
                                                    ORDER BY BRD_DTM");

            
            var resultMapping = new Func<dynamic, DTO_DL30>((row) =>
            {
                return new DTO_DL30
                {
                    //RowNO = Convert.ToInt32(row.RNO),
                    Seq = Convert.ToInt64(row.SEQ),
                    DeviceSeq = Convert.ToInt64(row.DEVICE_SEQ),
                    MediaCD = row.MEDIA_CD,
                    MediaName = row.MEDIA_CD,
                    SchDate = row.SCH_DATE,
                    BrdDate = ((DateTime)row.BRD_DTM).ToString(Define.DTM19),
                    ProgramID = row.PRODUCT_ID,
                    SourceID = row.SOURCE_ID,
                    RecName = row.REC_NAME,
                    Duration = TimeSpan.FromSeconds(Convert.ToInt32(row.LENGTH)).ToString(),
                    FileSize = Convert.ToInt64(row.FILE_SIZE),
                    FilePath = string.IsNullOrEmpty((string)row.FILE_PATH) ? string.Empty : $"{row.FILE_PATH}\\{((DateTime)row.BRD_DTM).ToString(Define.DTM14)}_{row.SOURCE_ID}.{((string)row.FILE_EXT).ToLower()}",
                    RegDtm = ((DateTime)row.REG_DTM).ToString(Define.DTM19),
                    DeviceName = row.DEVICE_NAME,
                };
            });

           return _repository.Get(querySource.RawSql, param, resultMapping);
        }
    }
}

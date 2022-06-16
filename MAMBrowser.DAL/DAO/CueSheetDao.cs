using Dapper;
using MAMBrowser.Common;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.IO;

namespace MAMBrowser.DAL
{
    public class CueSheetDao
    {
        private readonly Repository _repository;
        public CueSheetDao(Repository repository)
        {
            _repository = repository;
        }

        #region 코드 가져오기
        public DTO_CODE Code_Read()
        {
            DTO_CODE returnData = new DTO_CODE();
            //코드 가져오기

            return returnData;
        }
        #endregion

        #region 프로그램 가져오기
        public DTO_PROGRAM Program_Read()
        {
            DTO_PROGRAM returnData = new DTO_PROGRAM();
            //프로그램 가져오기

            return returnData;
        }
        #endregion

        #region 맵핑유저 가져오기
        public DTO_MAPPING_USER Mapping_User_Read()
        {
            DTO_MAPPING_USER returnData = new DTO_MAPPING_USER();
            //맵핑유저 가져오기

            return returnData;
        }
        #endregion


        #region 기본편성표 리스트 INFOLIST 가져오기
        public DTO_RESULT_LIST<DTO_BASEINFOLIST> BaseList_Read(List<string> productid, string week)
        {

            DTO_RESULT_LIST<DTO_BASEINFOLIST> returnData = new DTO_RESULT_LIST<DTO_BASEINFOLIST>();

            var builder = new SqlBuilder();
            //프로그램, 요일&매체 리스트로 처리해야함
            var querySource = builder.AddTemplate(@"select onairday,onairtime,productid,eventname 
                                                    from OA_BASE_VIEW /**where**/ order by media ASC, onairtime ASC");
            builder.Where("producttype='P' and startdate='20200511'");
            foreach (var item in productid)
            {
            builder.Where($"productid LIKE '{item}'");

            }
            builder.Where($"REGEXP_LIKE(onairday, '{week}')");


            var resultMapping = new Func<dynamic, DTO_BASEINFOLIST>((row) =>
            {
                return new DTO_BASEINFOLIST
                {
                    Day = row.ONAIRDAY,
                    OnAirTime = ((DateTime)row.ONAIRTIME).ToString(Define.DTM19),
                    Id = row.PRODUCTID,
                    Name = row.EVENTNAME,
                };
            });
            returnData.Data = _repository.Select(querySource.RawSql, null, resultMapping);
            return returnData;
        }
        #endregion

        #region 일일편성표 리스트 INFOLIST 가져오기
        public DTO_RESULT_LIST<DTO_BASEINFOLIST> DayList_Read(List<string> productid, string st_dt, string end_dt, List<string> media)
        {

            DTO_RESULT_LIST<DTO_BASEINFOLIST> returnData = new DTO_RESULT_LIST<DTO_BASEINFOLIST>();

            var builder = new SqlBuilder();
            //프로그램, 요일&매체 리스트로 처리해야함
            var querySource = builder.AddTemplate(@"select onairday,onairtime,productid,eventname
                                                    from OA_SPECIAL_VIEW /**where**/ order by media ASC, onairtime ASC");
            foreach (var item in productid)
            {
                builder.Where($"productid = '{item}'");

            }
            foreach (var item in media)
            {
                builder.Where($"media = '{item}'");

            }
            builder.Where($"onairtime BETWEEN to_date('{st_dt}','YYYY-MM-DD') and to_date('{end_dt}', 'YYYY-MM-DD')");
            var resultMapping = new Func<dynamic, DTO_BASEINFOLIST>((row) =>
            {
                return new DTO_BASEINFOLIST
                {
                    Day = row.ONAIRDAY,
                    OnAirTime = ((DateTime)row.ONAIRTIME).ToString(Define.DTM19),
                    Id = row.PRODUCTID,
                    Name = row.EVENTNAME,
                };
            });
            returnData.Data = _repository.Select(querySource.RawSql, null, resultMapping);
            return returnData;
        }
        #endregion

        #region 일일큐시트 PQS READ
        public DTO_DAY_PQS DayCueSheetPQS_Read()
        {
            DTO_DAY_PQS returnData = new DTO_DAY_PQS();
            //일일큐시트 PQS 가져오기

            return returnData;
        }
        #endregion

        #region 일일큐시트 PQS CREATE
        public void DayCueSheetPQS_Create()
        {
            //일일큐시트 PQS Create
        }
        #endregion

        #region 일일큐시트 PQS UPDATE
        public void DayCueSheetPQS_Update()
        {
            //일일큐시트 PQS Update
        }
        #endregion

        #region 일일큐시트 PQS DELETE
        public void DayCueSheetPQS_Delete()
        {
            //일일큐시트 PQS Delete
        }
        #endregion

        #region 일일큐시트 PQSCON READ
        public DTO_DAY_PQSCON DayCueSheetPQSCON_Read()
        {
            DTO_DAY_PQSCON returnData = new DTO_DAY_PQSCON();
            //일일큐시트 PQSCON 가져오기

            return returnData;
        }
        #endregion

        #region 일일큐시트 PQSCON CREATE
        public void DayCueSheetPQSCON_Create()
        {
            //일일큐시트 PQSCON Create
        }
        #endregion

        #region 일일큐시트 PQSCON UPDATE
        public void DayCueSheetPQSCON_Update()
        {
            //일일큐시트 PQSCON Update
        }
        #endregion

        #region 일일큐시트 PQSCON DELETE
        public void DayCueSheetPQSCON_Delete()
        {
            //일일큐시트 PQSCON Delete
        }
        #endregion

        #region 주간큐시트 리스트 PQS READ
        public DTO_BASE_PQS BaseCueSheetPQS_Read()
        {
            DTO_BASE_PQS returnData = new DTO_BASE_PQS();
            //주간큐시트 PQS 가져오기

            return returnData;
        }
        #endregion

        #region 주간큐시트 PQS CREATE
        public void BaseCueSheetPQS_Create()
        {
            //주간큐시트 PQS Create
        }
        #endregion

        #region 주간큐시트 PQS UPDATE
        public void BaseCueSheetPQS_Update()
        {
            //주간큐시트 PQS Update
        }
        #endregion

        #region 주간큐시트 PQS DELETE
        public void BaseCueSheetPQS_Delete()
        {
            //주간큐시트 PQS Delete
        }
        #endregion

        #region 주간큐시트 PQSCON READ
        public DTO_BASE_PQSCON BaseCueSheetPQSCON_Read()
        {
            DTO_BASE_PQSCON returnData = new DTO_BASE_PQSCON();
            //주간큐시트 PQSCON 가져오기

            return returnData;
        }
        #endregion

        #region 주간큐시트 PQSCON CREATE
        public void BaseCueSheetPQSCON_Create()
        {
            //주간큐시트 PQSCON Create
        }
        #endregion

        #region 주간큐시트 PQSCON UPDATE
        public void BaseCueSheetPQSCON_Update()
        {
            //주간큐시트 PQSCON Update
        }
        #endregion

        #region 주간큐시트 PQSCON DELETE
        public void BaseCueSheetPQSCON_Delete()
        {
            //주간큐시트 PQSCON Delete
        }
        #endregion

        #region 템플릿 PQS READ
        public DTO_TEMPLATE_PQS TemplatePQS_Read()
        {
            DTO_TEMPLATE_PQS returnData = new DTO_TEMPLATE_PQS();
            //템플릿 가져오기

            return returnData;
        }
        #endregion

        #region 템플릿 PQS CREATE
        public void TemplatePQS_Create()
        {
            //템플릿 Create
        }
        #endregion

        #region 템플릿 PQS UPDATE
        public void TemplatePQS_Update()
        {
            //템플릿 Update
        }
        #endregion

        #region 템플릿 PQS DELETE
        public void TemplatePQS_Delete()
        {
            //템플릿 Delete
        }
        #endregion

        #region 템플릿 PQSCON READ
        public DTO_TEMPLATE_PQSCON TemplatePQSCON_Read()
        {
            DTO_TEMPLATE_PQSCON returnData = new DTO_TEMPLATE_PQSCON();
            //템플릿 PQSCON 가져오기

            return returnData;
        }
        #endregion

        #region 템플릿 PQSCON CREATE
        public void TemplatePQSCON_Create()
        {
            //템플릿 PQSCON Create
        }
        #endregion

        #region 템플릿 PQSCON UPDATE
        public void TemplatePQSCON_Update()
        {
            //템플릿 PQSCON Update
        }
        #endregion

        #region 템플릿 PQS DELETE
        public void TemplatePQSCON_Delete()
        {
            //템플릿 PQSCON Delete
        }
        #endregion

        #region 출력용 READ
        public DTO_VIEW View_Read()
        {
            DTO_VIEW returnData = new DTO_VIEW();
            //출력용 가져오기

            return returnData;
        }
        #endregion

        #region 출력용 CREATE
        public void View_Create()
        {
            //출력용 Create
        }
        #endregion

        #region 출력용 UPDATE
        public void View_Update()
        {
            //출력용 Update
        }
        #endregion

        #region 출력용 DELETE
        public void View_Delete()
        {
            //출력용 Delete
        }
        #endregion

        #region 즐겨찾기 READ
        public DTO_FAVORITES Favorites_Read()
        {
            DTO_FAVORITES returnData = new DTO_FAVORITES();
            //즐겨찾기 가져오기

            return returnData;
        }
        #endregion

        #region 즐겨찾기 CREATE
        public void Favorites_Create()
        {
            //즐겨찾기 Create
        }
        #endregion

        #region 즐겨찾기 UPDATE
        public void Favorites_Update()
        {
            //즐겨찾기 Update
        }
        #endregion

        #region 즐겨찾기 DELETE
        public void Favorites_Delete()
        {
            //즐겨찾기 Delete
        }
        #endregion

        #region 태그 READ
        public DTO_TAG Tag_Read()
        {
            DTO_TAG returnData = new DTO_TAG();
            //태그 가져오기

            return returnData;
        }
        #endregion

        #region 태그 CREATE
        public void Tag_Create()
        {
            //태그 Create
        }
        #endregion

        #region 태그 UPDATE
        public void Tag_Update()
        {
            //태그 Update
        }
        #endregion

        #region 태그 DELETE
        public void Tag_Delete()
        {
            //태그 Delete
        }
        #endregion

        #region 첨부파일 READ
        public DTO_ATCHD_FILE File_Read()
        {
            DTO_ATCHD_FILE returnData = new DTO_ATCHD_FILE();
            //첨부파일 가져오기

            return returnData;
        }
        #endregion

        #region 첨부파일 CREATE
        public void File_Create()
        {
            //첨부파일 Create
        }
        #endregion

        #region 첨부파일 UPDATE
        public void File_Update()
        {
            //첨부파일 Update
        }
        #endregion

        #region 첨부파일 DELETE
        public void File_Delete()
        {
            //첨부파일 Delete
        }
        #endregion


    }
}

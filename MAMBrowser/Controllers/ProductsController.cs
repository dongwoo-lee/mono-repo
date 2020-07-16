﻿using Dapper;
using MAMBrowser.BLL;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
     

        /// <summary>
        /// 프로그램 소재 조회 (페이징 x)
        /// </summary>
        /// <param name="media">매체코드(A,F,C,D)</param>
        /// <param name="brd_dt">방송일(20200101)</param>
        /// <returns></returns>
        [HttpGet("pgm")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PGM_INFO>> FindPGM([FromQuery] string media, [FromQuery] string brd_dt)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PGM_INFO>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PGM_INFO>>();
            try
            {
                ProductsBLL bll = new ProductsBLL();
                result.ResultObject = bll.FindPGM(media, brd_dt);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 부조 SPOT 소재 조회
        /// </summary>
        /// <param name="start_dt">20200101</param>
        /// <param name="end_dt">20200701</param>
        /// <param name="editor">사용자 
        /// ex) 최지민</param>
        /// <param name="name">SOS</param>
        /// <param name="rowPerPage">3</param>
        /// <param name="selectPage">1</param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("spot/scr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_SCR_SPOT>> FindSCRSpot([FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            //사용자 ID ex) 180988
            DTO_RESULT<DTO_RESULT_LIST<DTO_SCR_SPOT>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_SCR_SPOT>>();
            try
            {
                ProductsBLL bll = new ProductsBLL();
                result.ResultObject = bll.FindSCRSpot(start_dt, end_dt, editor, name, rowPerPage, selectPage, sortKey, sortValue);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 취재물 소재 조회
        /// </summary>
        /// <param name="cate">분류 : ex)NPS-M</param>
        /// <param name="start_dt"></param>
        ///  <param name="end_dt"></param>
        /// <param name="pgm">사용처 : ex) PM1200NA</param>
        /// <param name="editor"></param>
        /// <param name="reporter"> 취재인 이름 : ex) 한수연</param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("report")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_REPORT>> FindReport([FromQuery] string cate, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string pgm, [FromQuery] string editor, [FromQuery] string reporter, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_REPORT>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_REPORT>>();
            try
            {
                ProductsBLL bll = new ProductsBLL();
                result.ResultObject = bll.FindReport(cate, start_dt, end_dt, pgm, editor, reporter,name, rowPerPage, selectPage, sortKey, sortValue);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// (구)프로소재 조회
        /// </summary>
        /// <param name="media"></param>
        /// <param name="type"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("old_pro")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PRO>> FindOldPro([FromQuery] string media, [FromQuery] string type, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PRO>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PRO>>();
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
        /// <summary>
        /// 음반 기록실 조회
        /// </summary>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("music")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_SONG>> FindMusic([FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_SONG>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_SONG>>();
            try
            {
                var startNO = 1;
                startNO += (rowPerPage * selectPage) - rowPerPage;
                var lastNO = startNO + rowPerPage;

                //var resultObj = "";
                //var obj = resultObj.FirstOrDefault();
                //if (obj != null)
                //{
                //    result.ResultObject.TotalRowCount = obj.Count;
                //    result.ResultObject.RowPerPage = rowPerPage;
                //    result.ResultObject.SelectPage = selectPage;
                //    //result.ResultObject
                //}

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 효과음 조회
        /// </summary>
        /// <param name="searchWord"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("effect")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_EFFECT>> FindEffect([FromQuery] string searchWord, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_EFFECT>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_EFFECT>>();
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
        /// <summary>
        /// 주조SB 소재 조회 (페이징x)
        /// </summary>
        /// <param name="media"></param>
        /// <param name="brd_dt"></param>
        /// <param name="pgm"></param>
        /// <returns></returns>
        [HttpGet("sb/mcr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_SB>> FindMcrSB([FromQuery] string media, [FromQuery] string brd_dt, [FromQuery] string pgm)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_SB>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_SB>>();
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
        /// <summary>
        /// 부조SB 소재 조회 (페이징x)
        /// </summary>
        /// <param name="media"></param>
        /// <param name="brd_dt"></param>
        /// <param name="pgm"></param>
        /// <returns></returns>
        [HttpGet("sb/scr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_SB>> FindScrSB([FromQuery] string media, [FromQuery] string brd_dt, [FromQuery] string pgm)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_SB>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_SB>>();
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
        /// <summary>
        /// 광고 소재 조회 (페이징x)
        /// </summary>
        /// <param name="media"></param>
        /// <param name="brd_dt"></param>
        /// <param name="adcd"></param>
        /// <param name="pgm"></param>
        /// <returns></returns>

        [HttpGet("cm")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CM>> FindCM([FromQuery] string media, [FromQuery] string brd_dt, [FromQuery] string adcd, [FromQuery] string pgm)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CM>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CM>>();
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
        /// <summary>
        /// 주조SPOT 소재 조회
        /// </summary>
        /// <param name="media"></param>
        /// <param name="cate"></param>
        /// <param name="start_dt"></param>
        /// <param name="end_dt"></param>
        /// <param name="status"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        [HttpGet("spot/mcr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_MCR_SPOT>> FindMcrSpot([FromQuery] string media, [FromQuery] string cate, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string status, [FromQuery] string editor, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MCR_SPOT>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MCR_SPOT>>();
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
        /// <summary>
        /// 필러(pr) 소재 조회
        /// </summary>
        /// <param name="media"></param>
        /// <param name="cate"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("filler/pr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FindProFiller([FromQuery] string media, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
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
        /// <summary>
        /// 필러(일반) 소재 조회
        /// </summary>
        /// <param name="media"></param>
        /// <param name="cate"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("filler/general")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FindFeneralFiller([FromQuery] string media, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
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
        /// <summary>
        /// 필러(시간) 소재 조회
        /// </summary>
        /// <param name="media"></param>
        /// <param name="start_dt"></param>
        /// <param name="end_dt"></param>
        /// <param name="status"></param>
        /// <param name="cate"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("filler/time")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FindTimetoneFiller([FromQuery] string media, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string status, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
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
        /// <summary>
        /// 필러(기타) 소재 조회
        /// </summary>
        /// <param name="media"></param>
        /// <param name="cate"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("filler/etc")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FindETCFiller([FromQuery] string media, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
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
        /// <summary>
        /// DL3.0 소재 조회 (페이징x)
        /// </summary>
        /// <param name="media">매체(A,C,D,F)</param>
        /// <param name="cate"></param>
        /// <param name="brd_dt">방송일</param>
        /// <returns></returns>
        [HttpGet("dl30")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_DL30>> FindNewDL([FromQuery] string media, [FromQuery] string cate, [FromQuery] string brd_dt)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_DL30>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_DL30>>();
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
    }
}

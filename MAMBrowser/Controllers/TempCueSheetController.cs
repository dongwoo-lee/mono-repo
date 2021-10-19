using DAP3.CueSheetCommon.DTO.Result;
using Dapper;
using MAMBrowser.BLL;
using MAMBrowser.Common;
using MAMBrowser.Common.Foundation;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TempCueSheetController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly TemplateBll _bll;
        private readonly IFileProtocol _fileService;
        private readonly ILogger<ProductsController> _logger;
        private readonly WebServerFileHelper _fileHelper;

        public TempCueSheetController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, TemplateBll bll, ServiceResolver sr, ILogger<ProductsController> logger, WebServerFileHelper fileHelper)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _bll = bll;
            _fileService = sr("MirosConnection");
            _logger = logger;
            _fileHelper = fileHelper;
        }

        // 템플릿 목록 가져오기
        [HttpGet("GetTempList")]
        public List<TemplateListDTO> GetTempList([FromQuery] string personid)
        {
            try
            {
                return _bll.GetTemplateList(personid);
            }
            catch
            {
                throw;
            }
            
        }
        //템플릿 상세내용 가져오기
        [HttpGet("GetTempCue")]
        public TemplateCollectionDTO GetTempCue([FromQuery] int cueid)
        {
            try
            {
                return _bll.GetTemplate(cueid);
            }
            catch
            {
                throw;
            }
        }
        //템플릿 생성 & 업데이트 (테스트필요)
        [HttpPost("SaveTempCue")]
        public bool SaveTempCue([FromBody] CueData pram)
        {
            try
            {
                return _bll.SaveTemplate(pram.temParam, pram.conParams, pram.tagParams, pram.printParams, pram.attParams);
            }
            catch
            {
                throw;
            }
        }

        //템플릿 삭제
        [HttpDelete("DelTempCue")]
        public bool DelTempCue([FromQuery] int[] tempids)
        {
            try
            {
                return _bll.DeleteTemplate(tempids);
            }
            catch
            {
                throw;
            }
        }
    }
}

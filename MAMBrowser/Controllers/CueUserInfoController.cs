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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CueUserInfoController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly CueUserInfoBll _bll;
        private readonly IFileProtocol _fileService;
        private readonly ILogger<ProductsController> _logger;
        private readonly WebServerFileHelper _fileHelper;

        public CueUserInfoController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, CueUserInfoBll bll, ServiceResolver sr, ILogger<ProductsController> logger, WebServerFileHelper fileHelper)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _bll = bll;
            _fileService = sr("MirosConnection");
            _logger = logger;
            _fileHelper = fileHelper;
        }
        //유저별 프로그램 리스트 가져오기
        [HttpGet("GetProgramList")]
        public List<PgmListDTO> GetUserProgramList([FromQuery] string personid, string media)
        {
            try
            {
                return _bll.GetUserPgmList(personid, media);
            }
            catch
            {
                throw;
            }
        }

        //프로그램 전체 담당자 가져오기
        [HttpGet("GetDirectorList")]
        public string GetDirectorList([FromQuery] string productid)
        {
            try
            {
                var result = _bll.GetDirectorList(productid);
                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}
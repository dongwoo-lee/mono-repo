using DAP3.CueSheetCommon.DTO.Param;
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
    public class FavoriteController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly FavoriteBll _bll;
        private readonly IFileProtocol _fileService;
        private readonly ILogger<ProductsController> _logger;
        private readonly WebServerFileHelper _fileHelper;

        public FavoriteController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, FavoriteBll bll, ServiceResolver sr, ILogger<ProductsController> logger, WebServerFileHelper fileHelper)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _bll = bll;
            _fileService = sr("MirosConnection");
            _logger = logger;
            _fileHelper = fileHelper;
        }
        //즐겨찾기 가져오기
        [HttpGet("GetFavorites")]
        public List<UserFavDTO> GetFavorites([FromQuery] string personid)
        {
            try
            {
                return _bll.GetUserFavorites(personid).ToList();
            }
            catch
            {
                throw;
            }
        }
        //즐겨찾기 저장 (테스트 필요)
        [HttpPost("SetFavorites")]
        public bool SetFavorites([FromQuery] string personid, [FromBody] List<FavConParamDTO> favConParam)
        {
            try
            {
                return _bll.SaveUserFavorites(personid, favConParam);
            }
            catch
            {
                throw;
            }
        }
        //즐겨찾기 삭제
        [HttpDelete("DelFavorites")]
        public bool DelFavorites([FromBody] string personid)
        {
            try
            {
                return _bll.DeleteUserFavorites(personid);
            }
            catch
            {
                throw;
            }
        }
    }
}

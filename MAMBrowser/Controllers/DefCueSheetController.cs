using DAP3.CueSheetCommon.DTO.Result;
using MAMBrowser.BLL;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DefCueSheetController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly DefCueSheetBll _bll;
        private readonly IFileProtocol _fileService;
        private readonly ILogger<ProductsController> _logger;
        private readonly WebServerFileHelper _fileHelper;

        public DefCueSheetController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, DefCueSheetBll bll, ServiceResolver sr, ILogger<ProductsController> logger, WebServerFileHelper fileHelper)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _bll = bll;
            _fileService = sr("MirosConnection");
            _logger = logger;
            _fileHelper = fileHelper;
        }
        // 기본큐시트 목록 가져오기
        [HttpGet("GetDefList")]
        public List<DefaultCueSheetListDTO> GetDefList([FromQuery] string[] productids)
        {
            try
            {
                return _bll.GetDefCueList(productids);
            }
            catch
            {
                throw;
            }
        }
        // 기본큐시트 상세내용 가져오기
        [HttpGet("GetDefCue")]
        public CueSheetCollectionDTO GetDefCue([FromQuery]string productid, [FromQuery] int[] cueid)
        {
            try
            {
                var result = _bll.GetDefCue(productid, cueid);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // 기본큐시트 저장 (테스트필요)
        [HttpPost("SaveDefCue")]
        public bool SaveDefCue([FromBody]CueData pram)
        {
            try
            {
                //pram.defParams.Skip(2);
                //var def = pram.defParams.Skip(2);
                return _bll.SaveDefaultCueSheet(pram.cueParam, pram.defParams, pram.conParams, pram.tagParams, pram.printParams, pram.attParams);
            }
            catch
            {
                throw;
            }
        }

        //기본큐시트 삭제
        [HttpDelete("DelDefCue")]
        public bool DelDefCue([FromQuery] int[] delParams)
        {
            try
            {
                var result = _bll.DeleteDefaultCueSheet(delParams);
                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}

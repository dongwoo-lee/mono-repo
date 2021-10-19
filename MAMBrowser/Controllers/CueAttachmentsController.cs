using MAMBrowser.BLL;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CueAttachmentsController : ControllerBase
    {
        //없어도됨
        public CueAttachmentsController()
        {

        }
        //private readonly IHostingEnvironment _hostingEnvironment;
        //private readonly AppSettings _appSesstings;
        //private readonly CueAttachmentsBll _bll;
        //private readonly IFileProtocol _fileService;
        //private readonly ILogger<ProductsController> _logger;
        //private readonly WebServerFileHelper _fileHelper;

        //public CueAttachmentsController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, CueAttachmentsBll bll, ServiceResolver sr, ILogger<ProductsController> logger, WebServerFileHelper fileHelper)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //    _appSesstings = appSesstings.Value;
        //    _bll = bll;
        //    _fileService = sr("MirosConnection");
        //    _logger = logger;
        //    _fileHelper = fileHelper;
        //}
        ////첨부파일 가져오기
        //[HttpGet("GetCueFile")]
        //public IEnumerable<AttachmentsDTO> GetCueFile([FromQuery] int[] cueids)
        //{
        //    return _bll.GetFile(cueids);
        //}
        ////출력용 가져오기
        //[HttpGet("GetPrint")]
        //public IEnumerable<PrintDTO> GetPrint([FromQuery] int[] cueids)
        //{
        //    return _bll.GetPrints(cueids);
        //}
        ////태그 가져오기
        //[HttpGet("GetTag")]
        //public IEnumerable<TagDTO> GetTag([FromQuery] int[] cueids)
        //{
        //    return _bll.GetTags(cueids);
        //}
    }
}
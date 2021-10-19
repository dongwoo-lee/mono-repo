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
    public class CueSheetController : ControllerBase
    {
    }
}

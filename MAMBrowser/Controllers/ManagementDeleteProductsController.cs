using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common;
using MAMBrowser.BLL;
using Microsoft.AspNetCore.Mvc;
using static MAMBrowser.DTO.ManagementDeleteProductsDTO;
using System.Collections.Generic;
using System;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using MAMBrowser.Helpers;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementDeleteProductsController : ControllerBase
    {
        private readonly ManagementDeleteProductsBll _bll;
        private readonly HttpContextDBLogger _dbLogger;

        public ManagementDeleteProductsController(ManagementDeleteProductsBll bll, HttpContextDBLogger dbLogger)
        {
            _bll = bll;
            _dbLogger = dbLogger;
        }

        #region 소재 삭제 관리

        [HttpPost("GetDelAudioList")]
        public DTO_RESULT<PageListCollectionDTO<AudioFileDTO>> GetDelAudioList([FromBody] SelectDelProductParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<AudioFileDTO>> result = new DTO_RESULT<PageListCollectionDTO<AudioFileDTO>>();
            try
            {
                result.ResultObject = _bll.GetDelAudioFileList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetDelSpotList")]
        public DTO_RESULT<PageListCollectionDTO<SpotFileDTO>> GetDelSpotList([FromBody] SelectDelProductParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<SpotFileDTO>> result = new DTO_RESULT<PageListCollectionDTO<SpotFileDTO>>();
            try
            {
                result.ResultObject = _bll.GetDelSpotFileList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        [HttpPost("GetDelEtcList")]
        public DTO_RESULT<PageListCollectionDTO<EtcFileDTO>> GetDelEtcList([FromBody] SelectDelProductParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<EtcFileDTO>> result = new DTO_RESULT<PageListCollectionDTO<EtcFileDTO>>();
            try
            {
                result.ResultObject = _bll.GetDelEtcFileList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        [HttpPost("GetDelFillerList")]
        public DTO_RESULT<PageListCollectionDTO<FillerFileDTO>> GetDelFillerList([FromBody] SelectDelProductParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<FillerFileDTO>> result = new DTO_RESULT<PageListCollectionDTO<FillerFileDTO>>();
            try
            {
                result.ResultObject = _bll.GetDelFillerFileList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        [HttpPost("GetDelReportList")]
        public DTO_RESULT<PageListCollectionDTO<ReportFileDTO>> GetDelReportList([FromBody] SelectDelProductParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<ReportFileDTO>> result = new DTO_RESULT<PageListCollectionDTO<ReportFileDTO>>();
            try
            {
                result.ResultObject = _bll.GetDelReportFileList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }


        [HttpPost("GetDelProductList")]
        public DTO_RESULT<PageListCollectionDTO<ProductFileDTO>> GetDelProductList([FromBody] SelectDelProductParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<ProductFileDTO>> result = new DTO_RESULT<PageListCollectionDTO<ProductFileDTO>>();
            try
            {
                result.ResultObject = _bll.GetDelProductFileList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetDelSongList")]
        public DTO_RESULT<PageListCollectionDTO<SongFileDTO>> GetDelSongList([FromBody] SelectDelProductParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<SongFileDTO>> result = new DTO_RESULT<PageListCollectionDTO<SongFileDTO>>();
            try
            {
                result.ResultObject = _bll.GetDelSongFileList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteAudioClipFile")]
        public DTO_RESULT<bool> DeleteAudioClipFile(DeleteAudioClipIdsParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteAudioFiles(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                _dbLogger.ErrorAsync(dto.SystemCd, dto.UserId, $"소재 삭제 (수동) : {ex.Message}", null);
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        #endregion

        #region MIROS 휴지통
        [HttpPost("GetRecycleList")]
        public DTO_RESULT<PageListCollectionDTO<RecycleDTO>> GetRecycleList([FromBody] SelectRecycleParamDTO dto)
        {
            DTO_RESULT<PageListCollectionDTO<RecycleDTO>> result = new DTO_RESULT<PageListCollectionDTO<RecycleDTO>>();
            try
            {
                result.ResultObject = _bll.GetRecycleList(dto);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteRecycle")]
        public DTO_RESULT<bool> DeleteRecycle(DeleteAudioClipIdsParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteRecycleFiles(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                _dbLogger.ErrorAsync(dto.SystemCd, dto.UserId, $"MIROS 휴지통 (수동) : {ex.Message}", null);
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        [HttpGet("RecycleFileDownload")]
        public FileResult RecycleFileDownload([FromQuery] Pram queryPram)
        {
            FileResult result;
            var rootFolder = Path.Combine(Startup.AppSetting.TempExportPath, @$"{queryPram.userid}\{queryPram.guid}");
            var FilePath = Path.Combine(Path.GetDirectoryName(rootFolder), queryPram.guid);
            var fileName = Path.GetFileName(queryPram.guid);
            var provider = new FileExtensionContentTypeProvider();

            string contentType;
            if (!provider.TryGetContentType(FilePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            result = PhysicalFile(FilePath, contentType, fileName);
            return result;
        }
        #endregion

        #region 자동 삭제 규칙
        [HttpPost("GetCommLogList")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_LOG>> GetCommLogList([FromBody] SelectCommLogParamDTO dto)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_LOG>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_LOG>>();
            try
            {
                result.ResultObject = _dbLogger.GetLogList(
                    dto.systemCode,
                    dto.startdate,
                    dto.enddate,
                    dto.logLevel,
                    dto.userName,
                    dto.description,
                    dto.RowPerPage,
                    dto.SelectPage,
                    dto.sortKey,
                    dto.sortValue );
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }
        #endregion
    }
}

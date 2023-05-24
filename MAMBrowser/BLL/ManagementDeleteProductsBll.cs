using M30_ManagementDAO.Interfaces;
using M30_ManagementDAO.ParamEntity;
using MAMBrowser.Utils;
using static MAMBrowser.DTO.ManagementDeleteProductsDTO;
using MAMBrowser.DTO;
using System.IO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Http;

namespace MAMBrowser.BLL
{
    public class ManagementDeleteProductsBll
    {
        private readonly IDelManagementDAO _dao_del;
        private readonly HttpContextDBLogger _dbLogger;

        public ManagementDeleteProductsBll(IDelManagementDAO dao_del,HttpContextDBLogger dbLogger)
        {
            _dao_del = dao_del;
            _dbLogger= dbLogger;
        }

        #region 소재 삭제, miros 휴지통 - Select

        public PageListCollectionDTO<AudioFileDTO> GetDelAudioFileList(SelectDelProductParamDTO dto)
        {
            var result = new PageListCollectionDTO<AudioFileDTO>();
            SelectProductFileParam param = new SelectProductFileParamBuilder()
                .SetStartDate(dto.startdate)
                .SetEndDate(dto.enddate)
                .SetName(dto.name)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();
            var data = _dao_del.GetAudioFileList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }
        public PageListCollectionDTO<SpotFileDTO> GetDelSpotFileList(SelectDelProductParamDTO dto)
        {
            var result = new PageListCollectionDTO<SpotFileDTO>();
            SelectProductFileParam param = new SelectProductFileParamBuilder()
                .SetBrdDate(dto.brddate)
                .SetName(dto.name)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();
            var data = _dao_del.GetSpotFileList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }
        public PageListCollectionDTO<EtcFileDTO> GetDelEtcFileList(SelectDelProductParamDTO dto)
        {
            var result = new PageListCollectionDTO<EtcFileDTO>();
            SelectProductFileParam param = new SelectProductFileParamBuilder()
                .SetName(dto.name)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();
            var data = _dao_del.GetEtcFileList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }
        public PageListCollectionDTO<FillerFileDTO> GetDelFillerFileList(SelectDelProductParamDTO dto)
        {
            var result = new PageListCollectionDTO<FillerFileDTO>();
            SelectProductFileParam param = new SelectProductFileParamBuilder()
                .SetStartDate(dto.startdate)
                .SetEndDate(dto.enddate)
                .SetName(dto.name)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();
            var data = _dao_del.GetFillerFileList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }
        public PageListCollectionDTO<ReportFileDTO> GetDelReportFileList(SelectDelProductParamDTO dto)
        {
            var result = new PageListCollectionDTO<ReportFileDTO>();
            SelectProductFileParam param = new SelectProductFileParamBuilder()
                .SetStartDate(dto.startdate)
                .SetEndDate(dto.enddate)
                .SetName(dto.name)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();
            var data = _dao_del.GetReportFileList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }
        public PageListCollectionDTO<ProductFileDTO> GetDelProductFileList(SelectDelProductParamDTO dto)
        {
            var result = new PageListCollectionDTO<ProductFileDTO>();
            SelectProductFileParam param = new SelectProductFileParamBuilder()
                .SetStartDate(dto.startdate)
                .SetEndDate(dto.enddate)
                .SetMedia(dto.media)
                .SetName(dto.name)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();
            var data = _dao_del.GetProductFileList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }
        public PageListCollectionDTO<SongFileDTO> GetDelSongFileList(SelectDelProductParamDTO dto)
        {
            var result = new PageListCollectionDTO<SongFileDTO>();
            SelectProductFileParam param = new SelectProductFileParamBuilder()
                .SetStartDate(dto.startdate)
                .SetEndDate(dto.enddate)
                .SetName(dto.name)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();
            var data = _dao_del.GetSongFileList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;

        }

        public PageListCollectionDTO<RecycleDTO> GetRecycleList(SelectRecycleParamDTO dto)
        {
            var result = new PageListCollectionDTO<RecycleDTO>();
            SelectRecycleParam param = new SelectRecycleParamBuilder()
                .SetStartDate(dto.startdate)
                .SetEndDate(dto.enddate)
                .SetAudioType(dto.audiotype)
                .SetRowPage(dto.RowPerPage)
                .SetSelectPage(dto.SelectPage)
                .Build();
            var data = _dao_del.GetRecycleList(param);
            result.RowPerPage = dto.RowPerPage;
            result.SelectPage = dto.SelectPage;
            result.TotalRowCount = data.TotalCount;
            result.Data = data.DataList?.Converting();
            return result;
        }

        #endregion

        #region 소재 삭제, miros 휴지통 - Delete

        public bool DeleteRecycleFiles(DeleteAudioClipIdsParamDTO dto)
        {
            foreach (var item in dto.IDs)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.MASTERFILE))
                    {
                        _dbLogger.WarnAsync(dto.SystemCd, dto.UserId, $"MIROS 휴지통 (수동) - {item.AUDIOCLIPID} : 파일 경로가 없습니다.", null);
                        DeleteRecycleData(item.AUDIOCLIPID, dto.SystemCd, dto.UserId);
                    }
                    else if (!File.Exists(item.MASTERFILE))
                    {
                        _dbLogger.WarnAsync(dto.SystemCd, dto.UserId, $"MIROS 휴지통 (수동) - {item.AUDIOCLIPID} : 파일이 존재하지 않습니다.", $"{item.MASTERFILE}");
                        DeleteRecycleData(item.AUDIOCLIPID, dto.SystemCd, dto.UserId);
                    }
                    else
                    {
                        File.Delete(item.MASTERFILE);
                        DeleteRecycleData(item.AUDIOCLIPID, dto.SystemCd, dto.UserId);
                    }
                }
                catch (System.Exception ex)
                {
                    _dbLogger.ErrorAsync(dto.SystemCd, dto.UserId, $"MIROS 휴지통 (수동) - {item.AUDIOCLIPID} : {ex.Message}", $"{item.MASTERFILE}");
                }
            }
            return true;
        }
        public bool DeleteAudioFiles(DeleteAudioClipIdsParamDTO dto)
        {
            foreach (var item in dto.IDs)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.MASTERFILE))
                    {
                        _dbLogger.WarnAsync(dto.SystemCd, dto.UserId, $"소재 삭제 (수동) - {item.AUDIOCLIPID} : 파일 경로가 없습니다.", null);
                        ImmediateDeleteAudioFile(item.AUDIOCLIPID, dto.SystemCd, dto.UserId);
                    }
                    else if (!File.Exists(item.MASTERFILE))
                    {
                        _dbLogger.WarnAsync(dto.SystemCd, dto.UserId, $"소재 삭제 (수동) - {item.AUDIOCLIPID} : 파일이 존재하지 않습니다.", $"{item.MASTERFILE}");
                        ImmediateDeleteAudioFile(item.AUDIOCLIPID, dto.SystemCd, dto.UserId);
                    }
                    else
                    {
                        if (!dto.PermanentlyVal)
                        {
                            var fileName = Path.GetFileName(item.MASTERFILE);
                            string recyclePath = Path.Combine(Path.GetDirectoryName(item.MASTERFILE), "RECYCLE");
                            if (!Directory.Exists(recyclePath))
                            {
                                Directory.CreateDirectory(recyclePath);
                            }
                            File.Move(item.MASTERFILE, Path.Combine(recyclePath, fileName));
                            DeleteAudioFileAndMoveToRecycle(item.AUDIOCLIPID, Path.Combine(recyclePath, fileName), dto.SystemCd, dto.UserId);
                        }
                        else
                        {
                            File.Delete(item.MASTERFILE);
                            ImmediateDeleteAudioFile(item.AUDIOCLIPID, dto.SystemCd, dto.UserId);
                        }

                        var egyFilePath = Path.ChangeExtension(item.MASTERFILE, ".egy");
                        if (File.Exists(egyFilePath))
                        {
                            File.Delete(egyFilePath);
                            _dbLogger.InfoAsync(dto.SystemCd, dto.UserId, $"소재 삭제 EGY File 삭제 (수동) - {item.AUDIOCLIPID}", null);
                        }

                        var editfolderName = Directory.GetParent(Directory.GetParent(item.MASTERFILE).FullName).FullName;
                        var editfilePath = Path.Combine(editfolderName, "EDIT", item.AUDIOCLIPID);
                        if (Directory.Exists(editfilePath))
                        {
                            Directory.Delete(editfilePath, true);
                            _dbLogger.InfoAsync(dto.SystemCd, dto.UserId, $"소재 삭제 EDIT File 삭제 (수동) - {item.AUDIOCLIPID}", null);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    _dbLogger.ErrorAsync(dto.SystemCd, dto.UserId, $"소재 삭제 (수동) - {item.AUDIOCLIPID} : {ex.Message}", $"{item.MASTERFILE}");
                }
            }
            return true;
        }
        private bool DeleteAudioFileAndMoveToRecycle(string audioClipId, string FilePath, string systemCd, string userId )
        {
                DeleteProductFileParam param = new DeleteProductFileParamBuilder()
                    .SetAudioClipId(audioClipId)
                    .SetFilePath(FilePath)
                    .SetUserId(userId)
                    .Build();
            var result = _dao_del.DeleteAudioFileAndMoveToTrash(param);
            _dbLogger.InfoAsync(systemCd, userId, $"소재 삭제 MIROS 휴지통 Move (수동) - {audioClipId}", null);

            return result;
        }
        private bool ImmediateDeleteAudioFile(string audioClipId, string systemCd, string userId)
        {
            DeleteAudioFileParam param = new DeleteAudioFileParamBuilder()
                .SetAudioClipid(audioClipId)
                .Build();
            var result = _dao_del.ImmediateDeleteAudioFile(param);
            _dbLogger.InfoAsync(systemCd, userId, $"소재 삭제 File Delete (수동) - {audioClipId}", null);
            return result;
        }
        private bool DeleteRecycleData(string audioClipId,string systemCd, string userId)
        {
            DeleteAudioFileParam param = new DeleteAudioFileParamBuilder()
                .SetAudioClipid(audioClipId)
                .Build();
            var result = _dao_del.DeleteRecycle(param);
            _dbLogger.InfoAsync(systemCd, userId, $"MIROS 휴지통 File Delete (수동) - {audioClipId}", null);
            return result;
        }
        #endregion

    }
}

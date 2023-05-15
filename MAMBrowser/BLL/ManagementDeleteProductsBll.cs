using M30_ManagementDAO.Interfaces;
using M30_ManagementDAO.ParamEntity;
using static MAMBrowser.DTO.ManagementSystemDTO;
using System.Collections.Generic;
using MAMBrowser.Utils;
using static MAMBrowser.DTO.ManagementDeleteProductsDTO;
using MAMBrowser.DTO;
using M30_CueSheetDAO.ParamEntity;
using M30.AudioFile.DAL.Repositories;
using M30.AudioFile.Common;
using MAMBrowser.Foundation;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using MAMBrowser.Entiies;
using System.Web.Http.Tracing;

namespace MAMBrowser.BLL
{
    public class ManagementDeleteProductsBll
    {
        private readonly IDelManagementDAO _dao_del;
        public ManagementDeleteProductsBll(IDelManagementDAO dao_del)
        {
            _dao_del = dao_del;
        }
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
        public bool DeleteAudioFiles(DeleteAudioClipIdsParamDTO dto)
        {
            foreach (var item in dto.IDs)
            {
                if (string.IsNullOrEmpty(item.MASTERFILE)||!File.Exists(item.MASTERFILE))
                {
                    //_dbLogger.WarnAsync(HttpContext, userId, $"마스터링 파일삭제(자동) - {audioClipId} : 파일을 찾을 수 없습니다.", $"{filePath}").Wait();
                    ImmediateDeleteAudioFile(item.AUDIOCLIPID);
                }
                else
                {
                    if (!dto.PermanentlyVal)
                    {
                        //miros 휴지통으로
                        var fileName = Path.GetFileName(item.MASTERFILE);
                        string recyclePath = Path.Combine(Path.GetDirectoryName(item.MASTERFILE), "RECYCLE");
                        if (!Directory.Exists(recyclePath))
                        {
                            Directory.CreateDirectory(recyclePath);
                        }
                        File.Move(item.MASTERFILE, Path.Combine(recyclePath, fileName));
                        DeleteAudioFileAndMoveToRecycle(item.AUDIOCLIPID, Path.Combine(recyclePath, fileName), dto.UserId);
                    }
                    else
                    {
                        //영구삭제
                        File.Delete(item.MASTERFILE);
                        ImmediateDeleteAudioFile(item.AUDIOCLIPID);
                    }

                    var egyFilePath = Path.ChangeExtension(item.MASTERFILE, ".egy");
                    if (File.Exists(egyFilePath))
                    {
                        File.Delete(egyFilePath);
                        //_dbLogger.InfoAsync(HttpContext, userId, $"마스터링 EGY파일 삭제(자동) - {audioClipId}", null).Wait();
                    }

                    var editfolderName = Directory.GetParent(Directory.GetParent(item.MASTERFILE).FullName).FullName;
                    var editfilePath = Path.Combine(editfolderName, "EDIT", item.AUDIOCLIPID);
                    if (Directory.Exists(editfilePath))
                    {
                        Directory.Delete(editfilePath, true);
                        //_dbLogger.InfoAsync(HttpContext, userId, $"마스터링 EGY파일 삭제(자동) - {audioClipId}", null).Wait();
                    }
                }

            }
            return true;
        }
        private bool DeleteAudioFileAndMoveToRecycle(string audioClipId, string FilePath,string UserId )
        {
                DeleteProductFileParam param = new DeleteProductFileParamBuilder()
                    .SetAudioClipId(audioClipId)
                    .SetFilePath(FilePath)
                    .SetUserId(UserId)
                    .Build();
            var result = _dao_del.DeleteAudioFileAndMoveToTrash(param);
            //_dbLogger.InfoAsync(HttpContext, userId, $"마스터링 EGY파일 삭제(자동) - {audioClipId}", null).Wait();

            return result;
        }
        private bool ImmediateDeleteAudioFile(string audioClipId)
        {
            DeleteAudioFileParam param = new DeleteAudioFileParamBuilder()
                .SetAudioClipid(audioClipId)
                .Build();
            var result = _dao_del.ImmediateDeleteAudioFile(param);
            //_dbLogger.InfoAsync(HttpContext, userId, $"마스터링 EGY파일 삭제(자동) - {audioClipId}", null).Wait();
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


    }
}

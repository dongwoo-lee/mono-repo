using MAMBrowser.BLL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using M30_ManagementDAO.Entity;
using MAMBrowser.DTO;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common;
using static MAMBrowser.DTO.ManagementSystemDTO;
using M30_ManagementDAO.ParamEntity;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementSystemController : ControllerBase
    {
        private readonly ManagementSystemBll _bll;
        public ManagementSystemController(ManagementSystemBll bll)
        {
            _bll = bll;
        }

        #region 그룹관리

        [HttpPost("GetGroup")]
        public DTO_RESULT<List<GroupDTO>> GetGroup()
        {
            DTO_RESULT<List<GroupDTO>> result = new DTO_RESULT<List<GroupDTO>>();
            try
            {
                result.ResultObject = _bll.GetGroup();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddGroup")]
        public DTO_RESULT<bool> AddGroup([FromBody] GroupDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddGroup(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateGroup")]
        public DTO_RESULT<bool> UpdateGroup([FromBody] GroupDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdataGroup(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteGroup")]
        public DTO_RESULT<bool> DeleteGroup([FromQuery] string role)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteGroup(role);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
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

        #region 사용자관리

        [HttpPost("GetUser")]
        public DTO_RESULT<List<UserDTO>> GetUser()
        {
            DTO_RESULT<List<UserDTO>> result = new DTO_RESULT<List<UserDTO>>();
            try
            {
                result.ResultObject = _bll.GetUser();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateUser")]
        public DTO_RESULT<bool> UpdateUser([FromBody] UserDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateUser(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteUser")]
        public DTO_RESULT<bool> DeleteUser([FromQuery] string personid)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteUser(personid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddUser")]
        public DTO_RESULT<bool> AddUser([FromBody] UserDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddUser(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        #endregion

        #region 코드관리

        [HttpPost("GetStspCode")]
        public DTO_RESULT<List<StspCodeDTO>> GetStspCode()
        {
            DTO_RESULT<List<StspCodeDTO>> result = new DTO_RESULT<List<StspCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetStspCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetSongsCode")]
        public DTO_RESULT<List<SongsCodeDTO>> GetSongsCode()
        {
            DTO_RESULT<List<SongsCodeDTO>> result = new DTO_RESULT<List<SongsCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetSongsCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetSongmCode")]
        public DTO_RESULT<List<SongmCodeDTO>> GetSongmCode()
        {
            DTO_RESULT<List<SongmCodeDTO>> result = new DTO_RESULT<List<SongmCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetSongmCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetRptCode")]
        public DTO_RESULT<List<RptCodeDTO>> GetRptCode()
        {
            DTO_RESULT<List<RptCodeDTO>> result = new DTO_RESULT<List<RptCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetRptCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetAudioCode")]
        public DTO_RESULT<List<AudioCodeDTO>> GetAudioCode()
        {
            DTO_RESULT<List<AudioCodeDTO>> result = new DTO_RESULT<List<AudioCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetAudioCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetFillerCode")]
        public DTO_RESULT<List<FillerCodeDTO>> GetFillerCode()
        {
            DTO_RESULT<List<FillerCodeDTO>> result = new DTO_RESULT<List<FillerCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetFillerCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetEtcCode")]
        public DTO_RESULT<List<EtcCodeDTO>> GetEtcCode()
        {
            DTO_RESULT<List<EtcCodeDTO>> result = new DTO_RESULT<List<EtcCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetEtcCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetSpotCode")]
        public DTO_RESULT<List<SpotCodeDTO>> GetSpotCode()
        {
            DTO_RESULT<List<SpotCodeDTO>> result = new DTO_RESULT<List<SpotCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetSpotCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddStspCode")]
        public DTO_RESULT<bool> AddStspCode(StspCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddStspCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddSongmCode")]
        public DTO_RESULT<bool> AddSongmCode(SongmCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddSongmCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddSongsCode")]
        public DTO_RESULT<bool> AddSongsCode(SongsCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddSongsCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddRptCode")]
        public DTO_RESULT<bool> AddRptCode(RptCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddRptCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddFillerCode")]
        public DTO_RESULT<bool> AddFillerCode(FillerCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddFillerCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddEtcCode")]
        public DTO_RESULT<bool> AddEtcCode(EtcCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddEtcCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddAudioCode")]
        public DTO_RESULT<bool> AddAudioCode(AudioCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddAudioCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddSpotCode")]
        public DTO_RESULT<bool> AddSpotCode(SpotCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddSpotCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateStspCode")]
        public DTO_RESULT<bool> UpdateStspCode(StspCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateStspCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateSongmCode")]
        public DTO_RESULT<bool> UpdateSongmCode(SongmCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateSongmCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateSongsCode")]
        public DTO_RESULT<bool> UpdateSongsCode(SongsCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateSongsCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateRptCode")]
        public DTO_RESULT<bool> UpdateRptCode(RptCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateRptCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateFillerCode")]
        public DTO_RESULT<bool> UpdateFillerCode(FillerCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateFillerCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateEtcCode")]
        public DTO_RESULT<bool> UpdateEtcCode(EtcCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateEtcCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateAudioCode")]
        public DTO_RESULT<bool> UpdateAudioCode(AudioCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateAudioCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateSpotCode")]
        public DTO_RESULT<bool> UpdateSpotCode(SpotCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateSpotCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteStspCode")]
        public DTO_RESULT<bool> DeleteStspCode(DeleteCodeParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteStspCode(dto.codeid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteSongmCode")]
        public DTO_RESULT<bool> DeleteSongmCode(DeleteCodeParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteSongmCode(dto.codeid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteSongsCode")]
        public DTO_RESULT<bool> DeleteSongsCode(DeleteCodeParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteSongsCode(dto.mcodeid,dto.scodeid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteRptCode")]
        public DTO_RESULT<bool> DeleteRptCode(DeleteCodeParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteRptCode(dto.codeid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteFillerCode")]
        public DTO_RESULT<bool> DeleteFillerCode(DeleteCodeParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteFillerCode(dto.codeid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteEtcCode")]
        public DTO_RESULT<bool> DeleteEtcCode(DeleteCodeParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteEtcCode(dto.codeid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteAudioCode")]
        public DTO_RESULT<bool> DeleteAudioCode(DeleteCodeParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteAudioCode(dto.codeid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteSpotCode")]
        public DTO_RESULT<bool> DeleteSpotCode(DeleteCodeParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteSpotCode(dto.spotid);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        #endregion

        #region MIROS관리
        [HttpPost("GetCommCode")]
        public DTO_RESULT<List<CommCodeDTO>> GetCommCodeList()
        {
            DTO_RESULT<List<CommCodeDTO>> result = new DTO_RESULT<List<CommCodeDTO>>();
            try
            {
                result.ResultObject = _bll.GetCommCodeList();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateCommCode")]
        public DTO_RESULT<bool> UpdateCommCode([FromBody] CommCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateCommCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteCommCode")]
        public DTO_RESULT<bool> DeleteCommCode(DeleteMirosParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteCommCode(dto.code);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddCommCode")]
        public DTO_RESULT<bool> AddCommCode([FromBody] CommCodeDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddCommCode(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetCommMenuMap")]
        public DTO_RESULT<List<CommMenuMapDTO>> GetCommMenuMapList([FromBody] string mapCd)
        {
            DTO_RESULT<List<CommMenuMapDTO>> result = new DTO_RESULT<List<CommMenuMapDTO>>();
            try
            {
                result.ResultObject = _bll.GetCommMenuMapList(mapCd);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("UpdateCommMenuMap")]
        public DTO_RESULT<bool> UpdateCommMenuMap([FromBody] CommMenuMapDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.UpdateCommMenuMap(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpDelete("DeleteCommMenuMap")]
        public DTO_RESULT<bool> DeleteCommMenuMap(DeleteMirosParamDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteCommMenuMap(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("AddCommMenuMap")]
        public DTO_RESULT<bool> AddCommMenuMap([FromBody] CommMenuMapDTO dto)
        {
            DTO_RESULT<bool> result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.AddCommMenuMap(dto);
                if (!result.ResultObject)
                {
                    throw new Exception();
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        #endregion

        #region 소재 삭제 관리

        [HttpPost("GetDelAudioList")]
        public DTO_RESULT<List<AudioFileDTO>> GetDelAudioList([FromBody]SelectDelProductParamDTO dto)
        {
            DTO_RESULT<List<AudioFileDTO>> result = new DTO_RESULT<List<AudioFileDTO>>();
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


        #endregion

        #region Select Options
        [HttpGet("GetRoleOptions")]
        public DTO_RESULT<MenuList> GetRoleOptions()
        {
            DTO_RESULT<MenuList> result = new DTO_RESULT<MenuList>();
            try
            {
                result.ResultObject = _bll.GetRoleOptions();
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpGet("GetCodeIdOptions")]
        public DTO_RESULT<MenuList> GetCodeIdOptions()
        {
            DTO_RESULT<MenuList> result = new DTO_RESULT<MenuList>();
            try
            {
                result.ResultObject = _bll.GetCodeIdOptions();
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        [HttpPost("GetGroupByMirosCode")]
        public DTO_RESULT<MenuList> GetGroupByMirosCode([FromBody]GroupByCommCodeParamDTO dto)
        {
            DTO_RESULT<MenuList> result = new DTO_RESULT<MenuList>();
            try
            {
                result.ResultObject = _bll.GetGroupByCommCodeList(dto);
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

using M30_ManagementDAO.Entity;
using M30_ManagementDAO.Interfaces;
using M30_ManagementDAO.ParamEntity;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static MAMBrowser.DTO.ManagementSystemDTO;

namespace MAMBrowser.BLL
{
    public class ManagementSystemBll
    {
        private readonly IGroupManagementDAO _dao_group;
        private readonly IUserManagementDAO _dao_user;
        private readonly ICodeManagementDAO _dao_code;
        private readonly IMirosManagementDAO _dao_miros;
        public ManagementSystemBll(IGroupManagementDAO dao_group, IUserManagementDAO dao_user, ICodeManagementDAO dao_code, IMirosManagementDAO dao_miros)
        {
            _dao_group = dao_group;
            _dao_user = dao_user;
            _dao_code= dao_code;
            _dao_miros = dao_miros;
        }
        public MenuList GetRoleOptions()
        {
            return _dao_user.GetGroupByRoleList().Converting();
        }
        public MenuList GetCodeIdOptions()
        {
            return _dao_code.GetGroupByCodeIdList().Converting();
        }
        public MenuList GetGroupByCommCodeList(GroupByCommCodeParamDTO dto)
        {
            GroupByCommCodeParam param = new GroupByCommCodeParamBuilder()
                .SetCode(dto.code)
                .SetMaxLength(dto.maxLength)
                .SetParentCode(dto.parentCode)
                .Build();

            return _dao_miros.GetGroupByCommCodeList(param).Converting();
        }
        public List<GroupDTO> GetGroup()
        {
            return _dao_group.GetGroupManagementList().Converting();
        }
        public List<UserDTO> GetUser()
        {
            return _dao_user.GetUserManagementList().Converting();
        }
        public List<StspCodeDTO> GetStspCodeList() 
        {
            return _dao_code.GetStspCodeList().Converting();
        }
        public List<SongmCodeDTO> GetSongmCodeList() 
        {
            return _dao_code.GetSongmCodeList().Converting();
        }
        public List<SongsCodeDTO> GetSongsCodeList() 
        {
            return _dao_code.GetSongsCodeList().Converting();
        }
        public List<RptCodeDTO> GetRptCodeList() 
        {
            return _dao_code.GetRptCodeList().Converting();
        }
        public List<AudioCodeDTO> GetAudioCodeList() 
        {
            return _dao_code.GetAudioCodeList().Converting();
        }
        public List<FillerCodeDTO> GetFillerCodeList() 
        {
            return _dao_code.GetFillerCodeList().Converting();
        }
        public List<EtcCodeDTO> GetEtcCodeList() 
        {
            return _dao_code.GetEtcCodeList().Converting();
        }
        public List<SpotCodeDTO> GetSpotCodeList() 
        {
            return _dao_code.GetSpotCodeList().Converting();
        }
        public List<CommCodeDTO> GetCommCodeList()
        {
            return _dao_miros.GetCommCodeList().Converting();
        }
        public List<CommMenuMapDTO> GetCommMenuMapList(string mapCd)
        {
            return _dao_miros.GetCommMenuMapList(mapCd).Converting();
        }

        public bool AddGroup(GroupDTO group)
        {
            GroupManagementParam pram = new GroupManagementParamBuilder()
                .SetRole(group.ROLE)
                .SetDbId(group.DBID)
                .SetDbpasswd(group.DBPASSWD)
                .SetAppRole(group.APPROLE)
                .SetSysRole(group.SYSROLE)
                .SetServerRole(group.SERVERROLE)
                .SetCode(group.CODE.StringToByte())
                .SetRoleName(group.ROLE_NAME)
                .Build();

            return _dao_group.InsertGroupManagement(pram);
        }
        public bool AddUser(UserDTO user)
        {
            UserManagementParam pram = new UserManagementParamBuilder()
                .SetPersonID(user.PERSONID)
                .SetPasswd(user.PASSWD)
                .SetPersonName(user.PERSONNAME)
                .SetDevision(user.DEVISION)
                .SetDepartment(user.DEPARTMENT)
                .SetInDate(user.INDATE.ToString("yyyyMMdd"))
                .SetTel1(user.TEL1)
                .SetTel2(user.TEL2)
                .SetEmailID(user.EMAILID)
                .SetRank(user.RANK)
                .SetRole(user.ROLE)
                .Build();

            return _dao_user.InsertUserManagement(pram);
        }
        public bool AddStspCode(StspCodeDTO dto)
        {
            StspCodeParam pram = new StspCodeParamBuilder()
                .SetCodeId(dto.CODEID) 
                .SetCodeName(dto.CODENAME) 
                .SetCreator(dto.CREATOR) 
                .Build();
            return _dao_code.InsertStspCode(pram);
        }
        public bool AddSongmCode(SongmCodeDTO dto)
        {
            SongmCodeParam pram = new SongmCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.InsertSongmCode(pram);
        }
        public bool AddSongsCode(SongsCodeDTO dto)
        {
            SongsCodeParam pram = new SongsCodeParamBuilder()
                .SetMcodeId(dto.MCODEID)
                .SetScodeId(dto.SCODEID)
                .SetScodeName(dto.SCODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.InsertSongsCode(pram);
        }
        public bool AddRptCode(RptCodeDTO dto)
        {
            RptCodeParam pram = new RptCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.InsertRptCode(pram);
        }
        public bool AddFillerCode(FillerCodeDTO dto)
        {
            FillerCodeParam pram = new FillerCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.InsertFillerCode(pram);
        }
        public bool AddEtcCode(EtcCodeDTO dto)
        {
            EtcCodeParam pram = new EtcCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.InsertEtcCode(pram);
        }
        public bool AddAudioCode(AudioCodeDTO dto)
        {
            AudioCodeParam pram = new AudioCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .SetPd(dto.PD)
                .SetAd(dto.AD)
                .Build();
            return _dao_code.InsertAudioCode(pram);
        }
        public bool AddSpotCode(SpotCodeDTO dto)
        {
            SpotCodeParam pram = new SpotCodeParamBuilder()
                .SetSpotId(dto.SPOTID)
                .SetSpotName(dto.SPOTNAME)
                .SetMedia(dto.MEDIA)
                .Build();
            return _dao_code.InsertSpotCode(pram);
        }
        public bool AddCommCode(CommCodeDTO dto)
        {
            CommCodeParam pram = new CommCodeParamBuilder()
                .SetCode(dto.CODE)
                .SetParentCode(dto.PARENT_CODE)
                .SetName(dto.NAME)
                .Build();
            return _dao_miros.InsertCommCode(pram);
        }
        public bool AddCommMenuMap(CommMenuMapDTO dto)
        {
            CommMenuMapParam pram = new CommMenuMapParamBuilder()
                .SetSystemCd(dto.SYSTEM_CD)
                .SetMapCd(dto.MAP_CD)
                .SetGrpCd(dto.GRP_CD)
                .SetCode(dto.CODE)
                .SetVisible(dto.VISIBLE)
                .SetEnable(dto.ENABLE)
                .Build();
            return _dao_miros.InsertCommMenuMap(pram);
        }

        public bool UpdataGroup(GroupDTO group)
        {
            GroupManagementParam pram = new GroupManagementParamBuilder()
                .SetRole(group.ROLE)
                .SetDbId(group.DBID)
                .SetDbpasswd(group.DBPASSWD)
                .SetAppRole(group.APPROLE)
                .SetSysRole(group.SYSROLE)
                .SetServerRole(group.SERVERROLE)
                .SetCode(group.CODE.StringToByte())
                .SetRoleName(group.ROLE_NAME)
                .Build();

            return _dao_group.UpdateGroupManagement(pram);
        }
        public bool UpdateUser(UserDTO user)
        {
            UserManagementParam pram = new UserManagementParamBuilder()
                .SetPersonID(user.PERSONID)
                .SetPasswd(user.PASSWD)
                .SetPersonName(user.PERSONNAME)
                .SetDevision(user.DEVISION)
                .SetDepartment(user.DEPARTMENT)
                .SetInDate(user.INDATE.ToString("yyyyMMdd"))
                .SetTel1(user.TEL1)
                .SetTel2(user.TEL2)
                .SetEmailID(user.EMAILID)
                .SetRank(user.RANK)
                .SetRole(user.ROLE)
                .Build();

            return _dao_user.UpdateUserManagement(pram);
        }
        public bool UpdateStspCode(StspCodeDTO dto)
        {
            StspCodeParam pram = new StspCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.UpdateStspCode(pram);
        }
        public bool UpdateSongmCode(SongmCodeDTO dto)
        {
            SongmCodeParam pram = new SongmCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.UpdateSongmCode(pram);
        }
        public bool UpdateSongsCode(SongsCodeDTO dto)
        {
            SongsCodeParam pram = new SongsCodeParamBuilder()
                .SetMcodeId(dto.MCODEID)
                .SetScodeId(dto.SCODEID)
                .SetScodeName(dto.SCODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.UpdateSongsCode(pram);
        }
        public bool UpdateRptCode(RptCodeDTO dto)
        {
            RptCodeParam pram = new RptCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.UpdateRptCode(pram);
        }
        public bool UpdateFillerCode(FillerCodeDTO dto)
        {
            FillerCodeParam pram = new FillerCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.UpdateFillerCode(pram);
        }
        public bool UpdateEtcCode(EtcCodeDTO dto)
        {
            EtcCodeParam pram = new EtcCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .Build();
            return _dao_code.UpdateEtcCode(pram);
        }
        public bool UpdateAudioCode(AudioCodeDTO dto)
        {
            AudioCodeParam pram = new AudioCodeParamBuilder()
                .SetCodeId(dto.CODEID)
                .SetCodeName(dto.CODENAME)
                .SetCreator(dto.CREATOR)
                .SetPd(dto.PD)
                .SetAd(dto.AD)
                .Build();
            return _dao_code.UpdateAudioCode(pram);
        }
        public bool UpdateSpotCode(SpotCodeDTO dto)
        {
            SpotCodeParam pram = new SpotCodeParamBuilder()
                .SetSpotId(dto.SPOTID)
                .SetSpotName(dto.SPOTNAME)
                .SetMedia(dto.MEDIA)
                .Build();
            return _dao_code.UpdateSpotCode(pram);
        }
        public bool UpdateCommCode(CommCodeDTO dto)
        {
            CommCodeParam pram = new CommCodeParamBuilder()
                .SetCode(dto.CODE)
                .SetParentCode(dto.PARENT_CODE)
                .SetName(dto.NAME)
                .Build();
            return _dao_miros.UpdateCommCode(pram);
        }
        public bool UpdateCommMenuMap(CommMenuMapDTO dto)
        {
            CommMenuMapParam pram = new CommMenuMapParamBuilder()
                .SetSystemCd(dto.SYSTEM_CD)
                .SetMapCd(dto.MAP_CD)
                .SetGrpCd(dto.GRP_CD)
                .SetCode(dto.CODE)
                .SetVisible(dto.VISIBLE)
                .SetEnable(dto.ENABLE)
                .Build();
            return _dao_miros.UpdateCommMenuMap(pram);
        }

        public bool DeleteGroup(string role)
        {
            DeleteRoleParam pram = new DeleteRoleParamBuilder().SetRole(role).Build();
            return _dao_group.DeleteGroupManagement(pram);
        }
        public bool DeleteUser(string personid)
        {
            DeleteUserParam pram = new DeleteUserParamBuilder().SetPersonID(personid).Build();
            return _dao_user.DeleteUserManagement(pram);
        }
        public bool DeleteStspCode(string codeId)
        {
            DeleteStspCodeParam pram = new DeleteStspCodeParamBuilder()
                .SetCodeId(codeId)
                .Build();
            return _dao_code.DeleteStspCode(pram);
        }
        public bool DeleteSongmCode(string codeId)
        {
            DeleteSongmCodeParam pram = new DeleteSongmCodeParamBuilder()
                .SetCodeId(codeId)
                .Build();
            return _dao_code.DeleteSongmCode(pram);
        }
        public bool DeleteSongsCode(string mcodeId, string scodeId)
        {
            DeleteSongsCodeParam pram = new DeleteSongsCodeParamBuilder()
                .SetMcodeId(mcodeId)
                .SetScodeId(scodeId)
                .Build();
            return _dao_code.DeleteSongsCode(pram);
        }
        public bool DeleteRptCode(string codeId)
        {
            DeleteRptCodeParam pram = new DeleteRptCodeParamBuilder()
                .SetCodeId(codeId)
                .Build();
            return _dao_code.DeleteRptCode(pram);
        }
        public bool DeleteFillerCode(string codeId)
        {
            DeleteFillerCodeParam pram = new DeleteFillerCodeParamBuilder()
                .SetCodeId(codeId)
                .Build();
            return _dao_code.DeleteFillerCode(pram);
        }
        public bool DeleteEtcCode(string codeId)
        {
            DeleteEtcCodeParam pram = new DeleteEtcCodeParamBuilder()
                .SetCodeId(codeId)
                .Build();
            return _dao_code.DeleteEtcCode(pram);
        }
        public bool DeleteAudioCode(string codeId)
        {
            DeleteAudioCodeParam pram = new DeleteAudioCodeParamBuilder()
                .SetCodeId(codeId)
                .Build();
            return _dao_code.DeleteAudioCode(pram);
        }
        public bool DeleteSpotCode(string spotId)
        {
            DeleteSpotCodeParam pram = new DeleteSpotCodeParamBuilder()
                .SetSpotId(spotId)
                .Build();
            return _dao_code.DeleteSpotCode(pram);
        }
        public bool DeleteCommCode(string code) 
        {
            DeleteCommCodeParam pram = new DeleteCommCodeParamBuilder()
                .SetCode(code) 
                .Build();
            return _dao_miros.DeleteCommCode(pram);
        }
        public bool DeleteCommMenuMap(DeleteMirosParamDTO dto)
        {
            DeleteCommMenuMapParam pram = new DeleteCommMenuMapParamBuilder()
                .SetSystemCd(dto.systeM_CD)
                .SetMapCd(dto.maP_CD)
                .SetGrpCd(dto.grP_CD)
                .SetCode(dto.code)
                .Build();
            return _dao_miros.DeleteCommMenuMap(pram);
        }

    }
}

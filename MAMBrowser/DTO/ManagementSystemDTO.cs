using System;
using System.Collections.Generic;

namespace MAMBrowser.DTO
{
    public class ManagementSystemDTO
    {
        public class GroupDTO
        {
            public string ROLE { get; set; }
            public string DBID { get; set; }
            public string DBPASSWD { get; set; }
            public string APPROLE { get; set; }
            public string SYSROLE { get; set; }
            public string SERVERROLE { get; set; }
            public string CODE { get; set; }
            public string ROLE_NAME { get; set; }
        }
        public class UserDTO
        {
            public string PERSONID { get; set; }
            public string PASSWD { get; set; }
            public string PERSONNAME { get; set; }
            public string DEVISION { get; set; }
            public string DEPARTMENT { get; set; }
            public DateTime INDATE { get; set; }
            public string TEL1 { get; set; }
            public string TEL2 { get; set; }
            public string EMAILID { get; set; }
            public string RANK { get; set; }
            public string ROLE { get; set; }
        }
        public class CodeBaseDTO
        {
            public string CODEID { get; set; }
            public string CODENAME { get; set; }
            public string CREATOR { get; set; }
        }
        public class StspCodeDTO : CodeBaseDTO { }
        public class SongmCodeDTO : CodeBaseDTO { }
        public class SongsCodeDTO
        {
            public string SCODEID { get; set; }
            public string SCODENAME { get; set; }
            public string CREATOR { get; set; }
            public string MCODEID { get; set; }
        }
        public class RptCodeDTO : CodeBaseDTO { }
        public class AudioCodeDTO : CodeBaseDTO
        {
            public string PD { get; set; }
            public string AD { get; set; }
        }
        public class FillerCodeDTO : CodeBaseDTO { }
        public class EtcCodeDTO : CodeBaseDTO { }
        public class SpotCodeDTO
        {
            public string SPOTID { get; set; }
            public string SPOTNAME { get; set; }
            public char MEDIA { get; set; }
        }
        public class CommCodeDTO
        {
            public string CODE { get; set; }
            public string PARENT_CODE { get; set; }
            public string NAME { get; set; }
        }
        public class CommMenuMapDTO
        {
            public string SYSTEM_CD { get; set; }
            public string MAP_CD { get; set; }
            public string GRP_CD { get; set; }
            public string CODE { get; set; }
            public char VISIBLE { get; set; }
            public char ENABLE { get; set; }
        }
        public class BaseFileDTO
        {
            public string AUDIOCLIPID { get; set; }
            public string MASTERFILE { get; set; }
            public DateTime MASTERTIME { get; set; }
            public DateTime EDITTIME { get; set; }
            public string EDITFILE { get; set; }
            public string ENERGYFILE { get; set; }
            public string CALLFILE { get; set; }
            public string EDITOR { get; set; }
        }
        public class AudioFileDTO : BaseFileDTO
        {
            public string AUDIONAME { get; set; }
            public DateTime LASTONAIRDATE { get; set; }
        }
        public class SpotFileDTO : BaseFileDTO
        {
            public string SPOTNAME { get; set; }
            public string OPRSPOTID { get; set; }
        }
        public class EtcFileDTO : BaseFileDTO
        {
            public string AUDIONAME { get; set; }
        }
        public class FillerFileDTO : BaseFileDTO
        {
            public string FILLERNAME { get; set; }
            public string ENDDATE { get; set; }
        }
        public class ReportFileDTO : BaseFileDTO
        {
            public string REPORTNAME { get; set; }
            public string ONAIRDATE { get; set; }
        }
        public class ProductFileDTO : BaseFileDTO
        {
            public string EVENTNAME { get; set; }
            public string ONAIRDATE { get; set; }
        }
        public class SongFileDTO : BaseFileDTO
        {
            public string SONGNAME { get; set; }
            public string LASTONAIRDATE { get; set; }
        }
        public class OptionsDTO
        {
            public string value { get; set; }
            public string text { get; set; }

        }
        public class MenuList
        {
            public List<OptionsDTO> role { get; set; }
            public List<OptionsDTO> mcodeid { get; set; }
            public List<OptionsDTO> code { get; set; }
        }
        public class DeleteCodeParamDTO
        {
            public string codeid { get; set; }
            public string mcodeid { get; set; }
            public string scodeid { get; set; }
            public string spotid { get; set; }
        }
        public class DeleteMirosParamDTO
        {
            public string systeM_CD { get; set; }
            public string maP_CD { get; set; }
            public string grP_CD { get; set; }
            public string code { get; set; }
        }
        public class GroupByCommCodeParamDTO
        {
            public string code { get; set; }
            public int maxLength { get; set; }
            public string parentCode { get; set; }
        }

        public class SelectDelProductParamDTO
        {
            public string name { get; set; }
            public string startdate { get; set; }
            public string enddate { get; set; }
            public string onairdate { get; set; }
            public char media { get; set; }
        }
    }

}

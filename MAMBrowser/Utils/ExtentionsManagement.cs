using M30_ManagementControlDAO.Entity;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using static MAMBrowser.DTO.ManagementDeleteProductsDTO;
using static MAMBrowser.DTO.ManagementSystemDTO;

namespace MAMBrowser.Utils
{
    public static class ExtentionsManagement
    {
        public static List<GroupDTO> Converting(this List<GroupManagementEntity> entity)
        {
            var result = new List<GroupDTO>();
            foreach (var group in entity)
            {
                var item = new GroupDTO();
                item.ROLE = group.ROLE;
                item.DBID = group.DBID;
                item.DBPASSWD = group.DBPASSWD;
                item.APPROLE = group.APPROLE;
                item.SYSROLE = group.SYSROLE;
                item.SERVERROLE = group.SERVERROLE;
                item.CODE = BitConverter.ToString(group.CODE).Replace("-", "");
                item.ROLE_NAME= group.ROLE_NAME;
                result.Add(item);
            }
            return result;
        }
        public static List<UserDTO> Converting(this List<UserManagementEntity> entity)
        {
            var result = new List<UserDTO>();
            foreach(var user in entity)
            {
                var item = new UserDTO();
                item.PERSONID = user.PERSONID;
                item.PASSWD = user.PASSWD;
                item.PERSONNAME = user.PERSONNAME;
                item.DEVISION = user.DEVISION;
                item.DEPARTMENT = user.DEPARTMENT;
                item.INDATE = DateTime.ParseExact(user.INDATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                item.TEL1 = user.TEL1;
                item.TEL2 = user.TEL2;
                item.EMAILID = user.EMAILID;
                item.RANK = user.RANK;
                item.ROLE = user.ROLE;
                result.Add(item);
            }
            return result;
        }
        public static byte[] StringToByte(this string val)
        {
            byte[] result = new byte[val.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(val.Substring(i * 2, 2), 16);
            }
            return result;
        }
        public static MenuList Converting(this List<GroupByRoleEntity> entity)
        {
            var result = new MenuList();
            result.role = new List<OptionsDTO>();
            foreach(var option in entity)
            {
                var item = new OptionsDTO();
                item.text = option.ROLE;
                item.value = option.ROLE;
                result.role.Add(item);
            }
            return result;
        }
        public static MenuList Converting(this List<GroupByCodeIdEntity> entity)
        {
            var result = new MenuList();
            result.mcodeid = new List<OptionsDTO>();
            foreach (var option in entity)
            {
                var item = new OptionsDTO();
                item.text = option.CODEID;
                item.value = option.CODEID;
                result.mcodeid.Add(item);
            }
            return result;
        }
        public static MenuList Converting(this List<GroupByCommCodeEntity> entity)
        {
            var result = new MenuList();
            result.code = new List<OptionsDTO>();
            foreach (var option in entity)
            {
                var item = new OptionsDTO();
                item.text = option.NAME + " <" + option.CODE + ">";
                item.value = option.CODE;
                result.code.Add(item);
            }
            return result;
        }
        public static MenuList Converting(this List<GroupByDevisionEntity> entity)
        {
            var groupEntity = entity.GroupBy(g => g.DEVISION);
            var result = new MenuList();
            result.devision = new List<OptionsDTO>();
            foreach (var group in groupEntity)
            {
                var item = new OptionsDTO();
                item.text = group.FirstOrDefault().DEVNAME;
                item.value = group.Key;
                result.devision.Add(item);
            }
            return result;
        }
        public static MenuList Converting(this List<GroupByDevisionEntity> entity, string devision="")
        {
            var devNameEntitys = entity.Where(d=>d.DEVISION==devision).ToList();
            var result = new MenuList();
            result.department = new List<OptionsDTO>();
            foreach (var option in devNameEntitys)
            {
                var item = new OptionsDTO();
                item.text = option.DPTNAME;
                item.value = option.DEPARTMENT;
                result.department.Add(item);
            }
            return result;
        }

        public static List<StspCodeDTO> Converting(this List<StspCodeEntity> entity)
        {
            var result = new List<StspCodeDTO>();
            foreach (var code in entity)
            {
                var item = new StspCodeDTO();
                item.CODEID = code.CODEID;
                item.CODENAME = code.CODENAME;
                item.CREATOR = code.CREATOR;
                result.Add(item);
            }
            return result;
        }
        public static List<SongmCodeDTO> Converting(this List<SongmCodeEntity> entity)
        {
            var result = new List<SongmCodeDTO>();
            foreach (var code in entity)
            {
                var item = new SongmCodeDTO();
                item.CODEID = code.CODEID;
                item.CODENAME = code.CODENAME;
                item.CREATOR = code.CREATOR;
                result.Add(item);
            }
            return result;
        }
        public static List<SongsCodeDTO> Converting(this List<SongsCodeEntity> entity)
        {
            var result = new List<SongsCodeDTO>();
            foreach (var code in entity)
            {
                var item = new SongsCodeDTO();
                item.SCODEID = code.SCODEID;
                item.SCODENAME = code.SCODENAME;
                item.CREATOR = code.CREATOR;
                item.MCODEID= code.MCODEID;
                result.Add(item);
            }
            return result;
        }
        public static List<RptCodeDTO> Converting(this List<RptCodeEntity> entity)
        {
            var result = new List<RptCodeDTO>();
            foreach (var code in entity)
            {
                var item = new RptCodeDTO();
                item.CODEID = code.CODEID;
                item.CODENAME = code.CODENAME;
                item.CREATOR = code.CREATOR;
                result.Add(item);
            }
            return result;
        }
        public static List<AudioCodeDTO> Converting(this List<AudioCodeEntity> entity)
        {
            var result = new List<AudioCodeDTO>();
            foreach (var code in entity)
            {
                var item = new AudioCodeDTO();
                item.CODEID = code.CODEID;
                item.CODENAME = code.CODENAME;
                item.CREATOR = code.CREATOR;
                item.PD = code.PD;
                item.PDID = code.PDID;
                item.AD = code.AD;
                item.ADID = code.ADID;
                result.Add(item);
            }
            return result;
        }
        public static List<FillerCodeDTO> Converting(this List<FillerCodeEntity> entity)
        {
            var result = new List<FillerCodeDTO>();
            foreach (var code in entity)
            {
                var item = new FillerCodeDTO();
                item.CODEID = code.CODEID;
                item.CODENAME = code.CODENAME;
                item.CREATOR = code.CREATOR;
                result.Add(item);
            }
            return result;
        }
        public static List<EtcCodeDTO> Converting(this List<EtcCodeEntity> entity)
        {
            var result = new List<EtcCodeDTO>();
            foreach (var code in entity)
            {
                var item = new EtcCodeDTO();
                item.CODEID = code.CODEID;
                item.CODENAME = code.CODENAME;
                item.CREATOR = code.CREATOR;
                result.Add(item);
            }
            return result;
        }
        public static List<SpotCodeDTO> Converting(this List<SpotCodeEntity> entity)
        {
            var result = new List<SpotCodeDTO>();
            foreach (var code in entity)
            {
                var item = new SpotCodeDTO();
                item.SPOTID = code.SPOTID;
                item.SPOTNAME = code.SPOTNAME;
                item.MEDIA = code.MEDIA;
                result.Add(item);
            }
            return result;
        }
        public static List<CommCodeDTO> Converting(this List<CommCodeEntity> entity)
        {
            var result = new List<CommCodeDTO>();
            foreach (var code in entity)
            {
                var item = new CommCodeDTO();
                item.CODE = code.CODE;
                item.PARENT_CODE = code.PARENT_CODE;
                item.NAME = code.NAME;
                result.Add(item);
            }
            return result;
        }
        public static List<CommMenuMapDTO> Converting(this List<CommMenuMapEntity> entity)
        {
            var result = new List<CommMenuMapDTO>();
            foreach (var code in entity)
            {
                var item = new CommMenuMapDTO();
                item.SYSTEM_CD = code.SYSTEM_CD;
                item.MAP_CD= code.MAP_CD;
                item.GRP_CD= code.GRP_CD;
                item.CODE= code.CODE;
                item.VISIBLE= code.VISIBLE;
                item.ENABLE= code.ENABLE;
                result.Add(item);
            }
            return result;
        }
        public static BaseFileDTO SetBaseFileDTO(BaseFileEntity entity)
        {
            BaseFileDTO result = new BaseFileDTO();
            result.AUDIOCLIPID = entity.AUDIOCLIPID;
            result.NAME= entity.NAME;
            result.MASTERFILE = entity.MASTERFILE ?? "";
            result.MASTERTIME = entity.MASTERTIME != null ? DateTime.ParseExact(entity.MASTERTIME, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture): default(DateTime);
            result.EDITTIME = entity.EDITTIME != null ? DateTime.ParseExact(entity.EDITTIME, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture) : default(DateTime);
            result.EDITFILE = entity.EDITFILE ?? "";
            result.ENERGYFILE = entity.ENERGYFILE ?? "";
            result.CALLFILE = entity.CALLFILE ?? "";
            result.EDITOR = entity.EDITOR;
            return result;
        }
        public static List<AudioFileDTO> Converting(this List<AudioFileEntity> entitys)
        {
            var result = new List<AudioFileDTO>();
            foreach (var entity in entitys)
            {
                BaseFileDTO baseItem = SetBaseFileDTO(entity);
                AudioFileDTO item = new AudioFileDTO()
                {
                    AUDIOCLIPID = baseItem.AUDIOCLIPID,
                    NAME= baseItem.NAME,
                    MASTERFILE = baseItem.MASTERFILE,
                    MASTERTIME = baseItem.MASTERTIME,
                    EDITTIME = baseItem.EDITTIME,
                    EDITFILE = baseItem.EDITFILE,
                    ENERGYFILE = baseItem.ENERGYFILE,
                    CALLFILE = baseItem.CALLFILE,
                    EDITOR = baseItem.EDITOR
                };
                item.LASTONAIRDATE = entity.LASTONAIRDATE!=null
                    ?DateTime.ParseExact(entity.LASTONAIRDATE, "yyyyMMdd", CultureInfo.InvariantCulture)
                    :default(DateTime);
                result.Add(item);
            }
            return result;
        }
        public static List<SpotFileDTO> Converting(this List<SpotFileEntity> entitys)
        {
            var result = new List<SpotFileDTO>();
            foreach (var entity in entitys)
            {
                BaseFileDTO baseItem = SetBaseFileDTO(entity);
                SpotFileDTO item = new SpotFileDTO()
                {
                    AUDIOCLIPID = baseItem.AUDIOCLIPID,
                    NAME = baseItem.NAME,
                    MASTERFILE = baseItem.MASTERFILE,
                    MASTERTIME = baseItem.MASTERTIME,
                    EDITTIME = baseItem.EDITTIME,
                    EDITFILE = baseItem.EDITFILE,
                    ENERGYFILE = baseItem.ENERGYFILE,
                    CALLFILE = baseItem.CALLFILE,
                    EDITOR = baseItem.EDITOR
                };
                item.OPRSPOTID = entity.OPRSPOTID ?? "";
                result.Add(item);
            }
            return result;
        }
        public static List<EtcFileDTO> Converting(this List<EtcFileEntity> entitys)
        {
            var result = new List<EtcFileDTO>();
            foreach (var entity in entitys)
            {
                BaseFileDTO baseItem = SetBaseFileDTO(entity);
                EtcFileDTO item = new EtcFileDTO()
                {
                    AUDIOCLIPID = baseItem.AUDIOCLIPID,
                    NAME = baseItem.NAME,
                    MASTERFILE = baseItem.MASTERFILE,
                    MASTERTIME = baseItem.MASTERTIME,
                    EDITTIME = baseItem.EDITTIME,
                    EDITFILE = baseItem.EDITFILE,
                    ENERGYFILE = baseItem.ENERGYFILE,
                    CALLFILE = baseItem.CALLFILE,
                    EDITOR = baseItem.EDITOR
                };
                result.Add(item);
            }
            return result;
        }
        public static List<FillerFileDTO> Converting(this List<FillerFileEntity> entitys)
        {
            var result = new List<FillerFileDTO>();
            foreach (var entity in entitys)
            {
                BaseFileDTO baseItem = SetBaseFileDTO(entity);
                FillerFileDTO item = new FillerFileDTO()
                {
                    AUDIOCLIPID = baseItem.AUDIOCLIPID,
                    NAME = baseItem.NAME,
                    MASTERFILE = baseItem.MASTERFILE,
                    MASTERTIME = baseItem.MASTERTIME,
                    EDITTIME = baseItem.EDITTIME,
                    EDITFILE = baseItem.EDITFILE,
                    ENERGYFILE = baseItem.ENERGYFILE,
                    CALLFILE = baseItem.CALLFILE,
                    EDITOR = baseItem.EDITOR
                };
                item.ENDDATE = entity.ENDDATE != null 
                    ? DateTime.ParseExact(entity.ENDDATE, "yyyyMMdd", CultureInfo.InvariantCulture) 
                    : default(DateTime);
                result.Add(item);
            }
            return result;
        }
        public static List<ReportFileDTO> Converting(this List<ReportFileEntity> entitys)
        {
            var result = new List<ReportFileDTO>();
            foreach (var entity in entitys)
            {
                BaseFileDTO baseItem = SetBaseFileDTO(entity);
                ReportFileDTO item = new ReportFileDTO()
                {
                    AUDIOCLIPID = baseItem.AUDIOCLIPID,
                    NAME = baseItem.NAME,
                    MASTERFILE = baseItem.MASTERFILE,
                    MASTERTIME = baseItem.MASTERTIME,
                    EDITTIME = baseItem.EDITTIME,
                    EDITFILE = baseItem.EDITFILE,
                    ENERGYFILE = baseItem.ENERGYFILE,
                    CALLFILE = baseItem.CALLFILE,
                    EDITOR = baseItem.EDITOR
                };
                item.ONAIRDATE = entity.ONAIRDATE != null 
                    ? DateTime.ParseExact(entity.ONAIRDATE, "yyyyMMdd", CultureInfo.InvariantCulture) 
                    : default(DateTime);
                result.Add(item);
            }
            return result;
        }

        public static List<ProductFileDTO> Converting(this List<ProductFileEntity> entitys)
        {
            var result = new List<ProductFileDTO>();
            foreach (var entity in entitys)
            {
                BaseFileDTO baseItem = SetBaseFileDTO(entity);
                ProductFileDTO item = new ProductFileDTO()
                {
                    AUDIOCLIPID = baseItem.AUDIOCLIPID,
                    NAME = baseItem.NAME,
                    MASTERFILE = baseItem.MASTERFILE,
                    MASTERTIME = baseItem.MASTERTIME,
                    EDITTIME = baseItem.EDITTIME,
                    EDITFILE = baseItem.EDITFILE,
                    ENERGYFILE = baseItem.ENERGYFILE,
                    CALLFILE = baseItem.CALLFILE,
                    EDITOR = baseItem.EDITOR
                };
                item.ONAIRDATE = entity.ONAIRDATE!=null
                    ?DateTime.ParseExact(entity.ONAIRDATE, "yyyyMMdd", CultureInfo.InvariantCulture)
                    :default(DateTime);
                result.Add(item);
            }
            return result;
        }
        public static List<SongFileDTO> Converting(this List<SongFileEntity> entitys)
        {
            var result = new List<SongFileDTO>();
            foreach (var entity in entitys)
            {
                BaseFileDTO baseItem = SetBaseFileDTO(entity);
                SongFileDTO item = new SongFileDTO()
                {
                    AUDIOCLIPID = baseItem.AUDIOCLIPID,
                    NAME = baseItem.NAME,
                    MASTERFILE = baseItem.MASTERFILE,
                    MASTERTIME = baseItem.MASTERTIME,
                    EDITTIME = baseItem.EDITTIME,
                    EDITFILE = baseItem.EDITFILE,
                    ENERGYFILE = baseItem.ENERGYFILE,
                    CALLFILE = baseItem.CALLFILE,
                    EDITOR = baseItem.EDITOR
                };
                item.LASTONAIRDATE = entity.LASTONAIRDATE != null 
                    ? DateTime.ParseExact(entity.LASTONAIRDATE, "yyyyMMdd", CultureInfo.InvariantCulture) 
                    : default(DateTime);
                result.Add(item);
            }
            return result;
        }

        public static List<MasSpotFileDTO> Converting(this List<MasSpotFileEntity> entitys)
        {
            var result = new List<MasSpotFileDTO>();
            foreach (var entity in entitys)
            {
                BaseFileDTO baseItem = SetBaseFileDTO(entity);
                MasSpotFileDTO item = new MasSpotFileDTO()
                {
                    AUDIOCLIPID = baseItem.AUDIOCLIPID,
                    NAME = baseItem.NAME,
                    MASTERFILE = baseItem.MASTERFILE,
                    MASTERTIME = baseItem.MASTERTIME,
                    EDITTIME = baseItem.EDITTIME,
                    EDITFILE = baseItem.EDITFILE,
                    ENERGYFILE = baseItem.ENERGYFILE,
                    CALLFILE = baseItem.CALLFILE,
                    EDITOR = baseItem.EDITOR
                };
                item.ENDDATE = entity.ENDDATE != null
                    ? DateTime.ParseExact(entity.ENDDATE, "yyyyMMdd", CultureInfo.InvariantCulture)
                    : default(DateTime);
                result.Add(item);
            }
            return result;
        }

        public static List<RecycleDTO> Converting(this List<RecycleEntity> entitys)
        {
            var result = new List<RecycleDTO>();
            foreach (var entity in entitys)
            {
                var item = new RecycleDTO();
                item.SEQ = entity.SEQ;
                item.AUDIOCLIPID = entity.AUDIOCLIPID;
                item.MASTERFILE = entity.MASTERFILE ?? "";
                item.USERID = entity.USERID ?? "";
                item.DELTIME = entity.DELTIME != null 
                    ? DateTime.ParseExact(entity.DELTIME, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture) 
                    : default(DateTime);
                result.Add(item);
            }
            return result;
        }

    }
}

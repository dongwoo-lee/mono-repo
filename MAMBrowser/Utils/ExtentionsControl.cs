using M30_ManagementControlDAO.Entity;
using static MAMBrowser.DTO.ManagementSystemDTO;
using System.Collections.Generic;
using System;
using MAMBrowser.DTO;
using M30.AudioFile.Common.Foundation;
using System.Drawing;
using System.Threading;
using DevExpress.Data.Filtering.Helpers;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Web.WebPages;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;
using System.IO;

namespace MAMBrowser.Utils
{
    public static class ExtentionsControl
    {
        public static List<TransMissionListItemDTO> Converting(this List<TransMissionEntity> entitys, StudioAssignListEntity studioAssigns)
        {
            var result = new List<TransMissionListItemDTO>();
            var index = 1;
            foreach (var entity in entitys)
            {
                var item = new TransMissionListItemDTO();
                item.ROWNO = index;
                item.MAINMACHINE = entity.MAINMACHINE;
                item.SEQNUM = entity.SEQNUM;
                item.PRODUCTID = entity.PRODUCTID;
                item.PGMCODE = entity.PGMCODE;
                item.STUDIONAME = entity.MAPI_STNAME;
                item.PRODUCTTYPE = entity.PRODUCTTYPE;
                item.EVENTMODF= entity.EVENTMODF;
                item.SOURCEID = entity.SOURCEID;
                item.ONAIRTIME = entity.ONAIRTIME;
                item.DURATION = entity.DURATION;
                item.EVENTNAME = entity.EVENTNAME;
                item.DLFILEPATH_1 = entity.DLFILEPATH_1;
                if (!string.IsNullOrEmpty(item.DLFILEPATH_1))
                {
                    item.DLFILETOKEN_1 = TokenGenerator.GenerateFileToken(item.DLFILEPATH_1);
                }
                item.DLFILEPATH_2 = entity.DLFILEPATH_2;
                if (!string.IsNullOrEmpty(item.DLFILEPATH_2))
                {
                    item.DLFILETOKEN_2 = TokenGenerator.GenerateFileToken(item.DLFILEPATH_2);
                }
                item.PGMFILEPATH = entity.PGMFILEPATH;
                if (!string.IsNullOrEmpty(item.PGMFILEPATH))
                {
                    item.PGMFILETOKEN = TokenGenerator.GenerateFileToken(item.PGMFILEPATH);
                }
                if (!String.IsNullOrEmpty(entity.MAPI_STNAME)&&entity.PRODUCTTYPE=='P'&&entity.EVENTMODF!='C'&&entity.EVENTMODF!='T')
                {
                    var studioSchedul = studioAssigns.RstudioAssignedList.Where(assign => assign.STNAME == entity.MAPI_STNAME).ToList();
                    foreach (var schedulItem in studioSchedul)
                    {
                        var startTimeDate = ConvertToDateTime(schedulItem.STIME, 0, 0, schedulItem.W_DATE);
                        var endTimeDate = ConvertToDateTime(schedulItem.ETIME, 0, 0, schedulItem.W_DATE);
                        if ((entity.ONAIRTIME >= startTimeDate) && (entity.ONAIRTIME <= endTimeDate))
                        {
                            item.TDNAME = schedulItem.GB_NAME;
                            break;
                        }
                    }
                }
                item.CUEID = entity.CUEID;
                item.ISHISTORY = (entity.ISHISTORY == '1') ? true : false;
                result.Add(item);
                index++;
            }
            return result;
        }
        public static ProgramInfomationDTO Converting(this List<ProgramInfomationEntity> entitys, StudioAssignListEntity studioAssigns,string imgPath)
        {
            var result = new ProgramInfomationDTO();
            result.productIdDetails = new List<ProductIdDetail>();
            var groupbyEntitys = entitys.GroupBy(entity => new { entity.PGMCODE,entity.MEDIA, entity.PGMNAME,entity.STARTDATE,entity.KEYWORD,entity.DESCRIPTION,entity.STAFFS,entity.PS_CODE,entity.AUDIOCODEID });

            foreach (var entity in groupbyEntitys)
            {
                result.PGMCODE = entity.Key.PGMCODE;
                result.PGMNAME = entity.Key.PGMNAME;
                result.PSCODE = entity.Key.PS_CODE;
                result.AUDIOCODEID = entity.Key.AUDIOCODEID;
                result.STARTDATE = entity.Key.STARTDATE;
                result.KEYWORD = entity.Key.KEYWORD;
                result.STAFFS= entity.Key.STAFFS;
                result.MEDIA = entity.Key.MEDIA;
                result.DESCRIPTION = entity.Key.DESCRIPTION;
                result.IMAGEPATH = GetpgmImgPath(entity.Key.PGMCODE, imgPath);
            }
            foreach (var entity in entitys)
            {
                var item = new ProductIdDetail();
                item.PRODUCTID= entity.PRODUCTID;
                item.EVENTNAME= entity.EVENTNAME;
                item.ONAIRTIME= entity.ONAIRTIME;
                item.MAINMACHINE = entity.MAINMACHINE;
                item.STUDIONAME = entity.MAPI_STNAME;
                item.LIVEFLAG= entity.LIVEFLAG;
                item.CUEID= entity.CUEID;
                if (!String.IsNullOrEmpty(entity.MAPI_STNAME))
                {
                    var studioSchedul = studioAssigns.RstudioAssignedList.Where(assign=>assign.STNAME==entity.MAPI_STNAME).ToList();
                    foreach (var schedulItem in studioSchedul)
                    {
                        var startTimeDate = ConvertToDateTime(schedulItem.STIME, 0, 0, schedulItem.W_DATE);
                        var endTimeDate = ConvertToDateTime(schedulItem.ETIME,0,0,schedulItem.W_DATE);
                        if((entity.ONAIRTIME >= startTimeDate) && (entity.ONAIRTIME <= endTimeDate))
                        {
                            item.TDNAME = schedulItem.GB_NAME;
                            break;
                        }
                    }
                }
                result.productIdDetails.Add(item);
            }

            return result;
        }
        public static List<StudioAssign> SetAssigns(this List<StudioAssignedList> entitys)
        {
            var result = new List<StudioAssign>();
            foreach (var entity in entitys)
            {
                if (entity.PTIME != "0000")
                {
                    var p_item = new StudioAssign();
                    var calHr = int.Parse(entity.PTIME.Substring(0, 2));
                    var calMi = int.Parse(entity.PTIME.Substring(2, 2));
                    p_item.STARTDATE = ConvertToDateTime(entity.STIME,-calHr,-calMi, entity.W_DATE);
                    p_item.ENDDATE = ConvertToDateTime(entity.STIME,0,0,entity.W_DATE);
                    p_item.DESCRIPTION = entity.P_DESC;
                    result.Add((p_item));
                }
                if(entity.ATIME != "0000")
                {
                    var a_item = new StudioAssign();
                    var calHr = int.Parse(entity.ATIME.Substring(0, 2));
                    var calMi = int.Parse(entity.ATIME.Substring(2, 2));
                    a_item.STARTDATE = ConvertToDateTime(entity.ETIME,0,0,entity.W_DATE);
                    a_item.ENDDATE=ConvertToDateTime(entity.ETIME,calHr,calMi, entity.W_DATE);
                    a_item.DESCRIPTION = entity.P_DESC;
                    result.Add((a_item));
                }
                var item = new StudioAssign();
                item.STARTDATE = ConvertToDateTime(entity.STIME, 0, 0, entity.W_DATE);
                item.ENDDATE = ConvertToDateTime(entity.ETIME, 0, 0, entity.W_DATE);
                item.TDNAME = entity.GB_NAME;
                item.TDID = entity.GB_EMPNO;
                item.DESCRIPTION = entity.PGMNAME;
                item.GUBUN = entity.B_GUBUN;
                result.Add((item));
            }

            return result;
        }
        public static List<SchedulerResources> SetResources(this List<StudioAssignedList> entitys)
        {
            var result = new List<SchedulerResources>();
            var groupbyEntitys = entitys.GroupBy(entity => new{ entity.GB_EMPNO,entity.TD_COLOR});
            foreach (var entity in groupbyEntitys)
            {
                if (!String.IsNullOrEmpty(entity.Key.TD_COLOR) && !String.IsNullOrEmpty(entity.Key.GB_EMPNO))
                {
                    var item = new SchedulerResources();
                    item.ID = entity.Key.GB_EMPNO;
                    item.COLOR = "rgb(" + entity.Key.TD_COLOR + ")";
                    result.Add(item);
                }
            }
            return result;
        }
        public static List<PlaylistPerBrdProgramDTO> Converting(this List<PlaylistPerBrdProgramEntity> entitys)
        {
            var result = new List<PlaylistPerBrdProgramDTO>();
            var index = 1;
            foreach (var entity in entitys)
            {
                var item = new PlaylistPerBrdProgramDTO();
                item.ROWNO = index;
                item.SEQ = entity.SEQ;
                item.SCHDATE = entity.SCH_DATE;
                item.AUDIOCLIPID = entity.AUDIO_CLIP_ID;
                item.MUSICID = entity.MUSIC_ID;
                item.ENCODEDATE = entity.ENCODE_DATE;
                item.AUDIOFILETYPE = entity.AUDIO_FILE_TYPE;
                item.PRODUCTID = entity.PRODUCT_ID;
                item.PRODUCTNAME = entity.PRODUCT_NAME;
                item.PGMCODE = entity.PGM_CODE;
                item.PGMNAME = entity.PGM_NAME;
                item.STARTDTM = entity.START_DTM;
                item.ENDDTM = entity.END_DTM;
                item.STUDIONAME = entity.STUDIO_NAME;
                item.SLAPNAME = entity.SLAP_NAME;
                item.BRDCTYPE = entity.BRDC_TYPE;
                item.SONGNAME = entity.SONGNAME;
                item.ALBUMNAME = entity.ALBUMNAME;
                item.PRODUCTYEAR= entity.PRODUCTYEAR;
                item.ARTIST = entity.ARTIST;
                item.PLAYTIME = entity.PLAY_TIME;
                item.TOTALTIME = entity.TOTAL_TIME;
                item.USERID = entity.USER_ID;
                item.USERNAME = entity.USER_NAME;
                item.REGDTM = entity.REG_DTM;
                result.Add(item);
                index++;
            }
            return result;
        }
        public static List<PlaylistStatisticsDTO> Converting(this List<PlaylistStatisticsEntity> entitys)
        {
            var result = new List<PlaylistStatisticsDTO>();
            foreach (var entity in entitys)
            {
                var item = new PlaylistStatisticsDTO();
                item.RANK = entity.RANK;
                //item.SEQ= entity.SEQ;
                item.AUDIOCLIPID = entity.AUDIO_CLIP_ID;
                item.MUSICID = entity.MUSICID;
                //item.PERIOD = entity.PERIOD;
                //item.MEDIA = entity.MEDIA_CD;
                //item.PERSONID = entity.PERSONID;
                //item.PERSONNAME = entity.PERSONNAME;
                item.SONGNAME = entity.SONGNAME;
                item.ARTIST = entity.ARTIST;
                item.ALBUMNAME= entity.ALBUMNAME;
                //item.PGMNAME = entity.PGM_NAME;
                item.PLAYCNT = entity.PLAY_CNT;
                //item.PLAYTIME = entity.PLAY_TIME;
                //item.TOTALTIME = entity.TOTAL_TIME;
                item.SUMMARYDATE = entity.SUMMARY_DATE;
                item.FilePath = entity.MASTERFILE;
                if (!string.IsNullOrEmpty(item.FilePath))
                {
                    item.FileToken = TokenGenerator.GenerateFileToken(item.FilePath);
                    item.ExistFile = true;
                }
                else
                {
                    item.ExistFile = false;
                }
                result.Add(item);
            }
            return result;
        }
        public static DateTime ConvertToDateTime(string timeData,int calHours, int calMinutes, string dateData)
        {
            int hours = int.Parse(timeData.Substring(0, 2));
            int minutes = int.Parse(timeData.Substring(2, 2));

            int year = int.Parse(dateData.Substring(0, 4));
            int month = int.Parse(dateData.Substring(4, 2));
            int day = int.Parse(dateData.Substring(6, 2));

            DateTime result = new DateTime(year, month, day);
            result = result.AddHours(hours).AddMinutes(minutes);
            return result.AddHours(calHours).AddMinutes(calMinutes);
        }
        public static string GetpgmImgPath(string pgmcode, string path)
        {
            if (!String.IsNullOrEmpty(pgmcode) && Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, $"{pgmcode}.*");
                if (files.Length > 0)
                {
                    return files[0];
                }
            }
            return null;
        }
    }
}

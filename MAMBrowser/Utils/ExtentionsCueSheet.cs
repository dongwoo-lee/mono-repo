using M30.AudioFile.Common.Foundation;
using M30_CueSheetDAO.Entity;
using M30_CueSheetDAO.ParamEntity;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MAMBrowser.Utils
{
    public static class ExtentionsCueSheet
    {
        public static IEnumerable<DayCueSheetListDTO> Converting(this List<DayCueSheetListEntity> entity)
        {
            List<DayCueSheetListDTO> dto = new List<DayCueSheetListDTO>();

            foreach (var item in entity)
            {
                dto.Add(new DayCueSheetListDTO()
                {
                    PGMCODE = item.PGMCODE,
                    SERVICENAME = item.SERVICENAME,
                    ONAIRDAY = item.ONAIRDAY,
                    STARTDATE = item.STARTDATE,
                    PRODUCTID = item.PRODUCTID,
                    EVENTNAME = item.EVENTNAME,
                    DAY = item.DAY,
                    R_ONAIRTIME = item.ONAIRTIME,
                    CUEID = item.CUEID,
                    MEDIA = item.MEDIA,
                    EDIT = item.EDIT,
                    SEQNUM = item.SEQNUM,
                    LIVEFLAG = item.LIVEFLAG,
                });
            }

            return dto;
        }
        public static IEnumerable<DefCueSheetListDTO> Converting(this List<DefCueSheetListEntity> entity)
        {
            List<DefCueSheetListDTO> dto = new List<DefCueSheetListDTO>();
            foreach (var item in entity)
            {
                var dtoItem = new DefCueSheetListDTO();
                dtoItem.PRODUCTID = item.PRODUCTID;
                dtoItem.EVENTNAME = item.EVENTNAME;
                dtoItem.SERVICENAME = item.SERVICENAME;
                dtoItem.MEDIA = item.MEDIA;
                dtoItem.CUETYPE = item.CUETYPE;
                dtoItem.EDITTIME = item.EDITTIME;
                dtoItem.PGMCODE = item.PGMCODE;

                List<DefCueSheetListDetailDTO> detail = new List<DefCueSheetListDetailDTO>();
                foreach (var detailItem in item.DETAIL)
                {
                    var dtoDetail = new DefCueSheetListDetailDTO();
                    dtoDetail.CUEID = detailItem.CUEID;
                    dtoDetail.WEEK = detailItem.WEEK;
                    detail.Add(dtoDetail);
                }
                dtoItem.DETAIL = detail;
                dto.Add(dtoItem);
            }
            return dto;
        }
        public static IEnumerable<TemplateListDTO> Converting(this List<TemplateListEntity> entity)
        {
            List<TemplateListDTO> dto = new List<TemplateListDTO>();

            foreach (var item in entity)
            {
                dto.Add(new TemplateListDTO()
                {
                    CUEID = item.CUEID,
                    CUETYPE = item.CUETYPE,
                    PERSONID = item.PERSONID,
                    EDITTIME = item.EDITTIME,
                    CREATETIME = item.CREATETIME,
                    TMPTITLE = item.TMPTITLE,
                });
            }

            return dto;
        }
        public static IEnumerable<ArchiveCueSheetListDTO> Converting(this List<ArchiveCueSheetListEntity> entity)
        {
            List<ArchiveCueSheetListDTO> dto = new List<ArchiveCueSheetListDTO>();

            foreach (var item in entity)
            {
                dto.Add(new ArchiveCueSheetListDTO()
                {
                    CUEID = item.CUEID,
                    PRODUCTID = item.PRODUCTID,
                    TITLE = item.TITLE,
                    MEDIA = item.MEDIA,
                    BRDDATE = item.BRDDATE,
                    BRDTIME = item.BRDTIME,
                    CUETYPE = "A",
                    TAG = item.TAG
                });
            }

            return dto;
        }
        public static IEnumerable<CueSheetConDTO> Converting(this List<UserFavConEntity> entity)
        {
            var favList = new List<CueSheetConDTO>();
            foreach (var item in entity)
            {
                var favItem = new CueSheetConDTO();
                favItem.ROWNUM = item.SEQNUM;
                favItem.CARTCODE = item.CARTCODE ?? "";
                favItem.PGMCODE = item.PGMCODE ?? "";
                favItem.CARTID = item.CARTID ?? "";
                favItem.ONAIRDATE = item.ONAIRDATE ?? "";
                favItem.DURATION = item.AUDIOS[0].P_DURATION; //나중에 그룹소재 적용되면 바꿔야함
                favItem.STARTPOSITION = item.STARTPOSITION;
                favItem.ENDPOSITION = item.ENDPOSITION;
                favItem.FADEINTIME = item.FADEINTIME > 0 ? true : false;
                favItem.FADEOUTTIME = item.FADEOUTTIME > 0 ? true : false;
                favItem.MAINTITLE = item.MAINTITLE ?? "";
                favItem.SUBTITLE = item.SUBTITLE ?? "";
                favItem.MEMO = item.MEMO ?? "";
                favItem.EDITTARGET = true;
                favItem.CARTTYPE = SetCartCode(item.CARTCODE);

                if (item.ONAIRDATE == null || item.ONAIRDATE == "")
                {
                    favItem.FILEPATH = item.AUDIOS[0].P_MASTERFILE ?? "";
                    if (favItem.FILEPATH != "")
                    {
                        favItem.FILETOKEN = TokenGenerator.GenerateFileToken(favItem.FILEPATH);

                    }
                }
                else
                {
                    favItem.FILEPATH = "";
                    favItem.FILETOKEN = "";
                }
                if (string.IsNullOrEmpty(favItem.FILEPATH)) { favItem.ExistFile = false; } else { favItem.ExistFile = true; }
                favList.Add(favItem);
            }

            var resultFavList = new List<CueSheetConDTO>();
            for (var i = 0; 16 > i; i++)
            {
                var row = new CueSheetConDTO() { ROWNUM = i + 1 };
                for (var itemIndex = 0; favList.Count > itemIndex; itemIndex++)
                {
                    if (favList[itemIndex].ROWNUM == i + 1)
                    {
                        row = favList[itemIndex];
                        break;
                    }
                    else
                    {
                        row.ROWNUM = i + 1;
                        row.EDITTARGET = true;
                    }
                }
                resultFavList.Add(row);
            }
            return resultFavList;
        }
        public static CueSheetCollectionDTO DayConverting(this CueSheetCollectionEntity entity)
        {
            CueSheetCollectionDTO collectionDTO = SetCueData(entity.CueSheetConEntities, entity.PrintEntities, entity.AttachmentEntities, entity.TagEntities);

            collectionDTO.CueSheetDTO = new CueSheetDTO();

            collectionDTO.CueSheetDTO.CUETYPE = "D";
            collectionDTO.CueSheetDTO.PRODUCTID = entity.CueSheetEntity.PRODUCTID;
            collectionDTO.CueSheetDTO.PGMCODE = entity.CueSheetEntity.PGMCODE;
            collectionDTO.CueSheetDTO.PERSONID = entity.CueSheetEntity.PERSONID;
            collectionDTO.CueSheetDTO.MEDIA = entity.CueSheetEntity.MEDIA;
            collectionDTO.CueSheetDTO.TITLE = entity.CueSheetEntity.EVENTNAME;
            collectionDTO.CueSheetDTO.DIRECTORNAME = entity.CueSheetEntity.DIRECTORNAME ?? "";
            collectionDTO.CueSheetDTO.DJNAME = entity.CueSheetEntity.DJNAME ?? "";
            collectionDTO.CueSheetDTO.EDITTIME = entity.CueSheetEntity.EDITTIME;
            collectionDTO.CueSheetDTO.FOOTERTITLE = entity.CueSheetEntity.FOOTERTITLE ?? "";
            collectionDTO.CueSheetDTO.HEADERTITLE = entity.CueSheetEntity.HEADERTITLE ?? "";
            collectionDTO.CueSheetDTO.MEMBERNAME = entity.CueSheetEntity.MEMBERNAME ?? "";
            collectionDTO.CueSheetDTO.MEMO = entity.CueSheetEntity.MEMO ?? "";
            

            foreach (DayCueSheetEntity item in entity.CueSheetEntity.Detail)
            {
                var detailArr = new List<ViewDetail>();
                var detailItem = new ViewDetail();
                detailItem.CUEID = item.CUEID;
                detailArr.Add(detailItem);
                collectionDTO.CueSheetDTO.DETAIL = detailArr;
                collectionDTO.CueSheetDTO.BRDDATE = item.BRDDATE;
                collectionDTO.CueSheetDTO.BRDTIME = item.BRDTIME;
            }
            return collectionDTO;
        }
        public static CueSheetCollectionDTO DefConverting(this CueSheetCollectionEntity entity)
        {
            CueSheetCollectionDTO collectionDTO = SetCueData(entity.CueSheetConEntities, entity.PrintEntities, null, entity.TagEntities);
            collectionDTO.CueSheetDTO = new CueSheetDTO();
            var detailArr = new List<ViewDetail>();

            // defdue 가공
            collectionDTO.CueSheetDTO.CUETYPE = "B";
            collectionDTO.CueSheetDTO.PRODUCTID = entity.CueSheetEntity.PRODUCTID;
            collectionDTO.CueSheetDTO.PGMCODE = entity.CueSheetEntity.PGMCODE;
            collectionDTO.CueSheetDTO.MEDIA = entity.CueSheetEntity.MEDIA;
            collectionDTO.CueSheetDTO.PERSONID = entity.CueSheetEntity.PERSONID;
            collectionDTO.CueSheetDTO.TITLE = entity.CueSheetEntity.EVENTNAME;
            collectionDTO.CueSheetDTO.DIRECTORNAME = entity.CueSheetEntity.DIRECTORNAME ?? "";
            collectionDTO.CueSheetDTO.DJNAME = entity.CueSheetEntity.DJNAME ?? "";
            collectionDTO.CueSheetDTO.EDITTIME = entity.CueSheetEntity.EDITTIME;
            collectionDTO.CueSheetDTO.FOOTERTITLE = entity.CueSheetEntity.FOOTERTITLE ?? "";
            collectionDTO.CueSheetDTO.HEADERTITLE = entity.CueSheetEntity.HEADERTITLE ?? "";
            collectionDTO.CueSheetDTO.MEMBERNAME = entity.CueSheetEntity.MEMBERNAME ?? "";
            collectionDTO.CueSheetDTO.MEMO = entity.CueSheetEntity.MEMO ?? "";
            foreach (DefCueSheetEntity item in entity.CueSheetEntity.Detail)
            {
                var detailItem = new ViewDetail();
                detailItem.CUEID = item.CUEID;
                detailItem.ONAIRTIME = item.ONAIRTIME;
                detailItem.WEEK = item.WEEK;
                detailArr.Add(detailItem);
            }
            collectionDTO.CueSheetDTO.DETAIL = detailArr;



            return collectionDTO;
        }
        public static CueSheetCollectionDTO TemConverting(this TemplateCollectionEntity entity)
        {
            CueSheetCollectionDTO collectionDTO = SetCueData(entity.CueSheetConEntities, entity.PrintEntities, null, entity.TagEntities);
            collectionDTO.CueSheetDTO = new CueSheetDTO();
            var detailArr = new List<ViewDetail>();

            // defdue 가공
            collectionDTO.CueSheetDTO.CUETYPE = "T";
            collectionDTO.CueSheetDTO.PRODUCTID = entity.TemplateEntity.PRODUCTID ?? "";
            collectionDTO.CueSheetDTO.PGMCODE = entity.TemplateEntity.PGMCODE ?? "";
            collectionDTO.CueSheetDTO.PERSONID = entity.TemplateEntity.PERSONID;
            collectionDTO.CueSheetDTO.MEDIA = entity.TemplateEntity.MEDIA ?? "";
            collectionDTO.CueSheetDTO.DIRECTORNAME = entity.TemplateEntity.DIRECTORNAME ?? "";
            collectionDTO.CueSheetDTO.DJNAME = entity.TemplateEntity.DJNAME ?? "";
            collectionDTO.CueSheetDTO.EDITTIME = entity.TemplateEntity.EDITTIME;
            collectionDTO.CueSheetDTO.FOOTERTITLE = entity.TemplateEntity.FOOTERTITLE ?? "";
            collectionDTO.CueSheetDTO.HEADERTITLE = entity.TemplateEntity.HEADERTITLE ?? "";
            collectionDTO.CueSheetDTO.MEMBERNAME = entity.TemplateEntity.MEMBERNAME ?? "";
            collectionDTO.CueSheetDTO.MEMO = entity.TemplateEntity.MEMO ?? "";
            foreach (TemplateEntity item in entity.TemplateEntity.Detail)
            {
                var detailItem = new ViewDetail();
                detailItem.CUEID = item.CUEID;
                detailArr.Add(detailItem);
                collectionDTO.CueSheetDTO.TITLE = item.TMPTITLE;
                collectionDTO.CueSheetDTO.CREATETIME = item.CREATETIME;

            }
            collectionDTO.CueSheetDTO.DETAIL = detailArr;

            return collectionDTO;
        }
        public static CueSheetCollectionDTO ArchiveConverting(this ArchiveCueSheetCollectionEntity entity)
        {
            CueSheetCollectionDTO collectionDTO = SetCueData(entity.CueSheetConEntities, entity.PrintEntities, entity.AttachmentEntities, entity.TagEntities);
            collectionDTO.CueSheetDTO = new CueSheetDTO();

            collectionDTO.CueSheetDTO.CUETYPE = "A";
            collectionDTO.CueSheetDTO.PRODUCTID = entity.ArchiveCueSheetEntity.PRODUCTID;
            collectionDTO.CueSheetDTO.PERSONID = entity.ArchiveCueSheetEntity.PERSONID;
            collectionDTO.CueSheetDTO.MEDIA = entity.ArchiveCueSheetEntity.MEDIA.ToString();
            collectionDTO.CueSheetDTO.EDITTIME = entity.ArchiveCueSheetEntity.EDITTIME;
            collectionDTO.CueSheetDTO.DJNAME = entity.ArchiveCueSheetEntity.DJNAME ?? "";
            collectionDTO.CueSheetDTO.DIRECTORNAME = entity.ArchiveCueSheetEntity.DIRECTORNAME ?? "";
            collectionDTO.CueSheetDTO.MEMBERNAME = entity.ArchiveCueSheetEntity.MEMBERNAME ?? "";
            collectionDTO.CueSheetDTO.HEADERTITLE = entity.ArchiveCueSheetEntity.HEADERTITLE ?? "";
            collectionDTO.CueSheetDTO.FOOTERTITLE = entity.ArchiveCueSheetEntity.FOTTERTITLE ?? "";
            collectionDTO.CueSheetDTO.MEMO = entity.ArchiveCueSheetEntity.MEMO ?? "";
            collectionDTO.CueSheetDTO.BRDTIME = entity.ArchiveCueSheetEntity.BRDTIME;
            collectionDTO.CueSheetDTO.BRDDATE = entity.ArchiveCueSheetEntity.BRDDATE ?? "";
            collectionDTO.CueSheetDTO.TITLE = entity.ArchiveCueSheetEntity.TITLE;

            var detailArr = new List<ViewDetail>();
            var detailItem = new ViewDetail();
            detailItem.CUEID = entity.ArchiveCueSheetEntity.CUEID;
            detailArr.Add(detailItem);
            collectionDTO.CueSheetDTO.DETAIL = detailArr;

            return collectionDTO;
        }
        public static IEnumerable<PgmListDTO> Converting(this List<ProgramListEntity> entity)
        {
            List<PgmListDTO> dto = new List<PgmListDTO>();
            var group_entity = entity.GroupBy(x => new { x.MEDIA, x.PGMCODE });
            foreach (var group in group_entity)
            {
                var dtoItem = new PgmListDTO();
                dtoItem.pgmItem  = new List<PgmItem>();
                foreach (var item in group)
                {
                    dtoItem.PGMCODE = item.PGMCODE;
                    dtoItem.PGMNAME = item.PGMNAME;
                    dtoItem.MEDIA = item.MEDIA;
                    var pgmItem = new PgmItem();
                    pgmItem.EVENTNAME = item.EVENTNAME;
                    pgmItem.PRODUCTID = item.PRODUCTID;
                    pgmItem.SERVICENAME = item.SERVICENAME;
                    dtoItem.pgmItem.Add(pgmItem);
                }
                dto.Add(dtoItem);
            }
            return dto;

        }
        public static IEnumerable<CueSheetConDTO> SponsorConverting(this SponsorCollectionEntity entity)
        {
            List<CueSheetConDTO> collectionDTO = new List<CueSheetConDTO>();
            if (entity.CM?.Any() == true)
            {
                var rownum = 1;
                foreach (var item in entity.CM)
                {
                    var itemResult = new CueSheetConDTO();
                    itemResult.ROWNUM = rownum;
                    itemResult.CHANNELTYPE = "N";
                    itemResult.CARTCODE = "S01G01C018";
                    itemResult.TRANSTYPE = "S";
                    itemResult.USEFLAG = "Y";
                    itemResult.CARTID = item.CMGROUPID ?? "";
                    itemResult.ONAIRDATE = item.ONAIRDATE ?? "";
                    itemResult.STARTPOSITION = 0;
                    itemResult.FADEINTIME = false;
                    itemResult.FADEOUTTIME = false;
                    itemResult.MAINTITLE = item.CMGROUPNAME ?? "";
                    itemResult.SUBTITLE = item.STATENAME ?? "";
                    itemResult.MEMO = "";
                    itemResult.EDITTARGET = true;
                    itemResult.CARTTYPE = "CM";
                    var duration = 0;
                    foreach (var clip in item.Clips)
                    {
                        duration = duration + clip.LENGTH;
                    }
                    itemResult.DURATION = duration;
                    itemResult.ENDPOSITION = duration;
                    collectionDTO.Add(itemResult);
                    rownum++;
                }

            }
            //이거 제외시켜도됨
            if (entity.SB?.Any() == true)
            {
                foreach (var item in entity.SB)
                {
                    var itemResult = new CueSheetConDTO();
                    itemResult.ONAIRDATE = item.ONAIRDATE ?? "";
                    itemResult.CARTID = item.GROUPCONTENTID ?? "";
                    itemResult.STARTPOSITION = 0;
                    itemResult.FADEINTIME = false;
                    itemResult.FADEOUTTIME = false;
                    itemResult.MAINTITLE = item.NAME ?? "";
                    itemResult.SUBTITLE = item.ID ?? "";
                    itemResult.MEMO = "";
                    itemResult.EDITTARGET = true;
                    itemResult.CARTTYPE = "FC";
                    var duration = 0;
                    foreach (var clip in item.Clips)
                    {
                        duration = duration + clip.LENGTH;

                    }
                    itemResult.DURATION = duration;
                    itemResult.ENDPOSITION = duration;
                    collectionDTO.Add(itemResult);
                }
            }

            return collectionDTO;
        }
        public static List<CueSheetConDTO> AddSponsorListToDTO(this SponsorCollectionEntity spons, string pgmcode)
        {
            var result = new List<CueSheetConDTO>();
            var rownum = 1;
            foreach (var item in spons.CM)
            {
                var itemResult = new CueSheetConDTO();
                itemResult.ROWNUM = rownum;
                itemResult.PGMCODE = pgmcode;
                itemResult.CHANNELTYPE = "N";
                itemResult.CARTCODE = "S01G01C018";
                itemResult.TRANSTYPE = "S";
                itemResult.USEFLAG = "Y";
                itemResult.CARTID = item.CMGROUPID ?? "";
                itemResult.ONAIRDATE = item.ONAIRDATE ?? "";
                itemResult.STARTPOSITION = 0;
                itemResult.FADEINTIME = false;
                itemResult.FADEOUTTIME = false;
                itemResult.MAINTITLE = item.CMGROUPNAME ?? "";
                itemResult.SUBTITLE = item.STATENAME ?? "";
                itemResult.MEMO = "";
                itemResult.EDITTARGET = true;
                itemResult.CARTTYPE = "CM";
                var duration = 0;
                //var keylist = item.Clips.GroupBy((d) => d.ID).Select((dt) => dt.Key).ToList();
                //foreach(var key in keylist)
                //{
                //    var data = item.Clips.Find(dt => dt.ID == key);
                //    duration += data.LENGTH;
                //}
                foreach (var clip in item.Clips)
                {
                    duration = duration + clip.LENGTH;
                }
                itemResult.DURATION = duration;
                itemResult.ENDPOSITION = duration;
                result.Add(itemResult);
                rownum++;
            }
            return result;


        }
        public static List<CueSheetConEntity> SetSponsorToEntity(this SponsorCollectionEntity spons, List<CueSheetConEntity> entity)
        {
            if (spons.CM?.Any() == true)
            {
                foreach (var item in spons.CM)
                {
                    var cartId = item.CMGROUPID.Substring(2);
                    foreach (var cueItem in entity)
                    {
                        if (cueItem.CARTID != null && cueItem.CARTID.Contains(cartId))
                        {
                            cueItem.PGMCODE = item.PGMCODE;
                            cueItem.CARTID = item.CMGROUPID;
                            cueItem.ONAIRDATE = item.ONAIRDATE;
                            cueItem.STARTPOSITION = 0;
                            var duration = 0;
                            //var keylist = item.Clips.GroupBy((d) => d.ID).Select((dt) => dt.Key).ToList();
                            //foreach (var key in keylist)
                            //{
                            //    var data = item.Clips.Find(dt => dt.ID == key);
                            //    duration += data.LENGTH;
                            //}
                            foreach (var clip in item.Clips)
                            {
                                duration = duration + clip.LENGTH;
                            }
                            cueItem.ENDPOSITION = duration;
                            cueItem.FADEINTIME = 0;
                            cueItem.FADEOUTTIME = 0;
                            cueItem.MAINTITLE = item.CMGROUPNAME;
                            cueItem.SUBTITLE = item.STATENAME;
                        }
                    }
                }

            }
            if (spons.SB?.Any() == true)
            {
                var query = from arr in spons.SB
                            group arr by arr.PRODUCTID into g
                            select new
                            {
                                GroupKey = g.Key,
                                Cons = g
                            };

                foreach (var Group in query)
                {
                    foreach (var con in Group.Cons)
                    {
                        con.Clips = con.Clips.DistinctBy(p => p.CLIPID).ToList();
                    }
                }
                var products = query.ToList();

                foreach (var item in products)
                {
                    var arr = item.Cons.ToList();
                    var cartId = arr[0].ID.Remove(3, 2);
                    foreach (var cueItem in entity)
                    {
                        if (cueItem.CARTID != null && arr[0].ID.Remove(3, 2) == cueItem.CARTID.Remove(3, 2))
                        {
                            cueItem.PGMCODE = arr[0].PGMCODE;
                            cueItem.CARTID = arr[0].ID;
                            cueItem.ONAIRDATE = arr[0].ONAIRDATE;
                            cueItem.STARTPOSITION = 0;
                            cueItem.FADEINTIME = 0;
                            cueItem.FADEOUTTIME = 0;
                            cueItem.MAINTITLE = arr[0].EVENTNAME;
                            cueItem.SUBTITLE = arr[0].STATENAME;
                            var duration = 0;
                            foreach (var item2 in arr)
                            {
                                foreach (var clip in item2.Clips)
                                {
                                    duration = duration + clip.LENGTH;
                                }
                            }
                            cueItem.ENDPOSITION = duration;
                        }
                    }
                }
            }
            return entity;
        }
        public static DayCueSheetCreateParam DayToEntity(this CueSheetCollectionDTO dto)
        {
            //cue
            DayCueSheetCreateParam result = new DayCueSheetCreateParam();
            result.CueSheetConParams = dto?.Converting();
            result.PrintParams = dto.PrintDTO?.Converting() ?? new List<PrintParam>();
            result.AttachmentsParams = dto.Attachments?.Converting() ?? new List<AttachmentsParam>();
            result.TagParams = dto.Tags?.Converting() ?? new List<TagParam>();

            result.DayCueSheetParam = new DayCueSheetParam();
            result.DayCueSheetParam.p_cueid = dto.CueSheetDTO.DETAIL[0].CUEID;
            result.DayCueSheetParam.p_productid = dto.CueSheetDTO.PRODUCTID;
            result.DayCueSheetParam.p_personid = dto.CueSheetDTO.PERSONID;
            result.DayCueSheetParam.p_media = char.Parse(dto.CueSheetDTO.MEDIA);
            result.DayCueSheetParam.p_djname = dto.CueSheetDTO.DJNAME;
            result.DayCueSheetParam.p_directorname = dto.CueSheetDTO.DIRECTORNAME;
            result.DayCueSheetParam.p_membername = dto.CueSheetDTO.MEMBERNAME;
            result.DayCueSheetParam.p_headertitle = dto.CueSheetDTO.HEADERTITLE;
            result.DayCueSheetParam.p_footertitle = dto.CueSheetDTO.FOOTERTITLE;
            result.DayCueSheetParam.p_memo = dto.CueSheetDTO.MEMO;
            result.DayCueSheetParam.p_brdtime = dto.CueSheetDTO.BRDTIME;
            result.DayCueSheetParam.p_brddate = dto.CueSheetDTO.BRDDATE;

            return result;
        }
        public static DefCueSheetCreateParam DefToEntity(this CueSheetCollectionDTO dto)
        {
            //cue
            DefCueSheetCreateParam result = new DefCueSheetCreateParam();
            result.DefCueSheetParam = new List<DefCueSheetParam>();
            result.DelDefCueParams = new List<DefDeleteParam>();

            DateTime editTime = DateTime.Now;

            foreach (var item in dto.DefCueSheetDTO)
            {
                var defCue = new DefCueSheetParam();
                defCue.p_cueid = item.DETAIL[0].CUEID;
                defCue.p_productid = item.PRODUCTID;
                defCue.p_personid = item.PERSONID;
                defCue.p_media = char.Parse(item.MEDIA);
                defCue.p_edittime = editTime;
                defCue.p_djname = item.DJNAME;
                defCue.p_directorname = item.DIRECTORNAME;
                defCue.p_membername = item.MEMBERNAME;
                defCue.p_headertitle = item.HEADERTITLE;
                defCue.p_footertitle = item.FOOTERTITLE;
                defCue.p_memo = item.MEMO;
                defCue.p_week = item.DETAIL[0].WEEK;
                result.DefCueSheetParam.Add(defCue);
            }
            if (dto.delParams != null)
            {
                foreach (var item in dto.delParams)
                {
                    var delParm = new DefDeleteParam();
                    delParm.p_del_cueid = item;
                    result.DelDefCueParams.Add(delParm);
                }
            }

            result.CueSheetConParams = dto?.Converting();
            result.PrintParams = dto.PrintDTO?.Converting() ?? new List<PrintParam>();
            result.TagParams = dto.Tags?.Converting() ?? new List<TagParam>();

            return result;

        }
        public static TemplateCreateParam TempToEntity(this CueSheetCollectionDTO dto)
        {
            TemplateCreateParam result = new TemplateCreateParam();
            result.CueSheetConParams = dto?.Converting();
            result.PrintParams = dto.PrintDTO?.Converting() ?? new List<PrintParam>();
            result.TagParams = dto.Tags?.Converting() ?? new List<TagParam>();

            result.TemplateParam = new TemplateParam();
            result.TemplateParam.p_tempid = dto.CueSheetDTO.DETAIL[0].CUEID;
            result.TemplateParam.p_personid = dto.CueSheetDTO.PERSONID;
            result.TemplateParam.p_djname = dto.CueSheetDTO.DJNAME;
            result.TemplateParam.p_directorname = dto.CueSheetDTO.DIRECTORNAME;
            result.TemplateParam.p_membername = dto.CueSheetDTO.MEMBERNAME;
            result.TemplateParam.p_headertitle = dto.CueSheetDTO.HEADERTITLE;
            result.TemplateParam.p_footertitle = dto.CueSheetDTO.FOOTERTITLE;
            result.TemplateParam.p_memo = dto.CueSheetDTO.MEMO;
            result.TemplateParam.p_temptitle = dto.CueSheetDTO.TITLE;

            return result;
        }
        public static IEnumerable<UserFavCreateParam> FavToEntity(this List<CueSheetConDTO> dto)
        {
            List<UserFavCreateParam> result = new List<UserFavCreateParam>();

            foreach (var item in dto)
            {
                if (item.CARTCODE != "" && item.CARTCODE != null)
                {
                    var favItem = new UserFavCreateParam();
                    favItem.p_seqnum = item.ROWNUM;
                    favItem.p_onairdate = item.ONAIRDATE;
                    favItem.p_cartid = item.CARTID;
                    favItem.p_cartcode = item.CARTCODE;
                    favItem.p_startposition = item.STARTPOSITION;
                    favItem.p_endposition = item.ENDPOSITION;
                    favItem.p_fadeintime = item.FADEINTIME ? 4000 : 0;
                    favItem.p_fadeouttime = item.FADEOUTTIME ? 4000 : 0;
                    favItem.p_maintitle = TrimMaxLength(item.MAINTITLE,100);
                    favItem.p_subtitle =TrimMaxLength(item.SUBTITLE,100);
                    favItem.p_memo = item.MEMO;
                    result.Add(favItem);
                }
            }
            return result;

        }
        public static PDPQSCreateCollectionParam PDPQSToEntity(this CueSheetCollectionDTO dto, char type)
        {
            var result = new PDPQSCreateCollectionParam();
            result.PDPQSParam = new PDPQSParam();
            result.PDPQSParam.ProductID_in = dto.CueSheetDTO.PRODUCTID;
            result.PDPQSParam.OnairDate_in = dto.CueSheetDTO.BRDDATE;
            result.PDPQSParam.PQSType_in = type;
            result.PDPQSParam.Media_in = Char.Parse(dto.CueSheetDTO.MEDIA);
            result.PDPQSParam.LiveFlag_in = dto.CueSheetDTO.LIVEFLAG;
            result.PDPQSParam.StateID_in = "P000";
            result.PDPQSParam.Editor_in = dto.CueSheetDTO.PERSONID;
            result.PDPQSParam.OnairDay_in = dto.CueSheetDTO.ONAIRDAY;
            result.PDPQSParam.StartDate_in = dto.CueSheetDTO.STARTDATE;
            result.PDPQSParam.SeqNum_in = dto.CueSheetDTO.SEQNUM;

            if (type == 'N')
            {
                //ab
                foreach (var item in dto.NormalCon)
                {
                    if (item.CARTID != "" &&
                    (!string.IsNullOrEmpty(item.MEMO) || !string.IsNullOrEmpty(item.MAINTITLE)))
                    {
                        var resultItem = new PDPQSConParam();
                        resultItem.Description_in = item.MEMO;
                        resultItem.LLevel_in = 0;
                        resultItem.RLevel_in = 0;
                        resultItem.FadeInTime_in = 0;
                        resultItem.FadeOutTime_in = 0;
                        resultItem.ExtroTime_in = 0;
                        resultItem.IntroTime_in = 0;
                        resultItem.MainTitle_in = item.MAINTITLE;
                        resultItem.TransType_in = char.Parse(item.TRANSTYPE);
                        resultItem.SOM_in = item.STARTPOSITION;
                        resultItem.CartType_in = item.CARTTYPE;
                        resultItem.CartID_in = item.CARTID;
                        resultItem.SeqNum_in = item.ROWNUM;
                        resultItem.PqsType_in = 'N';
                        resultItem.OnAirDate_in = dto.CueSheetDTO.BRDDATE;
                        resultItem.ProductID_in = dto.CueSheetDTO.PRODUCTID;
                        resultItem.EOM_in = item.ENDPOSITION;
                        resultItem.SubTitle_in = item.SUBTITLE;
                        result.PDPQSConParam.Add(resultItem);
                    }
                }
            }
            if (type == 'I')
            {
                var index = 0;
                //c
                foreach (var cData in dto.InstanceCon)
                {
                    foreach (var item in cData.Value)
                    {
                        if (item.CARTCODE != "" && item.CARTCODE != null && item.ROWNUM + (index * 16) < 33)
                        {
                            var resultItem = new PDPQSConParam();
                            resultItem.Description_in = item.MEMO;
                            resultItem.LLevel_in = 0;
                            resultItem.RLevel_in = 0;
                            resultItem.FadeInTime_in = 0;
                            resultItem.FadeOutTime_in = 0;
                            resultItem.ExtroTime_in = 0;
                            resultItem.IntroTime_in = 0;
                            resultItem.MainTitle_in = item.MAINTITLE;
                            resultItem.TransType_in = char.Parse(item.TRANSTYPE);
                            resultItem.SOM_in = item.STARTPOSITION;
                            resultItem.CartType_in = item.CARTTYPE;
                            resultItem.CartID_in = item.CARTID;
                            resultItem.SeqNum_in = item.ROWNUM + (index * 16);
                            resultItem.PqsType_in = 'I';
                            resultItem.OnAirDate_in = dto.CueSheetDTO.BRDDATE;
                            resultItem.ProductID_in = dto.CueSheetDTO.PRODUCTID;
                            resultItem.EOM_in = item.ENDPOSITION;
                            resultItem.SubTitle_in = item.SUBTITLE;
                            result.PDPQSConParam.Add(resultItem);
                        }
                    }
                    index++;
                }
            }
            return result;
        }
        public static List<PrintParam> Converting(this List<PrintDTO> prints)
        {
            var result = new List<PrintParam>();

            foreach (var item in prints)
            {
                PrintParam obj = new PrintParam();
                obj.p_code = item.CODE;
                obj.p_seqnum = item.ROWNUM;
                obj.p_contents = TrimMaxLength(item.CONTENTS, 100);
                obj.p_usedtime = item.USEDTIME;
                obj.p_etc = item.ETC;
                result.Add(obj);
            }
            return result;
        }
        public static List<CueSheetConParam> Converting(this CueSheetCollectionDTO cons)
        {
            var result = new List<CueSheetConParam>();

            //ab
            if (cons.NormalCon != null)
            {
                foreach (var item in cons.NormalCon)
                {
                    CueSheetConParam obj = new CueSheetConParam();
                    obj.p_channeltype = 'N';
                    obj.p_seqnum = item.ROWNUM;
                    obj.p_onairdate = item.ONAIRDATE;
                    obj.p_cartid = item.CARTID;
                    obj.p_cartcode = item.CARTCODE;
                    obj.p_useflag = (item.USEFLAG == null) ? 'Y' : char.Parse(item.USEFLAG);
                    obj.p_startposition = item.STARTPOSITION;
                    obj.p_endposition = item.ENDPOSITION;
                    obj.p_fadeintime = item.FADEINTIME ? 4000 : 0;
                    obj.p_fadeouttime = item.FADEOUTTIME ? 4000 : 0;
                    obj.p_transtype = char.Parse(item.TRANSTYPE);
                    obj.p_maintitle = TrimMaxLength(item.MAINTITLE, 100);
                    obj.p_subtitle = TrimMaxLength(item.SUBTITLE, 100);
                    obj.p_memo = item.MEMO;
                    result.Add(obj);
                }
            }

            //c
            if (cons.InstanceCon != null)
            {
                var index = 0;
                foreach (var cData in cons.InstanceCon)
                {
                    foreach (var item in cData.Value)
                    {
                        if (item.CARTCODE != "" && item.CARTCODE != null)
                        {
                            CueSheetConParam obj = new CueSheetConParam();
                            obj.p_channeltype = 'I';
                            obj.p_seqnum = item.ROWNUM + (index * 16);
                            obj.p_onairdate = item.ONAIRDATE;
                            obj.p_cartid = item.CARTID;
                            obj.p_cartcode = item.CARTCODE;
                            obj.p_useflag = 'Y';
                            obj.p_startposition = item.STARTPOSITION;
                            obj.p_endposition = item.ENDPOSITION;
                            obj.p_fadeintime = item.FADEINTIME ? 4000 : 0;
                            obj.p_fadeouttime = item.FADEOUTTIME ? 4000 : 0;
                            obj.p_transtype = item.TRANSTYPE == null? 'S' : Char.Parse(item.TRANSTYPE);
                            obj.p_maintitle = TrimMaxLength(item.MAINTITLE, 100);
                            obj.p_subtitle = TrimMaxLength(item.SUBTITLE, 100);
                            obj.p_memo = item.MEMO;
                            result.Add(obj);
                        }
                    }
                    index++;
                }
            }

            return result;
        }
        public static CueSheetConEntity DtoToEntity(this CueSheetConDTO con)
        {
            var result = new CueSheetConEntity();
            result.AUDIOS = new List<CueSheetConAudioEntity>();

            if (con != null)
            {
                result.CHANNELTYPE = con.CHANNELTYPE;
                result.SEQNUM = con.ROWNUM;
                result.ONAIRDATE = con.ONAIRDATE;
                result.CARTID = con.CARTID;
                result.CARTCODE = con.CARTCODE;
                result.USEFLAG = (con.USEFLAG == null) ? "Y" : con.USEFLAG;
                result.STARTPOSITION = con.STARTPOSITION;
                result.ENDPOSITION = con.ENDPOSITION;
                result.FADEINTIME = con.FADEINTIME ? 4000 : 0;
                result.FADEOUTTIME = con.FADEOUTTIME ? 4000 : 0;
                result.TRANSTYPE = con.TRANSTYPE;
                result.MAINTITLE = TrimMaxLength(con.MAINTITLE, 100);
                result.SUBTITLE = TrimMaxLength(con.SUBTITLE, 100);
                result.MEMO = con.MEMO;
                result.PGMCODE = con.PGMCODE;
            }
            return result;
        }
        public static List<AttachmentsParam> Converting(this List<AttachmentDTO> attas)
        {
            var result = new List<AttachmentsParam>();

            foreach (var item in attas)
            {
                AttachmentsParam obj = new AttachmentsParam();
                obj.p_fileid = item.FILEID;
                obj.p_filepath = item.FILEPATH;
                obj.p_filesize = item.FILESIZE;
                result.Add(obj);
            }

            return result;
        }
        public static List<TagParam> Converting(this List<string> tags)
        {
            var result = new List<TagParam>();
            foreach (var item in tags)
            {
                var tag = new TagParam();
                tag.p_tag = item;
                result.Add(tag);
            }

            return result;
        }
        public static CueSheetCollectionDTO SetCueData(List<CueSheetConEntity> conData, List<PrintEntity> prints, List<AttachmentEntity> attachments, List<string> tags)
        {
            var collectionDTO = new CueSheetCollectionDTO();
            collectionDTO.NormalCon = new List<CueSheetConDTO>();
            collectionDTO.InstanceCon = new Dictionary<string, List<CueSheetConDTO>>();
            collectionDTO.PrintDTO = prints?.Converting();
            collectionDTO.Attachments = attachments?.Converting();
            collectionDTO.Tags = tags;
            var InstanceConList = new List<CueSheetConDTO>();

            // AB, C 가공
            foreach (var item in conData)
            {
                var con = new CueSheetConDTO();
                con.CARTCODE = item.CARTCODE ?? "";
                con.PGMCODE = item.PGMCODE ?? "";
                con.CHANNELTYPE = item.CHANNELTYPE ?? "";
                con.ONAIRDATE = item.ONAIRDATE ?? "";
                if (!string.IsNullOrEmpty(item.CARTID))
                {
                    con.DURATION = item.AUDIOS[0].P_DURATION; //나중에 그룹소재 적용되면 바꿔야함
                }

                con.CARTID = item.CARTID ?? "";
                con.MEMO = item.MEMO ?? "";
                con.STARTPOSITION = item.STARTPOSITION;
                con.ENDPOSITION = item.ENDPOSITION;
                con.FADEINTIME = item.FADEINTIME > 0 ? true : false;
                con.FADEOUTTIME = item.FADEOUTTIME > 0 ? true : false;
                con.MAINTITLE = item.MAINTITLE ?? "";
                con.SUBTITLE = item.SUBTITLE ?? "";
                con.TRANSTYPE = item.TRANSTYPE;
                con.USEFLAG = item.USEFLAG ?? "Y";
                con.EDITTARGET = true;
                con.CARTTYPE = SetCartCode(item.CARTCODE);
                if (item.CARTCODE != null && (item.ONAIRDATE == null || item.ONAIRDATE == ""))
                {
                    con.FILEPATH = item.AUDIOS[0].P_MASTERFILE ?? "";
                    if (con.FILEPATH != "")
                    {
                        con.FILETOKEN = TokenGenerator.GenerateFileToken(con.FILEPATH);
                    }
                }
                else
                {
                    con.FILEPATH = "";
                    con.FILETOKEN = "";
                }
                if (string.IsNullOrEmpty(con.FILEPATH)) { con.ExistFile = false; } else { con.ExistFile = true; }
                if (item.CHANNELTYPE == "N")
                {
                    con.ROWNUM = item.SEQNUM;
                    collectionDTO.NormalCon.Add(con);
                }
                if (item.CHANNELTYPE == "I")
                {
                    con.ROWNUM = item.SEQNUM;
                    InstanceConList.Add(con);
                }

            }

            for (var channelNum = 0; 4 > channelNum; channelNum++)
            {
                var cartData = new List<CueSheetConDTO>();
                for (var i = 0; 16 > i; i++)
                {
                    var row = new CueSheetConDTO();
                    if (InstanceConList.Count != 0)
                    {
                        for (var itemIndex = 0; InstanceConList.Count > itemIndex; itemIndex++)
                        {
                            if (InstanceConList[itemIndex].ROWNUM == i + 16 * channelNum + 1)
                            {
                                row = InstanceConList[itemIndex];
                                row.ROWNUM = i + 1;
                                break;
                            }
                            else
                            {
                                row.ROWNUM = i + 1;
                                row.EDITTARGET = true;
                            }
                        }

                    }
                    else
                    {
                        row.ROWNUM = i + 1;
                        row.EDITTARGET = true;
                    }
                    cartData.Add(row);
                }
                switch (channelNum)
                {
                    case 0:
                        collectionDTO.InstanceCon["channel_1"] = cartData;
                        break;
                    case 1:
                        collectionDTO.InstanceCon["channel_2"] = cartData;
                        break;
                    case 2:
                        collectionDTO.InstanceCon["channel_3"] = cartData;
                        break;
                    case 3:
                        collectionDTO.InstanceCon["channel_4"] = cartData;
                        break;
                    default:
                        break;
                }

            }

            //정렬
            collectionDTO.NormalCon = collectionDTO.NormalCon.OrderBy(nomal => nomal.ROWNUM).ToList();
            return collectionDTO;
        }
        public static List<PrintDTO> Converting(this List<PrintEntity> prints)
        {
            var result = new List<PrintDTO>();

            foreach (var item in prints)
            {
                var printItem = new PrintDTO();
                printItem.ROWNUM = item.SEQNUM;
                printItem.CODE = item.CODE.TrimEnd();
                printItem.CONTENTS = item.CONTENTS ?? "";
                printItem.ETC = item.ETC ?? "";
                printItem.USEDTIME = item.USEDTIME;
                result.Add(printItem);
            }

            return result.OrderBy(print => print.ROWNUM).ToList();
        }
        public static List<AttachmentDTO> Converting(this List<AttachmentEntity> attachments)
        {
            var result = new List<AttachmentDTO>();

            foreach (var item in attachments)
            {
                var attachItem = new AttachmentDTO();
                attachItem.FILEID = item.FILEID;
                attachItem.FILEPATH = item.FILEPATH ?? "";
                attachItem.FILENAME = SetFileName_SplitFileId(item);
                attachItem.FILESIZE = item.FILESIZE;
                attachItem.DELSTATE = false;
                result.Add(attachItem);
            }

            return result;
        }
        private static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        private static string TrimMaxLength(string data, int length)
        {
            if (string.IsNullOrEmpty(data))
                return data;
            else
                return data.Length >= length ? data.Substring(0, length) : data;
        }

        public static bool CheckByteLength(String Data, int maxLength)
        {
            byte[] byteTEMP = Encoding.Default.GetBytes(Data);
            return (byteTEMP.Length > maxLength) ? false : true;
        }
        public static String ByteSubstring(String Data, int StartIdx, int byteLength)
        {
            String retVal = "";
            byte[] byteTEMP = Encoding.Default.GetBytes(Data);
            retVal = Encoding.Default.GetString(byteTEMP, StartIdx, byteLength);

            return retVal;
        }
        public static String SetFileName_SplitFileId(AttachmentEntity file)
        {
            var filename = Path.GetFileName(file.FILEPATH);
            var count = file.FILEID.ToString().Length + 1;
            var result = filename.Substring(count);
            return result;
        }
        public static string SetCartCode(string code)
        {
            var result = "";
            switch (code)
            {
                case "S01G01C013":
                    result = "AC";
                    break;
                case "S01G01C017":
                    result = "AS";
                    break;
                case "S01G01C010":
                    result = "ST";
                    break;
                case "S01G01C018":
                    result = "CM";
                    break;
                case "S01G01C019":
                    result = "CM";
                    break;
                case "S01G01C012":
                    result = "RC";
                    break;
                case "S01G01C021":
                    result = "FC";
                    break;
                case "S01G01C022":
                    result = "FC";
                    break;
                case "S01G01C023":
                    result = "FC";
                    break;
                case "S01G01C024":
                    result = "FC";
                    break;
                case "S01G01C009":
                    result = "PM";
                    break;
                case "S01G01C016":
                    result = "AS";
                    break;
                case "S01G01C020":
                    result = "MS";
                    break;
                //구DB지원 안함 구DB 레이아웃 분리하면 뺴기
                case "S01G01C014":
                    result = "SS";
                    break;
                case "S01G01C015":
                    result = "SS";
                    break;
                case "S01G01C007":
                    result = "";
                    break;
                case "S01G01C006":
                    result = "";
                    break;
                case "S01G01C032":
                    result = "SS";
                    break;


                default:
                    break;
            }
            return result;
        }
    }
}
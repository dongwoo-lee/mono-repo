using M30.AudioFile.Common.Foundation;
using M30_CueSheetDAO.Entity;
using M30_CueSheetDAO.ParamEntity;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Utils
{
    public static class Extentions
    {
        //일일 큐시트 Entity TO DTO로 변환 -List
        public static IEnumerable<DayCueSheetListDTO> Converting(this List<DayCueSheetListEntity> entity)
        {
            List<DayCueSheetListDTO> dto = new List<DayCueSheetListDTO>();

            foreach (var item in entity)
            {
                dto.Add(new DayCueSheetListDTO()
                {
                    PGMCODE = item.PGMCODE,
                    SERVICENAME = item.SERVICENAME,
                    CODENAME = item.CODENAME,
                    ONAIRDAY = item.ONAIRDAY,
                    STARTDATE = item.STARTDATE,
                    PRODUCTID = item.PRODUCTID,
                    EVENTNAME = item.EVENTNAME,
                    DAY = item.DAY,
                    R_ONAIRTIME = item.R_ONAIRTIME,
                    CUEID = item.CUEID,
                    MEDIA = item.MEDIA,
                    EDIT = item.EDIT,
                    SEQNUM = item.SEQNUM,
                    LIVEFLAG = item.LIVEFLAG,
                });
            }

            return dto;
        }

        //기본 큐시트 Entity TO DTO로 변환 -List
        public static IEnumerable<DefCueSheetListDTO> Converting(this List<DefCueSheetListEntity> entity)
        {
            List<DefCueSheetListDTO> dto = new List<DefCueSheetListDTO>();
            foreach (var item in entity)
            {
                var dtoItem = new DefCueSheetListDTO();
                dtoItem.PRODUCTID = item.PRODUCTID;
                dtoItem.EVENTNAME = item.EVENTNAME;
                dtoItem.SERVICENAME = item.SERVICENAME;
                dtoItem.CODENAME = item.CODENAME;
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

        //템플릿 Entity TO DTO로 변환 -List
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

        //이전 큐시트 Entity TO DTO로 변환 -List
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

                });
            }

            return dto;
        }

        //일일 큐시트 Entity TO DTO로 변환 - 상세내용
        public static CueSheetCollectionDTO DayConverting(this CueSheetCollectionEntity entity)
        {
            CueSheetCollectionDTO collectionDTO = new CueSheetCollectionDTO();
            collectionDTO.CueSheetDTO = new CueSheetDTO();
            collectionDTO.NormalCon = new List<CueSheetConDTO>();
            collectionDTO.InstanceCon = new Dictionary<string, List<CueSheetConDTO>>();
            collectionDTO.PrintDTO = new List<PrintDTO>();

            var InstanceConList = new List<CueSheetConDTO>();

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

            //AB가공
            foreach (var item in entity.CueSheetConEntities)
            {
                var con = new CueSheetConDTO();
                con.CARTCODE = item.CARTCODE ?? "";
                con.CHANNELTYPE = item.CHANNELTYPE ?? "";
                con.ONAIRDATE = item.ONAIRDATE ?? "";
                if (!string.IsNullOrEmpty(item.CARTID) && string.IsNullOrEmpty(item.MEMO))
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

                if (item.ONAIRDATE == null || item.ONAIRDATE == "")
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

            //C가공
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

            //print 가공
            foreach (var item in entity.PrintEntities)
            {
                var printItem = new PrintDTO();
                printItem.ROWNUM = item.SEQNUM;
                printItem.CODE = item.CODE.TrimEnd();
                printItem.CONTENTS = item.CONTENTS ?? "";
                printItem.ETC = item.ETC ?? "";
                printItem.USEDTIME = item.USEDTIME;
                collectionDTO.PrintDTO.Add(printItem);
            }
            return collectionDTO;
        }

        //기본 큐시트 Entity TO DTO로 변환 - 상세내용
        public static CueSheetCollectionDTO DefConverting(this CueSheetCollectionEntity entity)
        {
            CueSheetCollectionDTO collectionDTO = new CueSheetCollectionDTO();
            collectionDTO.CueSheetDTO = new CueSheetDTO();
            collectionDTO.NormalCon = new List<CueSheetConDTO>();
            collectionDTO.InstanceCon = new Dictionary<string, List<CueSheetConDTO>>();
            collectionDTO.PrintDTO = new List<PrintDTO>();
            var detailArr = new List<ViewDetail>();

            var InstanceConList = new List<CueSheetConDTO>();

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
                detailItem.WEEK = item.WEEK;
                detailArr.Add(detailItem);
            }
            collectionDTO.CueSheetDTO.DETAIL = detailArr;

            //AB가공
            foreach (var item in entity.CueSheetConEntities)
            {
                var con = new CueSheetConDTO();
                con.CARTCODE = item.CARTCODE ?? "";
                con.CHANNELTYPE = item.CHANNELTYPE ?? "";
                con.CARTID = item.CARTID ?? "";
                con.ONAIRDATE = item.ONAIRDATE ?? "";
                if (!string.IsNullOrEmpty(item.CARTID) && string.IsNullOrEmpty(item.MEMO))
                {
                    con.DURATION = item.AUDIOS[0].P_DURATION; //나중에 그룹소재 적용되면 바꿔야함

                }
                con.STARTPOSITION = item.STARTPOSITION;
                con.ENDPOSITION = item.ENDPOSITION;
                con.FADEINTIME = item.FADEINTIME > 0 ? true : false;
                con.FADEOUTTIME = item.FADEOUTTIME > 0 ? true : false;
                con.MAINTITLE = item.MAINTITLE ?? "";
                con.SUBTITLE = item.SUBTITLE ?? "";
                con.MEMO = item.MEMO ?? "";
                con.TRANSTYPE = item.TRANSTYPE;
                con.USEFLAG = item.USEFLAG ?? "Y";
                con.EDITTARGET = true;
                con.CARTTYPE = SetCartCode(item.CARTCODE);

                if (item.ONAIRDATE == null || item.ONAIRDATE == "")
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

            //C가공
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

            //print 가공
            foreach (var item in entity.PrintEntities)
            {
                var printItem = new PrintDTO();
                printItem.ROWNUM = item.SEQNUM;
                printItem.CODE = item.CODE.TrimEnd();
                printItem.CONTENTS = item.CONTENTS ?? "";
                printItem.ETC = item.ETC ?? "";
                printItem.USEDTIME = item.USEDTIME;
                collectionDTO.PrintDTO.Add(printItem);
            }

            return collectionDTO;
        }

        //템플릿 Entity TO DTO로 변환 - 상세내용
        public static CueSheetCollectionDTO TemConverting(this TemplateCollectionEntity entity)
        {
            CueSheetCollectionDTO collectionDTO = new CueSheetCollectionDTO();
            collectionDTO.CueSheetDTO = new CueSheetDTO();
            collectionDTO.NormalCon = new List<CueSheetConDTO>();
            collectionDTO.InstanceCon = new Dictionary<string, List<CueSheetConDTO>>();
            collectionDTO.PrintDTO = new List<PrintDTO>();
            var detailArr = new List<ViewDetail>();

            var InstanceConList = new List<CueSheetConDTO>();

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

            //AB가공
            foreach (var item in entity.CueSheetConEntities)
            {
                var con = new CueSheetConDTO();
                con.CARTCODE = item.CARTCODE ?? "";
                con.CHANNELTYPE = item.CHANNELTYPE ?? "";
                con.CARTID = item.CARTID ?? "";
                con.ONAIRDATE = item.ONAIRDATE ?? "";
                if (!string.IsNullOrEmpty(item.CARTID) && string.IsNullOrEmpty(item.MEMO))
                {
                    con.DURATION = item.AUDIOS[0].P_DURATION; //나중에 그룹소재 적용되면 바꿔야함

                }
                con.STARTPOSITION = item.STARTPOSITION;
                con.ENDPOSITION = item.ENDPOSITION;
                con.FADEINTIME = item.FADEINTIME > 0 ? true : false;
                con.FADEOUTTIME = item.FADEOUTTIME > 0 ? true : false;
                con.MAINTITLE = item.MAINTITLE ?? "";
                con.SUBTITLE = item.SUBTITLE ?? "";
                con.MEMO = item.MEMO ?? "";
                con.TRANSTYPE = item.TRANSTYPE;
                con.USEFLAG = item.USEFLAG ?? "Y";
                con.EDITTARGET = true;
                con.CARTTYPE = SetCartCode(item.CARTCODE);

                if (item.ONAIRDATE == null || item.ONAIRDATE == "")
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

            //C가공
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

            //print 가공
            foreach (var item in entity.PrintEntities)
            {
                var printItem = new PrintDTO();
                printItem.ROWNUM = item.SEQNUM;
                printItem.CODE = item.CODE.TrimEnd();
                printItem.CONTENTS = item.CONTENTS ?? "";
                printItem.ETC = item.ETC ?? "";
                printItem.USEDTIME = item.USEDTIME;
                collectionDTO.PrintDTO.Add(printItem);
            }

            return collectionDTO;
        }

        //이전 큐시트 Entity TO DTO로 변환 - 상세내용
        public static CueSheetCollectionDTO ArchiveConverting(this ArchiveCueSheetCollectionEntity entity)
        {
            CueSheetCollectionDTO collectionDTO = new CueSheetCollectionDTO();
            collectionDTO.CueSheetDTO = new CueSheetDTO();
            collectionDTO.NormalCon = new List<CueSheetConDTO>();
            collectionDTO.InstanceCon = new Dictionary<string, List<CueSheetConDTO>>();
            collectionDTO.PrintDTO = new List<PrintDTO>();

            var InstanceConList = new List<CueSheetConDTO>();

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


            //AB가공
            foreach (var item in entity.CueSheetConEntities)
            {
                var con = new CueSheetConDTO();
                con.CARTCODE = item.CARTCODE ?? "";
                con.CHANNELTYPE = item.CHANNELTYPE ?? "";
                con.ONAIRDATE = item.ONAIRDATE ?? "";
                if (!string.IsNullOrEmpty(item.CARTID) && string.IsNullOrEmpty(item.MEMO))
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

                if (item.ONAIRDATE == null || item.ONAIRDATE == "")
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

            //C가공
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

            //print 가공
            foreach (var item in entity.PrintEntities)
            {
                var printItem = new PrintDTO();
                printItem.ROWNUM = item.SEQNUM;
                printItem.CODE = item.CODE.TrimEnd();
                printItem.CONTENTS = item.CONTENTS ?? "";
                printItem.ETC = item.ETC ?? "";
                printItem.USEDTIME = item.USEDTIME;
                collectionDTO.PrintDTO.Add(printItem);
            }
            return collectionDTO;
        }

        //Entity TO DTO로 변환 - 유저당 프로그램 목록 
        public static IEnumerable<PgmListDTO> Converting(this List<ProgramListEntity> entity)
        {
            List<PgmListDTO> dto = new List<PgmListDTO>();
            List<ProgramListDetailDTO> detail = new List<ProgramListDetailDTO>();
            foreach (var item in entity)
            {
                var dtoItem = new PgmListDTO();
                dtoItem.MEDIA = item.MEDIA;
                dtoItem.PERSONID = item.PERSONID;
                foreach (var detailItem in item.DETAILS)
                {
                    var dtoDetail = new ProgramListDetailDTO();
                    dtoDetail.EVENTNAME = detailItem.EVENTNAME;
                    dtoDetail.PRODUCTID = detailItem.PRODUCTID;
                    dtoDetail.SERVICENAME = detailItem.SERVICENAME;
                    detail.Add(dtoDetail);
                }
                dtoItem.DETAILS = detail;
                dto.Add(dtoItem);
            }
            return dto;

        }

        //Entity TO DTO로 변환 - CM 광고 가져오기
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

        //Entity TO DTO로 변환 - 즐겨찾기
        public static IEnumerable<CueSheetConDTO> Converting(this List<UserFavConEntity> entity)
        {
            var favList = new List<CueSheetConDTO>();
            foreach (var item in entity)
            {
                var favItem = new CueSheetConDTO();
                favItem.ROWNUM = item.SEQNUM;
                favItem.CARTCODE = item.CARTCODE ?? "";
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
                favList.Add(favItem);
            }

            var resultFavList = new List<CueSheetConDTO>();
            for (var i = 0; 16 > i; i++)
            {
                var row = new CueSheetConDTO();
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

        // DTO TO Entity - 일일 큐시트 저장
        public static DayCueSheetCreateParam DayToEntity(this CueSheetCollectionDTO dto)
        {
            //cue
            DayCueSheetCreateParam result = new DayCueSheetCreateParam();
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

            //print
            result.PrintParams = new List<PrintParam>();
            foreach (var item in dto.PrintDTO)
            {
                PrintParam obj = new PrintParam();
                obj.p_code = item.CODE;
                obj.p_seqnum = item.ROWNUM;
                obj.p_contents = item.CONTENTS;
                obj.p_usedtime = item.USEDTIME;
                obj.p_etc = item.ETC;
                result.PrintParams.Add(obj);
            }

            result.CueSheetConParams = new List<CueSheetConParam>();

            //ab
            foreach (var item in dto.NormalCon)
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
                obj.p_fadeintime = item.FADEINTIME ? 300 : 0;
                obj.p_fadeouttime = item.FADEOUTTIME ? 300 : 0;
                obj.p_transtype = char.Parse(item.TRANSTYPE);
                obj.p_maintitle = item.MAINTITLE;
                obj.p_subtitle = item.SUBTITLE;
                obj.p_memo = item.MEMO;
                result.CueSheetConParams.Add(obj);
            }
            var index = 0;
            //c
            foreach (var cData in dto.InstanceCon)
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
                        obj.p_fadeintime = item.FADEINTIME ? 300 : 0;
                        obj.p_fadeouttime = item.FADEOUTTIME ? 300 : 0;
                        obj.p_transtype = char.Parse(item.TRANSTYPE);
                        obj.p_maintitle = item.MAINTITLE;
                        obj.p_subtitle = item.SUBTITLE;
                        obj.p_memo = item.MEMO;
                        result.CueSheetConParams.Add(obj);
                    }
                }
                index++;
            }
            return result;
        }

        // DTO TO Entity - 기본 큐시트 저장
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
            if (dto.PrintDTO != null && dto.PrintDTO.Count != 0)
            {
                //print
                result.PrintParams = new List<PrintParam>();
                foreach (var item in dto.PrintDTO)
                {
                    PrintParam obj = new PrintParam();
                    obj.p_code = item.CODE;
                    obj.p_seqnum = item.ROWNUM;
                    obj.p_contents = item.CONTENTS;
                    obj.p_usedtime = item.USEDTIME;
                    obj.p_etc = item.ETC;
                    result.PrintParams.Add(obj);
                }

                result.CueSheetConParams = new List<CueSheetConParam>();
            }

            if (dto.NormalCon != null)
            {
                //ab
                foreach (var item in dto.NormalCon)
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
                    obj.p_fadeintime = item.FADEINTIME ? 300 : 0;
                    obj.p_fadeouttime = item.FADEOUTTIME ? 300 : 0;
                    obj.p_transtype = char.Parse(item.TRANSTYPE);
                    obj.p_maintitle = item.MAINTITLE;
                    obj.p_subtitle = item.SUBTITLE;
                    obj.p_memo = item.MEMO;
                    result.CueSheetConParams.Add(obj);
                }

            }
            if (dto.InstanceCon != null)
            {
                var index = 0;
                //c
                foreach (var cData in dto.InstanceCon)
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
                            obj.p_fadeintime = item.FADEINTIME ? 300 : 0;
                            obj.p_fadeouttime = item.FADEOUTTIME ? 300 : 0;
                            obj.p_transtype = char.Parse(item.TRANSTYPE);
                            obj.p_maintitle = item.MAINTITLE;
                            obj.p_subtitle = item.SUBTITLE;
                            obj.p_memo = item.MEMO;
                            result.CueSheetConParams.Add(obj);
                        }
                    }
                    index++;
                }
            }
            return result;

        }

        // DTO TO Entity - 템플릿 저장
        public static TemplateCreateParam TempToEntity(this CueSheetCollectionDTO dto)
        {
            //cue
            TemplateCreateParam result = new TemplateCreateParam();
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

            if (dto.PrintDTO != null)
            {
                //print
                result.PrintParams = new List<PrintParam>();
                foreach (var item in dto.PrintDTO)
                {
                    PrintParam obj = new PrintParam();
                    obj.p_code = item.CODE;
                    obj.p_seqnum = item.ROWNUM;
                    obj.p_contents = item.CONTENTS;
                    obj.p_usedtime = item.USEDTIME;
                    obj.p_etc = item.ETC;
                    result.PrintParams.Add(obj);
                }

                result.CueSheetConParams = new List<CueSheetConParam>();
            }

            if (dto.NormalCon != null)
            {
                //ab
                foreach (var item in dto.NormalCon)
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
                    obj.p_fadeintime = item.FADEINTIME ? 300 : 0;
                    obj.p_fadeouttime = item.FADEOUTTIME ? 300 : 0;
                    obj.p_transtype = char.Parse(item.TRANSTYPE);
                    obj.p_maintitle = item.MAINTITLE;
                    obj.p_subtitle = item.SUBTITLE;
                    obj.p_memo = item.MEMO;
                    result.CueSheetConParams.Add(obj);
                }

            }
            if (dto.InstanceCon != null)
            {
                var index = 0;
                //c
                foreach (var cData in dto.InstanceCon)
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
                            obj.p_fadeintime = item.FADEINTIME ? 300 : 0;
                            obj.p_fadeouttime = item.FADEOUTTIME ? 300 : 0;
                            obj.p_transtype = char.Parse(item.TRANSTYPE);
                            obj.p_maintitle = item.MAINTITLE;
                            obj.p_subtitle = item.SUBTITLE;
                            obj.p_memo = item.MEMO;
                            result.CueSheetConParams.Add(obj);
                        }
                    }
                    index++;
                }
            }
            return result;
        }

        // DTO TO Entity - 즐겨찾기 저장
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
                    favItem.p_fadeintime = item.FADEINTIME ? 300 : 0;
                    favItem.p_fadeouttime = item.FADEOUTTIME ? 300 : 0;
                    favItem.p_maintitle = item.MAINTITLE;
                    favItem.p_subtitle = item.SUBTITLE;
                    favItem.p_memo = item.MEMO;
                    result.Add(favItem);
                }
            }
            return result;

        }

        // DTO TO Entity - 구 DB
        //public static PDPQSParam PDPQSToEntity(this CueSheetDTO dto, char type)
        //{
        //    var result = new PDPQSParam();
        //    result.ProductID_in = dto.PRODUCTID;
        //    result.OnairDate_in = dto.BRDDATE;
        //    result.PQSType_in = type;
        //    result.Media_in = Char.Parse(dto.MEDIA);
        //    result.LiveFlag_in = dto.LIVEFLAG;
        //    result.StateID_in = "P000";
        //    result.Editor_in = dto.PERSONID;
        //    result.OnairDay_in = dto.ONAIRDAY;
        //    result.StartDate_in = dto.STARTDATE;
        //    result.SeqNum_in = dto.SEQNUM;

        //    return result;
        //}

        // DTO TO Entity - 구 DB

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
                        resultItem.FadeInTime_in = item.FADEINTIME ? 300 : 0;
                        resultItem.FadeOutTime_in = item.FADEOUTTIME ? 300 : 0;
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
                            resultItem.FadeInTime_in = item.FADEINTIME ? 300 : 0;
                            resultItem.FadeOutTime_in = item.FADEOUTTIME ? 300 : 0;
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

        //상세내용 가져오기 시 광고 업데이트
        public static List<CueSheetConEntity> SetSponsor(this SponsorCollectionEntity spons, List<CueSheetConEntity> entity)
        {
            if (spons.CM?.Any() == true)
            {
                foreach (var item in spons.CM)
                {
                    var cartId = item.CMGROUPID.Substring(2);
                    foreach (var cueItem in entity)
                    {
                        if (cueItem.CARTID.Contains(cartId))
                        {
                            cueItem.ONAIRDATE = item.ONAIRDATE;
                            cueItem.STARTPOSITION = 0;
                            var duration = 0;
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
                foreach (var item in spons.SB)
                {
                    var cartId = item.GROUPCONTENTID.Substring(2);
                    foreach (var cueItem in entity)
                    {
                        if (cueItem.CARTID.Contains(cartId))
                        {
                            cueItem.ONAIRDATE = item.ONAIRDATE;
                            cueItem.STARTPOSITION = 0;
                            var duration = 0;
                            foreach (var clip in item.Clips)
                            {
                                duration = duration + clip.LENGTH;

                            }
                            cueItem.ENDPOSITION = duration;
                            cueItem.FADEINTIME = 0;
                            cueItem.FADEOUTTIME = 0;
                            cueItem.MAINTITLE = item.NAME;
                            cueItem.SUBTITLE = item.ID;

                        }
                    }
                }
            }
            return entity;
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
                default:
                    break;
            }
            return result;
        }
    }
}
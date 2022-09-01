﻿using M30_CueSheetDAO;
using M30_CueSheetDAO.DAO;
using M30_CueSheetDAO.Entity;
using M30_CueSheetDAO.Interfaces;
using M30_CueSheetDAO.ParamEntity;
using M30.AudioFile.Common.DTO;
using MAMBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.DTO;

namespace MAMBrowser.BLL
{
    public class CueUserInfoBll
    {
        private readonly ICommonDAO _dao;

        public CueUserInfoBll(ICommonDAO dao)
        {
            _dao = dao;
        }

        //프로그램 목록 가져오기
        public IEnumerable<PgmListDTO> GetUserPgmList(string person, char media)
        {
            ProgramListParam param = new ProgramListParamBuilder()
                .SetMedia(media)
                .SetPerson(person)
                .Build();

            return _dao.GetProgramList(param)?.Converting();
        }

        //담당자 목록 가져오기
        public string GetDirectorList(string productid)
        {
            return _dao.GetCueSheetDirectorName(productid);
        }

    }
}
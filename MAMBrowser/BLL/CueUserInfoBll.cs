using M30_CueSheetDAO;
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

        //방송일 프로그램 목록 가져오기 
        public IEnumerable<PgmListDTO> GetPgmListByBrdDate(string person, char media,string brd_dt)
        {
            ProgramListParam param = new ProgramListParamBuilder()
                .SetMedia(media)
                .SetPerson(person)
                .SetBrdDate(brd_dt)
                .Build();

            return _dao.GetProgramList(param)?.Converting();
        }
        //송출표 프로그램 목록 가져오기
        public IEnumerable<PgmListDTO> GetSCHPgmList(string person, char media, string brd_dt)
        {
            ProgramListParam param = new ProgramListParamBuilder()
                .SetMedia(media)
                .SetPerson(person)
                .SetBrdDate(brd_dt)
                .Build();

            return _dao.GetSCHPpgmList(param)?.Converting();
        }

        //담당자 목록 가져오기
        public string GetDirectorList(string productid)
        {
            return _dao.GetCueSheetDirectorName(productid);
        }

        //프로그램 키워드 가져오기
        public string GetPgmcodeKeyword(string pgmcode)
        {
            return  _dao.GetPgmcodeKeyword(pgmcode);
        }

    }
}
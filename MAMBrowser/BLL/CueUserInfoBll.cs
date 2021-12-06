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

        public IEnumerable<PgmListDTO> GetUserPgmList(string personid, char media)
        {
            ProgramListParam param = new ProgramListParamBuilder()
                .SetMedia(media)
                .SetPersonid(personid)
                .Build();

            return _dao.GetProgramList(param)?.Converting();
        }
        public string GetDirectorList(string productid)
        {
            return _dao.GetCueSheetDirectorName(productid);
        }

        public IEnumerable<CueSheetConDTO> GetSponsorList(string pgmcode, string brd_dt)
        {
            SponsorParam param = new SponsorParam();
            param.BrdDate = brd_dt;
            param.PgmCode = pgmcode;

            return _dao.GetSponsor(param)?.SponsorConverting();
        }

    }
}
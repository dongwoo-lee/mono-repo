using DAP3.CueSheetCommon.DTO.Result;
using DAP3.CueSheetDAL.Factories.Web;
using Dapper;
using M30.AudioFile.DAL;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAMBrowser.BLL
{
    public class CueUserInfoBll
    {
        private readonly WebCueSheetFactory _factory;
        public CueUserInfoBll(WebCueSheetFactory factory)
        {
            _factory = factory;
        }

        public List<PgmListDTO> GetUserPgmList(string personid, string media)
        {
            return _factory.InformationRepository.GetPgmList(personid, media).ToList();
        }

        public string GetDirectorList(string productid)
        {
            return _factory.CueSheetRepository.GetCueSheetDirectorName(productid);
        }
    }
}

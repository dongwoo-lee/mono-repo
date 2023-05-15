using System;
using System.Collections.Generic;

namespace MAMBrowser.DTO
{
    public class ManagementDeleteProductsDTO
    {
        public class BaseFileDTO
        {
            public string AUDIOCLIPID { get; set; }
            public string NAME { get; set; }
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
            public DateTime LASTONAIRDATE { get; set; }
        }
        public class SpotFileDTO : BaseFileDTO
        {
            public string OPRSPOTID { get; set; }
        }
        public class EtcFileDTO : BaseFileDTO { }
        public class FillerFileDTO : BaseFileDTO
        {
            public DateTime ENDDATE { get; set; }
        }
        public class ReportFileDTO : BaseFileDTO
        {
            public DateTime ONAIRDATE { get; set; }
        }
        public class ProductFileDTO : BaseFileDTO
        {
            public DateTime ONAIRDATE { get; set; }
        }
        public class SongFileDTO : BaseFileDTO
        {
            public DateTime LASTONAIRDATE { get; set; }
        }

        public class RecycleDTO
        {
            public long SEQ { get; set; }
            public string AUDIOCLIPID { get; set; }
            public string MASTERFILE { get; set; }
            public string USERID { get; set; }
            public DateTime DELTIME { get; set; }
        }
        public class SelectDelProductParamDTO : PageParamDTO
        {
            public string name { get; set; }
            public string startdate { get; set; }
            public string enddate { get; set; }
            public string brddate { get; set; }
            public char media { get; set; }
        }
        public class DeleteAudioClipFileParamDTO
        {
            public string AUDIOCLIPID { get; set; }
            public string MASTERFILE { get; set; }
        }

        public class DeleteAudioClipIdsParamDTO
        {
            public string UserId { get; set; }
            public bool PermanentlyVal { get; set; }
            public List<DeleteAudioClipFileParamDTO> IDs { get; set; }
        }

        public class SelectRecycleParamDTO : PageParamDTO
        {
            public string startdate { get; set; }
            public string enddate { get; set; }
            public string audiotype { get; set; }
        }
    }
}

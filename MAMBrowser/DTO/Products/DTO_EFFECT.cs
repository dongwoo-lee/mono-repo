using MAMBrowser.Entiies;
using MAMBrowser.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_EFFECT : DTO_FILEBASE
    {
        public string Name { get; set; }    //효과음명
        public string Description { get; set; } //설명
        public string Duration { get; set; }    //길이
        public string AudioFormat { get; set; }    //오디오 포맷
        public string MusicFileToken { get; set; }

        public DTO_EFFECT()
        {

        }
        public DTO_EFFECT(EDTO_EFFECT edto)
        {
            this.Name = edto.Name;
            this.Description = edto.Description;
            this.Duration = edto.Duration2;
            this.AudioFormat = edto.AudioFormat;
            this.FilePath = Path.Combine(edto.WavFilePath, edto.WavFileName);

            MusicFileToken = MAMUtility.GenerateMusicToken(this.FilePath);
        }
    }
}

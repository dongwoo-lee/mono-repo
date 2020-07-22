using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_EFFECT : DTO_BASE
    {
        public string Name { get; set; }    //효과음명
        public string Description { get; set; } //설명
        public string Duration { get; set; }    //길이
        public string AudioFormat { get; set; }    //오디오 포맷
        public string FilePath { get; set; }    //파일경로 

        public DTO_EFFECT()
        {

        }
        public DTO_EFFECT(EDTO_EFFECT edto)
        {
            this.Name = edto.Name;
            this.Description = edto.Description;
            this.Duration = edto.Duration;
            this.AudioFormat = edto.AudioFormat;
            this.FilePath = Path.Combine(edto.WavFilePath, edto.WavFileName);
        }
    }
}

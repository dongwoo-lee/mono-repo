using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DTO
{
    public class DTO_EFFECT
    {
        public string Name { get; set; }    //효과음명
        public string Description { get; set; } //설명
        public string Duration { get; set; }    //길이
        public string AudioFormat { get; set; }    //오디오 포맷
        public string FilePath { get; set; }    //파일경로 
    }
}

using MAMBrowser.Entiies;
using MAMBrowser.Helpers;
using System;

namespace MAMBrowser.DTO
{
    public class DTO_FILEBASE : DTO_BASE
    {
        private string filePath;
        public string FilePath { 
            get=> filePath; 
            set
            {
                if (filePath == value)
                    return;

                filePath = value;
                FileToken = MAMUtility.GetJwtFileToken(filePath);
            }
        }
        public string FileToken { get; set; }
    }
}

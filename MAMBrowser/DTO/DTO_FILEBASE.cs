using MAMBrowser.Entiies;
using MAMBrowser.Helpers;
using System;

namespace MAMBrowser.DTO
{
    public class DTO_FILEBASE : DTO_BASE
    {
        protected string filePath;
        public virtual string FilePath { 
            get=> filePath; 
            set
            {
                if (filePath == value)
                    return;

                filePath = value;
                FileToken = MAMUtility.GenerateMAMToken(filePath);
            }
        }
        public string FileToken { get; set; }
    }
}

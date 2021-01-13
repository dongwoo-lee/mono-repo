using MAMBrowser.Common.Foundation;
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
                
                FileToken = TokenGenerator.GenerateFileToken(filePath);
                if (string.IsNullOrEmpty(filePath))
                    ExistFile = false;
                else
                    ExistFile = true;
            }
        }
        public string FileToken { get; set; }
        public bool ExistFile { get; set; }
  
    }
}

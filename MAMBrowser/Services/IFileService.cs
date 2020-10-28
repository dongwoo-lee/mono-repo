using MAMBrowser.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Processor
{
    public interface IFileService
    {
        string Name { get; set; }
        string TmpUploadFolder { get; set; }
        string UploadFolder { get; set; }

        void MakeDirectory(string relativeDirectoryPath);
        void Upload(Stream fileStream, string relativeSourcePath, long fileLength);
        void Move(string source, string destination);
        bool DownloadFile(string fromRelativePath, string toFilePath);
        Stream GetFileStream(string relativeSourcePath, long offSet);
        bool ExistFile(string fromRelativePath);
    }
}

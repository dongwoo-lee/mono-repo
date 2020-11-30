using MAMBrowser.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Processor
{
    public interface IFileService : IFileDownloadService
    {
        string Name { get; set; }
        string TmpUploadFolder { get; set; }
        string UploadFolder { get; set; }
        void MakeDirectory(string directoryPath);
        void Upload(Stream fileStream, string sourcePath, long fileLength);
        void Move(string source, string destination);
        bool ExistFile(string fromPath);
        string GetAudioFormat(string filePath);
        void Delete(string filePath);
        void DeleteDirectory(string userId);
    }

    public interface IFileDownloadService
    {
        string UploadHost { get;  }
        bool DownloadFile(string fromPath, string toPath);
        Stream GetFileStream(string sourcePath, long offSet);
        bool ExistFile(string fromPath);
    }
}

using System.IO;

namespace MAMBrowser.Foundation
{
    public interface IFileProtocol : IFileDownloadProtocol
    {
        string TmpUploadFolder { get; set; }
        string UploadFolder { get; set; }
        void MakeDirectory(string directoryPath);
        void Upload(Stream headerStream, Stream fileStream, string sourcePath);
        long GetFileSize(string sourcePath);
        void Move(string source, string destination);
        bool ExistFile(string fromPath);
        void Delete(string filePath);
    }

    public interface IFileDownloadProtocol
    {
        string UploadHost { get;  }
        bool DownloadFile(string fromPath, string toPath);
        Stream GetFileStream(string sourcePath, long offSet);
        bool ExistFile(string fromPath);
    }
}

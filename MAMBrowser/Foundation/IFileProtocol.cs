using System.IO;

namespace MAMBrowser.Foundation
{
    public interface IFileProtocol : IFileDownloadProtocol
    {
        void MakeDirectory(string directoryPath);
        void Upload(string localSourcePath, string targetPath);
        void Upload(Stream localSourceStream, string targetPath);
        /// <summary>
        /// copy to storage from storage
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        void Copy(string sourcePath, string targetPath);
        long GetFileSize(string sourcePath);
        void Move(string source, string destination);
        bool ExistFile(string fromPath);
        bool ExistDirectory(string directoryPath);
        void Delete(string filePath);
    }

    public interface IFileDownloadProtocol
    {
        bool DownloadFile(string fromPath, string toPath);
        Stream GetFileStream(string sourcePath, long offSet);
        bool ExistFile(string fromPath);
    }
}

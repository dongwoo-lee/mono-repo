using MAMBrowser.Helpers;
using System;
using System.IO;

namespace MAMBrowser.Foundation
{
    //path : 호스트명이 포함됨 풀경로
    public class NetDriveProtocol : IFileProtocol
    {
        public string UploadHost { get; set; }
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        
        public NetDriveProtocol(string uploadHost, string userId, string userPass, string tmpUploadFolder, string uploadFolder)
        {
            UploadHost = uploadHost;
            UserId = userId;
            UserPass = userPass;
            TmpUploadFolder = tmpUploadFolder;
            UploadFolder = uploadFolder;
        }
        public Stream GetFileStream(string sourcePath, long offSet)
        {
            string sourceHost = MAMUtility.GetHost(sourcePath);
            NetworkShareAccessor.Access(sourceHost, UserId, UserPass);
            FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return fs;
        }
        public bool DownloadFile(string fromPath, string toPath)
        {
            string sourceHost = MAMUtility.GetHost(fromPath);

            NetworkShareAccessor.Access(sourceHost, UserId, UserPass);
            //확인필요
            using (FileStream inStream = new FileStream(fromPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream outStream = new FileStream(toPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    inStream.CopyTo(outStream);
                    return true;
                }
            }
        }
        public void MakeDirectory(string directoryPath)
        {
            NetworkShareAccessor.Access(UploadHost, UserId, UserPass);
            Directory.CreateDirectory($@"\\{UploadHost}\{directoryPath}");
        }
        public void Move(string source, string destination)
        {
            //source : self server temp folder (x) -> remote storage temp folder
            //destination : remote storage
            NetworkShareAccessor.Access(UploadHost, UserId, UserPass);
            var sourcePath = $@"\\{UploadHost}\{source}";
            var targetPath = $@"\\{UploadHost}\{destination}";
            File.Move(sourcePath, targetPath, true);
        }
        public void Upload(Stream headerStream, Stream fileStream, string sourcePath)
        {
            //구현 및 테스트 필요.
            NetworkShareAccessor.Access(UploadHost, UserId, UserPass);
            var sourceFullPath = $@"\\{UploadHost}\{sourcePath}";
            using (FileStream fs = new FileStream(sourceFullPath, FileMode.Create, FileAccess.ReadWrite))
            {
                headerStream.CopyTo(fs);
                fileStream.CopyTo(fs);
            }
        }
        public bool ExistFile(string fromPath)
        {
            string sourceHost = MAMUtility.GetHost(fromPath);
            NetworkShareAccessor.Access(sourceHost, UserId, UserPass);
            return File.Exists(fromPath);
        }
        public void Delete(string filePath)
        {
            string sourceHost = MAMUtility.GetHost(filePath);
            NetworkShareAccessor.Access(sourceHost, UserId, UserPass);
            File.Delete(filePath);
        }
        public long GetFileSize(string sourcePath)
        {
            string sourceHost = MAMUtility.GetHost(sourcePath);
            NetworkShareAccessor.Access(sourceHost, UserId, UserPass);
            return new FileInfo(sourcePath).Length;
        }
    }
}

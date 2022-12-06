using M30.AudioFile.Common;
using MAMBrowser.Helpers;
using System;
using System.IO;

namespace MAMBrowser.Foundation
{
    //path : 호스트명이 포함됨 풀경로
    public class NetDriveProtocol : IFileProtocol
    {
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public NetDriveProtocol(string userId, string userPass)
        {
            UserId = userId;
            UserPass = userPass;
        }
        public Stream GetFileStream(string sourcePath, long offSet)
        {
            //string sourceHost = CommonUtility.GetHost(sourcePath);
            string directory = Path.GetDirectoryName(sourcePath);
            ConnectNetDrive.Connect(directory, UserId, UserPass);
            FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return fs;
        }
        public bool DownloadFile(string fromPath, string toPath)
        {
            string directory = Path.GetDirectoryName(fromPath);
            ConnectNetDrive.Connect(directory, UserId, UserPass);
            
            //확인필요
            using (FileStream inStream = new FileStream(fromPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream outStream = new FileStream(toPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    inStream.CopyTo(outStream);
                    return true;
                }
            }
        }
        public void MakeDirectory(string uncDirectoryPath)
        {
            //string directory = Path.GetDirectoryName(uncDirectoryPath);
            ConnectNetDrive.Connect(uncDirectoryPath, UserId, UserPass);
            Directory.CreateDirectory(uncDirectoryPath);
        }
        public bool ExistDirectory(string uncDirectoryPath)
        {
            //string directory = Path.GetDirectoryName(uncDirectoryPath);
            ConnectNetDrive.Connect(uncDirectoryPath, UserId, UserPass);
            return Directory.Exists(uncDirectoryPath);
        }
        public void Move(string source, string destination)
        {
            string sourceDirectory = Path.GetDirectoryName(source);
            string targetDirectory = Path.GetDirectoryName(destination);
            ConnectNetDrive.Connect(sourceDirectory, UserId, UserPass);
            ConnectNetDrive.Connect(targetDirectory, UserId, UserPass);
            File.Move(source, destination, true);
        }
        public void Upload(string localSourcePath, string targetPath)
        {
            string targetDirectory = Path.GetDirectoryName(targetPath);
            ConnectNetDrive.Connect(targetDirectory, UserId, UserPass);
            using (var readStream = new FileStream(localSourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    readStream.CopyTo(fs);
                }
            }
        }
        public void Upload(Stream localSourceStream, string targetPath)
        {
            string targetDirectory = Path.GetDirectoryName(targetPath);
            ConnectNetDrive.Connect(targetDirectory, UserId, UserPass);
            using (FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite))
            {
                localSourceStream.CopyTo(fs);
            }
        }
        public void Copy(string sourcePath, string targetPath)
        {
            string sourceDirectory = Path.GetDirectoryName(sourcePath);
            string targetDirectory = Path.GetDirectoryName(targetPath);
            ConnectNetDrive.Connect(sourceDirectory, UserId, UserPass);
            ConnectNetDrive.Connect(targetDirectory, UserId, UserPass);

            using (var readStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    readStream.CopyTo(fs);
                }
            }
        }
        public bool ExistFile(string fromPath)
        {
            string sourceDirectory = Path.GetDirectoryName(fromPath);
            ConnectNetDrive.Connect(sourceDirectory, UserId, UserPass);
            return File.Exists(fromPath);
        }
        public void Delete(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            ConnectNetDrive.Connect(directory, UserId, UserPass);
            File.Delete(filePath);
        }
        public long GetFileSize(string sourcePath)
        {
            string directory = Path.GetDirectoryName(sourcePath);
            ConnectNetDrive.Connect(directory, UserId, UserPass);
            return new FileInfo(sourcePath).Length;
        }
    }
       
}

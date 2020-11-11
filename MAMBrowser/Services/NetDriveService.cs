using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MAMBrowser.Processor
{
    //path : 호스트명이 포함됨 풀경로
    public class NetDriveService : IFileService
    {
        public string Host { get; set; }
        public string Name { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public NetDriveService()
        {
        }
        public Stream GetFileStream(string sourcePath, long offSet)
        {
            using (NetworkShareAccessor.Access(Host, UserId, UserPass))
            {
                FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return fs;
            }
        }
        public bool DownloadFile(string fromPath, string toPath)
        {
            using(NetworkShareAccessor.Access(Host, UserId, UserPass))
            {
                //확인필요
                using (FileStream inStream = new FileStream(fromPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    FileStream outStream = new FileStream(toPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    inStream.CopyTo(outStream);
                    return true;
                }
            }
        }
        public void MakeDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        public void Move(string source, string destination)
        {
            File.Move(source, destination, true);
        }

        public void Upload(Stream fileStream, string sourcePath, long fileLength)
        {
            throw new NotImplementedException();
        }

        public bool ExistFile(string fromPath)
        {
            return File.Exists(fromPath);
        }
    }
}

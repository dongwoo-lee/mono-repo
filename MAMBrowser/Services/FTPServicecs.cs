using FluentFTP;
using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using NLayer.NAudioSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MAMBrowser.Processor
{
    //path : 상대경로
    public class FtpService : IFileService
    {
        private const string FTP = @"ftp://";
        public FtpService()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public string Name { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        public string UploadHost { get; set; }
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public int EncodingType { get; set; }

        public void MakeDirectory(string directoryPath)
        {
            using (FtpClient ftpClient = new FtpClient(UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                ftpClient.CreateDirectory(directoryPath);
            }
        }

        public void Upload(Stream headerStream, Stream fileStream, string sourcePath, long fileLength)  //나중에 경로를 여기서 지정할수 있게끔...(지금은 상대경로 셋팅해서 들어옴)
        {
            using (FtpClient ftpClient = new FtpClient(UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                ftpClient.Upload(headerStream, sourcePath);
            }
            using (FtpClient ftpClient = new FtpClient(UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                ftpClient.Upload(fileStream, sourcePath, FtpRemoteExists.Append);
            }
        }

        public void Move(string source, string destination)
        {
            using (FtpClient ftpClient = new FtpClient(UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                var result = ftpClient.MoveFile(source, destination);
            }
        }

        public bool DownloadFile(string fromPath, string toPath)
        {
            string relativePath = MAMUtility.GetRelativePath(fromPath);
            relativePath = relativePath.Replace(@"\", @"/");
            var sourceHost = MAMUtility.GetHost(fromPath);
            using (Stream toStream = new FileStream(toPath, FileMode.Create, FileAccess.ReadWrite))
            {
                bool result = false;
                using (FtpClient ftpClient = new FtpClient(FTP + sourceHost, UserId, UserPass))
                {
                    ftpClient.Encoding = Encoding.GetEncoding(EncodingType);

                    if (!ftpClient.FileExists(relativePath))
                        throw new FileNotFoundException();

                    result = ftpClient.Download(toStream, relativePath);
                }
                return result;
            }
        }
        public Stream GetFileStream( string path, long offSet)
        {
            //using ()
            //{
       
            string relativePath = MAMUtility.GetRelativePath(path);
            var sourceHost = MAMUtility.GetHost(path);
            relativePath = relativePath.Replace(@"\", @"/");

            FtpClient ftpClient = new FtpClient(sourceHost, UserId, UserPass);
            ftpClient.Encoding = Encoding.GetEncoding(EncodingType);

            if (!ftpClient.FileExists(relativePath))
                throw new FileNotFoundException();
            var stream = ftpClient.OpenRead(relativePath, offSet);
            return stream;
            //}
        }
        public bool ExistFile(string fromPath)
        {
            bool result = false;
            var sourceHost = MAMUtility.GetHost(fromPath);
            using (FtpClient ftpClient = new FtpClient(FTP + sourceHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);

                string relativePath = MAMUtility.GetRelativePath(fromPath);
                relativePath = relativePath.Replace(@"\", @"/");

                if (!ftpClient.FileExists(relativePath))
                    result = false;
                else
                    result = true;
            }
            return result;
        }

        public void Delete(string filePath)
        {
            using (FtpClient ftpClient = new FtpClient(FTP + UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                string relativePath = MAMUtility.GetRelativePath(filePath);
                relativePath = relativePath.Replace(@"\", @"/");

                if (ftpClient.FileExists(relativePath))
                    ftpClient.DeleteFile(relativePath);
            }
        }
    }
}

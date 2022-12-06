using FluentFTP;
using M30.AudioFile.Common;
using MAMBrowser.Helpers;
using System;
using System.IO;
using System.Text;

namespace MAMBrowser.Foundation
{
    //path : 상대경로
    public class FTPProtocol : IFileProtocol
    {
        private const string FTP = @"ftp://";
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public int EncodingType { get; set; }
        public string UploadHost { get; set; } = "";

        public FTPProtocol(string userId, string userPass, int encodingType)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            UserId = userId;
            UserPass = userPass;
            EncodingType = encodingType;
        }
        public void MakeDirectory(string directoryPath)
        {
            using (FtpClient ftpClient = new FtpClient(UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                ftpClient.CreateDirectory(directoryPath);
            }
        }
        public void Upload(string sourcePath, string targetPath)
        {
            throw new NotImplementedException("구현되지 않음");
        }
        public void Upload(Stream headerStream, Stream fileStream, string sourcePath) 
        {
            using (FtpClient ftpClient = new FtpClient(UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                var result = ftpClient.Upload(headerStream, sourcePath);
                
                if (result != FtpStatus.Success)
                    new Exception($"{result.ToString()}, {headerStream.Length}, {fileStream.Length}, {sourcePath}, {EncodingType}");
            }
            using (FtpClient ftpClient = new FtpClient(UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                var result = ftpClient.Upload(fileStream, sourcePath, FtpRemoteExists.Append);
                if (result != FtpStatus.Success)
                    new Exception($"{result.ToString()}, {headerStream.Length}, {fileStream.Length}, {sourcePath}, {EncodingType}");
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
            string relativePath = CommonUtility.GetRelativePath(fromPath);
            relativePath = relativePath.Replace(@"\", @"/");
            var sourceHost = CommonUtility.GetHost(fromPath);
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
            string relativePath = CommonUtility.GetRelativePath(path);
            var sourceHost = CommonUtility.GetHost(path);
            relativePath = relativePath.Replace(@"\", @"/");

            FtpClient ftpClient = new FtpClient(FTP + sourceHost, UserId, UserPass);
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
            var sourceHost = CommonUtility.GetHost(fromPath);
            using (FtpClient ftpClient = new FtpClient(FTP + sourceHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);

                string relativePath = CommonUtility.GetRelativePath(fromPath);
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
                string relativePath = CommonUtility.GetRelativePath(filePath);
                relativePath = relativePath.Replace(@"\", @"/");

                if (ftpClient.FileExists(relativePath))
                    ftpClient.DeleteFile(relativePath);
            }
        }
        public long GetFileSize(string sourcePath)
        {
            using (FtpClient ftpClient = new FtpClient(UploadHost, UserId, UserPass))
            {
                ftpClient.Encoding = Encoding.GetEncoding(EncodingType);
                return ftpClient.GetFileSize(sourcePath);
            }
        }

        public bool ExistDirectory(string directoryPath)
        {
            throw new NotImplementedException();
        }

        public void Upload(Stream localSourceStream, string targetPath)
        {
            throw new NotImplementedException();
        }

        public void Copy(string sourcePath, string targetPath)
        {
            throw new NotImplementedException();
        }
    }
}

using FluentFTP;
using MAMBrowser.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MAMBrowser.Processor
{
    public class FtpService : IFileService
    {
        public FtpService()
        {
        }
        public string Name { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        public string Host { get; set; }
        public string UserId { get; set; }
        public string UserPass { get; set; }

        public void MakeDirectory(string relativeDirectoryPath)
        {
            using (FtpClient ftpClient = new FtpClient(Host, UserId, UserPass))
            {
                ftpClient.CreateDirectory(relativeDirectoryPath);
            }
            
            //FtpWebRequest ftpRequest = null;
            //Stream ftpStream = null;

            //string[] subDirs = relativeDirectoryPath.Split('/');
            //string currentDir = Host;

            //foreach (string subDir in subDirs)
            //{
            //    try
            //    {
            //        currentDir = $"{currentDir}/{subDir}";
            //        ftpRequest = (FtpWebRequest)FtpWebRequest.Create(currentDir);
            //        ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            //        ftpRequest.UseBinary = true;
            //        ftpRequest.Credentials = new NetworkCredential(UserId, UserPass);
            //        FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
            //        ftpStream = response.GetResponseStream();
            //        ftpStream.Close();
            //        response.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
            //    }
            //}
        }

        public void Upload(Stream fileStream, string relativePath, long fileLength)
        {
            using (FtpClient ftpClient = new FtpClient(Host, UserId, UserPass))
            {
                ftpClient.Upload(fileStream, relativePath);
            }
        }

        public void Move(string source, string destination)
        {
            using (FtpClient ftpClient = new FtpClient(Host, UserId, UserPass))
            {
                ftpClient.MoveFile(source, destination);
            }
        }

        public bool DownloadFile(string fromRelativePath, string toRelativePath)
        {
            Stream returnStream = new FileStream(toRelativePath, FileMode.Create, FileAccess.ReadWrite);
            using (FtpClient ftpClient = new FtpClient(Host, UserId, UserPass))
            {
                if (!ftpClient.FileExists(fromRelativePath))
                    throw new FileNotFoundException();

                return ftpClient.Download(returnStream, fromRelativePath);
            }
        }
        public Stream GetFileStream(string relativePath, long offSet)
        {
            //using ()
            //{
            FtpClient ftpClient = new FtpClient(Host, UserId, UserPass);
            if (!ftpClient.FileExists(relativePath))
                throw new FileNotFoundException();
            var stream = ftpClient.OpenRead(relativePath, offSet);
            return stream;
            //}
        }

        public bool ExistFile(string fromRelativePath)
        {
            using (FtpClient ftpClient = new FtpClient(Host, UserId, UserPass))
            {
                if (!ftpClient.FileExists(fromRelativePath))
                    return false;
                else
                    return true;
            }
        }
    }
}

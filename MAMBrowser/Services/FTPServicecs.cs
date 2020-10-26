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
            FtpWebRequest ftpRequest = null;
            Stream ftpStream = null;

            string[] subDirs = relativeDirectoryPath.Split('/');
            string currentDir = Host;

            foreach (string subDir in subDirs)
            {
                try
                {
                    currentDir = $"{currentDir}/{subDir}";
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(currentDir);
                    ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    ftpRequest.UseBinary = true;
                    ftpRequest.Credentials = new NetworkCredential(UserId, UserPass);
                    FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpStream = response.GetResponseStream();
                    ftpStream.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                }
            }
        }

        public void Upload(Stream fileStream, string relativeSourcePath, long fileLength)
        {
            FtpWebRequest ftpRequest = FtpWebRequest.Create($"{Host}/{relativeSourcePath}") as FtpWebRequest;
            ftpRequest.Credentials = new NetworkCredential(UserId, UserPass);
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpRequest.ContentLength = fileLength;
            using (Stream stream = ftpRequest.GetRequestStream())
            {
                fileStream.CopyTo(stream);
            }
        }

        public void Move(string source, string destination)
        {
            //if (source == destination)
            //    return;

            Uri uriSource = new Uri($"{Host}/{source}", UriKind.Absolute);
            Uri uriDestination = new Uri($"{Host}/{destination}", UriKind.Absolute);
            Uri targetUriRelative = uriSource.MakeRelativeUri(uriDestination);

            //perform rename
            FtpWebRequest ftpRequest = FtpWebRequest.Create(uriSource.AbsoluteUri) as FtpWebRequest;
            ftpRequest.Credentials = new NetworkCredential(UserId, UserPass);
            ftpRequest.Method = WebRequestMethods.Ftp.Rename;
            ftpRequest.RenameTo = Uri.UnescapeDataString(targetUriRelative.OriginalString);

            using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
            {
            }
        }

        public Stream GetDownloadStream(string relativeSourcePath, long offSet)
        {
            FtpWebRequest ftpRequest = FtpWebRequest.Create($"{Host}/{relativeSourcePath}") as FtpWebRequest;
            ftpRequest.Credentials = new NetworkCredential(UserId, UserPass);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpRequest.ContentOffset = offSet;
            return ftpRequest.GetResponse().GetResponseStream();
        }
    }
}

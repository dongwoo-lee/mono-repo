using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public static class MyFtp
    {
        public static void MakeFTPDir(string relativeDirectoryPath)
        {
            FtpWebRequest ftpRequest = null;
            Stream ftpStream = null;

            string[] subDirs = relativeDirectoryPath.Split('/');
            string currentDir = SystemConfig.AppSettings.FtpUri;

            foreach (string subDir in subDirs)
            {
                try
                {
                    currentDir = $"{currentDir}/{subDir}";
                    ftpRequest = (FtpWebRequest)FtpWebRequest.Create(currentDir);
                    ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    ftpRequest.UseBinary = true;
                    ftpRequest.Credentials = new NetworkCredential(SystemConfig.AppSettings.FtpId, SystemConfig.AppSettings.FtpPass);
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

        public static bool Upload(Stream fileStream, string relativeSourcePath, long fileLength)
        {
           

            FtpWebRequest ftpRequest = FtpWebRequest.Create($"{SystemConfig.AppSettings.FtpUri}/{relativeSourcePath}") as FtpWebRequest;
            ftpRequest.Credentials = new NetworkCredential(SystemConfig.AppSettings.FtpId, SystemConfig.AppSettings.FtpPass);
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpRequest.ContentLength = fileLength;
            using (Stream stream = ftpRequest.GetRequestStream())
            {
                fileStream.CopyTo(stream);
            }
            return true;
        }
        public static bool FtpRename(string source, string destination)
        {
            //if (source == destination)
            //    return;

            Uri uriSource = new Uri($"{SystemConfig.AppSettings.FtpUri}/{source}", UriKind.Absolute );
            Uri uriDestination = new Uri($"{SystemConfig.AppSettings.FtpUri}/{destination}", UriKind.Absolute);
            Uri targetUriRelative = uriSource.MakeRelativeUri(uriDestination);

            //perform rename
            FtpWebRequest ftpRequest = FtpWebRequest.Create(uriSource.AbsoluteUri) as FtpWebRequest;
            ftpRequest.Credentials = new NetworkCredential(SystemConfig.AppSettings.FtpId, SystemConfig.AppSettings.FtpPass);
            ftpRequest.Method = WebRequestMethods.Ftp.Rename;
            ftpRequest.RenameTo = Uri.UnescapeDataString(targetUriRelative.OriginalString);

            using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
            {
            }
            return true;
        }
        public static Stream Download(string relativeSourcePath, long offSet)
        {
            FtpWebRequest ftpRequest = FtpWebRequest.Create($"{SystemConfig.AppSettings.FtpUri}/{relativeSourcePath}") as FtpWebRequest;
            ftpRequest.Credentials = new NetworkCredential(SystemConfig.AppSettings.FtpId, SystemConfig.AppSettings.FtpPass);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            //ftpRequest.ContentOffset = offSet;
            return ftpRequest.GetResponse().GetResponseStream();
        }
    }
}

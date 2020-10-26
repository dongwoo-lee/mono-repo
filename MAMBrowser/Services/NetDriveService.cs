using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Processor
{
    public class NetDriveService : IFileService
    {
        public string Name { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public NetDriveService()
        {
        }
        public Stream GetDownloadStream(string relativeSourcePath, long offSet)
        {
            return null;
        }

        public void MakeDirectory(string relativeDirectoryPath)
        {
            throw new NotImplementedException();
        }

        public void Move(string source, string destination)
        {
            throw new NotImplementedException();
        }

        public void Upload(Stream fileStream, string relativeSourcePath, long fileLength)
        {
            throw new NotImplementedException();
        }
    }
}

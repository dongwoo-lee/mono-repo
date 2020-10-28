using MAMBrowser.Helpers;
using MAMBrowser.Processor;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Services
{
    public class MusicService : IFileService
    {
        public string Name { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        public string MbcDomain { get; set; }
        public string AuthorKey { get; set; }

        public MusicService()
        {
        }
        public void SearchSong(string text)
        {

        }
        public void SearchLyrics(string text)
        {

        }
        private string GetAlbumPath(string filePath)
        {
            var wordArray = filePath.Split(Path.DirectorySeparatorChar);
            var albumDomain = $"http://{wordArray[2] }.{MbcDomain}/";
            var relativePath = string.Join("/", albumDomain.Skip(2));
            return albumDomain + relativePath;
        }


        public void MakeDirectory(string relativeDirectoryPath)
        {
            throw new NotImplementedException();
        }

        public void Upload(Stream fileStream, string relativeSourcePath, long fileLength)
        {
            throw new NotImplementedException();
        }

        public void Move(string source, string destination)
        {
            throw new NotImplementedException();
        }

        public Stream GetFileStream(string relativeSourcePath, long offSet)
        {
            throw new NotImplementedException();
        }
        public bool DownloadFile(string fromRelativePath, string toRelativePath)
        {
            return true;
        }

        public bool ExistFile(string fromRelativePath)
        {
            throw new NotImplementedException();
        }
    }
}

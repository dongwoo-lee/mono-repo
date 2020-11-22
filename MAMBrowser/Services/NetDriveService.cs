using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.Extensions.Options;
using NAudio.Wave;
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
            string hostName = MAMUtility.GetDomain(sourcePath);
            using (NetworkShareAccessor.Access(hostName, UserId, UserPass))
            {
                FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return fs;
            }
        }
        public bool DownloadFile(string fromPath, string toPath)
        {
            string sourceHostName = MAMUtility.GetDomain(fromPath);
            
            using (NetworkShareAccessor.Access(sourceHostName, UserId, UserPass))
            {
                //확인필요
                using (FileStream inStream = new FileStream(fromPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var accessor = NetworkShareAccessor.Access(Host, UserId, UserPass);
                    FileStream outStream = new FileStream(toPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    inStream.CopyTo(outStream);
                    if (sourceHostName != Host)  //업로드 호스트와 다운로드호스트가 같으면 인증 매모리 해제를 1번만.
                        accessor.Dispose();

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

        public string GetAudioFormat(string filePath)
        {
            string sourceHostName = MAMUtility.GetDomain(filePath);
            using (NetworkShareAccessor.Access(sourceHostName, UserId, UserPass))
            {
                var ext = Path.GetExtension(filePath);
                if (ext.ToUpper() == ".WAV")
                {
                    WaveFileReader reader = new WaveFileReader(filePath);
                    return $"{reader.WaveFormat.SampleRate}, {reader.WaveFormat.BitsPerSample}, {reader.WaveFormat.Channels}";
                }
                else if (ext.ToUpper() == ".MP2")
                {
                    WaveFileReader reader = new WaveFileReader(filePath);
                    return $"";
                }
                else if (ext.ToUpper() == ".MP3")
                {
                    Mp3FileReader reader = new Mp3FileReader(filePath);
                    return $"";
                }
            }
            return "unknown";
        }
    }
}

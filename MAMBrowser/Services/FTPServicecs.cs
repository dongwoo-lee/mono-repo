using FluentFTP;
using MAMBrowser.Helpers;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using NLayer.NAudioSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MAMBrowser.Processor
{
    //path : 상대경로
    public class FtpService : IFileService
    {
        private const string FTP = @"ftp://";
        public FtpService()
        {
        }
        public string Name { get; set; }
        public string TmpUploadFolder { get; set; }
        public string UploadFolder { get; set; }
        public string Host { get; set; }
        public string UserId { get; set; }
        public string UserPass { get; set; }

        public void MakeDirectory(string directoryPath)
        {
            using (FtpClient ftpClient = new FtpClient(Host, UserId, UserPass))
            {
                ftpClient.CreateDirectory(directoryPath);
            }
        }

        public void Upload(Stream fileStream, string sourcePath, long fileLength)
        {
            using (FtpClient ftpClient = new FtpClient(FTP+Host, UserId, UserPass))
            {
                ftpClient.Upload(fileStream, sourcePath);
            }
        }

        public void Move(string source, string destination)
        {
            using (FtpClient ftpClient = new FtpClient(FTP + Host, UserId, UserPass))
            {
                ftpClient.MoveFile(source, destination);
            }
        }

        public bool DownloadFile(string fromPath, string toPath)
        {
            Stream toStream = new FileStream(toPath, FileMode.Create, FileAccess.ReadWrite);
            using (FtpClient ftpClient = new FtpClient(FTP + Host, UserId, UserPass))
            {
                if (!ftpClient.FileExists(fromPath))
                    throw new FileNotFoundException();

                return ftpClient.Download(toStream, fromPath);
            }
        }
        public Stream GetFileStream(string path, long offSet)
        {
            //using ()
            //{
            FtpClient ftpClient = new FtpClient(FTP + Host, UserId, UserPass);
            if (!ftpClient.FileExists(path))
                throw new FileNotFoundException();
            var stream = ftpClient.OpenRead(path, offSet);
            return stream;
            //}
        }

        public bool ExistFile(string fromPath)
        {
            using (FtpClient ftpClient = new FtpClient(FTP + Host, UserId, UserPass))
            {
                if (!ftpClient.FileExists(fromPath))
                    return false;
                else
                    return true;
            }
        }
        public string GetAudioFormat(string filePath)
        {
            string sourceHostName = MAMUtility.GetDomain(filePath);
            using (FtpClient ftpClient = new FtpClient(FTP + Host, UserId, UserPass))
            {
                var ext = Path.GetExtension(filePath);
                byte[] buffer = new byte[500000];
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var fs = ftpClient.OpenRead(filePath))
                    {
                        var read = fs.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, read);
                    }
                    ms.Position = 0;

                    if (ext.ToUpper() == ".WAV")
                    {
                        WaveFileReader reader = new WaveFileReader(ms);
                        return $"{reader.WaveFormat.SampleRate}, {reader.WaveFormat.BitsPerSample}, {reader.WaveFormat.Channels}";
                    }
                    else if (ext.ToUpper() == ".MP2")
                    {
                        Mp3FileReader reader = new Mp3FileReader(ms, new Mp3FileReader.FrameDecompressorBuilder(waveFormat => new Mp3FrameDecompressor(waveFormat)));
                        var frame = reader.ReadNextFrame();
                        return $"{frame.BitRate / 1000} kbps ({frame.SampleRate},{frame.ChannelMode.ToString()})";
                    }
                    else if (ext.ToUpper() == ".MP3")
                    {
                        Mp3FileReader reader = new Mp3FileReader(ms);
                        var frame = reader.ReadNextFrame();
                        return $"{frame.BitRate / 1000} kbps ({frame.SampleRate},{frame.ChannelMode.ToString()})";
                    }
                }
            }
            return "unknown";
        }

        public void Delete(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}

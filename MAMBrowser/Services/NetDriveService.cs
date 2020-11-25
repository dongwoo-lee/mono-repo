using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using NLayer.NAudioSupport;
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
                    using (FileStream outStream = new FileStream(toPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        inStream.CopyTo(outStream);
                        return true;
                    }
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
            string sourceHostName = MAMUtility.GetDomain(fromPath);

            using (NetworkShareAccessor.Access(sourceHostName, UserId, UserPass))
            {
                return File.Exists(fromPath);
            }
        }

        public string GetAudioFormat(string filePath)
        {
            string sourceHostName = MAMUtility.GetDomain(filePath);
            using (NetworkShareAccessor.Access(sourceHostName, UserId, UserPass))
            {
                var ext = Path.GetExtension(filePath);
                //wav 44byte
                //mp3 4byte
                byte[] buffer = new byte[500000];
                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var read = fs.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, read);
                    }
                    ms.Position = 0;

                    if (ext.ToUpper() == MAMUtility.WAV)
                    {
                        WaveFileReader reader = new WaveFileReader(ms);
                        return $"{reader.WaveFormat.SampleRate}, {reader.WaveFormat.BitsPerSample}, {reader.WaveFormat.Channels}";
                    }
                    else if (ext.ToUpper() == MAMUtility.MP2)
                    {
                        Mp3FileReader reader = new Mp3FileReader(ms, new Mp3FileReader.FrameDecompressorBuilder(waveFormat => new Mp3FrameDecompressor(waveFormat)));
                        var frame = reader.ReadNextFrame();
                        return $"{frame.BitRate / 1000} kbps ({frame.SampleRate},{frame.ChannelMode.ToString()})";
                    }
                    else if (ext.ToUpper() == MAMUtility.MP3)
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

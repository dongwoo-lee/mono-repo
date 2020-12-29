using MAMBrowser.DAL;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class PrivateFileBLL
    {
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly PrivateFileDAL _dal;

        public PrivateFileBLL(IOptions<AppSettings> appSesstings, ServiceResolver sr, PrivateFileDAL dal)
        {
            _appSettings = appSesstings.Value;
            _fileService = sr("PrivateWorkConnection");
            _dal = dal;
        }

        public long UploadFile(string userId, Stream stream, string fileName,PrivateFileModel metaData)
        {
            long ID = _dal.GetID();
            string date = DateTime.Now.ToString(MAMUtility.DTM8);
            string newFileName = $"{ ID.ToString() }_{fileName}";
            var relativeSourceFolder = $"{_fileService.TmpUploadFolder}";
            var relativeTargetFolder = @$"{_fileService.UploadFolder}\{userId}\{userId}-{date}";
            var relativeSourcePath = @$"{relativeSourceFolder}\{newFileName}";
            var relativeTargetPath = @$"{relativeTargetFolder}\{newFileName}";

            var headerStream = AudioEngine.GetHeaderStream(stream);
            headerStream.Position = 0;
            var audioFormat = AudioEngine.GetAudioFormat(headerStream, relativeTargetPath);
            headerStream.Position = 0;
            _fileService.MakeDirectory(relativeSourceFolder);
            _fileService.Upload(headerStream, stream, relativeSourcePath, stream.Length);
            _fileService.MakeDirectory(relativeTargetFolder);
            _fileService.Move(relativeSourcePath, relativeTargetPath);
            stream.Dispose();
            headerStream.Dispose();


            metaData.SEQ = ID;
            metaData.AUDIO_FORMAT = audioFormat;
            metaData.FILE_PATH = @$"\\{_fileService.UploadHost}\{relativeTargetPath}";
            _dal.Insert(userId, metaData);
            return ID;
        }
    }
}

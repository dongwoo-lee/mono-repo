using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Foundation;
using M30.AudioFile.DAL.Dto;
using M30_CueSheetDAO.Interfaces;
using MAMBrowser.DTO;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using MAMBrowser.Hubs;
using MAMBrowser.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudioFadeInOutTest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAMBrowser.BLL
{
    public class CueAttachmentsBll
    {
        private readonly ProductsBll _bll;
        private readonly IFileProtocol _fileService;
        private readonly WebServerFileHelper _fileHelper;
        private readonly APIBll _apiBll;
        private readonly ICueSheetDAO _dao;
        private readonly IHubContext<ProgressHub> _hubContext;

        private static bool isStopMerge = false;

        public CueAttachmentsBll(ServiceResolver sr, ProductsBll bll, APIBll apiBll, WebServerFileHelper fileHelper, ICueSheetDAO dao,IHubContext<ProgressHub> hubContext)
        {
            _fileService = sr(MAMDefine.MirosConnection).FileSystem;
            _bll = bll;
            _apiBll = apiBll;
            _fileHelper = fileHelper;
            _dao = dao;
            _hubContext = hubContext;
        }

        //CueCon > ZipFile 내려받기
        public ActionResult<string> ExportToZipFile(string userid, List<CueSheetConDTO> pram)
        {
            ActionResult<string> result;

            var guid = Guid.NewGuid().ToString();
            var rootFolder = Path.Combine(Startup.AppSetting.TempExportPath, @$"{userid}\{guid}");
            string xmlFileName = $"MetaData.xml";
            string jsonFileName = $"MetaData.json";
            string xmlFileFullPath = Path.Combine(rootFolder, xmlFileName);
            string jsonFileFullPath = Path.Combine(rootFolder, jsonFileName);

            DirectoryInfo di_folder = new DirectoryInfo(rootFolder);
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(rootFolder));
            if (!di_folder.Exists)
            {
                di_folder.Create();
            }

            foreach (CueSheetConDTO ele in pram)
            {
                if (ele.FILEPATH != null && ele.FILEPATH != "")
                {
                    var outFilePath = Path.Combine(rootFolder, Path.GetFileName(ele.FILEPATH));
                    var audioData = new CueSheetConAudioDTO();
                    ele.AUDIOS = new List<CueSheetConAudioDTO>();

                    using (FileStream outFileStream = new FileStream(outFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        try
                        {
                            using (var inStream = _fileService.GetFileStream(ele.FILEPATH, 0))
                            {
                                inStream.CopyTo(outFileStream);

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            ele.FILEPATH = "";
                        }
                    }
                    ele.FILEPATH = outFilePath;
                    audioData.P_TYPE = ele.CARTTYPE;
                    audioData.P_SEQNUM = 1;
                    audioData.P_CLIPID = ele.CARTID;
                    audioData.P_MAINTITLE = ele.MAINTITLE;
                    audioData.P_SUBTITLE = ele.SUBTITLE;
                    audioData.P_DURATION = ele.DURATION;
                    audioData.P_MASTERFILE = outFilePath;
                    ele.AUDIOS.Add(audioData);
                }
                else
                {
                    // 그룹소재 아이템 가져오기
                    if (ele.ONAIRDATE != "")
                    {
                        ele.AUDIOS = new List<CueSheetConAudioDTO>();
                        if (ele.CARTTYPE == "SB")
                        {
                            var collection = _bll.FindSBContents(ele.ONAIRDATE, ele.CARTID);
                            if (collection.Data.Any())
                            {
                                foreach (var item in collection.Data)
                                {
                                    var result_dto = new CueSheetConAudioDTO();
                                    result_dto.P_SEQNUM = item.RowNO;
                                    result_dto.P_CLIPID = item.ID;
                                    result_dto.P_MAINTITLE = item.Name;
                                    result_dto.P_DURATION = item.IntDuration;
                                    result_dto.P_MASTERFILE = item.FilePath;
                                    result_dto.P_CODEID = item.PgmCODE ?? "";
                                    ele.AUDIOS.Add(result_dto);
                                }

                            }
                        }
                        if (ele.CARTTYPE == "CM")
                        {
                            var collection = _bll.FindCMContents(ele.ONAIRDATE, ele.CARTID);
                            if (collection.Data.Any())
                            {
                                foreach (var item in collection.Data)
                                {
                                    var result_dto = new CueSheetConAudioDTO();
                                    result_dto.P_SEQNUM = item.RowNO;
                                    result_dto.P_CLIPID = item.ID;
                                    result_dto.P_MAINTITLE = item.Name;
                                    result_dto.P_DURATION = item.IntDuration;
                                    result_dto.P_MASTERFILE = item.FilePath;
                                    result_dto.P_CODEID = item.PgmCODE ?? "";
                                    ele.AUDIOS.Add(result_dto);
                                }

                            }
                        }
                    }
                    if (ele.AUDIOS == null)
                    {
                        var audioData = new CueSheetConAudioDTO();
                        ele.AUDIOS = new List<CueSheetConAudioDTO>();
                        audioData.P_TYPE = ele.CARTTYPE;
                        audioData.P_SEQNUM = 1;
                        audioData.P_CLIPID = ele.CARTID;
                        audioData.P_MAINTITLE = ele.MAINTITLE;
                        audioData.P_SUBTITLE = ele.SUBTITLE;
                        audioData.P_DURATION = ele.DURATION;
                        audioData.P_MASTERFILE = "";
                        ele.AUDIOS.Add(audioData);
                    }
                    else
                    {
                        foreach (var item in ele.AUDIOS)
                        {
                            var outFilePath = Path.Combine(rootFolder, Path.GetFileName(item.P_MASTERFILE));

                            using (FileStream outFileStream = new FileStream(outFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                try
                                {
                                    using (var inStream = _fileService.GetFileStream(item.P_MASTERFILE, 0))
                                    {
                                        inStream.CopyTo(outFileStream);
                                        item.P_MASTERFILE = outFilePath;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    item.P_MASTERFILE = "";
                                }
                            }
                        }
                    }
                }


            }
            using (var FileStream = new StreamWriter(xmlFileFullPath))
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<CueSheetConDTO>));
                serialiser.Serialize(FileStream, pram);

            }

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(jsonFileFullPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, pram);
            }
            string zipFileName = $"{guid}.zip";
            var zipFilePath = Path.Combine(Path.GetDirectoryName(rootFolder), zipFileName);

            ZipFileManager.Instance.CreateZIPFile(rootFolder, zipFilePath);
            di_folder.Delete(true);
            result = Path.Combine(Path.GetDirectoryName(rootFolder), zipFileName);
            return result;
        }

        //CueCon > Wave 파일 하나로 합쳐 내려받기
        public async Task<DTO_RESULT<ActionResult<string>>> MergeAudioFilesIntoOneWav(string userid, string connectionId, List<CueSheetConDTO> pram,CancellationToken token)
        {
            DTO_RESULT<ActionResult<string>> result = new DTO_RESULT<ActionResult<string>>();
            try
            {
                var playlist = new List<AudioFileReader>();
                var guid = Guid.NewGuid().ToString();
                var rootFolder = Path.Combine(Startup.AppSetting.TempExportPath, @$"{userid}\{guid}");
                string wavFileName = $"{guid}.wav";
                var resultPath = Path.Combine(Path.GetDirectoryName(rootFolder), wavFileName);

                DirectoryInfo di_folder = new DirectoryInfo(rootFolder);
                DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(rootFolder));

                if (!di_folder.Exists)
                {
                    di_folder.Create();
                }

                int pramIndex = 0;
                foreach (CueSheetConDTO ele in pram)
                {
                    if (ele.FILEPATH != null && ele.FILEPATH != "")
                    {
                        var fileGuid = Guid.NewGuid().ToString();
                        var outFilePath = Path.Combine(rootFolder, fileGuid + Path.GetFileName(ele.FILEPATH));
                        _fileHelper.TrimWavFile(ele.FILEPATH, outFilePath, TimeSpan.FromMilliseconds(ele.STARTPOSITION), TimeSpan.FromMilliseconds(ele.ENDPOSITION));
                        var fileItem = new AudioFileReader(outFilePath);
                        if (ele.FADEINTIME || ele.FADEOUTTIME)
                        {
                            if (ele.FADEINTIME)
                            {
                                var fade = new DelayFadeOutSampleProvider(fileItem);
                                var fadeInPath = Path.Combine(Path.GetDirectoryName(outFilePath), "_fadeIn_" + Path.GetFileName(outFilePath));
                                fade.BeginFadeIn(2000);
                                WaveFileWriter.CreateWaveFile16(fadeInPath, fade);
                                fileItem.Close();
                                fileItem = new AudioFileReader(fadeInPath);
                            }
                            if (ele.FADEOUTTIME)
                            {
                                var fade = new DelayFadeOutSampleProvider(fileItem);
                                var fadeOutPath = Path.Combine(Path.GetDirectoryName(outFilePath), "_fadeOut_" + Path.GetFileName(outFilePath));
                                fade.BeginFadeOut(ele.ENDPOSITION - ele.STARTPOSITION - 2000, 2000);
                                WaveFileWriter.CreateWaveFile16(fadeOutPath, fade);
                                fileItem.Close();
                                fileItem = new AudioFileReader(fadeOutPath);
                            }
                        }
                        playlist.Add(fileItem);
                    }
                    else
                    {
                        if (ele.ONAIRDATE != "")
                        {
                            ele.AUDIOS = new List<CueSheetConAudioDTO>();
                            if (ele.CARTTYPE == "SB")
                            {
                                var collection = _bll.FindSBContents(ele.ONAIRDATE, ele.CARTID);
                                if (collection.Data.Any())
                                {
                                    foreach (var item in collection.Data)
                                    {
                                        var fileItem = new AudioFileReader(item.FilePath);
                                        playlist.Add(fileItem);
                                    }
                                }
                            }
                            if (ele.CARTTYPE == "CM")
                            {
                                var collection = _bll.FindCMContents(ele.ONAIRDATE, ele.CARTID);
                                if (collection.Data.Any())
                                {
                                    foreach (var item in collection.Data)
                                    {
                                        var fileItem = new AudioFileReader(item.FilePath);
                                        playlist.Add(fileItem);
                                    }
                                }
                            }
                        }
                        if (ele.AUDIOS != null)
                        {
                            foreach (var item in ele.AUDIOS)
                            {
                                var fileItem = new AudioFileReader(item.P_MASTERFILE);
                                playlist.Add(fileItem);
                            }
                        }
                    }
                    await _hubContext.Clients.Client(connectionId).SendAsync("sendProgress", Convert.ToInt32((pramIndex + 1) * 50 / pram.Count));
                    if (token.IsCancellationRequested)
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync("sendProgress", 0);
                        token.ThrowIfCancellationRequested();
                    }
                    pramIndex++;
                }
                var resultPlaylist = new ConcatenatingSampleProvider(playlist).ToWaveProvider16();
                var totalPlayTime = new TimeSpan();
                foreach (var p_item in playlist)
                {
                    totalPlayTime = totalPlayTime + p_item.TotalTime;
                }
                int write_index = 0;

                using (WaveFileWriter writer = new WaveFileWriter(resultPath, resultPlaylist.WaveFormat))
                {
                    var buffer = new byte[resultPlaylist.WaveFormat.AverageBytesPerSecond * 4];
                    var totalIndex = Math.Ceiling(totalPlayTime.TotalSeconds / 4);
                    while (true)
                    {
                        int bytesRead = resultPlaylist.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            // end of source provider
                            break;
                        }
                        writer.Write(buffer, 0, bytesRead);
                        await _hubContext.Clients.Client(connectionId).SendAsync("sendProgress", Convert.ToInt32((write_index + 1) * 50 / totalIndex) + 50);
                        if (token.IsCancellationRequested)
                        {
                            await _hubContext.Clients.Client(connectionId).SendAsync("sendProgress", 0);
                            token.ThrowIfCancellationRequested();
                        }
                        write_index++;
                    }
                }

                foreach (var item in playlist)
                {
                    item.Close();
                }

                di_folder.Delete(true);
                result.ResultObject = resultPath;
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            // 중단
            catch (OperationCanceledException ex)
            {
                //파일, 폴더 삭제
                // 중단 문구 보내야 함
                //if (di_folder.Exists)
                //{
                //    di_folder.Create();
                //}
                result.ResultCode = RESUlT_CODES.SUCCESS;
                result.ResultObject = "중단되었습니다";
                return result;
            }
            catch(Exception ex)
            {
                //파일, 폴더 삭제
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        //attachments 목록 가져오기
        public List<AttachmentDTO> GetAttachmentDTOs(int cueid)
        {
            return _dao.GetAttachmentEntities(cueid)?.Converting();
        }

        //fileid 가져오기
        public long GetAttachmentsFileId()
        {
            return _dao.GetAttachmentsFileid();
        }

        //attachments -> Temp 업로드
        public string UploadToTemp(IFormFile file, string chunkMetadata, string folder_date)
        {
            var result = "";

            if (!string.IsNullOrEmpty(chunkMetadata))
            {
                var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);
                string[] extensionRuls = { ".txt", ".docx", "xlsx", "pdf", "hwp" };
                long chunkMaxSize = 104857600;
                _fileHelper.CheckFileExtensionValid(extensionRuls, metaDataObject.FileName);
                var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                //var tempPath = option.Find(dt => dt.Name == "MAM_UPLOAD_PATH").Value.ToString();
                //var tempFilePath = Path.Combine(tempPath, GetTempFileName(metaDataObject));
                var tempPath = @"\\ad2022-nas\AUDIO-FILE\mbcdata\CUESHEET_TEMP_UPLOAD"; //임시 경로
                var path = Path.Combine(tempPath, folder_date);
                var filePath = Path.Combine(path, _fileHelper.GetTempFileName(metaDataObject));
                var host = CommonUtility.GetHost(path);
                var userinfo = GetStorageUserInfo(option);
                NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //청크 파일 어펜드
                _fileHelper.AppendContentToFile(filePath, file, chunkMaxSize);

                if (metaDataObject.index == (metaDataObject.TotalCount - 1))
                {
                    //token 생성
                    //result = TokenGenerator.GenerateFileToken(filePath);
                    result = filePath;
                }
            }
            return result;
        }

        //Temp -> Storage Move
        public AttachmentDTO MoveToStorage(AttachmentDTO file, bool copyVal)
        {
            var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
            var path = @"\\ad2022-nas\AUDIO-FILE\mbcdata\CUESHEET_ATTACHMENTS"; //임시 경로
            var storagePath = Path.Combine(path, file.FILEID + "_" + Path.GetFileName(file.FILENAME));
            //var filePath = Path.Combine(path, _fileHelper.GetTempFileName(metaDataObject));
            var host = CommonUtility.GetHost(storagePath);
            var userinfo = GetStorageUserInfo(option);
            NetworkShareAccessor.Access(host, userinfo["id"], userinfo["pass"]);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (copyVal)
            {
                File.Copy(file.FILEPATH, storagePath);
            }
            else
            {
                File.Move(file.FILEPATH, storagePath);
                var folder = new DirectoryInfo(Path.GetDirectoryName(file.FILEPATH));
                if (folder.GetFileSystemInfos().Length == 0)
                {
                    Directory.Delete(Path.GetDirectoryName(file.FILEPATH));
                }
            }

            file.FILEPATH = storagePath;
            return file;
        }

        //attachments 다운로드
        public DTO_RESULT AttachmentsFileValidation(string token)
        {
            //api controller 부분 긁어온 것 맞게 바꿔야 함
            DTO_RESULT result = new DTO_RESULT();

            try
            {
                string filePath = "";
                if (TokenGenerator.ValidateFileToken(token, ref filePath))
                {
                    if (string.IsNullOrEmpty(filePath))
                    {
                        result.ResultCode = RESUlT_CODES.FILE_NOT_FOUND;
                        result.ErrorMsg = "등록된 파일이 없습니다.";
                        return result;
                    }

                    var option = _apiBll.GetOptions(Define.MASTERING_OPTION_GRPCODE).ToList();
                    var userInfo = GetStorageUserInfo(option);
                    var hostName = CommonUtility.GetHost(filePath);
                    NetworkShareAccessor.Access(hostName, userInfo["id"], userInfo["pass"]);
                    if (!System.IO.File.Exists(filePath))
                    {
                        result.ResultCode = RESUlT_CODES.FILE_NOT_FOUND;
                        result.ErrorMsg = "스토리지에 파일이 없습니다.";
                        return result;
                    }
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.FILE_NOT_FOUND;
                    result.ErrorMsg = "등록된 파일이 없습니다.";
                    return result;
                }

                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        //attachments 삭제 
        public void DeleteAttachmentsFile(AttachmentDTO file)
        {
            if (File.Exists(file.FILEPATH))
            {
                File.Delete(file.FILEPATH);
            }
        }

        //추후에 M30_COMM_OPTIONS에 cuesheet 부분 추가되면 수정하기 
        Dictionary<string, string> GetStorageUserInfo(IList<Dto_MasteringOptions> option)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("id", option.ToList().Find(dt => dt.Name == "STORAGE_ID").Value.ToString());
            dictionary.Add("pass", option.ToList().Find(dt => dt.Name == "STORAGE_PASS").Value.ToString());
            return dictionary;
        }
    }
}

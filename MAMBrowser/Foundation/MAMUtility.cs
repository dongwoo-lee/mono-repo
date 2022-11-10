using M30.AudioFile.Common;
using M30.AudioFile.Common.Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MAMBrowser.Foundation
{
    public static class MAMUtility
    {
        public static void TempDownloadToLocal(string userId, string remoteIp, IFileDownloadProtocol fileProtocol, string filePath)
        {
            MAMUtility.ClearTempFolder(Startup.AppSetting.TempDownloadPath, userId, remoteIp);
            var targetFolder = MAMUtility.GetTempFolder(Startup.AppSetting.TempDownloadPath, userId, remoteIp);
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var soundFileName = Path.GetFileName(filePath);
            var ext = Path.GetExtension(filePath).ToUpper();
            var targetSoundPath = MAMUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, soundFileName);
            if (fileProtocol.ExistFile(filePath))
            {
                fileProtocol.DownloadFile(filePath, targetSoundPath);
                //다운로드된 mp2는 wav로 변환함.(mp3는 상관없음)
                if (ext == Define.MP2)
                {
                    var convertFilePath = MAMUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, soundFileName.ToUpper().Replace(ext, Define.WAV));
                    using (FileStream inStream = new FileStream(targetSoundPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (FileStream outStream = new FileStream(convertFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            MAMAudioEngine.ConvertMp2ToWav(inStream, outStream);
                        }
                    }
                }
            }
            //else
            //{
            //    if(ext != Define.EGY)   //음원파일 다운로드 실패시.. 파형파일은 없어도 다시만드므로 상관없음.
            //        throw new Exception("등록된 파일을 찾을 수 없습니다.");
            //}
        }
        
        public static List<float> GetWaveformCore(string soundFilePath)
        {
            var soundfileName = Path.GetFileName(soundFilePath);
            var egyFileName = Path.GetFileNameWithoutExtension(soundFilePath) + Define.EGY;
            var egyFilePath = soundFilePath.Replace(soundfileName, egyFileName);
            if (File.Exists(egyFilePath))
            {
                using (var stream = new FileStream(egyFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    return MAMAudioEngine.GetDecibelFromEgy(stream);
                }
            }
            else
            {
                using (var stream = new FileStream(soundFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    var ext = Path.GetExtension(soundFilePath).ToUpper();
                    switch (ext)
                    {
                        case Define.WAV:
                            return MAMAudioEngine.GetDecibelFromWav(stream, 2);
                        //case MP2:       파형검색시 mp2파일은 wav로 치환됨.
                        //    break;
                        case Define.MP3:
                            return MAMAudioEngine.GetDecibelFromMp3(stream, 2);
                        default:
                            return new List<float>();
                    }
                }
            }
        }
       
        public static void InitTempFoler(string userId, string remoteIp)
        {
            MAMUtility.ClearTempFolder(Startup.AppSetting.TempDownloadPath, userId, remoteIp);
            var targetFolder = MAMUtility.GetTempFolder(Startup.AppSetting.TempDownloadPath, userId, remoteIp);
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);
        }


        public static string NetworkName(string startNetwork, string endNetwork, IPAddress clientIp)
        {
            var stNetwork = IPAddress.Parse(startNetwork);
            var edNetwork = IPAddress.Parse(endNetwork);
            if (IsInRange(stNetwork, edNetwork, clientIp))
                return "방송망";
            else
                return "사내망";
        }
        public static bool IsInRange(IPAddress stIp, IPAddress edIp, IPAddress checkIp)
        {
            if (stIp.AddressFamily != checkIp.AddressFamily)
                return false;

            var lowerBytes = stIp.GetAddressBytes();
            var upperBytes = edIp.GetAddressBytes();

            byte[] checkBytes = checkIp.GetAddressBytes();
            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < lowerBytes.Length && (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && checkBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && checkBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (checkBytes[i] == lowerBytes[i]);
                upperBoundary &= (checkBytes[i] == upperBytes[i]);
            }

            return true;
        }
        public static string GetTempFilePath(string tempDownloadRoot, string userId, string remoteIp, string fileName)
        {
            return $@"{GetTempFolder(tempDownloadRoot, userId, remoteIp)}\{fileName}";
        }
        public static string GetTempFolder(string tempDownloadRoot, string userId, string remoteIp)
        {
            if (string.IsNullOrEmpty(remoteIp) || remoteIp == "::1" || remoteIp == "127.0.0.1")
            {
                remoteIp = "localhost";
            }
            return $@"{tempDownloadRoot}\{userId}_{remoteIp}";
        }
        public static void ClearTempFolder(string tempDownloadRoot, string userId, string remoteIp)
        {
            var targetFolder = GetTempFolder(tempDownloadRoot, userId, remoteIp);
            if (Directory.Exists(targetFolder))
            {
                DateTime now = DateTime.Now;
                var fileFullPathList = Directory.GetFiles(targetFolder);
                foreach (var filePath in fileFullPathList)
                {
                    try
                    {
                        var createdDtm = File.GetLastAccessTime(filePath);

                        if (now.Subtract(createdDtm).TotalSeconds > 300)
                            File.Delete(filePath);
                    }
                    catch (IOException ex)
                    {
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
    }
}

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
            CommonUtility.ClearTempFolder(Startup.AppSetting.TempDownloadPath, userId, remoteIp);
            var targetFolder = CommonUtility.GetTempFolder(Startup.AppSetting.TempDownloadPath, userId, remoteIp);
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var soundFileName = Path.GetFileName(filePath);
            var ext = Path.GetExtension(filePath).ToUpper();
            var targetSoundPath = CommonUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, soundFileName);
            if (fileProtocol.ExistFile(filePath))
            {
                fileProtocol.DownloadFile(filePath, targetSoundPath);
                //다운로드된 mp2는 wav로 변환함.(mp3는 상관없음)
                if (ext == Define.MP2)
                {
                    var convertFilePath = CommonUtility.GetTempFilePath(Startup.AppSetting.TempDownloadPath, userId, remoteIp, soundFileName.ToUpper().Replace(ext, Define.WAV));
                    using (FileStream inStream = new FileStream(targetSoundPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (FileStream outStream = new FileStream(convertFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            AudioEngine.ConvertMp2ToWav(inStream, outStream);
                        }
                    }
                }
            }
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
                    return AudioEngine.GetDecibelFromEgy(stream);
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
                            return AudioEngine.GetDecibelFromWav(stream, 2);
                        //case MP2:       파형검색시 mp2파일은 wav로 치환됨.
                        //    break;
                        case Define.MP3:
                            return AudioEngine.GetDecibelFromMp3(stream, 2);
                        default:
                            return new List<float>();
                    }
                }
            }
        }
       
        public static void InitTempFoler(string userId, string remoteIp)
        {
            CommonUtility.ClearTempFolder(Startup.AppSetting.TempDownloadPath, userId, remoteIp);
            var targetFolder = CommonUtility.GetTempFolder(Startup.AppSetting.TempDownloadPath, userId, remoteIp);
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
    }
}

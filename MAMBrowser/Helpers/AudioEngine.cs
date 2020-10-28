using MAMBrowser.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class AudioEngine
    {
        private const int peekValuesBufferSize = 15000;
        public enum BitDepths : byte
        {
            NRJ_RAW_8BITS = 1,
            NRJ_RAW_16BITS = 2,
            NRJ_RAW_24BITS = 3
        }
        public enum Channels : byte
        {
            MONO = 1,
            STEREO = 2
        }
        public enum Resolution : short
        {
            MP2 = 1152,
            PCM = 1152
        }

        public static List<float> GetPeekValuesFromEgy(Stream stream)
        {
            //16비트버퍼
            //int bufferSize = 16384;
            //버퍼
           
            List<float> peekValues = new List<float>();
            byte[] buffer;

            buffer = new byte[32];
            stream.Read(buffer, 0, 32);
            var strTitle = Encoding.ASCII.GetString(buffer, 0, 32);

            buffer = new byte[512];
            stream.Read(buffer, 0, 512);
            var fileName = Encoding.ASCII.GetString(buffer, 0, 512);    //확장자포함

            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            var blockNumber = BitConverter.ToInt32(buffer, 0);


            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            var waveformType = Encoding.ASCII.GetString(buffer, 0, 4);


            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            var blockBodySize = BitConverter.ToInt32(buffer, 0);
            //------------------------------------------------------이 밑이 BockBody의 실데이터. (블럭바디에서 라인 4바이트 +플럭바디사이즈 4바이트 제외)

            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            var channel = BitConverter.ToInt32(buffer, 0);


            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            var codingSize = BitConverter.ToInt32(buffer, 0);


            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            //BitConverter.ToInt16
            var sampleNumber = BitConverter.ToInt32(buffer, 0);

            buffer = new byte[peekValuesBufferSize];
            //codingSize = 1;
            while (true)
            {
                var readCount = stream.Read(buffer, 0, buffer.Length);
                if (readCount > 0)
                {
                    for (int i = 0; i < readCount; i += 2 * codingSize)
                    {
                        var left = GetVolume(buffer, codingSize,i);
                        var right = GetVolume(buffer, codingSize, i+ codingSize);
                        peekValues.Add(left);
                        peekValues.Add(-right);
                    }
                    buffer = new byte[peekValuesBufferSize];
                }
                else
                    break;
            }
            return peekValues;
        }
        public static List<float> GetPeekValuesFromWav(Stream stream, int codingSize)
        {
            List<float> peekValues = new List<float>();
            byte[] buffer = new byte[peekValuesBufferSize];
            while (true)
            {
                int leftMax = 0;
                int rightMax = 0;
                var readCount = stream.Read(buffer, 0, buffer.Length);
                if (readCount > 0)
                {
                    for (int i = 0; i < readCount; i += 2 * codingSize)
                    {
                        int left = Math.Abs(GetValue(buffer, codingSize, i));
                        int right = Math.Abs(GetValue(buffer, codingSize, i + codingSize));
                        if (leftMax < left)
                            leftMax = left;
                        if (rightMax < right)
                            rightMax = right;
                        
                    }
                    peekValues.Add(GetVolume(leftMax, codingSize));
                    peekValues.Add(-GetVolume(rightMax, codingSize));
                    buffer = new byte[peekValuesBufferSize];
                }
                else
                    break;
            }
            return peekValues;
        }
        private static int GetValue(byte[] data, int codingSize, int offset)
        {
            switch (codingSize)
            {
                case 1:
                    return (int)data[offset];
                case 2:
                    return (int)BitConverter.ToInt16(data, offset);
                case 3:
                    return 0;
                default:
                    return 0;
            }
        }
        private static float GetVolume(byte[] data, int codingSize, int offset)
        {
            switch (codingSize)
            {
                case 1:
                    return (float)data[offset] / sbyte.MaxValue;
                case 2:
                    return (float)BitConverter.ToInt16(data, offset) / Int16.MaxValue;
                case 3:
                    return 0;
                default:
                    return 0;
            }
        }
        private static float GetVolume(int value, int codingSize)
        {
            switch (codingSize)
            {
                case 1:
                    return (float)value / sbyte.MaxValue;
                case 2:
                    return (float)value / Int16.MaxValue;
                case 3:
                    return 0;
                default:
                    return 0;
            }
        }
       
        //public static List<float> CreateEgyFile(Stream inStream, Stream outStream, string soundFileName, BitDepths bitDepth, Resolution resolution, Channels channel)
        //{
        //    const string energyFile = "ENERGY FILE";
        //    const int blockNo = 1;  //int32 라서 4바이트 배열로 자동반환됨.
        //    const string waveFormType = "line";

        //    byte[] buffer;

        //    buffer = new byte[32];
        //    var strTitle = Encoding.ASCII.GetBytes(energyFile);
        //    Array.Copy(strTitle, 0, buffer, 0, strTitle.Length);
        //    outStream.Write(buffer, 0, buffer.Length);

        //    buffer = new byte[512];
        //    var fileName = Encoding.ASCII.GetBytes(soundFileName);
        //    Array.Copy(strTitle, 0, buffer, 0, fileName.Length);
        //    outStream.Write(buffer, 0, buffer.Length);

        //    var blockNumber = BitConverter.GetBytes(blockNo);  // 1고정. int32 라서 4바이트 배열로 자동반환됨.
        //    outStream.Write(blockNumber, 0, blockNumber.Length);

        //    buffer = new byte[4];
        //    var waveformType = Encoding.ASCII.GetBytes(waveFormType);  //line 고정
        //    Array.Copy(waveformType, 0, buffer, 0, waveformType.Length);
        //    outStream.Write(buffer, 0, 4);

        //    var blockBodySize = BitConverter.GetBytes((int)(4+4)+1);    //(1==energy data)        // length - (32+512+4+4+4)
        //    outStream.Write(blockBodySize, 0, blockBodySize.Length);
        //    //------------------------------------------------------이 밑이 BockBody의 실데이터. (블럭바디에서 라인 4바이트 +플럭바디사이즈 4바이트 제외)


        //    var channelData = BitConverter.GetBytes((int)channel);          //채널 2
        //    outStream.Write(channelData, 0, channelData.Length);

        //    var codingSize = BitConverter.GetBytes((int)bitDepth);       //1 
        //    outStream.Write(codingSize, 0, codingSize.Length);

        //    var sampleNumber = BitConverter.GetBytes((int)resolution);     //1152고정
        //    outStream.Write(sampleNumber, 0, sampleNumber.Length);

        //    buffer = new byte[peekValuesBufferSize];

        //    while (true)
        //    {
        //        var readCount = outStream.Read(buffer, 0, buffer.Length);
        //        if (readCount > 0)
        //        {
        //            for (int i = 0; i < buffer.Length; i += 2)
        //            {
        //                var left = buffer[i] / (float)128;
        //                var right = buffer[i + 1] / (float)128;
        //                peekValues.Add(left);
        //                peekValues.Add(-right);

        //            }
        //            buffer = new byte[1024];
        //        }
        //        else
        //            break;
        //    }

        //    return peekValues;
        //}

        //private static int GetEnergDataSize()
        //{
        //}
    }
}

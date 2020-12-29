using MAMBrowser.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.CompilerServices;
using NAudio.Lame;
using NAudio.Utils;
using NAudio.Wave;
using NLayer;
using NLayer.NAudioSupport;
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
        private const int _silence = -42;
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

        public static List<float> GetVolumeFromEgy(Stream stream)
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
        public static List<float> GetVolumeFromWav(Stream stream, int codingSize)
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
        public static List<float> GetDecibelFromEgy(Stream stream)
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
                        var left = GetDecibelPercent(GetDecibel(buffer, codingSize, i));
                        var right = GetDecibelPercent(GetDecibel(buffer, codingSize, i + codingSize));
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
        public static List<float> GetDecibelFromWav(Stream stream, int codingSize)
        {
            WaveFileReader reader = new WaveFileReader(stream);
            int count = 1152 * 2 * 2; //wav일때만...
            List<float> peekValues = new List<float>();
            byte[] buffer = new byte[count];
            while (true)
            {
                int leftMax = 0;
                int rightMax = 0;
                var readCount = reader.Read(buffer, 0, buffer.Length);
                if (readCount >= count)
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
                    peekValues.Add(GetDecibelPercent(GetDecibel(leftMax, codingSize)));
                    peekValues.Add(-GetDecibelPercent(GetDecibel(rightMax, codingSize)));
                    buffer = new byte[count];
                }
                else
                    break;
            }
            return peekValues;
        }
        public static List<float> GetDecibelFromMp2(Stream stream, int codingSize)
        {
            Mp3FileReader reader = new Mp3FileReader(stream, new Mp3FileReader.FrameDecompressorBuilder(waveFormat => new Mp3FrameDecompressor(waveFormat)));
            int count = 1152 * 2 * 2; //wav일때만...
            List<float> peekValues = new List<float>();
            byte[] buffer = new byte[count];
            while (true)
            {
                int leftMax = 0;
                int rightMax = 0;
                var readCount = reader.Read(buffer, 0, buffer.Length);
                if (readCount >= count)
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
                    peekValues.Add(GetDecibelPercent(GetDecibel(leftMax, codingSize)));
                    peekValues.Add(-GetDecibelPercent(GetDecibel(rightMax, codingSize)));
                    buffer = new byte[count];
                }
                else
                    break;
            }
            return peekValues;
        }
        public static List<float> GetDecibelFromMp3(Stream stream, int codingSize)
        {
            Mp3FileReader reader = new Mp3FileReader(stream);

            int count = 1152 * 2 * 2; //wav일때만...
            List<float> peekValues = new List<float>();
            byte[] buffer = new byte[count];
            while (true)
            {
                int leftMax = 0;
                int rightMax = 0;
                var readCount = reader.Read(buffer, 0, buffer.Length);
                if (readCount >= count)
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
                    peekValues.Add(GetDecibelPercent(GetDecibel(leftMax, codingSize)));
                    peekValues.Add(-GetDecibelPercent(GetDecibel(rightMax, codingSize)));
                    buffer = new byte[count];
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
        private static double GetDecibel(byte[] data, int codingSize, int offset)
        {
            double value;
            switch (codingSize)
            {
                case 1:
                    value = ((double)data[offset] / sbyte.MaxValue);
                    if (value < 0)
                        return -72;
                    else
                        return Decibels.LinearToDecibels(value);
                case 2:
                    value = (double)BitConverter.ToInt16(data, offset) / Int16.MaxValue;
                    if (value < 0)
                        return -72;
                    else
                        return  Decibels.LinearToDecibels(value);
                case 3:
                    return 0;
                default:
                    return 0;
            }
        }
        private static double GetDecibel(int value, int codingSize)
        {
            switch (codingSize)
            {
                case 1:
                    return Decibels.LinearToDecibels((double)value / sbyte.MaxValue);
                case 2:
                    return Decibels.LinearToDecibels((double)value / Int16.MaxValue);
                case 3:
                    return 0;
                default:
                    return 0;
            }
        }
        private static float GetDecibelPercent(double decibelValue)
        {
            if (decibelValue <= _silence)
                return 0;
            else
                return 1 - (float)(Math.Abs(decibelValue) / Math.Abs(_silence));
        }




        public static void ConvertMp2ToWav(Stream mp2Stream, Stream outWavStream)
        {
            var wf = new WaveFormat(44100, 16, 2);
            var mpegFile = new MpegFile(mp2Stream);
            float[] buffer = new float[wf.SampleRate * 100];

            using (WaveFileWriter writer = new WaveFileWriter(outWavStream, wf))
            {
                while (true)
                {
                    var read = mpegFile.ReadSamples(buffer, 0, buffer.Length);

                    if (read <= 0)
                        break;
                    else
                    {
                        WriteSamples(buffer, 0, read, writer, wf);
                    }
                }
                writer.Flush();
            }
        }
        public static void ConvertMp2ToWav(Stream mp2Stream, WaveFileWriter outWavStream)   //dispose없이...연결해서 씀..
        {
            var wf = new WaveFormat(44100, 16, 2);
            var mpegFile = new MpegFile(mp2Stream);
            float[] buffer = new float[wf.SampleRate * 100];

            while (true)
            {
                var read = mpegFile.ReadSamples(buffer, 0, buffer.Length);

                if (read <= 0)
                    break;
                else
                {
                    WriteSamples(buffer, 0, read, outWavStream, wf);
                }
            }
            outWavStream.Flush();
        }
        

        public static void ConvertMp2ToMp3(Stream mp2Stream, Stream outWavStream)
        {
            var wf = new WaveFormat(44100, 16, 2);
            var mpegFile = new MpegFile(mp2Stream);
            float[] buffer = new float[wf.SampleRate * 100];

            using (LameMP3FileWriter writer = new LameMP3FileWriter(outWavStream, wf, LAMEPreset.ABR_320))
            {
                while (true)
                {
                    var read = mpegFile.ReadSamples(buffer, 0, buffer.Length);

                    if (read <= 0)
                        break;
                    else
                    {
                        WriteSamples(buffer, 0, read, writer, wf);
                    }
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// Writes 32 bit floating point samples to the Wave file
        /// They will be converted to the appropriate bit depth depending on the WaveFormat of the WAV file
        /// </summary>
        /// <param name="samples">The buffer containing the floating point samples</param>
        /// <param name="offset">The offset from which to start writing</param>
        /// <param name="count">The number of floating point samples to write</param>
        public static void WriteSamples(float[] samples, int offset, int count, Stream writer, WaveFormat wf)
        {
            var byteStep = (wf.BitsPerSample / 8);
            var totalByteLenth = byteStep * count;
            byte[] totalBuffer = new byte[totalByteLenth]; // keep this around to save us creating it every time

            byte[] value24 = new byte[3]; // keep this around to save us creating it every time
            byte[] byteData;
            int currentBytePos = 0;
            for (int n = 0; n < count; n++)
            {
                if (wf.BitsPerSample == 16)
                {
                    byteData = BitConverter.GetBytes((Int16)Get16Value(samples[offset + n]));
                    totalBuffer[currentBytePos] = byteData[0];
                    totalBuffer[currentBytePos+1] = byteData[1];
                }
                else if (wf.BitsPerSample == 24)
                {
                    Get24Value(samples[offset + n], ref value24);
                    byteData = value24;
                    totalBuffer[currentBytePos] = byteData[0];
                    totalBuffer[currentBytePos+1] = byteData[1];
                    totalBuffer[currentBytePos+2] = byteData[2];
                }
                else if (wf.BitsPerSample == 32 && wf.Encoding == WaveFormatEncoding.Extensible)
                {
                    byteData = BitConverter.GetBytes((Int32)Get32Value(samples[offset + n]));
                    totalBuffer[currentBytePos] = byteData[0];
                    totalBuffer[currentBytePos + 1] = byteData[1];
                    totalBuffer[currentBytePos + 2] = byteData[2];
                    totalBuffer[currentBytePos + 3] = byteData[3];
                }
                else if (wf.Encoding == WaveFormatEncoding.IeeeFloat)
                {
                    byteData = BitConverter.GetBytes((float)Get32ValueIeeeFloat(samples[offset + n]));
                    totalBuffer[currentBytePos] = byteData[0];
                    totalBuffer[currentBytePos + 1] = byteData[1];
                    totalBuffer[currentBytePos + 2] = byteData[2];
                    totalBuffer[currentBytePos + 3] = byteData[3];
                }
                else
                {
                    throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
                }
                currentBytePos += byteStep;
            }
            writer.Write(totalBuffer, 0, totalBuffer.Length);
        }

        /// <summary>
        /// Writes a single sample to the Wave file
        /// </summary>
        /// <param name="sample">the sample to write (assumed floating point with 1.0f as max value)</param>
        public static Int16 Get16Value(float sample)
        {
            return (Int16)(Int16.MaxValue * sample);
        }
        public static byte[] Get24Value(float sample, ref byte[] value24)
        {
            var value = BitConverter.GetBytes((Int32)(Int32.MaxValue * sample));
            value24[0] = value[1];
            value24[1] = value[2];
            value24[2] = value[3];
            return value24;
        }
        public static Int32 Get32Value(float sample)
        {
            return UInt16.MaxValue * (Int32)sample;
        }
        public static float Get32ValueIeeeFloat(float sample)
        {
            return sample;
        }


        public static string GetAudioFormat(MemoryStream memoryStream, string fileName)
        {
            var ext = Path.GetExtension(fileName);
           
            if (ext.ToUpper() == MAMUtility.WAV)
            {
                WaveFileReader reader = new WaveFileReader(memoryStream);
                return $"{reader.WaveFormat.SampleRate}, {reader.WaveFormat.BitsPerSample}, {reader.WaveFormat.Channels}";
            }
            else if (ext.ToUpper() == MAMUtility.MP2)
            {
                Mp3FileReader reader = new Mp3FileReader(memoryStream, new Mp3FileReader.FrameDecompressorBuilder(waveFormat => new Mp3FrameDecompressor(waveFormat)));
                var frame = reader.ReadNextFrame();
                return $"{frame.BitRate / 1000} kbps ({frame.SampleRate},{frame.ChannelMode.ToString()})";
            }
            else if (ext.ToUpper() == MAMUtility.MP3)
            {
                Mp3FileReader reader = new Mp3FileReader(memoryStream);
                var frame = reader.ReadNextFrame();
                return $"{frame.BitRate / 1000} kbps ({frame.SampleRate},{frame.ChannelMode.ToString()})";
            }
           
            return "unknown";
        }
        public static MemoryStream GetHeaderStream(Stream stream)
        {
            int stepLength = 100000;
            int totalHeadLength = stepLength * 5;
            int totalRead = 0;
            
            byte[] buffer = new byte[stepLength];
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                if (totalRead >= totalHeadLength)
                    break;

                var read = stream.Read(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, read);
                totalRead += read;
            }
            ms.Flush();
            return ms;
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

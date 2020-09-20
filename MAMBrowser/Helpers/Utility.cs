using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public static class Utility
    {
        public const string DTM8 = "yyyyMMdd";
        public const string DTM10 = "yyyy-MM-dd";
        public const string DTM19 = "yyyy-MM-dd HH:mm:ss";


        public static List<float> GetPeekValues(Stream stream)
        {
            List<float> peekValues = new List<float>();
            byte[] buffer;

            buffer = new byte[1024];
            stream.Read(buffer, 0, 32);
            var strTitle = Encoding.ASCII.GetString(buffer, 0, 32);

            buffer = new byte[1024];
            stream.Read(buffer, 0, 512);
            var fileName = Encoding.ASCII.GetString(buffer, 0, 512);

            buffer = new byte[1024];
            stream.Read(buffer, 0, 4);
            var blockNumber = BitConverter.ToInt32(buffer, 0);


            buffer = new byte[1024];
            stream.Read(buffer, 0, 4);
            var waveformType = Encoding.ASCII.GetString(buffer, 0, 4);


            buffer = new byte[1024];
            stream.Read(buffer, 0, 4);
            var blockBodySize = BitConverter.ToInt32(buffer, 0);


            buffer = new byte[1024];
            stream.Read(buffer, 0, 4);
            var channel = BitConverter.ToInt32(buffer, 0);


            buffer = new byte[1024];
            stream.Read(buffer, 0, 4);
            var codingSize = BitConverter.ToInt32(buffer, 0);


            buffer = new byte[1024];
            stream.Read(buffer, 0, 4);
            //BitConverter.ToInt16
            var sampleNumber = BitConverter.ToInt32(buffer, 0);

            buffer = new byte[1024];

            while (true)
            {
                int j = 0;
                var readCount = stream.Read(buffer, 0, buffer.Length);
                if (readCount > 0)
                {
                    for (int i = 0; i < buffer.Length; i += 2)
                    {
                        var left = buffer[i] / (float)128;
                        var right = buffer[i + 1] / (float)128;
                        peekValues.Add(left);
                        peekValues.Add(-right);

                    }
                    buffer = new byte[1024];
                }
                else
                    break;
            }

            return peekValues;
        }
    }
}

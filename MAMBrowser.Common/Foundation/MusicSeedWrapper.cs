using System;
using System.Text;
using System.Web;

namespace MAMBrowser.Common
{
    public static class MusicSeedWrapper
    {
        public static string SeedEncrypt(string data)
        {
            byte[] pbUserKey = { (byte)0x7e, (byte)0x7f, (byte)0x45, (byte)0x85, (byte)0x12, (byte)0x0d, (byte)0x6f, (byte)0xe7, (byte)0xdf, (byte)0xe9, (byte)0x8a, (byte)0x2d, (byte)0x14, (byte)0xca, (byte)0x0d, (byte)0x7f };
            byte[] bszIV =
            {
                (byte)0x01c, (byte)0x083, (byte)0x05c, (byte)0x09d,
                (byte)0x02b, (byte)0x09e, (byte)0x010, (byte)0x077,
                (byte)0x065, (byte)0x0b0, (byte)0x0cf, (byte)0x0f0,
                (byte)0x02c, (byte)0x00c, (byte)0x01b, (byte)0x00b
            };
            //Encoding euckr = Encoding.GetEncoding("euc-kr");
            var encodingCode = 51949;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding euckr = Encoding.GetEncoding("euc-kr");
            var encodingString = euckr.GetBytes(data);
            var seedData = KISA_SEED_CBC.SEED_CBC_Encrypt(pbUserKey, bszIV, encodingString, 0, encodingString.Length);
            var base64Data = Convert.ToBase64String(seedData);
            var utf8UrlEncodingData = HttpUtility.UrlEncode(base64Data);
            return utf8UrlEncodingData;
        }
        public static string SeedDecrypt(string data)
        {
            byte[] pbUserKey = { (byte)0x7e, (byte)0x7f, (byte)0x45, (byte)0x85, (byte)0x12, (byte)0x0d, (byte)0x6f, (byte)0xe7, (byte)0xdf, (byte)0xe9, (byte)0x8a, (byte)0x2d, (byte)0x14, (byte)0xca, (byte)0x0d, (byte)0x7f };
            byte[] bszIV =
            {
                (byte)0x01c, (byte)0x083, (byte)0x05c, (byte)0x09d,
                (byte)0x02b, (byte)0x09e, (byte)0x010, (byte)0x077,
                (byte)0x065, (byte)0x0b0, (byte)0x0cf, (byte)0x0f0,
                (byte)0x02c, (byte)0x00c, (byte)0x01b, (byte)0x00b
            };

            var decodeString = HttpUtility.UrlDecode(data);
            var seedData = Convert.FromBase64String(decodeString);
            var decrypByteArray = KISA_SEED_CBC.SEED_CBC_Decrypt(pbUserKey, bszIV, seedData, 0, seedData.Length);
            Encoding euckr = Encoding.GetEncoding("euc-kr");
            var strData = euckr.GetString(decrypByteArray);
            return strData;
        }
    }
}

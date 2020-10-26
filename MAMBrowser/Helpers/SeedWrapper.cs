using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class SeedWrapper
    {
        byte[] pbUserKey = { (byte)0x7e, (byte)0x7f, (byte)0x45, (byte)0x85, (byte)0x12, (byte)0x0d, (byte)0x6f, (byte)0xe7, (byte)0xdf, (byte)0xe9, (byte)0x8a, (byte)0x2d, (byte)0x14, (byte)0xca, (byte)0x0d, (byte)0x7f };
        byte[] bszIV =
        {
                (byte)0x01c, (byte)0x083, (byte)0x05c, (byte)0x09d,
                (byte)0x02b, (byte)0x09e, (byte)0x010, (byte)0x077,
                (byte)0x065, (byte)0x0b0, (byte)0x0cf, (byte)0x0f0,
                (byte)0x02c, (byte)0x00c, (byte)0x01b, (byte)0x00b
            };
        public void Encrypt(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var encryptedData = KISA_SEED_CBC.SEED_CBC_Encrypt(pbUserKey, bszIV, bytes, 0, bytes.Length);
            var based64Data = Convert.ToBase64String(encryptedData);
        }
        public void Decrypt()
        {
            return;
        }
    }
}

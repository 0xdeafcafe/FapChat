using System.IO;
using System.Security.Cryptography;
using System.Text;
using FapChat.Core.Helpers;

namespace FapChat.Core.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    public static class Aes
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] DecryptData(byte[] data, string key)
        {
            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = new UTF8Encoding().GetBytes(key);
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, new byte[] { 0x00 });
                using (var msDecrypt = new MemoryStream(data))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        return DataHelpers.ReadFully(csDecrypt);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] EncryptData(byte[] data, string key)
        {
            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = new UTF8Encoding().GetBytes(key);
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, new byte[] { 0x00 });
                using (var msEncrypt = new MemoryStream())
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        return DataHelpers.ReadFully(csEncrypt);
            }
        }
    }
}

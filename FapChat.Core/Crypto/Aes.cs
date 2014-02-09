using System.IO;
using System.Security.Cryptography;
using System.Text;
using FapChat.Core.Helpers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace FapChat.Core.Crypto
{
	/// <summary>
	/// </summary>
	public static class Aes
	{
		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] DecryptData(byte[] data, string key)
		{
			byte[] encKey = Encoding.UTF8.GetBytes(key);

			// AES algorthim with ECB cipher & PKCS5 padding...
			IBufferedCipher cipher = CipherUtilities.GetCipher("AES/ECB/PKCS5Padding");

			// Initialise the cipher...
			cipher.Init(false, new KeyParameter(encKey));

			// Decrypt the data and write the 'final' byte stream...
			byte[] decryptionbytes = cipher.ProcessBytes(data);
			byte[] decryptedfinal = cipher.DoFinal();

			// Write the decrypt bytes & final to memory...
			var decryptedstream = new MemoryStream(decryptionbytes.Length);
			decryptedstream.Write(decryptionbytes, 0, decryptionbytes.Length);
			decryptedstream.Write(decryptedfinal, 0, decryptedfinal.Length);
			decryptedstream.Flush();

			var decryptedData = new byte[decryptedstream.Length];
			decryptedstream.Position = 0;
			decryptedstream.Read(decryptedData, 0, (int) decryptedstream.Length);

			return decryptedData;
		}

		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] EncryptData(byte[] data, string key)
		{
			using (var aesAlg = new AesManaged())
			{
				aesAlg.Key = new UTF8Encoding().GetBytes(key);
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, new byte[] {0x00});
				using (var msEncrypt = new MemoryStream())
				using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					return DataHelpers.ReadFully(csEncrypt);
			}
		}
	}
}
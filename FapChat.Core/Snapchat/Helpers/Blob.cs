using FapChat.Core.Crypto;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Core.Snapchat.Helpers
{
	/// <summary>
	/// </summary>
	public static class Blob
	{
		/// <summary>
		///     Checks if a blob is a valid media blob
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool ValidateMediaBlob(byte[] data)
		{
			if (data == null)
				return false;

			if (data.Length < 2)
				return false;

			return (data[0] == 0xFF && data[1] == 0xD8) ||
			       (data[0] == 0x00 && data[1] == 0x00);
		}

		/// <summary>
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool CheckMediaIsCached(string id, MediaType mediaType)
		{
			string filePath = IsolatedStorage.GetFileNameTypeFromMediaType(id, mediaType);
			return IsolatedStorage.FileExists(filePath);
		}

		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] DecryptBlob(byte[] data)
		{
			try
			{
				data = Aes.DecryptData(data, KeyVault.BlobEncryptionKey);
				return ValidateMediaBlob(data) ? data : null;
			}
			catch
			{
				return null;
			}
		}
	}
}
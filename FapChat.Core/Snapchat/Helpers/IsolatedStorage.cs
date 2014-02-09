using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Core.Snapchat.Helpers
{
	/// <summary>
	/// </summary>
	public static class IsolatedStorage
	{
		/// <summary>
		/// </summary>
		/// <param name="mediaType"></param>
		/// <returns></returns>
		public static string GetFileNameTypeFromMediaType(string id, MediaType mediaType)
		{
			string appendedExtension;
			switch (mediaType)
			{
				case MediaType.Image:
				case MediaType.FriendRequestImage:
					appendedExtension = "image.jpg";
					break;

				case MediaType.FriendRequestVideo:
				case MediaType.Video:
					appendedExtension = "video.mp4";
					break;

				default:
					appendedExtension = "";
					break;
			}

			return string.Format("{0}.{1}", id, appendedExtension);
		}

		/// <summary>
		/// </summary>
		/// <param name="path"></param>
		/// <param name="data"></param>
		public static void SaveFile(string path, byte[] data)
		{
			using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (isoStore.FileExists(path))
					isoStore.DeleteFile(path);

				using (var fileStream = new IsolatedStorageFileStream(path, FileMode.Create, isoStore))
				{
					using (var writer = new BinaryWriter(fileStream))
					{
						var resourceStream = new MemoryStream(data);
						long length = resourceStream.Length;
						var buffer = new byte[32];
						int readCount = 0;
						using (var reader = new BinaryReader(resourceStream))
						{
							while (readCount < length)
							{
								int actual = reader.Read(buffer, 0, buffer.Length);
								readCount += actual;
								writer.Write(buffer, 0, actual);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool FileExists(string path)
		{
			using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
				return isoStore.FileExists(path);
		}

		/// <summary>
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static BitmapImage RetrieveImageFile(string path)
		{
			using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (!isoStore.FileExists(path))
					return null;

				using (IsolatedStorageFileStream fileStream = isoStore.OpenFile(path, FileMode.Open, FileAccess.Read))
				{
					var bitmapImage = new BitmapImage();
					bitmapImage.SetSource(fileStream);
					return bitmapImage;
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static IsolatedStorageFileStream RetrieveVideoFile(string path)
		{
			using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
				return !isoStore.FileExists(path) ? null : isoStore.OpenFile(path, FileMode.Open, FileAccess.Read);
		}
	}
}
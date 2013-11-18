using System.IO;
using System.IO.IsolatedStorage;
using FapChat.Core.Helpers;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Core.Snapchat.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class IsolatedStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static string GetFileNameTypeFromMediaType(MediaType mediaType)
        {
            switch (mediaType)
            {
                case MediaType.Image:
                case MediaType.FriendRequestImage:
                    return "image.jpg";

                case MediaType.FriendRequestVideo:
                case MediaType.Video:
                    return "video.mp4";

                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void SaveFile(string path, byte[] data)
        {
            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.FileExists(path))
                    isoStore.DeleteFile(path);

                using (var fileStream = isoStore.CreateFile(path))
                    fileStream.WriteAsync(data, 0, data.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetFile(string path)
        {
            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isoStore.FileExists(path))
                    return null;

                using (var fileStream = isoStore.OpenFile(path, FileMode.Open))
                    return DataHelpers.ReadFully(fileStream);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using FapChat.Core.Helpers;

namespace FapChat.Core.Snapchat.Models
{
    public class CachedMediaBlob : INotifyPropertyChanged
    {
        /// <summary>
        /// The Snap Id, that the Media Blob is from
        /// </summary>
        public String Id
        {
            get { return _id; }
            set { SetField(ref _id, value, "Id"); }
        }
        private String _id;

        /// <summary>
        /// 
        /// </summary>
        public MediaType BlobMediaType
        {
            get { return _blobMediaType; }
            set { SetField(ref _blobMediaType, value, "BlobMediaType"); }
        }
        private MediaType _blobMediaType;

        /// <summary>
        /// 
        /// </summary>
        public Byte[] LocalFileBytes
        {
            get
            {
                var filePath = string.Format("{0}.{1}", Id, GeFriendlyTypeFromMediaType(_blobMediaType));

                using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isoStore.FileExists(filePath))
                        return null;

                    using (var fileStream = isoStore.OpenFile(filePath, FileMode.Open))
                        return DataHelpers.ReadFully(fileStream);
                }
            }
            set
            {
                var filePath = string.Format("{0}.{1}", Id, GeFriendlyTypeFromMediaType(_blobMediaType));

                using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isoStore.FileExists(filePath))
                        isoStore.DeleteFile(filePath);

                    using (var fileStream = isoStore.CreateFile(filePath))
                        new MemoryStream(value).CopyToAsync(fileStream);
                }
            }
        }

        #region Boilerplate

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region Helpers

        private static string GeFriendlyTypeFromMediaType(MediaType mediaType)
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

        #endregion
    }
}

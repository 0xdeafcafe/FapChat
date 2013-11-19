using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using FapChat.Core.Snapchat.Helpers;

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
        /// The Media Type of the Blob
        /// </summary>
        public MediaType BlobMediaType
        {
            get { return _blobMediaType; }
            set { SetField(ref _blobMediaType, value, "BlobMediaType"); }
        }
        private MediaType _blobMediaType;

        /// <summary>
        /// Gets the Data of the Blob from Isolated Storage
        /// </summary>
        /// <returns></returns>
        public object RetrieveBlobData()
        {
            var filePath = IsolatedStorage.GetFileNameTypeFromMediaType(Id, _blobMediaType);
            switch (_blobMediaType)
            {
                case MediaType.Image:
                case MediaType.FriendRequestImage:
                    return IsolatedStorage.RetrieveImageFile(filePath);

                case MediaType.VideoNoAudio:
                case MediaType.Video:
                case MediaType.FriendRequestVideoNoAudio:
                case MediaType.FriendRequestVideo:
                    return IsolatedStorage.RetrieveVideoFile(filePath);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Writes the Blob to Isolated Storage
        /// </summary>
        /// <param name="data"></param>
        public void SetLocalFileBytes(Byte[] data)
        {
            var filePath = IsolatedStorage.GetFileNameTypeFromMediaType(Id, _blobMediaType);
            IsolatedStorage.SaveFile(filePath, data);
            //new MediaLibrary().SavePicture(filePath, data);
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
    }
}

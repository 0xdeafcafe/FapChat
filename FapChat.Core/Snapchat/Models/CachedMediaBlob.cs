using System;
using System.Collections.Generic;
using System.ComponentModel;
using FapChat.Core.Snapchat.Helpers;
using Microsoft.Xna.Framework.Media;

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
        /// 
        /// </summary>
        public Byte[] LocalFileBytes
        {
            get
            {
                var filePath = string.Format("{0}.{1}", Id, IsolatedStorage.GetFileNameTypeFromMediaType(_blobMediaType));
                return IsolatedStorage.GetFile(filePath);
            }
            set
            {
                var filePath = string.Format("{0}.{1}", Id, IsolatedStorage.GetFileNameTypeFromMediaType(_blobMediaType));
                IsolatedStorage.SaveFile(filePath, value);
                new MediaLibrary().SavePicture(filePath, value);
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    /// <summary>
    /// The Different Types of Friend Status
    /// </summary>
    public enum FriendType
    {
        Accepted = 0x00,
        PendingAccept = 0x01,
        Blocked = 0x02
    }

    /// <summary>
    /// A Friend on Snapchat
    /// </summary>
    public class Friend : INotifyPropertyChanged
    {
        /// <summary>
        /// Shows if the Friend allows you to see custom stories
        /// </summary>
        [JsonPropertyAttribute("can_see_custom_stories")] 
        public Boolean CanSeeCustomStories
        {
            get { return _canSeeCustomStories; }
            set { SetField(ref _canSeeCustomStories, value, "CanSeeCustomStories"); }
        }
        private Boolean _canSeeCustomStories;

        /// <summary>
        /// Shows the Name of the Friend
        /// </summary>
        [JsonPropertyAttribute("name")] 
        public String Name
        {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }
        private String _name;

        /// <summary>
        /// The Custom Name for the Friend
        /// </summary>
        [JsonPropertyAttribute("display")] 
        public String Display
        {
            get { return _display; }
            set { SetField(ref _display, value, "Display"); }
        }
        private String _display;

        /// <summary>
        /// Shows if the Friend is Pending, Blocked or not a Creep
        /// </summary>
        [JsonPropertyAttribute("type")] 
        public FriendType Type
        {
            get { return _type; }
            set { SetField(ref _type, value, "Type"); }
        }
        private FriendType _type;

        #region Boilerplate

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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

    /// <summary>
    /// A friendship that both parties have accepted.
    /// </summary>
    public class AddedFriend : INotifyPropertyChanged
    {
        /// <summary>
        /// Timestamp of when the Friendship started
        /// </summary>
        [JsonPropertyAttribute("ts")]
        public Int64 Ts
        {
            get { return _ts; }
            set { SetField(ref _ts, value, "Ts"); }
        }
        private Int64 _ts;

        /// <summary>
        /// Shows the Name of the Friend
        /// </summary>
        [JsonPropertyAttribute("name")]
        public String Name
        {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }
        private String _name;

        /// <summary>
        /// The Custom Name for the Friend
        /// </summary>
        [JsonPropertyAttribute("display")]
        public String Display
        {
            get { return _display; }
            set { SetField(ref _display, value, "Display"); }
        }
        private String _display;

        /// <summary>
        /// Shows if the Friend is Pending, Blocked or not a Creep
        /// </summary>
        [JsonPropertyAttribute("type")]
        public FriendType Type
        {
            get { return _type; }
            set { SetField(ref _type, value, "Type"); }
        }
        private FriendType _type;

        #region Boilerplate

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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

    /// <summary>
    /// 
    /// </summary>
    public class FriendAction : Base, INotifyPropertyChanged
    {
        /// <summary>
        /// Object Data (probally the Friend Model)
        /// </summary>
        [JsonPropertyAttribute("object")]
        public Friend Object
        {
            get { return _object; }
            set { SetField(ref _object, value, "Object"); }
        }
        private Friend _object;

        #region Boilerplate

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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

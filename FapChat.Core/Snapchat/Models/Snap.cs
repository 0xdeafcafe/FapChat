﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    /// <summary>
    /// Possible Status's of the Snap.
    /// </summary>
    public enum SnapStatus
    {
        None = -1,
        Sent = 0x00,
        Delivered = 0x01,
        Opened = 0x02,
        Screenshotted = 0x03
    }

    /// <summary>
    /// Possible Media Type's of the Snap.
    /// </summary>
    public enum MediaType
    {
        Image = 0x00,
        Video = 0x01,
        VideoNoAudio = 0x02,
        FriendRequest = 0x03,
        FriendRequestImage = 0x04,
        FriendRequestVideo = 0x05,
        FriendRequestVideoNoAudio = 0x06
    }

	/// <summary>
	/// 
	/// </summary>
    public class Snap : INotifyPropertyChanged
    {
	    /// <summary>
	    /// The Id of the Snap that was sent.
	    /// </summary>
	    [JsonPropertyAttribute("id")]
	    public String Id
	    {
            get { return _id; }
            set { SetField(ref _id, value, "Id"); }
	    }
	    private String _id;

		/// <summary>
		/// The recipient of the Snap.
        /// </summary>
        [JsonPropertyAttribute("rp")]
        public String Recipient
        {
            get { return _recipient; }
            set { SetField(ref _recipient, value, "Recipient"); }
        }
        private String _recipient;

        /// <summary>
        /// The username of the sender of the Snap.
        /// </summary>
        [JsonPropertyAttribute("sn")]
        public String ScreenName
        {
            get { return _screenName; }
            set { SetField(ref _screenName, value, "ScreenName"); }
        }
        private String _screenName;

		/// <summary>
		/// The timestamp that the Snap was sent? Seems to be two of these
        /// </summary>
        [JsonPropertyAttribute("ts")]
        public Int64 Timestamp
        {
            get { return _timestamp; }
            set { SetField(ref _timestamp, value, "Timestamp"); }
        }
        private Int64 _timestamp;

	    /// <summary>
	    /// The Status of the Snap.
	    /// </summary>
	    [JsonPropertyAttribute("st")]
	    public SnapStatus Status
	    {
	        get { return _status; }
            set { SetField(ref _status, value, "Status"); }
	    }
	    private SnapStatus _status;

	    /// <summary>
	    /// The time the Snap was sent.
	    /// </summary>
	    [JsonPropertyAttribute("sts")]
	    public Int64 SentTimestamp
	    {
	        get { return _sentTimestamp; }
            set { SetField(ref _sentTimestamp, value, "SentTimestamp"); }
	    }
	    private Int64 _sentTimestamp;

	    /// <summary>
	    /// The Media ID of the Snap. Format ([sender]~[guid][recipient])
	    /// </summary>
	    [JsonPropertyAttribute("c_id")]
	    public String MediaId
	    {
	        get { return _mediaId; }
            set { SetField(ref _mediaId, value, "MediaId"); }
	    }
	    private String _mediaId;

	    /// <summary>
	    /// The Media Type of the Snap.
	    /// </summary>
	    [JsonPropertyAttribute("m")]
	    public MediaType MediaType
	    {
	        get { return _mediaType; }
            set { SetField(ref _mediaType, value, "MediaType"); }
	    }
	    private MediaType _mediaType;

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

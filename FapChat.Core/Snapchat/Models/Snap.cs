using System;
using System.Collections.Generic;
using System.ComponentModel;
using FapChat.Core.Snapchat.Helpers;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
	/// <summary>
	///     Possible Status's of the Snap.
	/// </summary>
	public enum SnapStatus
	{
		None = -1,
		Sent = 0x00,
		Delivered = 0x01,
		Opened = 0x02,
		Screenshotted = 0x03,

		// My Swagga Shit
		Downloading = 0xDEAD
	}

	/// <summary>
	///     Possible Media Type's of the Snap.
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
	///     The Possible State's of the Snap
	/// </summary>
	public enum SnapState
	{
		NotApplicable,
		Available,
		Expired
	}

	/// <summary>
	/// </summary>
	public class Snap : INotifyPropertyChanged
	{
		private String _captionText;
		private Double _capturePosition;
		private Int32? _captureTime;
		private String _id;
		private String _mediaId;
		private MediaType _mediaType;
		private DateTime? _openedAt;

		private String _recipientName;
		private int? _remainingSeconds;

		private String _screenName;
		private Int64 _sentTimestamp;
		private SnapStatus _status;

		private Int64 _timestamp;

		/// <summary>
		///     The Id of the Snap that was sent.
		/// </summary>
		[JsonProperty("id")]
		public String Id
		{
			get { return _id; }
			set { SetField(ref _id, value, "Id"); }
		}

		/// <summary>
		///     The recipient of the Snap.
		/// </summary>
		[JsonProperty("rp")]
		public String RecipientName
		{
			get { return _recipientName; }
			set { SetField(ref _recipientName, value, "RecipientName"); }
		}

		/// <summary>
		///     The username of the sender of the Snap.
		/// </summary>
		[JsonProperty("sn")]
		public String ScreenName
		{
			get { return _screenName; }
			set { SetField(ref _screenName, value, "ScreenName"); }
		}

		/// <summary>
		///     The timestamp that the Snap was sent? Seems to be two of these
		/// </summary>
		[JsonProperty("ts")]
		public Int64 Timestamp
		{
			get { return _timestamp; }
			set { SetField(ref _timestamp, value, "Timestamp"); }
		}

		/// <summary>
		///     The Status of the Snap.
		/// </summary>
		[JsonProperty("st")]
		public SnapStatus Status
		{
			get { return _status; }
			set { SetField(ref _status, value, "Status"); }
		}

		/// <summary>
		///     The time the Snap was sent.
		/// </summary>
		[JsonProperty("sts")]
		public Int64 SentTimestamp
		{
			get { return _sentTimestamp; }
			set { SetField(ref _sentTimestamp, value, "SentTimestamp"); }
		}

		/// <summary>
		///     The Media ID of the Snap. Format ([sender]~[guid][recipient])
		/// </summary>
		[JsonProperty("c_id")]
		public String MediaId
		{
			get { return _mediaId; }
			set { SetField(ref _mediaId, value, "MediaId"); }
		}

		/// <summary>
		///     The Media Type of the Snap.
		/// </summary>
		[JsonProperty("m")]
		public MediaType MediaType
		{
			get { return _mediaType; }
			set { SetField(ref _mediaType, value, "MediaType"); }
		}

		/// <summary>
		///     The Caption Text to display on the snap
		/// </summary>
		[JsonProperty("cap_text")]
		public String CaptionText
		{
			get { return _captionText; }
			set { SetField(ref _captionText, value, "CaptionText"); }
		}

		/// <summary>
		///     The position of the screen to display the snap (percentage)
		/// </summary>
		[JsonProperty("cap_pos")]
		public Double CapturePosition
		{
			get { return _capturePosition; }
			set { SetField(ref _capturePosition, value, "CapturePosition"); }
		}

		/// <summary>
		///     The length of the, in seconds, to display the snap
		/// </summary>
		[JsonProperty("t")]
		public Int32? CaptureTime
		{
			get { return _captureTime ?? 10; }
			set { SetField(ref _captureTime, value ?? 10, "CaptureTime"); }
		}

		/// <summary>
		///     Stuff
		/// </summary>
		[JsonProperty("oat")]
		public DateTime? OpenedAt
		{
			get { return _openedAt; }
			set { SetField(ref _openedAt, value, "OpenedAt"); }
		}

		/// <summary>
		///     Stuff
		/// </summary>
		[JsonProperty("rsecs")]
		public int? RemainingSeconds
		{
			get { return _remainingSeconds; }
			set { SetField(ref _remainingSeconds, value, "RemainingSeconds"); }
		}

		/// <summary>
		///     Specifies if the Media has been cached locally
		/// </summary>
		public Boolean HasMedia
		{
			get { return Blob.CheckMediaIsCached(Id, MediaType); }
		}

		/// <summary>
		///     Gets the State of the Snap
		/// </summary>
		public SnapState GetState
		{
			get
			{
				if (OpenedAt == null)
					return SnapState.NotApplicable;

				// ReSharper disable once PossibleInvalidOperationException
				return ((DateTime) OpenedAt).AddSeconds((Int32) CaptureTime) > DateTime.UtcNow
					? SnapState.Available
					: SnapState.Expired;
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
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
	/// <summary>
	///     Types of Privacy that Snapchat Offer
	/// </summary>
	public enum PrivacyType
	{
		Everyone = 0x00,
		Friends = 0x01
	}

	/// <summary>
	/// </summary>
	public class Account : Base
	{
		// TODO
		// [JsonPropertyAttribute("bests")]
		// public String[] Bests { get; set; }

		private List<AddedFriend> _addedFriends;
		private Int64 _addedFriendsTimestamp;
		private String _authToken;
		private Boolean _canViewMatureContent;
		private String _countryCode;
		private String _deviceToken;
		private String _email;
		private List<Friend> _friends;
		private Boolean _imageCaption;
		private Boolean _isVip;
		private String _mobile;
		private String _mobileVerificationKey;
		private String _notificationSoundsSettings;
		private String[] _recents;
		private Int32 _recieved;

		private Int32 _score;
		private Int32 _sent;
		private Boolean _shouldSendTextToVerifyNumber;
		private PrivacyType _snapPrivacy;
		private String _snapchatPhoneNumber;
		private List<Snap> _snaps;
		private String _storyPrivacy;
		private String _userName;

		/// <summary>
		///     Says if the User is VIP or not
		/// </summary>
		[JsonProperty("is_vip")]
		public Boolean IsVip
		{
			get { return _isVip; }
			set { SetField(ref _isVip, value, "IsVip"); }
		}

		/// <summary>
		///     The Users Score
		/// </summary>
		[JsonProperty("score")]
		public Int32 Score
		{
			get { return _score; }
			set { SetField(ref _score, value, "Score"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("recieved")]
		public Int32 Recieved
		{
			get { return _recieved; }
			set { SetField(ref _recieved, value, "Recieved"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("added_friends")]
		public List<AddedFriend> AddedFriends
		{
			get { return _addedFriends; }
			set { SetField(ref _addedFriends, value, "AddedFriends"); }
		}

		// TODO
		// [JsonPropertyAttribute("requests")]
		// public String[] Requests { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("sent")]
		public Int32 Sent
		{
			get { return _sent; }
			set { SetField(ref _sent, value, "Sent"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("story_privacy")]
		public String StoryPrivacy
		{
			get { return _storyPrivacy; }
			set { SetField(ref _storyPrivacy, value, "StoryPrivacy"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("username")]
		public String UserName
		{
			get { return _userName; }
			set { SetField(ref _userName, value, "UserName"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("snaps")]
		public List<Snap> Snaps
		{
			get { return _snaps; }
			set { SetField(ref _snaps, value, "Snaps"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("friends")]
		public List<Friend> Friends
		{
			get { return _friends; }
			set { SetField(ref _friends, value, "Friends"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("device_token")]
		public String DeviceToken
		{
			get { return _deviceToken; }
			set { SetField(ref _deviceToken, value, "DeviceToken"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("snap_p")]
		public PrivacyType SnapPrivacy
		{
			get { return _snapPrivacy; }
			set { SetField(ref _snapPrivacy, value, "SnapPrivacy"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("mobile_verification_key")]
		public String MobileVerificationKey
		{
			get { return _mobileVerificationKey; }
			set { SetField(ref _mobileVerificationKey, value, "MobileVerificationKey"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("recents")]
		public String[] Recents
		{
			get { return _recents; }
			set { SetField(ref _recents, value, "Recents"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("added_friends_timestamp")]
		public Int64 AddedFriendsTimestamp
		{
			get { return _addedFriendsTimestamp; }
			set { SetField(ref _addedFriendsTimestamp, value, "AddedFriendsTimestamp"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("notification_sound_setting")]
		public String NotificationSoundsSettings
		{
			get { return _notificationSoundsSettings; }
			set { SetField(ref _notificationSoundsSettings, value, "NotificationSoundsSettings"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("snapchat_phone_number")]
		public String SnapchatPhoneNumber
		{
			get { return _snapchatPhoneNumber; }
			set { SetField(ref _snapchatPhoneNumber, value, "SnapchatPhoneNumber"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("auth_token")]
		public String AuthToken
		{
			get { return _authToken; }
			set { SetField(ref _authToken, value, "AuthToken"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("image_caption")]
		public Boolean ImageCaption
		{
			get { return _imageCaption; }
			set { SetField(ref _imageCaption, value, "ImageCaption"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("country_code")]
		public String CountryCode
		{
			get { return _countryCode; }
			set { SetField(ref _countryCode, value, "CountryCode"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("can_view_mature_content")]
		public Boolean CanViewMatureContent
		{
			get { return _canViewMatureContent; }
			set { SetField(ref _canViewMatureContent, value, "CanViewMatureContent"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("email")]
		public String Email
		{
			get { return _email; }
			set { SetField(ref _email, value, "Email"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("should_send_text_to_verify_number")]
		public Boolean ShouldSendTextToVerifyNumber
		{
			get { return _shouldSendTextToVerifyNumber; }
			set { SetField(ref _shouldSendTextToVerifyNumber, value, "ShouldSendTextToVerifyNumber"); }
		}

		/// <summary>
		/// </summary>
		[JsonProperty("mobile")]
		public String Mobile
		{
			get { return _mobile; }
			set { SetField(ref _mobile, value, "Mobile"); }
		}
	}
}
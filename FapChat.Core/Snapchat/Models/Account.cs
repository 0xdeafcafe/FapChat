using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Account : Base, INotifyPropertyChanged
    {
        // TODO
        // [JsonPropertyAttribute("bests")]
        // public String[] Bests { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("is_vip")]
        public Boolean IsVip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("score")]
        public Int32 Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("recieved")]
        public Int32 Recieved { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("added_friends")]
        public List<AddedFriend> AddedFriends
        {
            get { return _addedFriends; }
            set { SetField(ref _addedFriends, value, "AddedFriends"); }
        }
        private List<AddedFriend> _addedFriends;

            // TODO
        // [JsonPropertyAttribute("requests")]
        // public String[] Requests { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("sent")]
        public Int32 Sent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("story_privacy")]
        public String StoryPrivacy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("username")]
        public String UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("snaps")]
        public List<Snap> Snaps { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("friends")]
        public List<Friend> Friends
        {
            get { return _friends; }
            set { SetField(ref _friends, value, "Friends"); }
        }
        private List<Friend> _friends;

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("device_token")]
        public String DeviceToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("snap_p")]
        public Int32 SnapP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("mobile_verification_key")]
        public String MobileVerificationKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("recents")]
        public String[] Recents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("added_friends_timestamp")]
        public Int64 AddedFriendsTimestamp
        {
            get { return _addedFriendsTimestamp; }
            set { SetField(ref _addedFriendsTimestamp, value, "AddedFriendsTimestamp"); }
        }
        private Int64 _addedFriendsTimestamp;

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("notification_sound_setting")]
        public String NotificationSoundsSettings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("snapchat_phone_number")]
        public String SnapchatPhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("auth_token")]
        public String AuthToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("image_caption")]
        public Boolean ImageCaption { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("country_code")]
        public String CountryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("can_view_mature_content")]
        public Boolean CanViewMatureContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("email")]
        public String Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("should_send_text_to_verify_number")]
        public Boolean ShouldSendTextToVerifyNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("mobile")]
        public String Mobile { get; set; }

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

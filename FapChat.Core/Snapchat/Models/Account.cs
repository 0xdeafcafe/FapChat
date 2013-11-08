using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Account
    {
        // TODO
        // [JsonPropertyAttribute("bests")]
        // public String[] Bests { get; set; }

        /// <summary>
        /// The message explaining the error, if there was one
        /// </summary>
        public String Message { get; set; }

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
        [JsonPropertyAttribute("logged")]
        public Boolean Logged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("added_friends")]
        public List<AddedFriend> AddedFriends { get; set; }

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
        public List<Friend> Friends { get; set; }

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
        public Int64 AddedFriendsTimestamp { get; set; }

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
    }
}

using System;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    public enum FriendType
    {
        Accepted = 0x00,
        PendingAccept = 0x01,
        Blocked = 0x02
    }

    /// <summary>
    /// 
    /// </summary>
    public class Friend
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("can_see_custom_stories")]
        public Boolean CanSeeCustomStories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("name")]
        public String Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("display")]
        public String Display { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("type")]
        public FriendType Type { get; set; }
    }
}

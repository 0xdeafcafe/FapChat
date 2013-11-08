using System;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
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
        public Int32 Type { get; set; }
    }
}

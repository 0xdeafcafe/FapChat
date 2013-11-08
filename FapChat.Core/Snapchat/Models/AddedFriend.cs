using System;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddedFriend
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyAttribute("ts")]
        public Int64 Ts { get; set; }

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

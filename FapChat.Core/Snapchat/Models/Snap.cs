using System;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
	/// <summary>
	/// 
	/// </summary>
    public class Snap
    {
		/// <summary>
		/// 
        /// </summary>
        [JsonPropertyAttribute("id")]
        public String Id { get; set; }

		/// <summary>
		/// 
        /// </summary>
        [JsonPropertyAttribute("rp")]
        public String Rp { get; set; }

		/// <summary>
		/// 
        /// </summary>
        [JsonPropertyAttribute("ts")]
        public Int64 Ts { get; set; }

		/// <summary>
		/// 
        /// </summary>
        [JsonPropertyAttribute("sts")]
        public Int64 Sts { get; set; }

		/// <summary>
		/// 
        /// </summary>
        [JsonPropertyAttribute("c_id")]
        public String CId { get; set; }

		/// <summary>
		/// 
        /// </summary>
        [JsonPropertyAttribute("m")]
        public Int32 M { get; set; }

		/// <summary>
		/// 
        /// </summary>
        [JsonPropertyAttribute("st")]
        public Int32 St { get; set; }
    }
}

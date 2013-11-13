using System;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Best : Base
    {
        /// <summary>
        /// A list of this person's best friends
        /// </summary>
        [JsonPropertyAttribute("best_friends")]
        public String[] BestFriends 
        {
            get { return _bestFriends; }
            set { SetField(ref _bestFriends, value, "BestFriends"); }
        }
        private String[] _bestFriends;

        /// <summary>
        /// This persons Score
        /// </summary>
        [JsonPropertyAttribute("score")]
        public Int32 Score
        {
            get { return _score; }
            set { SetField(ref _score, value, "Score"); }
        }
        private Int32 _score;
    }
}

using System;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
	/// <summary>
	/// </summary>
	public class Best : Base
	{
		private String[] _bestFriends;

		private Int32 _score;

		/// <summary>
		///     A list of this person's best friends
		/// </summary>
		[JsonProperty("best_friends")]
		public String[] BestFriends
		{
			get { return _bestFriends; }
			set { SetField(ref _bestFriends, value, "BestFriends"); }
		}

		/// <summary>
		///     This persons Score
		/// </summary>
		[JsonProperty("score")]
		public Int32 Score
		{
			get { return _score; }
			set { SetField(ref _score, value, "Score"); }
		}
	}
}
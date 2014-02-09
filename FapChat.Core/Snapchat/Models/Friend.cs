using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
	/// <summary>
	///     The Different Types of Friend Status
	/// </summary>
	public enum FriendType
	{
		Accepted = 0x00,
		PendingAccept = 0x01,
		Blocked = 0x02
	}

	/// <summary>
	///     A Friend on Snapchat
	/// </summary>
	public class Friend : INotifyPropertyChanged
	{
		private Boolean _canSeeCustomStories;
		private String _display;

		private String _name;
		private FriendType _type;

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

		/// <summary>
		///     Shows if the Friend allows you to see custom stories
		/// </summary>
		[JsonProperty("can_see_custom_stories")]
		public Boolean CanSeeCustomStories
		{
			get { return _canSeeCustomStories; }
			set { SetField(ref _canSeeCustomStories, value, "CanSeeCustomStories"); }
		}

		/// <summary>
		///     Shows the Name of the Friend
		/// </summary>
		[JsonProperty("name")]
		public String Name
		{
			get { return _name; }
			set { SetField(ref _name, value, "Name"); }
		}

		/// <summary>
		///     The Custom Name for the Friend
		/// </summary>
		[JsonProperty("display")]
		public String Display
		{
			get { return _display; }
			set { SetField(ref _display, value, "Display"); }
		}

		/// <summary>
		///     Shows if the Friend is Pending, Blocked or not a Creep
		/// </summary>
		[JsonProperty("type")]
		public FriendType Type
		{
			get { return _type; }
			set { SetField(ref _type, value, "Type"); }
		}
	}

	/// <summary>
	/// </summary>
	public class AddedFriend : Friend
	{
		private Int64 _ts;

		/// <summary>
		///     Timestamp of when the Friendship started
		/// </summary>
		[JsonProperty("ts")]
		public Int64 Ts
		{
			get { return _ts; }
			set { SetField(ref _ts, value, "Ts"); }
		}
	}

	/// <summary>
	/// </summary>
	public class FriendAction : Base
	{
		private Friend _object;

		/// <summary>
		///     Object Data (probally the Friend Model)
		/// </summary>
		[JsonProperty("object")]
		public Friend Object
		{
			get { return _object; }
			set { SetField(ref _object, value, "Object"); }
		}
	}
}
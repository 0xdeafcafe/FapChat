using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Best : Base, INotifyPropertyChanged
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

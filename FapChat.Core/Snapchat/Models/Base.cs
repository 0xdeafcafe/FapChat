using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    public class Base : INotifyPropertyChanged
    {
        /// <summary>
        /// Says if you're still signed in or not after this Request
        /// </summary>
        [JsonPropertyAttribute("logged")]
        public Boolean Logged
        {
            get { return _logged; }
            set { SetField(ref _logged, value, "Logged"); }
        }
        private Boolean _logged;

        /// <summary>
        /// The Response Message
        /// </summary>
        [JsonPropertyAttribute("message")]
        public String Message
        {
            get { return _message; }
            set { SetField(ref _message, value, "Message"); }
        }
        private String _message;

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

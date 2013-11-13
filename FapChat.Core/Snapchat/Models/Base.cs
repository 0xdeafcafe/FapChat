using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat.Models
{
    /// <summary>
    /// The base response params snapchat return
    /// </summary>
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

        /// <summary>
        /// The Optional param response
        /// </summary>
        [JsonPropertyAttribute("param")]
        public String Param
        {
            get { return _param; }
            set { SetField(ref _param, value, "Param"); }
        }
        private String _param;

        #region Binding Stuff

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
    }
}

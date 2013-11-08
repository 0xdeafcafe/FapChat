using System.Collections.Generic;
using System.IO.IsolatedStorage;
using FapChat.Core.Snapchat.Models;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat
{
    /// <summary>
    /// Isolated Storage Vault
    /// </summary>
    public class Storage
    {
        /// <summary>
        /// Application Settings stored in Isolated Storage
        /// </summary>
        private readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

        /// <summary>
        /// Holds Account Details
        /// </summary>
        public Account UserAccount
        {
            get
            {
                // Return the Account Details
                return _userAccount;
            }
            set
            {
                // Set the Account Details
                _userAccount = value;

                // Save them to Isolated Storage
                Save("user-account", JsonConvert.SerializeObject(value));
            }
        }
        private Account _userAccount;

        /// <summary>
        /// Initalize the Isolated Storage Class
        /// </summary>
        public Storage()
        {
            Load();
        }

        /// <summary>
        /// Save all data to Isolated Storage
        /// </summary>
        public void Save()
        {
            var storageData = new Dictionary<string, object>
            {
                { "user-account", _userAccount }
            };

            foreach (var data in storageData)
                Save(data.Key, JsonConvert.SerializeObject(data.Value));
        }

        /// <summary>
        /// Saves certain data to Isolated Storage
        /// </summary>
        /// <param name="key">The Key of the data to store</param>
        /// <param name="data">The actual data to store</param>
        public void Save(string key, string data)
        {
            if (_appSettings.Contains(key))
                _appSettings[key] = data;
            else
                _appSettings.Add(key, data);

            _appSettings.Save();
        }

        /// <summary>
        /// Loads Data from Isolated Storage and collects it
        /// </summary>
        public void Load()
        {
            // Load Account Data
            _userAccount = _appSettings.Contains("user-account") ? JsonConvert.DeserializeObject<Account>(_appSettings["credentialVault"].ToString()) : null;
        }
    }
}

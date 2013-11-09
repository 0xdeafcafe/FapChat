using System;
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

                // Update the Last Updated data
                UserAccountLastUpdate = DateTime.UtcNow;
            }
        }
        private Account _userAccount;
        public DateTime UserAccountLastUpdate { get; private set; }

        /// <summary>
        /// Holds the Bests of an users friends
        /// </summary>
        public Dictionary<string, Best> FriendsBests
        {
            get
            {
                // Return the Account Details
                return _friendsBests;
            }
            set
            {
                // Set the Account Details
                _friendsBests = value;

                // Save them to Isolated Storage
                Save("friends-bests", JsonConvert.SerializeObject(value));

                // Update the Last Updated data
                FriendsBestsLastUpdate = DateTime.UtcNow;
            }
        }
        private Dictionary<string, Best> _friendsBests;
        public DateTime FriendsBestsLastUpdate { get; private set; }

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
                { "user-account", _userAccount },
                { "friends-bests", _friendsBests }
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
            _userAccount = _appSettings.Contains("user-account") ? JsonConvert.DeserializeObject<Account>(_appSettings["user-account"].ToString()) : null;
            UserAccountLastUpdate = new DateTime(1994, 08, 18, 14, 0, 0, 0);

            // Load Friends Bests
            _friendsBests = _appSettings.Contains("friends-bests") ? JsonConvert.DeserializeObject<Dictionary<string, Best>>(_appSettings["friends-bests"].ToString()) : null;
            FriendsBestsLastUpdate = new DateTime(1994, 08, 18, 14, 0, 0, 0);
        }

        /// <summary>
        /// Deletes everything in Isolated Storage, and resets the Timestamps
        /// </summary>
        public void Reset()
        {
            UserAccount = null;
            UserAccountLastUpdate = new DateTime(1994, 08, 18, 14, 0, 0, 0);

            FriendsBests = null;
            FriendsBestsLastUpdate = new DateTime(1994, 08, 18, 14, 0, 0, 0);
        }
    }
}

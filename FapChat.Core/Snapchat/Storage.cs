using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using FapChat.Core.Snapchat.Models;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat
{
    /// <summary>
    /// Isolated Storage Vault
    /// </summary>
    public class Storage : INotifyPropertyChanged
    {
        /// <summary>
        /// Application Settings stored in Isolated Storage
        /// </summary>
        private readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

        #region UserAccount

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
                SetField(ref _userAccount, value, "UserAccount");

                // Save them to Isolated Storage
                Save("user-account", JsonConvert.SerializeObject(value));

                // Update the Last Updated data
                UserAccountLastUpdate = DateTime.UtcNow;
            }
        }
        private Account _userAccount;
        public DateTime UserAccountLastUpdate { get; private set; }

        /// <summary>
        /// A really hacker way of updating the local data as to not overide changes we have made
        /// </summary>
        /// <param name="account"></param>
        public void UserAccountUpdate(Account account)
        {
            if (UserAccount == null)
            {
                UserAccount = account;
                return;
            }

            // save current states
            var snapsBackup = UserAccount.Snaps;
            var newSnaps = account.Snaps;

            // replace new snaps with current ones
            account.Snaps = snapsBackup;
            UserAccount = account;
            UserAccount.Snaps = snapsBackup;

            // carefully loop though new snaps, updateing one ones that be been sent
            foreach (var newSnap in newSnaps)
            {
                var oldSnap = UserAccount.Snaps.FirstOrDefault(s => s.Id == newSnap.Id);
                if (oldSnap == null)
                {
                    UserAccount.Snaps.Add(newSnap);
                    return;
                }
                if (oldSnap.RecipientName == null) return;

                // SO FUCKING HACKISH HOLY SHIT FUCCCK
                // WHAT IS THIS SHIT OMG
                newSnap.SentTimestamp = oldSnap.SentTimestamp;
                newSnap.Status = oldSnap.Status;
                newSnap.Timestamp = oldSnap.Timestamp;
            }
        }

        #endregion

        /// <summary>
        /// Holds the Bests of an users friends
        /// </summary>
        public Dictionary<string, Best> FriendsBests
        {
            get
            {
                // Return the Friend Bests
                return _friendsBests;
            }
            set
            {
                // Set the Friend Bests
                SetField(ref _friendsBests, value, "FriendsBests");

                // Save them to Isolated Storage
                Save("friends-bests", JsonConvert.SerializeObject(value));

                // Update the Last Updated data
                FriendsBestsLastUpdate = DateTime.UtcNow;
            }
        }
        private Dictionary<string, Best> _friendsBests;
        public DateTime FriendsBestsLastUpdate { get; private set; }

        /// <summary>
        /// Holds the Cached Media Blobs
        /// </summary>
        public List<CachedMediaBlob> CachedMediaBlobs
        {
            get
            {
                // Return the Cached Media Blobs
                return _cachedMediaBlobs;
            }
            set
            {
                // Set the Cached Media Blobs
                SetField(ref _cachedMediaBlobs, value, "CachedMediaBlobs");

                // Save them to Isolated Storage
                Save("cached-media-blobs", JsonConvert.SerializeObject(_cachedMediaBlobs));

                // Update the Last Updated data
                CachedMediaBlobsLastUpdate = DateTime.UtcNow;
            }
        }
        private List<CachedMediaBlob> _cachedMediaBlobs;
        public DateTime CachedMediaBlobsLastUpdate { get; private set; }

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
                { "friends-bests", _friendsBests },
                { "cached-media-blobs", _cachedMediaBlobs }
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
            UserAccount = _appSettings.Contains("user-account")
                ? JsonConvert.DeserializeObject<Account>(_appSettings["user-account"].ToString())
                : null;
            UserAccountLastUpdate = new DateTime(1994, 08, 18, 14, 0, 0, 0);

            // Load Friends Bests
            FriendsBests = _appSettings.Contains("friends-bests")
                ? JsonConvert.DeserializeObject<Dictionary<string, Best>>(_appSettings["friends-bests"].ToString())
                : null;
            FriendsBestsLastUpdate = new DateTime(1994, 08, 18, 14, 0, 0, 0);

            // Load Friends Bests
            CachedMediaBlobs = _appSettings.Contains("cached-media-blobs")
                ? JsonConvert.DeserializeObject<List<CachedMediaBlob>>(_appSettings["cached-media-blobs"].ToString()) ??
                  new List<CachedMediaBlob>()
                : new List<CachedMediaBlob>();
            CachedMediaBlobsLastUpdate = new DateTime(1994, 08, 18, 14, 0, 0, 0);
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

            CachedMediaBlobs = new List<CachedMediaBlob>();
            CachedMediaBlobsLastUpdate = new DateTime(1994, 08, 18, 14, 0, 0, 0);
        }

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
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using FapChat.Core.Snapchat.Helpers;
using FapChat.Core.Snapchat.Models;
using Newtonsoft.Json;

namespace FapChat.Core.Snapchat
{
    /// <summary>
    /// 
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static async Task<Tuple<TempEnumHolder.LoginStatus, Account>> Login(string username, string password)
        {
            var timestamp = Timestamps.GenerateRetardedTimestamp();
            var postData = new Dictionary<string, string>
		    {
		        { "password", password },
		        { "username", username },
                { "timestamp", timestamp.ToString(CultureInfo.InvariantCulture) }
		    };
            var response = await WebRequests.Post("login", postData, KeyVault.StaticToken, timestamp.ToString(CultureInfo.InvariantCulture));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    // Http Request Worked
                    var data = await response.Content.ReadAsStringAsync();
                    var parsedData = await JsonConvert.DeserializeObjectAsync<Account>(data);

                    // Check if we were logged in
                    if (!parsedData.Logged)
                        return
                            new Tuple<TempEnumHolder.LoginStatus, Account>(
                                TempEnumHolder.LoginStatus.InvalidCredentials, parsedData);

                    // Yup, save the data and return true
                    return 
                        new Tuple<TempEnumHolder.LoginStatus, Account>(
                            TempEnumHolder.LoginStatus.Success, parsedData);
                }
                default:
                    // Well, fuck
                    return 
                        new Tuple<TempEnumHolder.LoginStatus, Account>(
                            TempEnumHolder.LoginStatus.ServerError, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static async Task<Account> Update(string username, string authToken)
        {
            var timestamp = Timestamps.GenerateRetardedTimestamp();
            var postData = new Dictionary<string, string>
		    {
		        { "username", username },
                { "timestamp", timestamp.ToString(CultureInfo.InvariantCulture) }
		    };
            var response = await WebRequests.Post("updates", postData, authToken, timestamp.ToString(CultureInfo.InvariantCulture));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        // Http Request Worked
                        var data = await response.Content.ReadAsStringAsync();
                        var parsedData = await JsonConvert.DeserializeObjectAsync<Account>(data);

                        // we updated n shit
                        return !parsedData.Logged ? null : parsedData;
                    }
                default:
                    // Well, fuck
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="authToken"></param>
        public static async void Logout(string username, string authToken)
        {
            var timestamp = Timestamps.GenerateRetardedTimestamp();
            var postData = new Dictionary<string, string>
		    {
                { "json", "{}" },
		        { "username", username },
                { "timestamp", timestamp.ToString(CultureInfo.InvariantCulture) }
		    };
            var response = await WebRequests.Post("logout", postData, authToken, timestamp.ToString(CultureInfo.InvariantCulture));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        // Yup, save the data and return true
                        return;
                    }
                default:
                    // Well, fuck
                    return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friends"></param>
        /// <param name="username"></param>
        /// <param name="authToken"></param>
        public static async Task<Dictionary<string, Best>> GetBests(List<Friend> friends, string username, string authToken)
        {
            var friendsList = new string[friends.Count];
            for (var i = 0; i < friendsList.Length; i++)
                friendsList[i] = friends[i].Name;

            var timestamp = Timestamps.GenerateRetardedTimestamp();
            var postData = new Dictionary<string, string>
		    {
                { "friend_usernames", JsonConvert.SerializeObject(friendsList) },
		        { "username", username },
                { "timestamp", timestamp.ToString(CultureInfo.InvariantCulture) }
		    };
            var response = await WebRequests.Post("bests", postData, authToken, timestamp.ToString(CultureInfo.InvariantCulture));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var parsedData = await JsonConvert.DeserializeObjectAsync<Dictionary<string, Best>>(data);

                    // Yup, save the data and return true
                    return parsedData;
                }
                default:
                    // Well, fuck
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="username"></param>
        /// <param name="authToken"></param>
        /// <param name="friend"></param>
        /// <param name="postDataEntries"></param>
        public static async Task<FriendAction> Friend(string friend, string action, string username, string authToken, Dictionary<string, string> postDataEntries = null)
        {
            var timestamp = Timestamps.GenerateRetardedTimestamp();
            var postData = new Dictionary<string, string>
		    {
                { "friend", friend },
                { "action", action },
		        { "username", username },
                { "timestamp", timestamp.ToString(CultureInfo.InvariantCulture) }
		    };

            if (postDataEntries != null)
                foreach(var postDataEntry in postDataEntries)
                    postData.Add(postDataEntry.Key, postDataEntry.Value);

            var response = await WebRequests.Post("friend", postData, authToken, timestamp.ToString(CultureInfo.InvariantCulture));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var parsedData = await JsonConvert.DeserializeObjectAsync<FriendAction>(data);

                        // Yup, save the data and return true
                        return parsedData;
                    }
                default:
                    // Well, fuck
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapId"></param>
        /// <param name="username"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetBlob(string snapId, string username, string authToken)
        {
            var timestamp = Timestamps.GenerateRetardedTimestamp();
            var postData = new Dictionary<string, string>
		    {
                { "id", snapId },
		        { "username", username },
                { "timestamp", timestamp.ToString(CultureInfo.InvariantCulture) }
		    };

            var response = await WebRequests.Post("blob", postData, authToken, timestamp.ToString(CultureInfo.InvariantCulture));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var data = await response.Content.ReadAsByteArrayAsync();

                        // Validate Blob Shit
                        byte[] decryptedBlob = null;

                        if (Blob.ValidateMediaBlob(data))
                            decryptedBlob = data;
                        else
                        {
                            data = Blob.DecryptBlob(data);
                            if (Blob.ValidateMediaBlob(data))
                                decryptedBlob = data;
                        }

                        return decryptedBlob;

                    }
                default:
                    // Well, fuck
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="snapInfo"></param>
        /// <param name="username"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static async Task<bool> SendEvents(Dictionary<string, object>[] events, Dictionary<string, Dictionary<string, double>> snapInfo, string username,
            string authToken)
        {
            var timestamp = Timestamps.GenerateRetardedTimestamp();
            var postData = new Dictionary<string, string>
		    {
                { "events", JsonConvert.SerializeObject(events) },
                { "json", JsonConvert.SerializeObject(snapInfo) },
		        { "username", username },
                { "timestamp", timestamp.ToString(CultureInfo.InvariantCulture) }
		    };

            var response = await WebRequests.Post("update_snaps", postData, authToken, timestamp.ToString(CultureInfo.InvariantCulture));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;

                default:
                    // Well, fuck
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapId"></param>
        /// <param name="timeViewed"></param>
        /// <param name="captureTime"></param>
        /// <param name="username"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static async Task<bool> SendViewedEvent(string snapId, int timeViewed, int captureTime, string username,
            string authToken)
        {
            var snapInfo = new Dictionary<string, Dictionary<string, double>>
            {
                { 
                    snapId, 
                    new Dictionary<string, double>
                    {
                        { "t", Timestamps.GenerateRetardedTimestampWithMilliseconds() },
                        { "sv", captureTime + new Random(0xdead).NextDouble() }
                    }
                }
            };

            var events = new[]
            {
                Events.CreateEvent(Events.EventType.SnapViewed, snapId, timeViewed),
                Events.CreateEvent(Events.EventType.SnapExpired, snapId, timeViewed + captureTime)
            };

            return await SendEvents(events, snapInfo, username, authToken);
        }
    }
}

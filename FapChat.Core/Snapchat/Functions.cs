﻿using System;
using System.Collections.Generic;
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
                { "timestamp", timestamp }
		    };
            var response = await WebRequests.Post("login", postData, KeyVault.StaticToken, timestamp);
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
                { "timestamp", timestamp }
		    };
            var response = await WebRequests.Post("update", postData, KeyVault.StaticToken, timestamp);
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
                { "timestamp", timestamp }
		    };
            var response = await WebRequests.Post("logout", postData, authToken, timestamp);
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
                { "timestamp", timestamp }
		    };
            var response = await WebRequests.Post("bests", postData, authToken, timestamp);
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
    }
}

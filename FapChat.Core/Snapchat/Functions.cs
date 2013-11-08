using System;
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
        public static async Task<TempEnumHolder.LoginStatus> Login(string username, string password)
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
                    if (!parsedData.Logged) return TempEnumHolder.LoginStatus.InvalidCredentials;

                    // Yup, save the data and return true
                    Storage.UserAccount = parsedData;
                    return TempEnumHolder.LoginStatus.Success;
                }
                default:
                    // Well, fuck
                    return TempEnumHolder.LoginStatus.ServerError;
            }
        }
    }
}

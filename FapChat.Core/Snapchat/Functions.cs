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
    }
}

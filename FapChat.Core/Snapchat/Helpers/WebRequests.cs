using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FapChat.Core.Snapchat.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebRequests
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="postData"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Post(string endpoint, Dictionary<string, string> postData, string param1, string param2, Dictionary<string, string> headers = null)
        {
            var webClient = new HttpClient();
            webClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", KeyVault.UserAgent);
            webClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Length", "160");
            webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            webClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip,deflate");

            if (headers != null)
                foreach (var header in headers)
                    webClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

            postData["req_token"] = Tokens.GenerateRequestToken(param1, param2);
            postData["version"] = "5.0.0";
            var postBody = PostBodyParser(postData);
            var response = await webClient.PostAsync(new Uri(KeyVault.ApiBasePoint + endpoint), new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded"));
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postEntries"></param>
        /// <returns></returns>
        public static string PostBodyParser(Dictionary<string, string> postEntries)
        {
            var first = true;
            var output = "";
            foreach (var postEntry in postEntries)
            {
                if (!first)
                    output += string.Format("&{0}={1}", postEntry.Key, HttpUtility.UrlEncode(postEntry.Value));
                else
                    output += string.Format("{0}={1}", postEntry.Key, HttpUtility.UrlEncode(postEntry.Value));

                first = false;
            }

            return output;
        }
    }
}

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FapChat.Core.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    public class Sha
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Sha256(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            return hash.Aggregate(string.Empty, (current, x) => current + String.Format("{0:x2}", x));
        }
    }
}

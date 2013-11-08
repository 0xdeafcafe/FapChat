using System;
using System.Globalization;

namespace FapChat.Core.Snapchat.Helpers
{
    /// <summary>
    /// A Timestamp Helper for Snapchat
    /// </summary>
    public static class Timestamps
    {
        /// <summary>
        /// The date of the Unix Epoch
        /// </summary>
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Generates this fucking retarded timestamp that snapchat use...
        /// </summary>
        /// <returns>The timestamp as a string</returns>
        public static string GenerateRetardedTimestamp()
        {
            var now = Math.Round((DateTime.UtcNow - UnixEpoch).TotalSeconds * 1000).ToString(CultureInfo.InvariantCulture);
            return now;
        }
    }
}


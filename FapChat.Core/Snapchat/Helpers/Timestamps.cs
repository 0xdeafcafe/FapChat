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
        public static Int64 GenerateRetardedTimestamp()
        {
            return Convert.ToInt64(Math.Round((DateTime.UtcNow - UnixEpoch).TotalSeconds * 1000));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static double GenerateRetardedTimestampWithMilliseconds()
        {
            return double.Parse((DateTime.UtcNow - UnixEpoch).TotalSeconds.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Convert a SnapChat timestamp into a DateTime object.
        /// </summary>
        /// <param name="retardedTimeStamp">The snapchat timestamp.</param>
        /// <returns>A nice, non retarded DateTime object.</returns>
        public static DateTime ConvertToDateTime(Int64 retardedTimeStamp)
        {
            var unixShit = retardedTimeStamp.ToString(CultureInfo.InvariantCulture);

            if (unixShit.Length > 10)
                unixShit = unixShit.Remove(10);

            return UnixEpoch.AddSeconds(Int64.Parse(unixShit));
        }

        /// <summary>
        /// Convert a DateTime object into a normal Unix Timestamp
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert.</param>
        /// <returns>A 32bit integer that holds the unix Timestamp</returns>
        public static int ConvertToUnixTimestamp(DateTime dateTime)
        {
            return (int)((DateTime.UtcNow - UnixEpoch).TotalSeconds);
        }
    }
}

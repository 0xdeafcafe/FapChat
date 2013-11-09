using FapChat.Core.Crypto;

namespace FapChat.Core.Snapchat.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class Tokens
    {
        /// <summary>
        /// Generates a Request Token from Post Data and a Static Token
        /// </summary>
        /// <param name="postData">The Html Encoded Post Data</param>
        /// <param name="staticToken">The Snapchat Static Token.</param>
        /// <returns>The Request Token, all nice.</returns>
        public static string GenerateRequestToken(string postData, string staticToken)
        {
            var s1 = KeyVault.Secret + postData;
            var s2 = staticToken + KeyVault.Secret;

            var s3 = Sha.Sha256(s1);
            var s4 = Sha.Sha256(s2);

            var output = "";
            for (var i = 0; i < KeyVault.HashingPattern.Length; i++)
            {
                var c = KeyVault.HashingPattern[i];

                if (c == '0')
                    output += s3[i];
                else
                    output += s4[i];
            }
            return output;
        }
    }
}

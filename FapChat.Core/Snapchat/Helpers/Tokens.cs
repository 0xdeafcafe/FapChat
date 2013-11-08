using FapChat.Core.Crypto;

namespace FapChat.Core.Snapchat.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class Tokens
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        public static string GenerateRequestToken(string param1, string param2)
        {
            var s1 = KeyVault.Secret + param1;
            var s2 = param2 + KeyVault.Secret;

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

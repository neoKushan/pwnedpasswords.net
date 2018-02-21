using System;
using System.Net.Http;
using System.Text;

namespace PwnedPasswords.net
{
    public class PwnedPasswordsClient
    {
        private readonly string baseUrl = "https://api.pwnedpasswords.com";
        private readonly HttpClient client = new HttpClient();

        public PwnedPasswordsClient()
        {
            this.client = new HttpClient();
        }

        public PwnedPasswordsClient(HttpClient client)
        {
            this.client = client;
        }

        public bool CheckPassword(string clearPassword)
        {
            using (var crypto = System.Security.Cryptography.SHA1.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(clearPassword);
                var hashedPassword = crypto.ComputeHash(passwordBytes);
                var hashedPasswordString = ByteArrayToString(hashedPassword);
                return CheckSha1HashedPassword(hashedPasswordString);
            }
        }

        public bool CheckSha1HashedPassword(string hashedPassword)
        {
            string passwordHashes = GetPasswordHashBucket(hashedPassword);
            var hashMatch = hashedPassword.Substring(5);
            return passwordHashes.Contains(hashMatch);
        }

        private string GetPasswordHashBucket(string hashedPassword)
        {
            var hashPrefix = hashedPassword.Substring(0, 5);
            var passwordHashes = client.GetStringAsync($"{baseUrl}/range/{hashPrefix}").Result;
            return passwordHashes;
        }

        private static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }
}

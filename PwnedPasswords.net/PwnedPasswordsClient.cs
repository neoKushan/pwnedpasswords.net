using System;
using System.Net.Http;
using System.Text;

namespace PwnedPasswords.net
{
    public class PwnedPasswordsClient
    {
        private readonly string _baseUrl = "https://api.pwnedpasswords.com";
        private readonly HttpClient _client;

        public PwnedPasswordsClient()
        {
            _client = new HttpClient();
        }

        public PwnedPasswordsClient(HttpClient client)
        {
            _client = client;
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
            var passwordHashes = _client.GetStringAsync($"{_baseUrl}/range/{hashPrefix}").Result;
            return passwordHashes;
        }

        private static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }
}

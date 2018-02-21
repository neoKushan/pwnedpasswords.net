using System;
using System.Net.Http;
using System.Text;

namespace PwnedPasswords.net
{
    /// <summary>
    /// PwnedPasswordsClient is a client for www.pwnedpasswords.com"
    /// </summary>
    public class PwnedPasswordsClient
    {
        private readonly string _baseUrl = "https://api.pwnedpasswords.com";
        private readonly HttpClient _client;

        /// <summary>
        /// Default constructor will create an instance of HttpClient
        /// </summary>
        public PwnedPasswordsClient()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Constructor which takes an instance of HttpClient, 
        /// allowing you to use a static instance globally within
        /// your application
        /// </summary>
        /// <param name="client">Instance of HttpClient</param>
        public PwnedPasswordsClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Checks if a plaintext password is on a known passwords list.
        /// </summary>
        /// <param name="clearPassword">The password in clear text</param>
        /// <returns>Returns true if the password is known.</returns>
        public bool CheckPassword(string clearPassword)
        {
            if (string.IsNullOrWhiteSpace(clearPassword))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(clearPassword));

            using (var crypto = System.Security.Cryptography.SHA1.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(clearPassword);
                var hashedPassword = crypto.ComputeHash(passwordBytes);
                var hashedPasswordString = ByteArrayToString(hashedPassword);
                return CheckSha1HashedPassword(hashedPasswordString);
            }
        }

        /// <summary>
        /// Checks SHA1-hased password to check if it's known to be on
        /// password lists.
        /// </summary>
        /// <param name="hashedPassword">The hashed password as a hex string</param>
        /// <returns>Returns true if the password is known.</returns>
        public bool CheckSha1HashedPassword(string hashedPassword)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));

            if (hashedPassword.Length != 40)
                throw new ArgumentException("Length is incorrect, this is not a valid SHA1 hash", nameof(hashedPassword));

            var passwordHashes = GetPasswordHashBucket(hashedPassword);
            var hashMatch = hashedPassword.Substring(5);
            return passwordHashes.Contains(hashMatch);
        }

        /// <summary>
        /// Calls the PwnedPasswords API with the first 5 characters of the supplied hash
        /// Useful if you want to parse the response yourself
        /// </summary>
        /// <param name="hashedPassword">A SHA1 hashed password (or optionally the first 5 characters)</param>
        /// <returns>Returns the body of the response form the API</returns>
        public string GetPasswordHashBucket(string hashedPassword)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
            if (hashedPassword.Length < 5) throw new ArgumentException("Value must be at least 5 characters in lenth", nameof(hashedPassword));

            var hashPrefix = hashedPassword.Substring(0, 5);
            return _client.GetStringAsync($"{_baseUrl}/range/{hashPrefix}").Result;
        }

        /// <summary>
        /// Converts a byte array to a string. This is not the most optimal way
        /// to do this and can be improved.
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>The byte array in string form.</returns>
        private static string ByteArrayToString(byte[] ba)
        {
            var hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }
}

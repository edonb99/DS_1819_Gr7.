using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SaltedHash
    {
        public static string getSaltedHash(string password)
        {
            var random = new RNGCryptoServiceProvider();

            // Maximum length of salt
            int max_length = 32;

            // Empty salt array
            byte[] salt = new byte[max_length];

            // Build the random bytes
            random.GetNonZeroBytes(salt);
            string salt1 = Convert.ToBase64String(salt);
         
            string saltedPassword = password + salt;

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] byteSaltedPassword = Encoding.UTF8.GetBytes(saltedPassword);
            byte[] byteSaltedHash = sha.ComputeHash(byteSaltedPassword);

            return Convert.ToBase64String(byteSaltedHash);

        }
    }
}

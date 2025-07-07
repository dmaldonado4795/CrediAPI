using CrediAPI.Common.DTOs;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CrediAPI.Common.Helpers
{
    public static class PasswordHasherHelper
    {
        // Generate a hash and salt for a password
        public static PasswordHashDTO HashPassword(string password)
        {
            // Generate a random salt (recommended 16 bytes for SHA256)
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            string hashedPassword = GetSha256Hash(password, salt);

            return new PasswordHashDTO { Hash = hashedPassword, Salt = salt };
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            string hashOfInputPassword = GetSha256Hash(password, storedSalt);
            return hashOfInputPassword == storedHash;
        }

        // Helper method to get SHA256 hash
        private static string GetSha256Hash(string password, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Concatenate password and salt before hashing
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password + salt));

                // Convert the byte array to a hexadecimal (or Base64) string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // "x2" for two-digit hexadecimal format
                }
                return builder.ToString();
            }
        }
    }
}
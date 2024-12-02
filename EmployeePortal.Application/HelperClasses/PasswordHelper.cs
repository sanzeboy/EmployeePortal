using System.Security.Cryptography;
using System.Text;

namespace EmployeePortal.Application.HelperClasses
{
    public static class PasswordHelper
    {
        // private helper methods

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace only string.", "password");

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] ispdHash, byte[] ispdSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            if (ispdHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (ispdSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new HMACSHA512(ispdSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != ispdHash[i])
                        return false;
                }
            }

            return true;
        }

        public static string GetRandomPassword(int size = 8)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % chars.Length]);
            }
            return result.ToString();
        }
    }
}

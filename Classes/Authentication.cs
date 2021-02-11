using System;
using System.Security.Cryptography;
using System.Text;
namespace UserAuthentication.Classes
{
    public abstract class Authentication
    {
        private static int RefreshTokenExpiryDays = 7;
        private static int AccessTokenExpiryDays = 1;
        public static string GenerateRandomSalt(int length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[length];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }
        public static string GenerateHashedPWD(string password, string saltextt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] plainText = StringToByteArray(password);
            byte[] salt = StringToByteArray(saltextt);
            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }
            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
        }
        public static byte[] StringToByteArray(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
        public static bool VerifyHashedPassword(string password, string salt, string hashedPassword)
        {

            string tempHashedPassword = GenerateHashedPWD(password, salt);
            return tempHashedPassword == hashedPassword;

        }
        public static RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiryDate = DateTime.Now.AddDays(RefreshTokenExpiryDays),
                };
            }
        }
        public static RefreshToken RefreshToken(string ipAddress,string accesstoken, string oldrefreshtoken)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiryDate = DateTime.Now.AddDays(RefreshTokenExpiryDays),
                };
            }
        }
        public static RefreshToken GenerateTokens(string ipAddress, string accesstoken, string oldrefreshtoken)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiryDate = DateTime.Now.AddDays(RefreshTokenExpiryDays),
                };
            }
        }
        public static AccessToken GenerateAccessToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new AccessToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiryDate = DateTime.Now.AddDays(AccessTokenExpiryDays),
                };
            }
        }
    }
}

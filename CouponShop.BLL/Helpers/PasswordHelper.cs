
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Helpers
{
    public class PasswordHelper: IPasswordHelper
    {
        public string HashPassword(string password)
        {
            // 1. צור salt אקראי בגודל 16 bytes
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // 2. חישוב ה־hash באמצעות PBKDF2 עם SHA256 ו-100,000 איטרציות
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // hash בגודל 32 bytes

                // 3. איחוד salt + hash ל־byte array אחד
                byte[] hashBytes = new byte[48];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 32);

                // 4. המרה ל־Base64 כדי לאחסן בבסיס הנתונים
                return Convert.ToBase64String(hashBytes);
            }
        }

        // מאמת סיסמה מול hash קיים
        public bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // חילוץ salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // חישוב hash חדש עם הסיסמה שהוזנה וה־salt השמור
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                // השוואת hash חדש ל־hash השמור
                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }
            }

            return true;
        }
    }
}

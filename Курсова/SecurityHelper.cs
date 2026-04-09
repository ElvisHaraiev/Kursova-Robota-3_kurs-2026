using System;
using System.Security.Cryptography;
using System.Text;

namespace Курсова
{
    public static class SecurityHelper
    {
        /// <summary>
        /// Генерує SHA-256 хеш для переданого пароля.
        /// </summary>
        /// <param name="password">Пароль у відкритому вигляді (наприклад, "1234")</param>
        /// <returns>Шістнадцятковий рядок хешу (64 символи)</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
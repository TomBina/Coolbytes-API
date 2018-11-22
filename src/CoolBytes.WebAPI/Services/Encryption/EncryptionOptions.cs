using System;
using System.Security.Cryptography;

namespace CoolBytes.WebAPI.Services.Encryption
{
    public class EncryptionOptions
    {
        public byte[] Iv { get; private set; }
        public byte[] Key { get; private set; }

        /// <summary>
        /// Creates options for the encryption service based on a iv, password and salt.
        /// </summary>
        /// <param name="iv">A base64 encoded initialization vector.</param>
        /// <param name="password">The plain text password.</param>
        /// <param name="salt">The password salt.</param>
        public EncryptionOptions(string iv, string password, string salt)
        {
            if (iv == null) throw new ArgumentNullException(nameof(iv));
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (salt == null) throw new ArgumentNullException(nameof(salt));

            SetIv(iv);
            SetKey(password, salt);
        }

        private void SetKey(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            if (saltBytes.Length != 256)
            {
                throw new ArgumentException("Salt has to be 256 bytes.");
            }

            var rfc = new Rfc2898DeriveBytes(password, saltBytes);
            Key = rfc.GetBytes(32);
        }

        private void SetIv(string iv)
        {
            Iv = Convert.FromBase64String(iv);
            if (Iv.Length != 16)
            {
                throw new ArgumentException("IV has to be 16 bytes.");
            }
        }
    }
}
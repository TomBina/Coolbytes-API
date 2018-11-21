using System;
using System.IO;
using System.Security.Cryptography;

namespace CoolBytes.WebAPI.Services.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private readonly EncryptionOptions _options;

        public EncryptionService(EncryptionOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string Encrypt(string text)
        {
            var aes = Aes.Create();

            if (aes == null)
                throw new ArgumentNullException(nameof(aes));

            aes.IV = _options.Iv;
            aes.Key = _options.Key;

            var transform = aes.CreateEncryptor();

            using (var ms = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cryptoStream))
                    {
                        sw.Write(text);
                    }
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public string Decrypt(string encrypted)
        {
            var bytes = Convert.FromBase64String(encrypted);
            var aes = Aes.Create();

            if (aes == null)
                throw new ArgumentNullException(nameof(aes));

            aes.IV = _options.Iv;
            aes.Key = _options.Key;

            var transform = aes.CreateDecryptor();

            using (var ms = new MemoryStream(bytes))
            {
                using (var cryptoStream = new CryptoStream(ms, transform, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cryptoStream))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }

        }
    }
}
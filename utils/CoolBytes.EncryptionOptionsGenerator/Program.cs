using System;
using System.Security.Cryptography;
using CoolBytes.WebAPI.Services.Encryption;

namespace CoolBytes.EncryptionOptionsGenerator
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Choose a password");
            var password = Console.ReadLine();

            var saltBytes = new byte[256];
            var ivBytes = new byte[16];

            var generator = RandomNumberGenerator.Create();
            generator.GetBytes(ivBytes);
            generator.GetBytes(saltBytes);

            var iv = Convert.ToBase64String(ivBytes);
            var salt = Convert.ToBase64String(saltBytes);

            Console.WriteLine();
            Console.WriteLine("Variables");
            Console.WriteLine($"IV = {iv}");
            Console.WriteLine($"Password = {password}");
            Console.WriteLine($"Salt = {salt}");

            var options = new EncryptionOptions(iv, password, salt);
            var encryptionService = new EncryptionService(options);

            Console.WriteLine();
            Console.WriteLine("Choose a text to encrypt");

            while (true)
            {
                Console.WriteLine();
                var text = Console.ReadLine();

                if (text?.Length == 0)
                    return;

                Console.WriteLine($"{text} = {encryptionService.Encrypt(text)}");
            }
        }
    }
}

using System;

namespace CoolBytes.Tests.Web.Services.Encryption
{
    public class EncryptionTestContext
    {
        public string Iv { get; set; }
        public string Salt { get; set; }

        public EncryptionTestContext()
        {
            var saltBytes = new byte[256];
            var ivBytes = new byte[16];

            for (var i = 0; i < saltBytes.Length; i++)
            {
                if (i % 2 == 0)
                {
                    saltBytes[i] = 1;
                }
                else
                {
                    saltBytes[i] = 2;
                }
            }

           for (var i = 0; i < ivBytes.Length; i++)
           {
               if (i % 2 == 0)
               {
                   ivBytes[i] = 1;
               }
               else
               {
                   ivBytes[i] = 2;
               }
           }

            Iv = Convert.ToBase64String(ivBytes);
            Salt = Convert.ToBase64String(saltBytes);
        }
    }
}

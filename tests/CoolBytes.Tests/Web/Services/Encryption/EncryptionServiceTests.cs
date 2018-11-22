using System;
using System.Security.Cryptography;
using CoolBytes.WebAPI.Services.Encryption;
using Xunit;

namespace CoolBytes.Tests.Web.Services.Encryption
{
    public class EncryptionServiceTests : IClassFixture<EncryptionTestContext>
    {
        private readonly EncryptionTestContext _context;

        public EncryptionServiceTests(EncryptionTestContext context)
        {
            _context = context;
        }

        [Fact]
        public void EncryptionOptions_Are_Created_When_Valid_Arguments_Are_Used()
        {
            var saltBytes = new byte[256];
            var ivBytes = new byte[16];

            var generator = RandomNumberGenerator.Create();
            generator.GetBytes(ivBytes);
            generator.GetBytes(saltBytes);

            var iv = Convert.ToBase64String(ivBytes);
            var password = "HelloWorld!";
            var salt = Convert.ToBase64String(saltBytes);

            var _ = new EncryptionOptions(iv, password, salt);
        }

        [Fact]
        public void EncryptionOptions_Throws_When_Iv_Has_Invalid_Length()
        {
            var saltBytes = new byte[256];
            var ivBytes = new byte[10];

            var generator = RandomNumberGenerator.Create();
            generator.GetBytes(ivBytes);
            generator.GetBytes(saltBytes);

            var iv = Convert.ToBase64String(ivBytes);
            var password = "HelloWorld!";
            var salt = Convert.ToBase64String(saltBytes);

            Assert.Throws<ArgumentException>(() => new EncryptionOptions(iv, password, salt));
        }


        [Fact]
        public void EncryptionOptions_Throws_When_Salt_Has_Invalid_Length()
        {
            var saltBytes = new byte[100];
            var ivBytes = new byte[16];

            var generator = RandomNumberGenerator.Create();
            generator.GetBytes(ivBytes);
            generator.GetBytes(saltBytes);

            var iv = Convert.ToBase64String(ivBytes);
            var password = "HelloWorld!";
            var salt = Convert.ToBase64String(saltBytes);

            Assert.Throws<ArgumentException>(() => new EncryptionOptions(iv, password, salt));
        }


        [Fact]
        public void EncryptionService_Encrypts_Correctly()
        {
            var encryptionOptions = new EncryptionOptions(_context.Iv, "P@ssword", _context.Salt);
            var encryptionService = new EncryptionService(encryptionOptions);

            var encrypted = encryptionService.Encrypt("super secret text");

            Assert.Equal("x3f7RwP8K1DzrIUVC9sCNkV0JiQ5Hqp2lExJnUD+YWo=", encrypted);
        }

        [Fact]
        public void EncryptionService_Decrypts_Correctly()
        {
            var encryptionOptions = new EncryptionOptions(_context.Iv, "P@ssword", _context.Salt);
            var encryptionService = new EncryptionService(encryptionOptions);

            var decrypted = encryptionService.Decrypt("x3f7RwP8K1DzrIUVC9sCNkV0JiQ5Hqp2lExJnUD+YWo=");

            Assert.Equal("super secret text", decrypted);
        }
    }
}

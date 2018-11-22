using CoolBytes.WebAPI.Services.Environment;
using Microsoft.Extensions.Configuration;
using System;
using CoolBytes.WebAPI.Services.Encryption;

namespace CoolBytes.WebAPI.Services.Mailer
{
    public class MailgunMailerOptionsFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IEnvironmentService _environmentService;

        public MailgunMailerOptionsFactory(IConfiguration configuration, IEnvironmentService environmentService)
        {
            _configuration = configuration;
            _environmentService = environmentService;
        }

        public MailgunMailerOptions Create()
        {
            var server = new Uri(_configuration["Mailgun:Server"]);
            var encryptedUserName = _configuration["Mailgun:UserName"];
            var encryptedPassword = _configuration["Mailgun:Key"];
            var credentials = CreateCredentials(encryptedUserName, encryptedPassword);
            var domain = _configuration["Mailgun:Domain"];
            var mailgunMailerOptions = new MailgunMailerOptions(server, credentials, domain);

            return mailgunMailerOptions;
        }

        private MailgunMailerCredentials CreateCredentials(string encryptedUserName, string encryptedPassword)
        {
            var iv = _environmentService.GetVariable("Enc_Mailgun_IV");
            var keyPassword = _environmentService.GetVariable("Enc_Mailgun_Password");
            var salt = _environmentService.GetVariable("Enc_Mailgun_Salt");
            var encryptionOptions = new EncryptionOptions(iv, keyPassword, salt);
            var encryptionService = new EncryptionService(encryptionOptions);

            var userName = encryptionService.Decrypt(encryptedUserName);
            var password = encryptionService.Decrypt(encryptedPassword);

            return new MailgunMailerCredentials(userName, password);
        }
    }
}

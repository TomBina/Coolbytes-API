using CoolBytes.WebAPI.Services.Environment;
using CoolBytes.WebAPI.Services.Mailer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Xunit;

namespace CoolBytes.Tests.Web.Services.Mailer
{
    public class MailgunMailerOptionsFactoryTests
    {
        [Fact]
        public void When_Valid_Variables_Decryption_Succeeds()
        {
            var config = CreateConfiguration();
            var ev = ValidEnvironment();

            var factory = new MailgunMailerOptionsFactory(config, ev);
            var options = factory.Create();

            Assert.Equal("testuser", options.Credentials.UserName);
            Assert.Equal("testpassword", options.Credentials.Password);
        }
        
        [Fact]
        public void When_Invalid_Variables_Decryption_Fails()
        {
            var config = CreateConfiguration();
            var ev = InvalidEnvironment();

            var factory = new MailgunMailerOptionsFactory(config, ev);

            Assert.Throws<CryptographicException>(() => factory.Create());
        }

        private static ConfigurationRoot CreateConfiguration()
        {
            var root = Path.GetDirectoryName($"{Assembly.GetExecutingAssembly().Location}");
            var file = $@"assets\mailgunsettings.json";
            var physicalFileProvider = new PhysicalFileProvider(root);
            var jsonConfigurationSource = new JsonConfigurationSource { Path = file, FileProvider = physicalFileProvider };
            var jsonConfigurationProvider = new JsonConfigurationProvider(jsonConfigurationSource);
            var providers = new List<IConfigurationProvider> { jsonConfigurationProvider };
            var configurationRoot = new ConfigurationRoot(providers);

            return configurationRoot;
        }

        private static IEnvironmentService ValidEnvironment()
        {
            var evMock = new Mock<IEnvironmentService>();
            evMock.Setup(ev => ev.GetVariable(It.Is<string>(v => v == "Enc_Mailgun_IV"))).Returns("YmPr5WefWbVcbxN4JDWGEw==");
            evMock.Setup(ev => ev.GetVariable(It.Is<string>(v => v == "Enc_Mailgun_Password"))).Returns("testp@ssword");
            evMock.Setup(ev => ev.GetVariable(It.Is<string>(v => v == "Enc_Mailgun_Salt"))).Returns(@"z7oVRA0VGngMsUdq9qVxRuYB/7OJU11YTAFDKSwDa6JGs2FLBn/82KKIzuGP3snjw4fXP7ePA
                /1yoc9xSp/mp9UwPaWjfMEFzhiltbFbtjiD7bwu1IqUiVtOj7tgNmT6ZEtqd3rMj8UBkkm+M0pCOuUkB
                8rwBYY3CKhZX2aV2vXkSAoAyqfB+yV3dPNe0IYHzAfe1aNCQ76H5u3qFXJUKv22GudNRurajnnps/WdL
                mWMEKErrWmKGSQb/OGP/YwTGU6yp5SSfeDfeT+oGUJeGAgZkyzblKlg8MZlVLKPLBQZT7ylo+65F4uPi
                uMCgP/2lIxS2u8g/4ubPfuFbEacAQ==");

            return evMock.Object;
        }

        private static IEnvironmentService InvalidEnvironment()
        {
            var evMock = new Mock<IEnvironmentService>();
            evMock.Setup(ev => ev.GetVariable(It.Is<string>(v => v == "Enc_Mailgun_IV"))).Returns("YmPr5WefWbVcbxN4JDWGEw==");
            evMock.Setup(ev => ev.GetVariable(It.Is<string>(v => v == "Enc_Mailgun_Password"))).Returns("testpf@ssword");
            evMock.Setup(ev => ev.GetVariable(It.Is<string>(v => v == "Enc_Mailgun_Salt"))).Returns(@"z7oVRA0VGngMsUdq9qVxRuYB/7OJU11YTAFDKSwDa6JGs2FLBn/82KKIzuGP3snjw4fXP7ePA
                /1yoc9xSp/mp9UwPaWjfMEFzhiltbFbtjiD7bwu1IqUiVtOj7tgNmT6ZEtqd3rMj8UBkkm+M0pCOuUkB
                8rwBYY3CKhZX2aV2vXkSAoAyqfB+yV3dPNe0IYHzAfe1aNCQ76H5u3qFXJUKv22GudNRurajnnps/WdL
                mWMEKErrWmKGSQb/OGP/YwTGU6yp5SSfeDfeT+oGUJeGAgZkyzblKlg8MZlVLKPLBQZT7ylo+65F4uPi
                uMCgP/2lIxS2u8g/4ubPfuFbEacAQ==");

            return evMock.Object;
        }

    }
}

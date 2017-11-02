using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Services.Mailer;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Web.Services.Mailer
{
    public class MailerTests
    {
        [Fact]
        public async Task Mailer_SendsMessage()
        {
            var mailer = CreateMailer();
            var message = CreateMessage();

            var report = await mailer.Send(message);

            Assert.True(report.IsSend);
        }

        private static IMailer CreateMailer()
        {
            var httpClient = CreateHttpClient();
            var options = CreateOptions();
            var logger = new Mock<ILogger<MailgunMailer>>().Object;

            return new MailgunMailer(httpClient, options, logger);
        }

        private static HttpClient CreateHttpClient()
        {
            var fakeHandler = new StubHandler();
            var httpClient = new HttpClient(fakeHandler);
            return httpClient;
        }

        private static MailgunMailerOptions CreateOptions()
        {
            var server = new Uri("http://localhost/");
            var credentials = new MailgunMailerCredentials("test", "test");
            var domain = "test";
            var options = new MailgunMailerOptions(server, credentials, domain);
            return options;
        }

        private static EmailMessage CreateMessage()
        {
            const string body = "Test message";
            var from = new EmailAddress("TestFrom", "test@test.com");
            var to = new EmailAddress("TestTo", "test@test.com");
            var message = new EmailMessage(from, to, body, "Test message!");

            return message;
        }
    }
}

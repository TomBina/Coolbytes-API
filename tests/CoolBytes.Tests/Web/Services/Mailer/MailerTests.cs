using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoolBytes.Data;
using CoolBytes.Data.Models;
using CoolBytes.WebAPI.Services.Mailer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Web.Services.Mailer
{
    public class MailerTests : IClassFixture<TestContext>, IAsyncLifetime
    {
        private readonly TestContext _testContext;
        private readonly AppDbContext _context;

        public MailerTests(TestContext testContext)
        {
            _testContext = testContext;
            _context = testContext.CreateNewContext();
        }

        [Fact]
        public async Task Mailer_SendsMessage()
        {
            var mailer = CreateMailer();
            var message = CreateMessage();

            var report = await mailer.Send(message);

            Assert.True(report.IsSend);
        }

        [Fact]
        public async Task Mailer_WithReachedThreshold_ThrowsInvalidOperation()
        {
            using (var context = _testContext.CreateNewContext())
            {
                var name = typeof(MailgunMailer).FullName;
                var mailProvider = new MailProvider(name, 0);
                var mailStat = new MailStat(mailProvider, DateTime.Now);

                context.MailProviders.Add(mailProvider);
                context.MailStats.Add(mailStat);
                await context.SaveChangesAsync();
            }

            var mailer = CreateMailer();
            var message = CreateMessage();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await mailer.Send(message));
        }

        [Fact]
        public async Task Mailer_WithMailStatOfYesterday_AddNewMailStat()
        {
            using (var context = _testContext.CreateNewContext())
            {
                var name = typeof(MailgunMailer).FullName;
                var mailProvider = new MailProvider(name, 100);
                var mailStat = new MailStat(mailProvider, DateTime.Now.AddDays(-1));

                context.MailProviders.Add(mailProvider);
                context.MailStats.Add(mailStat);
                await context.SaveChangesAsync();
            }

            var mailer = CreateMailer();
            var message = CreateMessage();

            await mailer.Send(message);
            var lastStat = _context.MailStats.FirstOrDefaultAsync(ms => ms.Date == DateTime.Now.Date);

            Assert.NotNull(lastStat);
        }

        private IMailer CreateMailer()
        {
            var httpClient = CreateHttpClient();
            var options = CreateOptions();
            var logger = new Mock<ILogger<MailgunMailer>>().Object;
            var thresholdValidator = new ThresholdValidator(_context);

            return new MailgunMailer(httpClient, options, thresholdValidator, logger);
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

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            _context.MailProviders.RemoveRange(_context.MailProviders.ToArray());
            _context.MailStats.RemoveRange(_context.MailStats.ToArray());
            await _context.SaveChangesAsync();

            _context.Dispose();
        }
    }
}

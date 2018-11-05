using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Resume;
using CoolBytes.WebAPI.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Resume
{
    public class ResumeTests : TestBase, IClassFixture<Fixture>, IAsyncLifetime
    {
        public ResumeTests(Fixture fixture) : base(fixture)
        {
        }

        public async Task InitializeAsync()
        {
            await SeedData();
        }

        private async Task SeedData()
        {
            using (var context = Fixture.CreateNewContext())
            {
                var user = new User("Test");

                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorValidator = new AuthorValidator(Context);
                var author = await Author.Create(user, authorProfile, authorValidator);

                await context.SaveChangesAsync();

                for (var i = 0; i < 2; i++)
                {
                    var dateRange = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 2));
                    var resumeEvent = new ResumeEvent(author, dateRange, "Test", "Test");

                    context.ResumeEvents.Add(resumeEvent);
                    await context.SaveChangesAsync();
                }

                await context.SaveChangesAsync();

                InitUserService(user);
                InitAuthorService();
            }
        }

        [Fact]
        public async Task GetResumeQueryHandler_ReturnsResume()
        {
            var author = await AuthorService.GetAuthor();
            var message = new GetResumeQuery()
            {
                AuthorId = author.Id
            };
            var handler = new GetResumeQueryHandler(Context, AuthorService);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result);
        }

        public async Task DisposeAsync()
        {
            Context.Dispose();

            await Task.CompletedTask;
        }
    }
}

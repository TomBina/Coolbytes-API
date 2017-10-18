using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.Resume.CQ;
using CoolBytes.WebAPI.Features.Resume.Handlers;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Resume
{
    public class ResumeTests : TestBase, IClassFixture<Fixture>
    {
        public ResumeTests(Fixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task AddResumeEventCommandHandler_AddsResumeEvent()
        {
            var message = new AddResumeEventCommand();
            var handler = new AddResumeEventCommandHandler(Context, AuthorService);

            var result = await handler.Handle(message);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateResumeEventCommandHandler_UpdatesResumeEvent()
        {
            var message = new UpdateResumeEventCommand();
            var handler = new UpdateResumeEventHandler(Context);

            var result = await handler.Handle(message);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetResumeQueryHandler_ReturnsResume()
        {
            var message = new GetResumeQuery();
            var handler = new GetResumeQueryHandler(Context, AuthorService);

            await SeedData();

            var result = await handler.Handle(message);

            Assert.Equal(2, result.Count());
        }

        private async Task SeedData()
        {
            var author = await AuthorService.GetAuthor();

            using (var context = Fixture.CreateNewContext())
            {
                for (var i = 0; i < 2; i++)
                {
                    var dateRange = new DateRange(DateTime.Now, DateTime.Now);
                    var resumeEvent = new ResumeEvent(author, dateRange, "Test", "Test");

                    context.ResumeEvents.Add(resumeEvent);
                    await Context.SaveChangesAsync();
                }
            }

        }
    }
}
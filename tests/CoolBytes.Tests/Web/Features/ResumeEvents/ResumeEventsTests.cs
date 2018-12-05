using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.DTO;
using CoolBytes.WebAPI.Features.ResumeEvents.Handlers;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Utils;
using Moq;
using Xunit;

namespace CoolBytes.Tests.Web.Features.ResumeEvents
{
    public class ResumeEventsTests : TestBase
    {
        private IUserService _userService;
        private AuthorService _authorService;

        public ResumeEventsTests(TestContext testContext) : base(testContext)
        {
        }

        public override async Task InitializeAsync()
        {
            using (var context = TestContext.CreateNewContext())
            {
                var user = new User("Test");

                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorValidator = new AuthorValidator(Context);
                var author = await Author.Create(user, authorProfile, authorValidator);

                context.Authors.Add(author);

                await context.SaveChangesAsync();

                var userService = new Mock<IUserService>();
                userService.Setup(exp => exp.GetOrCreateCurrentUser()).ReturnsAsync(user);
                userService.Setup(exp => exp.TryGetCurrentUser()).ReturnsAsync(user.ToSuccessResult());
                _userService = userService.Object;
                _authorService = new AuthorService(_userService, Context);
            }
        }

        [Fact]
        public async Task GetResumeEventsQueryHandler_ReturnsResumeEvents()
        {
            var resumeEvents = await SeedData();
            var authorId = resumeEvents.First().AuthorId;
            var message = new GetResumeEventsQuery() { AuthorId = authorId };
            var handler = new GetResumeEventsQueryHandler(Context);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetResumeEventQueryHandler_ReturnsResumeEvent()
        {
            var resumeEvents = await SeedData();
            var resumeEvent = resumeEvents.First();

            var message = new GetResumeEventQuery() { Id = resumeEvent.Id };
            var handler = new GetResumeEventQueryHandler(Context);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal(resumeEvent.Name, result.Name);
        }

        [Fact]
        public async Task AddResumeEventCommandHandler_AddsResumeEvent()
        {
            var message = new AddResumeEventCommand()
            {
                DateRange = new DateRangeDto()
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1)
                },
                Name = "Test",
                Message = "test"
            };
            var handler = new AddResumeEventCommandHandler(Context, _authorService);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateResumeEventCommandHandler_UpdatesResumeEvent()
        {
            var resumeEvents = await SeedData();
            var resumeEvent = resumeEvents.First();

            var message = new UpdateResumeEventCommand()
            {
                Id = resumeEvent.Id,
                DateRange = new DateRangeDto()
                {
                    StartDate = resumeEvent.DateRange.StartDate,
                    EndDate = resumeEvent.DateRange.EndDate
                },
                Message = "Updated message",
                Name = resumeEvent.Name

            };
            var handler = new UpdateResumeEventHandler(Context);

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.Equal("Updated message", result.Message);
        }

        [Fact]
        public async Task DeleteResumeEventHandler_DeletesResume()
        {
            var resumeEvents = await SeedData();
            var currentCount = resumeEvents.Count;
            var message = new DeleteResumeEventCommand { Id = resumeEvents.First().Id };
            IRequestHandler<DeleteResumeEventCommand> handler = new DeleteResumeEventCommandHandler(Context);

            await handler.Handle(message, CancellationToken.None);
            var newCount = (await Context.ResumeEvents.ToListAsync()).Count;
            Assert.Equal(currentCount - 1, newCount);
        }

        private async Task<List<ResumeEvent>> SeedData()
        {
            var author = await _authorService.GetAuthor();

            using (var context = TestContext.CreateNewContext())
            {
                context.Attach(author).State = EntityState.Unchanged;

                for (var i = 0; i < 2; i++)
                {
                    var dateRange = new DateRange(DateTime.Now, DateTime.Now);
                    var resumeEvent = new ResumeEvent(author, dateRange, "Test", "Test");

                    context.ResumeEvents.Add(resumeEvent);
                    await context.SaveChangesAsync();
                }

                return await context.ResumeEvents.ToListAsync();
            }
        }

        public override async Task DisposeAsync()
        {
            var resumeEvents = await Context.ResumeEvents.ToArrayAsync();
            Context.ResumeEvents.RemoveRange(resumeEvents);
            await Context.SaveChangesAsync();

            Context.Dispose();
        }
    }
}
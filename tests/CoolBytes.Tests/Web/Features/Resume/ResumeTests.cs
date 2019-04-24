using CoolBytes.Core.Utils;
using CoolBytes.Services;
using CoolBytes.WebAPI.Features.Resume;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using Xunit;

namespace CoolBytes.Tests.Web.Features.Resume
{
    public class ResumeTests : TestBase
    {
        private IUserService _userService;
        private AuthorService _authorService;

        public ResumeTests(TestContext testContext) : base(testContext)
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

                await context.SaveChangesAsync();

                for (var i = 0; i < 2; i++)
                {
                    var dateRange = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 2));
                    var resumeEvent = new ResumeEvent(author, dateRange, "Test", "Test");

                    context.ResumeEvents.Add(resumeEvent);
                    await context.SaveChangesAsync();
                }

                await context.SaveChangesAsync();

                var userService = new Mock<IUserService>();
                userService.Setup(exp => exp.GetOrCreateCurrentUserAsync()).ReturnsAsync(user);
                userService.Setup(exp => exp.TryGetCurrentUserAsync()).ReturnsAsync(user.ToSuccessResult());
                _userService = userService.Object;
                _authorService = new AuthorService(_userService, Context);
            }
        }

        [Fact]
        public async Task GetResumeQueryHandler_ReturnsResume()
        {
            var author = await _authorService.GetAuthor();
            var message = new GetResumeQuery()
            {
                AuthorId = author.Id
            };
            var handler = new GetResumeQueryHandler(Context, _authorService, TestContext.CreateStubCacheService());

            var result = await handler.Handle(message, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}

using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.WebAPI
{
    public class DbSetup
    {
        public static void SeedDb()
        {
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(o =>
                o.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CoolBytes;Trusted_Connection=True"));

            var serviceProvider = services.BuildServiceProvider();

            using (serviceProvider.CreateScope())
            {
                var context = serviceProvider.GetService<AppDbContext>();
                var user = new User("Test");
                var authorProfile = new AuthorProfile("Tom", "Bina", "About me");
                var authorValidator = new AuthorValidator(context);
                var author = Author.Create(user, authorProfile, authorValidator).Result;

                for (var i = 0; i < 20; i++)
                {
                    var blogPost = new BlogPost("This is a test subject.", "Let's begin with a test intro", "And here's the test content", author);
                    context.BlogPosts.Add(blogPost);
                }

                context.SaveChanges();
            }
            serviceProvider.Dispose();
        }
    }
}
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.WebAPI
{
    public static class DbSetup
    {
        public static void InitDb(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(o =>
                o.UseSqlServer(configuration.GetConnectionString("Default")));

            var serviceProvider = services.BuildServiceProvider();

            using (serviceProvider.CreateScope())
            {
                var context = serviceProvider.GetService<AppDbContext>();

                context.Database.Migrate();
            }
            serviceProvider.Dispose();
        }

        public static void SeedDb(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(o =>
                o.UseSqlServer(configuration.GetConnectionString("Default")));

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
                    var blogPostContent = new BlogPostContent("This is a test subject.", "Let's begin with a test intro", "And here's the test content");
                    var category = new Category("Test");
                    var blogPost = new BlogPost(blogPostContent, author, category);
                    context.BlogPosts.Add(blogPost);
                }

                context.SaveChanges();
            }
            serviceProvider.Dispose();
        }
    }
}
using CoolBytes.Core.Models;
using CoolBytes.Data;
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
                var author = new Author(new User("Test"),"Tom", "Bina", "About me");
                var blogPost = new BlogPost("Testsubject", "Testintro", "Testcontent", author);
                context.BlogPosts.Add(blogPost);
                context.SaveChanges();
            }
            serviceProvider.Dispose();
        }
    }
}
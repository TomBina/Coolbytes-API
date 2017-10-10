using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Image> Images { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>(entity =>
                {
                    entity.Property(e => e.Identifier).HasMaxLength(200).IsRequired();
                    entity.ToTable("Users");
                })
                .Entity<Author>(entity =>
                {
                    entity.HasIndex(a => a.UserId).IsUnique();
                    entity.HasOne(a => a.AuthorProfile).WithOne(a => a.Author).OnDelete(DeleteBehavior.Cascade);
                    entity.ToTable("Authors");
                })
                .Entity<AuthorProfile>(entity =>
                {
                    entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.About).HasMaxLength(500).IsRequired();
                    entity.ToTable("AuthorsProfile");
                })
                .Entity<BlogPost>(entity =>
                {
                    entity.Property(e => e.Date).IsRequired();

                    var blogPostContentEntity = entity.OwnsOne(b => b.Content);
                    blogPostContentEntity.Property(e => e.Subject).HasMaxLength(100).IsRequired();
                    blogPostContentEntity.Property(e => e.ContentIntro).HasMaxLength(100).IsRequired();
                    blogPostContentEntity.Property(e => e.Content).HasMaxLength(4000).IsRequired();
                    blogPostContentEntity.Property(e => e.SubjectUrl).HasMaxLength(100).IsRequired();

                    entity.HasMany(b => b.Tags).WithOne(bt => bt.BlogPost).OnDelete(DeleteBehavior.Cascade);
                })
                .Entity<Image>(entity =>
                {
                    entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
                    entity.Property(e => e.Path).HasMaxLength(500).IsRequired();
                    entity.Property(e => e.UriPath).HasMaxLength(500).IsRequired();
                    entity.Property(e => e.Length).IsRequired();
                    entity.Property(e => e.ContentType).HasMaxLength(30).IsRequired();
                    entity.ToTable("Images");
                })
                .Entity<BlogPostTag>(entity =>
                {
                    entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                    entity.ToTable("BlogPostTags");
                });
        }

        public async Task<int> SaveChangesAsync(Action onFailure)
        {
            try
            {
                return await SaveChangesAsync();
            }
            catch (Exception)
            {
                onFailure?.Invoke();
                throw;
            }
        }
    }
}
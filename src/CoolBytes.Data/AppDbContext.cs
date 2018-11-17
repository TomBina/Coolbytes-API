using CoolBytes.Core.Models;
using CoolBytes.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CoolBytes.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogPostTag> BlogPostTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ResumeEvent> ResumeEvents { get; set; }
        public DbSet<MailProvider> MailProviders { get; private set; }
        public DbSet<MailStat> MailStats { get; set; }

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
                    entity.HasOne(a => a.AuthorProfile).WithOne(a => a.Author).OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    entity.ToTable("Authors");
                })
                .Entity<AuthorProfile>(entity =>
                {
                    entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.About).HasMaxLength(500).IsRequired();
                    entity.Property(e => e.ResumeUri).HasMaxLength(255);

                    entity.HasMany(e => e.Experiences).WithOne(ex => ex.AuthorProfile).IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    var socialHandlesEntity = entity.OwnsOne(e => e.SocialHandles);
                    socialHandlesEntity.Property(e => e.LinkedIn).HasMaxLength(255);
                    socialHandlesEntity.Property(e => e.GitHub).HasMaxLength(255);

                    entity.ToTable("AuthorsProfile");
                })
                .Entity<Experience>(entity =>
                {
                    entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.Color).HasColumnType("CHAR(6)").IsRequired();
                    entity.HasOne(e => e.Image).WithMany().OnDelete(DeleteBehavior.Restrict);
                    entity.ToTable("Experiences");
                })
                .Entity<BlogPost>(entity =>
                {
                    entity.Property(e => e.Date).IsRequired();

                    var blogPostContentEntity = entity.OwnsOne(b => b.Content);
                    blogPostContentEntity.Property(e => e.Subject).HasMaxLength(100).IsRequired();
                    blogPostContentEntity.Property(e => e.ContentIntro).HasMaxLength(120).IsRequired();
                    blogPostContentEntity.Property(e => e.Content).HasMaxLength(8000).IsRequired();
                    blogPostContentEntity.Property(e => e.SubjectUrl).HasMaxLength(100).IsRequired();

                    entity.HasMany(b => b.Tags).WithOne(bt => bt.BlogPost).IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);
                    entity.HasMany(b => b.ExternalLinks).WithOne(el => el.BlogPost).IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);
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
                })
                .Entity<ExternalLink>(entity =>
                {
                    entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.Url).HasMaxLength(255).IsRequired();
                    entity.ToTable("ExternalLinks");
                })
                .Entity<ResumeEvent>(entity =>
                {
                    entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.Message).HasMaxLength(1000).IsRequired();

                    var dateRangeRangeEntity = entity.OwnsOne(e => e.DateRange);
                    dateRangeRangeEntity.Property(e => e.StartDate).IsRequired();
                    dateRangeRangeEntity.Property(e => e.EndDate).IsRequired();
                })
                .Entity<MailProvider>(entity =>
                {
                    entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                    entity.HasIndex(e => e.Name).IsUnique();
                })
                .Entity<Category>(entity =>
                {
                    entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                    entity.HasIndex(e => e.Name).IsUnique();
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
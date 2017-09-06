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
        public DbSet<BlogPost> BlogPosts { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<BlogPost>(entity =>
                {
                    entity.Property(e => e.Date).IsRequired();
                    entity.Property(e => e.Subject).HasMaxLength(100).IsRequired();
                    entity.Property(e => e.ContentIntro).HasMaxLength(100).IsRequired();
                    entity.Property(e => e.Content).HasMaxLength(4000).IsRequired();
                })
                .Entity<Author>(entity =>
                {
                    entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                    entity.Property(e => e.About).HasMaxLength(500).IsRequired();
                    entity.ToTable("Authors");
                })
                .Entity<Photo>(entity =>
                {
                    entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
                    entity.Property(e => e.Path).HasMaxLength(500).IsRequired();
                    entity.Property(e => e.Length).IsRequired();
                    entity.Property(e => e.ContentType).HasMaxLength(30).IsRequired();
                    entity.ToTable("Photos");
                })
                .Entity<BlogPostTag>(entity =>
                {
                    entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                    entity.ToTable("BlogPostTags");
                });
        }
    }
}
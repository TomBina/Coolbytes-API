using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class RenamedBlogPostContentToContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlogPostContent_Updated",
                table: "BlogPosts",
                newName: "Content_Updated");

            migrationBuilder.RenameColumn(
                name: "BlogPostContent_SubjectUrl",
                table: "BlogPosts",
                newName: "Content_SubjectUrl");

            migrationBuilder.RenameColumn(
                name: "BlogPostContent_Subject",
                table: "BlogPosts",
                newName: "Content_Subject");

            migrationBuilder.RenameColumn(
                name: "BlogPostContent_ContentIntro",
                table: "BlogPosts",
                newName: "Content_ContentIntro");

            migrationBuilder.RenameColumn(
                name: "BlogPostContent_Content",
                table: "BlogPosts",
                newName: "Content_Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content_Updated",
                table: "BlogPosts",
                newName: "BlogPostContent_Updated");

            migrationBuilder.RenameColumn(
                name: "Content_SubjectUrl",
                table: "BlogPosts",
                newName: "BlogPostContent_SubjectUrl");

            migrationBuilder.RenameColumn(
                name: "Content_Subject",
                table: "BlogPosts",
                newName: "BlogPostContent_Subject");

            migrationBuilder.RenameColumn(
                name: "Content_ContentIntro",
                table: "BlogPosts",
                newName: "BlogPostContent_ContentIntro");

            migrationBuilder.RenameColumn(
                name: "Content_Content",
                table: "BlogPosts",
                newName: "BlogPostContent_Content");
        }
    }
}

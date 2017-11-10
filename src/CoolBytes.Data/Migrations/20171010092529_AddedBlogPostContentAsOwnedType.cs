using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedBlogPostContentAsOwnedType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectUrl",
                table: "BlogPosts",
                newName: "BlogPostContent_SubjectUrl");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "BlogPosts",
                newName: "BlogPostContent_Subject");

            migrationBuilder.RenameColumn(
                name: "ContentIntro",
                table: "BlogPosts",
                newName: "BlogPostContent_ContentIntro");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "BlogPosts",
                newName: "BlogPostContent_Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlogPostContent_SubjectUrl",
                table: "BlogPosts",
                newName: "SubjectUrl");

            migrationBuilder.RenameColumn(
                name: "BlogPostContent_Subject",
                table: "BlogPosts",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "BlogPostContent_ContentIntro",
                table: "BlogPosts",
                newName: "ContentIntro");

            migrationBuilder.RenameColumn(
                name: "BlogPostContent_Content",
                table: "BlogPosts",
                newName: "Content");
        }
    }
}

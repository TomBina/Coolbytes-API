using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class DeleteCascadeBehaviorOnBlogPostTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostTags_BlogPosts_BlogPostId",
                table: "BlogPostTags");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostTags_BlogPosts_BlogPostId",
                table: "BlogPostTags",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostTags_BlogPosts_BlogPostId",
                table: "BlogPostTags");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostTags_BlogPosts_BlogPostId",
                table: "BlogPostTags",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

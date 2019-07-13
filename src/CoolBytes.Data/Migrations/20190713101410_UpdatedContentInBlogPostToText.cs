using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolBytes.Data.Migrations
{
    public partial class UpdatedContentInBlogPostToText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content_Content",
                table: "BlogPosts",
                type: "text",
                maxLength: 8000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 8000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content_Content",
                table: "BlogPosts",
                maxLength: 8000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 8000);
        }
    }
}

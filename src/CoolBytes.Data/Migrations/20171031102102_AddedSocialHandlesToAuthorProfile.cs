using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedSocialHandlesToAuthorProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialHandles_GitHub",
                table: "AuthorsProfile",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialHandles_LinkedIn",
                table: "AuthorsProfile",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialHandles_GitHub",
                table: "AuthorsProfile");

            migrationBuilder.DropColumn(
                name: "SocialHandles_LinkedIn",
                table: "AuthorsProfile");
        }
    }
}

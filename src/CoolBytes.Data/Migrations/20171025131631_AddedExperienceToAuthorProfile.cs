using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedExperienceToAuthorProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Experience_Databases",
                table: "AuthorsProfile",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Experience_Frameworks",
                table: "AuthorsProfile",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Experience_Languages",
                table: "AuthorsProfile",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Experience_Orm",
                table: "AuthorsProfile",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience_Databases",
                table: "AuthorsProfile");

            migrationBuilder.DropColumn(
                name: "Experience_Frameworks",
                table: "AuthorsProfile");

            migrationBuilder.DropColumn(
                name: "Experience_Languages",
                table: "AuthorsProfile");

            migrationBuilder.DropColumn(
                name: "Experience_Orm",
                table: "AuthorsProfile");
        }
    }
}

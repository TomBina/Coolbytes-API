using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class SplitAuthorsInAuthorsAndAuthorsProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Photos_PhotoId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_PhotoId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "About",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Authors");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorProfileId",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthorsProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    About = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorsProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorsProfile_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AuthorId",
                table: "Photos",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsProfile_PhotoId",
                table: "AuthorsProfile",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AuthorsProfile_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId",
                principalTable: "AuthorsProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Authors_AuthorId",
                table: "Photos",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AuthorsProfile_AuthorProfileId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Authors_AuthorId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "AuthorsProfile");

            migrationBuilder.DropIndex(
                name: "IX_Photos_AuthorId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "AuthorProfileId",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Authors",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Authors",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Authors",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Authors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_PhotoId",
                table: "Authors",
                column: "PhotoId",
                unique: true,
                filter: "[PhotoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Photos_PhotoId",
                table: "Authors",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

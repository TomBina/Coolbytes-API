using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class RenamedPhotoToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorsProfile_Photos_PhotoId",
                table: "AuthorsProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Photos_PhotoId",
                table: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_PhotoId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_AuthorsProfile_PhotoId",
                table: "AuthorsProfile");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "AuthorsProfile");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "BlogPosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "AuthorsProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UriPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_ImageId",
                table: "BlogPosts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsProfile_ImageId",
                table: "AuthorsProfile",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorsProfile_Images_ImageId",
                table: "AuthorsProfile",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Images_ImageId",
                table: "BlogPosts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorsProfile_Images_ImageId",
                table: "AuthorsProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Images_ImageId",
                table: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_ImageId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_AuthorsProfile_ImageId",
                table: "AuthorsProfile");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "AuthorsProfile");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "BlogPosts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "AuthorsProfile",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentType = table.Column<string>(maxLength: 30, nullable: false),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    Length = table.Column<long>(nullable: false),
                    Path = table.Column<string>(maxLength: 500, nullable: false),
                    UriPath = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PhotoId",
                table: "BlogPosts",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsProfile_PhotoId",
                table: "AuthorsProfile",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorsProfile_Photos_PhotoId",
                table: "AuthorsProfile",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Photos_PhotoId",
                table: "BlogPosts",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

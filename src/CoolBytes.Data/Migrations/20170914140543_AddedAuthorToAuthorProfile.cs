using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedAuthorToAuthorProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AuthorsProfile_AuthorProfileId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId",
                unique: true,
                filter: "[AuthorProfileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AuthorsProfile_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId",
                principalTable: "AuthorsProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AuthorsProfile_AuthorProfileId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AuthorsProfile_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId",
                principalTable: "AuthorsProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

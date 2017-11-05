using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedIsRequiredToAuthorProfileId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorProfileId",
                table: "Authors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorProfileId",
                table: "Authors",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId",
                unique: true,
                filter: "[AuthorProfileId] IS NOT NULL");
        }
    }
}

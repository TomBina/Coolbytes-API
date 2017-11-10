using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class RemovedIgnoreExperiencesFromAuthorsProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_AuthorsProfile_AuthorProfileId",
                table: "Experiences");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorProfileId",
                table: "Experiences",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_AuthorsProfile_AuthorProfileId",
                table: "Experiences",
                column: "AuthorProfileId",
                principalTable: "AuthorsProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_AuthorsProfile_AuthorProfileId",
                table: "Experiences");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorProfileId",
                table: "Experiences",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_AuthorsProfile_AuthorProfileId",
                table: "Experiences",
                column: "AuthorProfileId",
                principalTable: "AuthorsProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

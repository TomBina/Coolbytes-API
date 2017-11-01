using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class RenamedExperienceToExperiences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experience_AuthorsProfile_AuthorProfileId",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_Images_ImageId",
                table: "Experience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experience",
                table: "Experience");

            migrationBuilder.RenameTable(
                name: "Experience",
                newName: "Experiences");

            migrationBuilder.RenameIndex(
                name: "IX_Experience_ImageId",
                table: "Experiences",
                newName: "IX_Experiences_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Experience_AuthorProfileId",
                table: "Experiences",
                newName: "IX_Experiences_AuthorProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experiences",
                table: "Experiences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_AuthorsProfile_AuthorProfileId",
                table: "Experiences",
                column: "AuthorProfileId",
                principalTable: "AuthorsProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_Images_ImageId",
                table: "Experiences",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_AuthorsProfile_AuthorProfileId",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Images_ImageId",
                table: "Experiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experiences",
                table: "Experiences");

            migrationBuilder.RenameTable(
                name: "Experiences",
                newName: "Experience");

            migrationBuilder.RenameIndex(
                name: "IX_Experiences_ImageId",
                table: "Experience",
                newName: "IX_Experience_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Experiences_AuthorProfileId",
                table: "Experience",
                newName: "IX_Experience_AuthorProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experience",
                table: "Experience",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_AuthorsProfile_AuthorProfileId",
                table: "Experience",
                column: "AuthorProfileId",
                principalTable: "AuthorsProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_Images_ImageId",
                table: "Experience",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

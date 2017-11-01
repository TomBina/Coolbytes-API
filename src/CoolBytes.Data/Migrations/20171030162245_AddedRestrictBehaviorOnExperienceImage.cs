using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedRestrictBehaviorOnExperienceImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Images_ImageId",
                table: "Experiences");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_Images_ImageId",
                table: "Experiences",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Images_ImageId",
                table: "Experiences");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_Images_ImageId",
                table: "Experiences",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

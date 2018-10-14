using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedAuthorIdToAuthorProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AuthorsProfile_AuthorProfileId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AuthorProfileId",
                table: "Authors");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "AuthorsProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsProfile_AuthorId",
                table: "AuthorsProfile",
                column: "AuthorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorsProfile_Authors_AuthorId",
                table: "AuthorsProfile",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorsProfile_Authors_AuthorId",
                table: "AuthorsProfile");

            migrationBuilder.DropIndex(
                name: "IX_AuthorsProfile_AuthorId",
                table: "AuthorsProfile");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "AuthorsProfile");

            migrationBuilder.AddColumn<int>(
                name: "AuthorProfileId",
                table: "Authors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AuthorsProfile_AuthorProfileId",
                table: "Authors",
                column: "AuthorProfileId",
                principalTable: "AuthorsProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

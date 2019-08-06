using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedMetaTagsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetaTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Value = table.Column<string>(maxLength: 1000, nullable: false),
                    BlogPostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaTags_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetaTags_BlogPostId",
                table: "MetaTags",
                column: "BlogPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetaTags");
        }
    }
}

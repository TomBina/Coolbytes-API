using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedMailProviderAndMailStat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DailyThreshold = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MailproviderId = table.Column<int>(type: "int", nullable: false),
                    Sent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailStats_MailProviders_MailproviderId",
                        column: x => x.MailproviderId,
                        principalTable: "MailProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailProviders_Name",
                table: "MailProviders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailStats_MailproviderId",
                table: "MailStats",
                column: "MailproviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailStats");

            migrationBuilder.DropTable(
                name: "MailProviders");
        }
    }
}

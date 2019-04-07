using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolBytes.Data.Migrations
{
    public partial class AddedSortOrderToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            string sql = @"SELECT		Id
                           INTO		    #Categories 
                           FROM         Categories
                           ORDER BY     Id ASC

                           DECLARE @Id int, @SortOrder int = 0;

                           WHILE (SELECT COUNT(*) FROM #Categories) > 0
                           BEGIN
                                SET @Id = (SELECT TOP(1) Id FROM #Categories ORDER BY Id ASC)
                                SET @SortOrder = @SortOrder + 1

                                UPDATE Categories
                                SET SortOrder = @SortOrder
                                WHERE Id = @Id
                            
                                DELETE FROM 	#Categories
                                WHERE           Id = @Id
                           END

                           DROP TABLE #Categories";

            migrationBuilder.Sql(sql);
            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Categories");
        }
    }
}

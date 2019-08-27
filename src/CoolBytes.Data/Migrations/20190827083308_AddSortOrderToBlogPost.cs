using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolBytes.Data.Migrations
{
    public partial class AddSortOrderToBlogPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "BlogPosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"DECLARE @BlogPostId int, @CategoryId int, @SortOrder int = 0;
                                CREATE TABLE	#BlogPosts
				                                (Id int NOT NULL)

                                SELECT			Id
                                INTO			#Categories 
                                FROM			Categories
                                ORDER BY		Id ASC

                                WHILE (SELECT COUNT(*) FROM #Categories) > 0
                                BEGIN
                                    SET @CategoryId = (SELECT TOP(1) Id FROM #Categories ORDER BY Id ASC)

	                                INSERT INTO		#BlogPosts
					                                (Id)
	                                SELECT			Id
	                                FROM			BlogPosts
	                                WHERE			CategoryId = @CategoryId
	                                ORDER BY		Id ASC

	                                SET @SortOrder = 0;

	                                WHILE (SELECT COUNT(*) FROM #BlogPosts) > 0
		                                BEGIN
			                                SET @BlogPostId = (SELECT TOP(1) Id FROM #BlogPosts ORDER BY Id ASC)

			                                SET				@SortOrder = @SortOrder + 1
			                                UPDATE			BlogPosts
			                                SET				SortOrder = @SortOrder
			                                WHERE			Id = @BlogPostId

			                                DELETE FROM		#BlogPosts
			                                WHERE			Id = @BlogPostId
		                                END
	                                                        
                                    DELETE FROM 	#Categories
                                    WHERE           Id = @CategoryId
                                END

                                DROP TABLE #Categories
                                DROP TABLE #BlogPosts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "BlogPosts");
        }
    }
}

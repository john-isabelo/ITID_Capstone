using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeeklyTask.Migrations.FoodDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "ImagePath", "Ingredients", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "/images/pizza.jpg", "ham, pineapple, tomatoes", "Pizza", 10.99m },
                    { 2, "/images/burger.jpg", "beef, egg, cheese", "Hamburger", 8.99m },
                    { 3, "/images/friedchicken.jpg", "flour, egg, sauce", "Fried Chicken", 12.99m },
                    { 4, "/images/spaghetti.jpg", "tomatoes, sausage, cheese", "Spaghetti", 9.99m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Foods");
        }
    }
}

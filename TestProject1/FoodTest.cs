using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeeklyTask.Controllers;
using WeeklyTask.Models;
using Xunit;

namespace WeeklyTask.Tests.Controllers
{
    public class FoodsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResultWithListOfFoods()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FoodDbContext>()
                .UseInMemoryDatabase(databaseName: "Index_ReturnsViewResultWithListOfFoods")
                .Options;

            using (var context = new FoodDbContext(options))
            {
                context.Foods.Add(new Food { Id = 1, Name = "Food1", Ingredients = "Ingredient1" });
                context.Foods.Add(new Food { Id = 2, Name = "Food2", Ingredients = "Ingredient2" });
                context.Foods.Add(new Food { Id = 3, Name = "Food3", Ingredients = "Ingredient3" });
                context.SaveChanges();
            }

            List<Food> expectedFoods;

            using (var context = new FoodDbContext(options))
            {
                expectedFoods = await context.Foods.ToListAsync();
            }

            using (var context = new FoodDbContext(options))
            {
                var controller = new FoodsController(context, null);

                // Act
                var result = await controller.Index(1);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Food>>(viewResult.Model);
                Assert.Equal(expectedFoods, model, new FoodEqualityComparer());
            }
        }
    }
}

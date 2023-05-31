using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeeklyTask.Controllers;
using WeeklyTask.Infrastructure;
using WeeklyTask.Models;
using WeeklyTask.Models.ViewModels;
using Xunit;

namespace WeeklyTask.Tests.Controllers
{
    public class CartControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FoodDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new FoodDbContext(options);

            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Price = 10, Quantity = 2 },
                new CartItem { ProductId = 2, Price = 20, Quantity = 1 }
            };

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            httpContext.Session.SetJson("Cart", cartItems);

            var controller = new CartController(context)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CartViewModel>(viewResult.Model);

            // Compare the properties of the objects within the collections
            Assert.Collection(model.CartItems,
                item =>
                {
                    Assert.Equal(cartItems[0].ProductId, item.ProductId);
                    Assert.Equal(cartItems[0].Price, item.Price);
                    Assert.Equal(cartItems[0].Quantity, item.Quantity);
                },
                item =>
                {
                    Assert.Equal(cartItems[1].ProductId, item.ProductId);
                    Assert.Equal(cartItems[1].Price, item.Price);
                    Assert.Equal(cartItems[1].Quantity, item.Quantity);
                }
            );
            Assert.Equal(40, model.GrandTotal);
        }
    }
}

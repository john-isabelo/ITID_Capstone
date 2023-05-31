using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using WeeklyTask.Controllers;
using WeeklyTask.Areas.Identity.Data;
using WeeklyTask.Models.Helpers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeeklyTask.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace WeeklyTask.Tests.Controllers
{
    public class ReservationTest
    {
        [Fact]
        public async Task CreateReservation_ReturnsJsonResult()
        {
            // Arrange
            var reservation = new Reservation
            {
                Id = 1,
                ReservationDate = DateTime.Now,
                PartySize = 4,
                Notes = "Table near window",
                ContactName = "John Doe",
                ContactPhone = "123-456-7890"
            };

            var options = new DbContextOptionsBuilder<FoodDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateReservation")
                .Options;

            var smtpSettings = new SmtpSettings
            {
                Server = "smtp-relay.sendinblue.com",
                Port = 587,
                Username = "nelson.chasemedia@gmail.com",
                Password = "1AfpZW8CwnDJ2vkj"
            };

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"SmtpSettings:Server", smtpSettings.Server},
                {"SmtpSettings:Port", smtpSettings.Port.ToString()},
                {"SmtpSettings:Username", smtpSettings.Username},
                {"SmtpSettings:Password", smtpSettings.Password}
            });

            var configuration = configurationBuilder.Build();

            await using var context = new FoodDbContext(options);
            var controller = new ReservationsController(context, configuration);

            // Act
            var result = await controller.Create(reservation);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(jsonResult.Value));
            Assert.NotNull(jsonData);
            Assert.True((bool)jsonData["success"]);
        }

    }
}

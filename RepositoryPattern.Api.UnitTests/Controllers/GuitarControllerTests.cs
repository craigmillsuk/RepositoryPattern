using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RepositoryPattern.Api.ResponseModels;
using RepositoryPattern.Controllers;
using RepositoryPattern.Domain.Interfaces;
using RepositoryPattern.Domain.Models;
using Xunit;

namespace RepositoryPattern.Api.UnitTests.Controllers

{
    public class GuitarControllerTests
    {
        [Fact]
        public async Task GetGuitar_Success_ReturnsGuitarResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var guitar = new Guitar

            {
                Id = id,
                Make = "Fender",
                Model = "Stratocaster",
                NumberOfFrets = 22,
                StringGauge = "10-46",
                Price = 1200
            };

            var logger = Substitute.For<ILogger<GuitarController>>();
            var guitarService = Substitute.For<IGuitarService>();

            var controller = new GuitarController(logger, guitarService);
            guitarService.GetGuitar(id).Returns(guitar);

            // Act
            var result = await controller.GetGuitar(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GuitarResponse>(okResult.Value);
            Assert.Equal(guitar.Id, response.Id);
            Assert.Equal(guitar.Make, response.Make);
            Assert.Equal(guitar.Model, response.Model);
            Assert.Equal(guitar.NumberOfFrets, response.NumberOfFrets);
            Assert.Equal(guitar.StringGauge, response.StringGauge);
            Assert.Equal(guitar.Price, response.Price);
        }

        [Fact]
        public async Task GetGuitar_Failure_IdEmptyReturnsBadRequestAndLogsError()
        {
            // Arrange
            var id = Guid.Empty;
            var errorMessage = $"Invalid guitar ID provided (empty GUID).";

            var logger = Substitute.For<ILogger<GuitarController>>();
            var guitarService = Substitute.For<IGuitarService>();

            var controller = new GuitarController(logger, guitarService);
            guitarService.GetGuitar(id).ReturnsNull();

            // Act
            var result = await controller.GetGuitar(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestResult.Value, "Guitar ID cannot be empty.");
            logger.Received().LogError(errorMessage);
        }

        [Fact]
        public async Task GetAllGuitars_Success_ReturnsGuitarResponse()
        {
            // Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var guitars = new List<Guitar>()

            {
                new()
                {
                    Id = id2,
                    Make = "Gibson",
                    Model = "Les Paul",
                    NumberOfFrets = 22,
                    StringGauge = "10-46",
                    Price = 1200
                },
                new()
                {
                    Id = id2,
                    Make = "Fender",
                    Model = "Stratocaster",
                    NumberOfFrets = 22,
                    StringGauge = "10-46",
                    Price = 800
                }
            };

            var logger = Substitute.For<ILogger<GuitarController>>();
            var guitarService = Substitute.For<IGuitarService>();

            var controller = new GuitarController(logger, guitarService);
            guitarService.GetAllGuitars().Returns(guitars);

            // Act
            var result = await controller.GetAllGuitars();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equivalent(guitars, okResult.Value);
        }

        [Fact]
        public async Task GetAllGuitars__Failure_NoGuitarsFoundReturnsNotFoundAndLogsError()
        {
            // Arrange
            var id = Guid.Empty;
            var errorMessage = $"No guitars found in the database.";

            var logger = Substitute.For<ILogger<GuitarController>>();
            var guitarService = Substitute.For<IGuitarService>();

            var controller = new GuitarController(logger, guitarService);
            guitarService.GetAllGuitars().ReturnsNull();

            // Act
            var result = await controller.GetAllGuitars();

            // Assert
            var notFoundRequestResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(notFoundRequestResult.Value, "No guitars found.");
            logger.Received().LogError(errorMessage);
        }
    }
}

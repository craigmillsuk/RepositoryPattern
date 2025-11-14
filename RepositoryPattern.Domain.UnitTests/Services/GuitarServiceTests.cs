using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;
using RepositoryPattern.Domain.Services;
using RepositoryPattern.Repository.Interfaces;
using RepositoryPattern.Repository.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;


namespace RepositoryPattern.Domain.UnitTests.Services
{
    public class GuitarServiceTests
    {
        #region GetGuitar

        [Fact]
        public async Task GetGuitar_Success_ReturnsGuitar()
        {
            // Arrange
            var id = Guid.NewGuid();
            var guitarDTO = new GuitarDTO
            {
                Id = id,
                Make = "Gibson",
                Model = "Les Paul",
                NumberOfFrets = 24,
                StringGauge = "10-48",
                Price = 1000
            };

            var logger = Substitute.For<ILogger<GuitarService>>();
            var guitarDetails = Substitute.For<IGuitarDetails>();
            guitarDetails.GetGuitarAsync(id).Returns(guitarDTO);

            var guitarService = new GuitarService(logger, guitarDetails);

            // Act
            var result = await guitarService.GetGuitar(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(guitarDTO, result);
            logger.DidNotReceive().LogError($"No guitar found with ID: {id}");
        }

        [Fact]
        public async Task GetGuitar_Falure_ThrowsValidationException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var errorMessage = $"No guitar found with Id: {id}";

            var logger = Substitute.For<ILogger<GuitarService>>();
            var guitarDetails = Substitute.For<IGuitarDetails>();
            guitarDetails.GetGuitarAsync(id).ReturnsNull();

            var guitarService = new GuitarService(logger, guitarDetails);

            // Act & Assert
           var result = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await guitarService.GetGuitar(id);
            });

            Assert.Equal(errorMessage, result.Message);
            logger.Received().LogError(errorMessage);
        }

        #endregion

        #region GetAllGuitars

        [Fact]
        public async Task GetAllGuitars_Success_ReturnsGuitar()
        {
            // Arrange
            var id = Guid.NewGuid();
            var guitarDTO = new List<GuitarDTO>
            {
                new()
                {
                    Id = id,
                    Make = "Gibson",
                    Model = "Les Paul",
                    NumberOfFrets = 24,
                    StringGauge = "10-48",
                    Price = 1000
                },
                  new()
                {
                    Id = id,
                    Make = "Fender",
                    Model = "Stratocaster",
                    NumberOfFrets = 24,
                    StringGauge = "10-48",
                    Price = 800
                }
            };

            var logger = Substitute.For<ILogger<GuitarService>>();
            var guitarDetails = Substitute.For<IGuitarDetails>();
            guitarDetails.GetAllGuitarAsync().Returns(guitarDTO);

            var guitarService = new GuitarService(logger, guitarDetails);

            // Act
            var result = await guitarService.GetAllGuitars();

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(guitarDTO, result);
            logger.DidNotReceive().LogError($"No guitars found!");
        }

        [Fact]
        public async Task GetAllGuitars_Falure_ThrowsValidationException()
        {
            // Arrange
            var logger = Substitute.For<ILogger<GuitarService>>();
            var guitarDetails = Substitute.For<IGuitarDetails>();
            var errorMessage = "No guitars found!";

            guitarDetails.GetAllGuitarAsync().ReturnsNull();

            var guitarService = new GuitarService(logger, guitarDetails);

            // Act & Assert
            var result = await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await guitarService.GetAllGuitars();
            });

            Assert.Equal(errorMessage, result.Message);
            logger.Received().LogError(errorMessage);
        }

        #endregion
    }
}

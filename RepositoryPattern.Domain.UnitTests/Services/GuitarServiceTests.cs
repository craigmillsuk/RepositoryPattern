using NSubstitute;
using RepositoryPattern.Domain.Interfaces;
using RepositoryPattern.Domain.Models;
using RepositoryPattern.Repository.Interfaces;
using RepositoryPattern.Repository.Models;
using Xunit;


namespace RepositoryPattern.Domain.UnitTests.Services
{
    public class GuitarServiceTests
    {
        [Fact]
        public async Task GetGuitarDetails_Success_ReturnsGuitar()
        {
            //Arrange
            var id = Guid.NewGuid();
            var guitarDTO = new GuitarDTO()
            {
                Id = id,
                Make = "Gibson",
                Model = "Leas Paul",
                NumberOfFrets = 24,
                StringGauge = "10-48",
                Price = 1000
            };

            var guitar = new Guitar()
            {
                Id = id,
                Make = "Gibson",
                Model = "Leas Paul",
                NumberOfFrets = 24,
                StringGauge = "10-48",
                Price = 1000
            };
            var guitarService = Substitute.For<IGuitarService>();
            var guitarDetails = Substitute.For<IGuitarDetails>();

            guitarDetails.GetGuitarAsync(id).Returns(guitarDTO);
            guitarService.GetGuitarDetails(id).Returns(guitar);

            //Act
            var result = await guitarService.GetGuitarDetails(id);

            //Assert
            Assert.NotNull(result); 
            Assert.Equivalent(guitar, result);
        }

    }
}

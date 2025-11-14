using Microsoft.Azure.Cosmos;
using NSubstitute;
using RepositoryPattern.Repository.Models;
using RepositoryPattern.Repository.Repositories;
using Xunit;
using Container = Microsoft.Azure.Cosmos.Container;

namespace RepositoryPattern.Repository.UnitTests.Repositories
{
    public class GuitarDetailsTests
    {
        [Fact]
        public async Task GetGuitarAsync_ReturnsGuitar_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var expected = new GuitarDTO
            {
                Id = id,
                Model = "Telecaster",
                Make = "Fender",
                NumberOfFrets = 24,
                StringGauge = "10-48",
                Price = 800
            };

            // Mock CosmosClient, Database, Container
            var client = Substitute.For<CosmosClient>();
            var database = Substitute.For<Database>();
            var container = Substitute.For<Container>();

            client.GetDatabase("GuitarDb").Returns(database);
            database.GetContainer("GuitarDetails").Returns(container);

            // Mock FeedIterator & FeedResponse
            var iterator = Substitute.For<FeedIterator<GuitarDTO>>();
            var response = Substitute.For<FeedResponse<GuitarDTO>>();

            container
                .GetItemQueryIterator<GuitarDTO>(Arg.Any<QueryDefinition>())
                .Returns(iterator);

            iterator.HasMoreResults.Returns(true, false); // Loop once
            iterator.ReadNextAsync(Arg.Any<CancellationToken>())
                    .Returns(response);

            response.GetEnumerator()
                    .Returns(((IEnumerable<GuitarDTO>)new[] { expected }).GetEnumerator());

            var service = new GuitarDetails(client);

            // Act
            var result = await service.GetGuitarAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result!.Id);
            Assert.Equal(expected.Model, result.Model);
            Assert.Equal(expected.Make, result.Make);
            Assert.Equal(expected.NumberOfFrets, result.NumberOfFrets);
            Assert.Equal(expected.StringGauge, result.StringGauge);
            Assert.Equal(expected.Price, result.Price);
        }

        [Fact]
        public async Task GetGuitarAsync_ReturnsEmptyGuitar_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var client = Substitute.For<CosmosClient>();
            var database = Substitute.For<Database>();
            var container = Substitute.For<Container>();

            client.GetDatabase("GuitarDb").Returns(database);
            database.GetContainer("GuitarDetails").Returns(container);

            var iterator = Substitute.For<FeedIterator<GuitarDTO>>();
            var response = Substitute.For<FeedResponse<GuitarDTO>>();

            container
                .GetItemQueryIterator<GuitarDTO>(Arg.Any<QueryDefinition>())
                .Returns(iterator);

            iterator.HasMoreResults.Returns(true, false);
            iterator.ReadNextAsync(Arg.Any<CancellationToken>())
                    .Returns(response);

            // Empty response
            response.GetEnumerator()
                    .Returns((new List<GuitarDTO>()).GetEnumerator());

            var service = new GuitarDetails(client);

            // Act
            var result = await service.GetGuitarAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result!.Id);
        }
    }
}

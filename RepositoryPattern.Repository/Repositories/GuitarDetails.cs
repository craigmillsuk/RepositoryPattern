using Microsoft.Azure.Cosmos;
using RepositoryPattern.Repository.Interfaces;
using RepositoryPattern.Repository.Models;

namespace RepositoryPattern.Repository.Repositories
{
    public class GuitarDetails : IGuitarDetails
    {
        private readonly Container _container;

        public GuitarDetails(CosmosClient client)
        {
            _container = client.GetDatabase("GuitarDb")
                               .GetContainer("GuitarDetails");
        }

        public async Task<GuitarDTO?> GetGuitarAsync(Guid id)
        {
            var query = "SELECT * FROM c WHERE c.id = @id";
            var queryDefinition = new QueryDefinition(query)
                                  .WithParameter("@id", id);

            var iterator = _container.GetItemQueryIterator<GuitarDTO>(queryDefinition);
            var results = new List<GuitarDTO>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results.FirstOrDefault() ?? new GuitarDTO();
        }

        public async Task<List<GuitarDTO>?> GetAllGuitarAsync()
        {
            var query = "SELECT * FROM c";
            var queryDefinition = new QueryDefinition(query);

            var iterator = _container.GetItemQueryIterator<GuitarDTO>(queryDefinition);
            var results = new List<GuitarDTO>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}
using Microsoft.Extensions.Logging;
using RepositoryPattern.Domain.Interfaces;
using RepositoryPattern.Domain.Models;
using RepositoryPattern.Repository.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RepositoryPattern.Domain.Services
{
    /// <summary>
    /// Class to handle guitar related services
    /// </summary>
    public class GuitarService : IGuitarService
    {
        private readonly ILogger<GuitarService> _logger;
        private IGuitarDetails _guitarDetails;

        public GuitarService(ILogger<GuitarService> logger, 
            IGuitarDetails guitarDetails) {
            _logger = logger;
            _guitarDetails = guitarDetails;
        }

        public async Task<Guitar> GetGuitar(Guid id)
        {
            var guitar = await _guitarDetails.GetGuitarAsync(id);

            if (guitar != null)
            {
                var guitarModel = new Guitar()
                {
                    Id = guitar.Id,
                    Make = guitar.Make,
                    Model = guitar.Model,
                    NumberOfFrets = guitar.NumberOfFrets,
                    StringGauge = guitar.StringGauge,
                    Price = guitar.Price
                };

                return guitarModel;
            }

            _logger.LogError($"No guitar found with Id: {id}");
            throw new ValidationException($"No guitar found with Id: {id}");
        }

        public async Task<List<Guitar>> GetAllGuitars()
        {
            var allGuitars = await _guitarDetails.GetAllGuitarAsync();
            var guitars = new List<Guitar>();

            if(allGuitars == null)
            {
                _logger.LogError($"No guitars found!");
                throw new ValidationException($"No guitars found!");
            }

            foreach (var item in allGuitars)
            {

                guitars.Add(new Guitar
                {
                    Id = item.Id,
                    Make = item.Make,
                    Model = item.Model,
                    NumberOfFrets = item.NumberOfFrets,
                    StringGauge = item.StringGauge,
                    Price = item.Price
                });
            }

            return guitars;
        }
    }
}
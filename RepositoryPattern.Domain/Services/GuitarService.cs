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
        private IGuitarDetails _guitarDetails;
        public GuitarService(IGuitarDetails guitarDetails) {
            _guitarDetails = guitarDetails;
        }

        public async Task<Guitar> GetGuitarDetails(Guid id)
        {
            var guitarDetails = await _guitarDetails.GetGuitarAsync(id);

            if (guitarDetails != null)
            {
                var guitar = new Guitar()
                {
                    Id = guitarDetails.Id,
                    Make = guitarDetails.Make,
                    Model = guitarDetails.Model,
                    NumberOfFrets = guitarDetails.NumberOfFrets,
                    StringGauge = guitarDetails.StringGauge,
                    Price = guitarDetails.Price
                };

                return guitar;
            }

            throw new ValidationException($"No guitar found for ID: {id}");
        }

        public async Task<List<Guitar>> GetAllGuitars()
        {
            var guitarDetails = await _guitarDetails.GetAllGuitarAsync();
            var guitars = new List<Guitar>();

            if(guitarDetails == null)
            {
                throw new ValidationException($"No guitars found!");
            }

            foreach (var item in guitarDetails)
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
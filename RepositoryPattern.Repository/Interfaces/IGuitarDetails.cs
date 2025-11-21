using RepositoryPattern.Repository.Models;
using System.Net;
using System.Threading.Tasks;

namespace RepositoryPattern.Repository.Interfaces
{
    public interface IGuitarDetails
    {
        // The Get Guitar Async method
        Task<GuitarDTO?> GetGuitarAsync(Guid id);

        // The Get All Guitar Async method
        Task<List<GuitarDTO>?> GetAllGuitarAsync();

        Task<HttpStatusCode> CreateGuitarAsync(GuitarDTO guitar);
    }
}

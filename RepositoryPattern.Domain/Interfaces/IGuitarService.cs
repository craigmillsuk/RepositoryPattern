using RepositoryPattern.Domain.Models;

namespace RepositoryPattern.Domain.Interfaces
{
    public interface IGuitarService
    {
        // The Get Guitar Details method
        Task<Guitar> GetGuitarDetails(Guid id);

        // The Get All Guitars method
        Task<List<Guitar>> GetAllGuitars();
    }
}

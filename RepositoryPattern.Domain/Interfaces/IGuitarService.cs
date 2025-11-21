using RepositoryPattern.Domain.Models;
using System.Net;

namespace RepositoryPattern.Domain.Interfaces
{
    public interface IGuitarService
    {
        // The Get Guitar Details method
        Task<Guitar> GetGuitar(Guid id);

        // The Get All Guitars method
        Task<List<Guitar>> GetAllGuitars();

        // Create a new guitar
        Task<HttpStatusCode> CreateGuitar(Guitar guitar);
    }
}

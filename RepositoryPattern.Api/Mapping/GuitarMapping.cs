using RepositoryPattern.Api.RequestModels;
using RepositoryPattern.Api.ResponseModels;
using RepositoryPattern.Domain.Models;
using RepositoryPattern.Repository.Models;

namespace RepositoryPattern.Api.Mapping
{
    public static class GuitarMapping
    {
        public static Guitar ToDomain(this GuitarRequest request) =>
            new()
            {
                Id = request.Id,
                Make = request.Make,
                Model = request.Model,
                NumberOfFrets = request.NumberOfFrets,
                StringGauge = request.StringGauge,
                Price = request.Price
            };

        public static GuitarResponse ToResponse(this Guitar domain) =>
            new()
            {
                Id = domain.Id,
                Make = domain.Make,
                Model = domain.Model,
                NumberOfFrets = domain.NumberOfFrets,
                StringGauge = domain.StringGauge,
                Price = domain.Price
            };
    }

}

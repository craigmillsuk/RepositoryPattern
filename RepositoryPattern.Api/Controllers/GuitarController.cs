using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Api.ResponseModels;
using RepositoryPattern.Domain.Interfaces;

namespace RepositoryPattern.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GuitarController : ControllerBase
    {
        private readonly ILogger<GuitarController> _logger;
        private IGuitarService _guitarService;

        public GuitarController(ILogger<GuitarController> logger, 
            IGuitarService guitarService)
        {
            _logger = logger;
            _guitarService = guitarService;
        }

        [HttpGet("GetGuitarById")]
        public async Task<IActionResult> GetGuitar([FromQuery] Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid guitar ID provided (empty GUID).");
                return BadRequest("Guitar ID cannot be empty.");
            }

            var guitarDetails = await _guitarService.GetGuitarDetails(id);

            var response = new GuitarResponse
            {
                Id = guitarDetails.Id,
                Make = guitarDetails.Make,
                Model = guitarDetails.Model,
                NumberOfFrets = guitarDetails.NumberOfFrets,
                StringGauge = guitarDetails.StringGauge,
                Price = guitarDetails.Price
            };

            return Ok(response);
        }

        [HttpGet("GetAllGuitars")]
        public async Task<IActionResult> GetAllGuitars()
        {
            var guitars = await _guitarService.GetAllGuitars();

            if (guitars == null || !guitars.Any())
            {
                _logger.LogInformation("No guitars found in the database.");
                return NotFound("No guitars found.");
            }

            var response = guitars.Select(g => new GuitarResponse
            {
                Id = g.Id,
                Make = g.Make,
                Model = g.Model,
                NumberOfFrets = g.NumberOfFrets,
                StringGauge = g.StringGauge,
                Price = g.Price
            }).ToList();

            return Ok(response);
        }
    }
}

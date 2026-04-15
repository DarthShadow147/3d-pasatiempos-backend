using _3d_pasatiempos_backend.Application.Dtos.Production;
using _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly IProductionService _Service;

        public ProductionController(IProductionService Service)
        {
            _Service = Service;
        }

        [HttpPut("{pProductionId}/complete")]
        public async Task<IActionResult> CompleteProduction(int pProductionId, [FromBody] CompleteProductionRequest pRequest)
        {
            try
            {
                await _Service.CompleteProductionAsync(pProductionId, pRequest);
                return NoContent();
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }
    }
}

using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.ProductionDto;
using _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly IProductionService _ProductionService;

        public ProductionController(IProductionService ProductionService)
        {
            _ProductionService = ProductionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductionRecords([FromQuery] QueryParams pQuery)
        {
            var lProductionData = await _ProductionService.GetPagedProductionAsync(pQuery);
            return Ok(lProductionData);
        }

        [HttpGet("{pProductionId}")]
        public async Task<IActionResult> GetProductionDetail(int pProductionId)
        {
            var lProductionData = await _ProductionService.GetProductionDetailAsync(pProductionId);
            return Ok(lProductionData);
        }

        [HttpPut("{pProductionId}/complete")]
        public async Task<IActionResult> CompleteProduction(int pProductionId, [FromBody] CompleteProductionDto pRequest)
        {
            await _ProductionService.CompleteProductionAsync(pProductionId, pRequest);
            return NoContent();
        }
    }
}

using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.Utils;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly IUtilsService _UtilsService;

        public UtilsController(IUtilsService UtilsService)
        {
            _UtilsService = UtilsService;
        }

        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer([FromQuery] QueryParams pQuery)
        {
            var lQueryData = await _UtilsService.GetCustomersAsync(pQuery);
            return Ok(lQueryData);
        }

        [HttpGet("GetMaterial")]
        public async Task<IActionResult> GetMaterial([FromQuery] QueryParams pQuery)
        {
            var lQueryData = await _UtilsService.GetMaterialAsync(pQuery);
            return Ok(lQueryData);
        }

        [HttpGet("GetPrinter")]
        public async Task<IActionResult> GetPrinter([FromQuery] QueryParams pQuery)
        {
            var lQueryData = await _UtilsService.GetPrinterAsync(pQuery);
            return Ok(lQueryData);
        }
    }
}

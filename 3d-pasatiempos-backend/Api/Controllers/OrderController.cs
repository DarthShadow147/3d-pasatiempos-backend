using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.OrderDto;
using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _OrderService;

        public OrderController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderRecords([FromQuery] QueryParams pQuery)
        {
            var lQuoteData = await _OrderService.GetPagedOrderAsync(pQuery);
            return Ok(lQuoteData);
        }

        [HttpPost("CreateProduction")]
        public async Task<IActionResult> CreateStandAloneProduction(CreateProductionDto pRequest,
            [FromServices] IValidator<CreateProductionDto> pValidator)
        {
            var lValidationTx = await pValidator.ValidateAsync(pRequest);
            if (!lValidationTx.IsValid)
                return BadRequest(lValidationTx.Errors);

            var lProductionId = await _OrderService.CreateStandAloneProduction(pRequest);
            return Ok(lProductionId);
        }

        [HttpPut("{pOrderId}/start")]
        public async Task<IActionResult> StartOrder(int pOrderId)
        {
            await _OrderService.StartOrderAsync(pOrderId);
            return NoContent();
        }
    }
}

using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _Service;

        public OrderController(IOrderService Service)
        {
            _Service = Service;
        }

        [HttpPut("{OrderId}/start")]
        public async Task<IActionResult> StartOrder(int OrderId)
        {
            try
            {
                await _Service.StartOrderAsync(OrderId);
                return NoContent();
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }
    }
}

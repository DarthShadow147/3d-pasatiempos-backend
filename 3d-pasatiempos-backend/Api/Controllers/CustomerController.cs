using _3d_pasatiempos_backend.Application.Dtos.Customer;
using _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _Service;

        public CustomerController(ICustomerService Service)
        {
            _Service = Service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest Request)
        {
            var lResult = await _Service.CreateAsync(Request);
            return Ok(lResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var lResult = await _Service.GetAllAsync();
            return Ok(lResult);
        }
    }
}

using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _CustomerService;

        public CustomerController(ICustomerService CustomerService)
        {
            _CustomerService = CustomerService;
        }

        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto pRequest, 
            [FromServices] IValidator<CreateCustomerDto> pValidator)
        {
            var lValidationTx = await pValidator.ValidateAsync(pRequest);
            if (!lValidationTx.IsValid)
                return BadRequest(lValidationTx.Errors);

            var lCreateTx = await _CustomerService.CreateAsync(pRequest);
            return Ok(lCreateTx);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecords([FromQuery] QueryParams pQuery)
        {
            var lCustomerData = await _CustomerService.GetPagedAsync(pQuery);
            return Ok(lCustomerData);
        }

        [HttpPatch("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CreateCustomerDto pRequest,
            [FromServices] IValidator<CreateCustomerDto> pValidator)
        {
            var lValidationTx = await pValidator.ValidateAsync(pRequest);
            if (!lValidationTx.IsValid)
                return BadRequest(lValidationTx.Errors);

            await _CustomerService.UpdateCustomerAsync(pRequest);
            return NoContent();
        }
    }
}

using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.PaymentDto;
using _3d_pasatiempos_backend.Application.Interfaces.PaymentInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _PaymentService;

        public PaymentController(IPaymentService PaymentService)
        {
            _PaymentService = PaymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentRecords([FromQuery] QueryParams pQuery)
        {
            var lPaymentData = await _PaymentService.GetPagedPaymentAsync(pQuery);
            return Ok(lPaymentData);
        }

        [HttpGet("{pPaymentId}")]
        public async Task<IActionResult> GetPaymentDetail(int pPaymentId)
        {
            var lPaymentData = await _PaymentService.GetPaymentDetailAsync(pPaymentId);
            return Ok(lPaymentData);
        }

        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto pRequest,
            [FromServices] IValidator<CreatePaymentDto> pValidator)
        {
            var lValidatorTx = await pValidator.ValidateAsync(pRequest);
            if (!lValidatorTx.IsValid)
                return BadRequest(lValidatorTx.Errors);

            var lCreateTx = await _PaymentService.CreatePaymentAsync(pRequest);
            return Ok(lCreateTx);
        }
    }
}

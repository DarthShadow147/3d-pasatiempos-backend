using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.QuoteDto;
using _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _QuoteService;

        public QuoteController(IQuoteService QuoteService)
        {
            _QuoteService = QuoteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuoteRecords([FromQuery] QueryParams pQuery)
        {
            var lQuoteData = await _QuoteService.GetPagedQuoteAsync(pQuery);
            return Ok(lQuoteData);
        }

        [HttpGet("{pQuoteId}")]
        public async Task<IActionResult> GetQuoteDetail(int pQuoteId)
        {
            var lQuoteData = await _QuoteService.GetQuoteDetailAsync(pQuoteId);
            return Ok(lQuoteData);
        }

        [HttpPost("CreateQuote")]
        public async Task<IActionResult> CreateQuote([FromBody] CreateQuoteDto pRequest,
            [FromServices] IValidator<CreateQuoteDto> pValidator)
        {
            var lValidationTx = await pValidator.ValidateAsync(pRequest);
            if (!lValidationTx.IsValid)
                return BadRequest(lValidationTx.Errors);

            var lCreateTx = await _QuoteService.CreateQuoteAsync(pRequest);
            return Ok(lCreateTx);
        }

        [HttpPut("{pQuoteId}/approve")]
        public async Task<IActionResult> Approve(int pQuoteId)
        {
            await _QuoteService.ApproveAsync(pQuoteId);
            return NoContent();
        }

        [HttpPut("{pQuoteId}/reject")]
        public async Task<IActionResult> Reject(int pQuoteId, [FromBody] RejectQuoteDto pRequest)
        {
            await _QuoteService.RejectAsync(pQuoteId, pRequest.RejectReason);
            return NoContent();
        }
    }
}

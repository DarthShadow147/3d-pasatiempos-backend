using _3d_pasatiempos_backend.Application.Dtos.Quote;
using _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _Service;

        public QuotesController(IQuoteService Service)
        {
            _Service = Service;
        }

        [HttpGet("GetQuoteResume")]
        public async Task<IActionResult> GetQuoteResume([FromQuery] List<string> pStatus = null)
        {
            try
            {
                var lResult = await _Service.GetAllAsync(pStatus);
                return Ok(lResult);
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }

        [HttpGet("{pQuoteId}")]
        public async Task<IActionResult> GetQuoteDetail(int pQuoteId)
        {
            try
            {
                var lResult = await _Service.GetDetailByIdAsync(pQuoteId);
                return Ok(lResult);
            }
            catch (Exception lEx)
            {
                return NotFound(lEx.Message);
            }
        }

        [HttpPost("CreateQuote")]
        public async Task<IActionResult> CreateQuote([FromBody] CreateQuoteRequest pRequest)
        {
            try
            {
                var lResult = await _Service.CreateQuoteAsync(pRequest);
                if (lResult)
                    return Ok(lResult);
                else
                    return BadRequest("Error to generate quote");
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }

        [HttpPut("{pQuoteId}/approve")]
        public async Task<IActionResult> Approve(int pQuoteId)
        {
            try
            {
                await _Service.ApproveAsync(pQuoteId);
                return NoContent();
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }

        [HttpPut("{pQuoteId}/reject")]
        public async Task<IActionResult> Reject(int pQuoteId, [FromBody] RejectQuoteRequest pRequest)
        {
            try
            {
                await _Service.RejectAsync(pQuoteId, pRequest.RejectReason);
                return NoContent();
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }
    }
}

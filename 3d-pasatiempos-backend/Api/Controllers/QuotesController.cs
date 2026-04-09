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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuoteRequest pRequest)
        {
            var lResult = await _Service.CreateQuoteAsync(pRequest);
            return Ok(lResult);
        }
    }
}

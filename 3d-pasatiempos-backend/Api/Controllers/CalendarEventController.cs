using _3d_pasatiempos_backend.Application.Dtos.CalendarEventsDto;
using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.CalendarEventInterfaces;
using _3d_pasatiempos_backend.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventController : ControllerBase
    {
        private readonly ICalendarEventsService _CalendarEventService;

        public CalendarEventController(ICalendarEventsService CalendarEventService)
        {
            _CalendarEventService = CalendarEventService;
        }

        [HttpPost("CreateCalendarEvent")]
        public async Task<IActionResult> CreateCalendarEvent([FromBody] CreateEventDto pRequest, 
            [FromQuery] EventType pType,
            [FromServices] IValidator<CreateEventDto> pValidator)
        {
            var lValidationTx = await pValidator.ValidateAsync(pRequest);
            if (!lValidationTx.IsValid)
                return BadRequest(lValidationTx.Errors);

            if (!Enum.IsDefined(pType))
                return BadRequest("The event type is invalid");

            var lCreateTx = await _CalendarEventService.CreateAsync(pRequest, pType);
            return Ok(lCreateTx);
        }

        [HttpGet]
        public async Task<IActionResult> GetCalendarEventsRecords([FromQuery] QueryParams pQuery)
        {
            var lEventData = await _CalendarEventService.GetPagedAsync(pQuery);
            return Ok(lEventData);
        }
    }
}

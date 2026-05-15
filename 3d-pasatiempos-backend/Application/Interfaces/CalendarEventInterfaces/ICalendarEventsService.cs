using _3d_pasatiempos_backend.Application.Dtos.CalendarEventsDto;
using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Interfaces.CalendarEventInterfaces
{
    public interface ICalendarEventsService
    {
        Task<int> CreateAsync(CreateEventDto pRequest, EventType pType);
        Task<PagedResult<CreateEventDto>> GetPagedAsync(QueryParams pQuery);
    }
}

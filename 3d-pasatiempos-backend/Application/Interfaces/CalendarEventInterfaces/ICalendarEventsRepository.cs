using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.CalendarEventInterfaces
{
    public interface ICalendarEventsRepository
    {
        Task AddAsync(CalendarEvent pCalendarEvent);
        Task<(List<CalendarEvent> Data, int TotalCount)> GetPagedAsync(QueryParams pQuery);
    }
}

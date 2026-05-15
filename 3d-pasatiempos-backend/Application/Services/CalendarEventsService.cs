using _3d_pasatiempos_backend.Application.Dtos.CalendarEventsDto;
using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.CalendarEventInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class CalendarEventsService : ICalendarEventsService
    {
        private readonly ICalendarEventsRepository _CalendarEventsRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public CalendarEventsService(ICalendarEventsRepository CalendarEventsRepository, IUnitOfWork UnitOfWork)
        {
            _CalendarEventsRepository = CalendarEventsRepository;
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(CreateEventDto pRequest, EventType pType)
        {
            var lCalendarEvent = new CalendarEvent
            {
                EventName = pRequest.EventName,
                EventType = pType.ToString(),
                StartDate = pRequest.StartDate,
                EndDate = pRequest.EndDate,
            };

            await _CalendarEventsRepository.AddAsync(lCalendarEvent);
            await _UnitOfWork.SaveChangesAsync();

            return lCalendarEvent.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<CreateEventDto>> GetPagedAsync(QueryParams pQuery)
        {
            var (lData, lTotal) = await _CalendarEventsRepository.GetPagedAsync(pQuery);

            var lQueryResult = lData.Select(x => new CreateEventDto
            {
                EventId = x.Id,
                EventName = x.EventName,
                EventType = x.EventType,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            });

            return new PagedResult<CreateEventDto>
            {
                Items = lQueryResult,
                TotalCount = lTotal,
                Page = pQuery.Page,
                PageSize = pQuery.PageSize
            };
        }
    }
}

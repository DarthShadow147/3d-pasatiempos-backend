using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.CalendarEventInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class CalendarEventsRepository : ICalendarEventsRepository
    {
        private readonly AppDbContext _Context;

        public CalendarEventsRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCalendarEvent"></param>
        /// <returns></returns>
        public async Task AddAsync(CalendarEvent pCalendarEvent)
        {
            await _Context.CalendarEvent.AddAsync(pCalendarEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<(List<CalendarEvent> Data, int TotalCount)> GetPagedAsync(QueryParams pQuery)
        {
            DateTime lNow = DateTime.Now;
            DateTime lStartMonth = new DateTime(lNow.Year, lNow.Month, 1);
            DateTime lEndMonth = lStartMonth.AddMonths(1).AddDays(-1);

            var lDbQuery = _Context.CalendarEvent.AsQueryable();

            lDbQuery = lDbQuery.Where(x => x.StartDate >= lStartMonth && x.EndDate <= lEndMonth)
                               .OrderBy(x => x.StartDate);

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .ToListAsync();

            return (lData, lTotalCount);
        }
    }
}

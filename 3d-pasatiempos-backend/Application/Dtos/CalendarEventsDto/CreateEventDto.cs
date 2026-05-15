namespace _3d_pasatiempos_backend.Application.Dtos.CalendarEventsDto
{
    public class CreateEventDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

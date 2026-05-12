namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

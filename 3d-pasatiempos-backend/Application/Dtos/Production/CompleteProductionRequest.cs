namespace _3d_pasatiempos_backend.Application.Dtos.Production
{
    public class CompleteProductionRequest
    {
        public int GramsUsed { get; set; }
        public int ExecutionHours {  get; set; }
        public int ExecutionMinutes { get; set; }
    }
}

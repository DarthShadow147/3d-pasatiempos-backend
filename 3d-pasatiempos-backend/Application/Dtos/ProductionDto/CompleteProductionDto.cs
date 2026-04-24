namespace _3d_pasatiempos_backend.Application.Dtos.ProductionDto
{
    public class CompleteProductionDto
    {
        public bool OrderFinish { get; set; }
        public int GramsUsed { get; set; }
        public int ExecutionHours {  get; set; }
        public int ExecutionMinutes { get; set; }
    }
}

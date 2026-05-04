namespace _3d_pasatiempos_backend.Application.Dtos.ProjectDto
{
    public class ProjectListDto
    {
        public int ProjectId { get; set; }
        public string? CustomerName { get; set; }
        public string? ProjectName { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

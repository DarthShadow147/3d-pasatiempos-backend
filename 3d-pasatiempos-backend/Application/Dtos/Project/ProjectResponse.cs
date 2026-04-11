namespace _3d_pasatiempos_backend.Application.Dtos.Project
{
    public class ProjectResponse
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Image {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

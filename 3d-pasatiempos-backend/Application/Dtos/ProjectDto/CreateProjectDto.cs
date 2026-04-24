namespace _3d_pasatiempos_backend.Application.Dtos.ProjectDto
{
    public class CreateProjectDto
    {
        public int CustomerId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image {  get; set; } = string.Empty;
    }
}

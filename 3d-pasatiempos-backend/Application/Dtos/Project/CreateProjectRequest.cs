namespace _3d_pasatiempos_backend.Application.Dtos.Project
{
    public class CreateProjectRequest
    {
        public int CustomerId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }
    }
}

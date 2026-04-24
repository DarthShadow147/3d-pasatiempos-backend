namespace _3d_pasatiempos_backend.Application.Dtos.CommonDto
{
    public class QueryParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public string? Status { get; set; }
    }
}

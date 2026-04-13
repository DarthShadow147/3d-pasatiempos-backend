using _3d_pasatiempos_backend.Application.Dtos.Project;

namespace _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces
{
    public interface IProjectService
    {
        Task<bool> CreateProjectAsync(CreateProjectRequest pRequest);
        Task<List<ProjectListResponse>> GetAllAsync(List<string> pStatuses = null);
        Task<ProjectDetailResponse> GetDetailByIdAsync(int pProjectId);
        Task UpdateStatusAsync(int pProjectId, string pStatus);
    }
}

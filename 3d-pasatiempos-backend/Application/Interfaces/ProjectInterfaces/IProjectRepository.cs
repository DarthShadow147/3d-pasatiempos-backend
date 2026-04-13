using _3d_pasatiempos_backend.Application.Dtos.Project;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces
{
    public interface IProjectRepository
    {
        Task<bool> AddAsync(Project pProject);
        Task<List<ProjectListResponse>> GetAllAsync(List<ProjectStatus> pStatuses = null);
        Task<Project> GetProjectByIdAsync(int pProjectId);
        Task UpdateAsync(Project pProject);
    }
}

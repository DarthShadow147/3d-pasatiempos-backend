using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces
{
    public interface IProjectRepository
    {
        Task AddAsync(Project pProject);
        Task<Project> GetProjectByIdAsync(int pProjectId);
        Task<(List<Project> Data, int TotalCount)> GetPagedProjectAsync(QueryParams pQuery);
    }
}

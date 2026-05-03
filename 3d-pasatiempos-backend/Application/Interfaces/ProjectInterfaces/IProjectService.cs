using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.ProjectDto;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces
{
    public interface IProjectService
    {
        Task<int> CreateProjectAsync(CreateProjectDto pRequest);
        Task<ProjectDetailDto> GetProjectDetailAsync(int pProjectId);
        Task<PagedResult<ProjectListDto>> GetPagedProjectAsync(QueryParams pQuery);
        Task ChangeProjectStatusAsync(int pProjectId, ProjectStatus pProjectStatus);
        Task UpdateProjectDetailAsync(CreateProjectDto pRequest);
    }
}

using _3d_pasatiempos_backend.Application.Dtos.Customer;
using _3d_pasatiempos_backend.Application.Dtos.Project;
using _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _ProjectRepository;

        public ProjectService(IProjectRepository ProjectRepository)
        {
            _ProjectRepository = ProjectRepository;
        }

        /// <summary>
        /// A method that creates print projects, which can be linked to a future quote
        /// </summary>
        /// <param name="pRequest">Request for a new project</param>
        /// <returns></returns>
        public async Task<bool> CreateProjectAsync(CreateProjectRequest pRequest)
        {
            if (pRequest.ProjectName == null)
                throw new Exception("Project must have at name");

            var lProject = new Project
            {
                CustomerId = pRequest.CustomerId,
                Name = pRequest.ProjectName,
                Description = pRequest.Description,
                Status = ProjectStatus.NO_MODEL,
                ImageUrl = pRequest.Image,
                CreatedAt = DateTime.UtcNow
            };

            var lTxResult = await _ProjectRepository.AddAsync(lProject);

            if (lTxResult)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Method that obtains the list of projects, which can be filtered by their statuses
        /// </summary>
        /// <param name="pStatuses">Project status</param>
        /// <returns></returns>
        public async Task<List<ProjectListResponse>> GetAllAsync(List<string> pStatuses = null)
        {
            if (pStatuses == null || pStatuses.Count == 0)
                return await _ProjectRepository.GetAllAsync(null);

            var lStatusEnum = new List<ProjectStatus>();

            foreach (var lStatus in pStatuses)
            {
                if (!Enum.TryParse<ProjectStatus>(lStatus, true, out var lParsed))
                    lStatusEnum.Add(lParsed);
            }

            if (lStatusEnum.Count == 0)
                throw new Exception("Invalid status values");

            return await _ProjectRepository.GetAllAsync(lStatusEnum);
        }

        /// <summary>
        /// Method that obtains the details of a project
        /// </summary>
        /// <param name="pProjectId">Project ID</param>
        /// <returns></returns>
        public async Task<ProjectDetailResponse> GetDetailByIdAsync(int pProjectId)
        {
            var lProject = await _ProjectRepository.GetProjectByIdAsync(pProjectId) ?? throw new Exception("Project not found");

            return new ProjectDetailResponse
            {
                ProjectId = lProject.Id,
                ProjectName = lProject.Name,
                Description = lProject.Description,
                Status = lProject.Status.ToString(),
                Image = lProject.ImageUrl,
                CreatedAt = lProject.CreatedAt,

                Customer = new CustomerResponse
                {
                    Id = lProject.Customer.Id,
                    Name = lProject.Customer.Name,
                    Phone = lProject.Customer.Phone,
                    Email = lProject.Customer.Email
                },
            };
        }

        /// <summary>
        /// Method that updates the states of a project
        /// </summary>
        /// <param name="pProjectId">Project ID</param>
        /// <param name="pStatus">Project status</param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(int pProjectId, string pStatus)
        {
            var lProject = await _ProjectRepository.GetProjectByIdAsync(pProjectId) ?? throw new Exception("Project not found");

            if (!Enum.TryParse<ProjectStatus>(pStatus, true, out var lParsed))
                throw new Exception("Invalid status");

            lProject.Status = lParsed;

            await _ProjectRepository.UpdateAsync(lProject);
        }
    }
}

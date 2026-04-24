using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using _3d_pasatiempos_backend.Application.Dtos.ProjectDto;
using _3d_pasatiempos_backend.Application.Exceptions.Common;
using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _ProjectRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public ProjectService(IProjectRepository ProjectRepository, IUnitOfWork UnitOfWork)
        {
            _ProjectRepository = ProjectRepository;
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public async Task<int> CreateProjectAsync(CreateProjectDto pRequest)
        {
            var lProject = new Project
            {
                CustomerId = pRequest.CustomerId,
                Name = pRequest.ProjectName,
                Description = pRequest.Description,
                Status = ProjectStatus.NO_MODEL.ToString(),
                ImageUrl = pRequest.Image,
                CreatedAt = DateTime.UtcNow
            };

            await _ProjectRepository.AddAsync(lProject);
            await _UnitOfWork.SaveChangesAsync();

            return lProject.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<ProjectListDto>> GetPagedProjectAsync(QueryParams pQuery)
        {
            var (lData, lTotal) = await _ProjectRepository.GetPagedProjectAsync(pQuery);

            var lQueryResult = lData.Select(x => new ProjectListDto
            {
                ProjectId = x.Id,
                CustomerName = x.Customer.Name,
                ProjectName = x.Name,
                Status = x.Status
            });

            return new PagedResult<ProjectListDto>
            {
                Items = lQueryResult,
                TotalCount = lTotal,
                Page = pQuery.Page,
                PageSize = pQuery.PageSize
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pProjectId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ProjectDetailDto> GetProjectDetailAsync(int pProjectId)
        {
            var lProjectRecord = await _ProjectRepository.GetProjectByIdAsync(pProjectId)
                ?? throw new NotFoundException("Project not found");

            return new ProjectDetailDto
            {
                ProjectId = lProjectRecord.Id,
                ProjectName = lProjectRecord.Name,
                Description = lProjectRecord.Description,
                Status = lProjectRecord.Status,
                Image = lProjectRecord.ImageUrl,
                CreatedAt = lProjectRecord.CreatedAt,

                Customer = new DetailCustomerDto
                {
                    Id = lProjectRecord.Customer.Id,
                    Name = lProjectRecord.Customer.Name,
                    Phone = lProjectRecord.Customer.Phone,
                    Email = lProjectRecord.Customer.Email
                }
            };
        }
    }
}

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
            string? lImagePath = null;

            if (pRequest.Image != null)
            {
                var lFolderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "projects"
                );

                if (!Directory.Exists(lFolderPath))
                    Directory.CreateDirectory(lFolderPath);

                var lExtension = Path.GetExtension(pRequest.Image.FileName);
                var lFileName = $"{Guid.NewGuid()}{lExtension}";
                var lFullPath = Path.Combine(lFolderPath, lFileName);

                using (var lStream = new FileStream(lFullPath, FileMode.Create))
                {
                    await pRequest.Image.CopyToAsync(lStream);
                }

                lImagePath = $"images/projects/{lFileName}";
            }

            var lProject = new Project
            {
                CustomerId = pRequest.CustomerId,
                Name = pRequest.ProjectName,
                Description = pRequest.Description,
                Status = ProjectStatus.NO_MODEL.ToString(),
                ImageUrl = lImagePath ?? string.Empty,
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
                Status = x.Status,
                CreatedAt = x.CreatedAt
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task UpdateProjectDetailAsync(CreateProjectDto pRequest, ProjectStatus pProjectStatus)
        {
            var lProjectRecord = await _ProjectRepository.GetProjectByIdAsync(pRequest.ProjectId)
                ?? throw new NotFoundException("Project not found");

            lProjectRecord.Name = pRequest.ProjectName;
            lProjectRecord.Description = pRequest.Description;
            lProjectRecord.Status = pProjectStatus.ToString();

            if (pRequest.Image != null)
            {
                var lFolderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "projects"
                );

                if (!Directory.Exists(lFolderPath))
                    Directory.CreateDirectory(lFolderPath);

                if (!string.IsNullOrEmpty(lProjectRecord.ImageUrl))
                {
                    var lOldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        lProjectRecord.ImageUrl
                    );

                    if (File.Exists(lOldPath))
                        File.Delete(lOldPath);
                }

                var lExtension = Path.GetExtension(pRequest.Image.FileName);
                var lFileName = $"{Guid.NewGuid()}{lExtension}";
                var lFullPath = Path.Combine(lFolderPath, lFileName);

                using (var lStream = new FileStream(lFullPath, FileMode.Create))
                {
                    await pRequest.Image.CopyToAsync(lStream);
                }

                lProjectRecord.ImageUrl = $"images/projects/{lFileName}";
            }

            await _UnitOfWork.SaveChangesAsync();
        }
    }
}

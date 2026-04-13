using _3d_pasatiempos_backend.Application.Dtos.Project;
using _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _Context;

        public ProjectRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// Method that saves the project in a database
        /// </summary>
        /// <param name="pProject">Project model</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(Project pProject)
        {
            _Context.Project.Add(pProject);
            var lTxResult = await _Context.SaveChangesAsync();

            if (lTxResult > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Method that obtains the list of projects from the database according to their filtered or unfiltered states
        /// </summary>
        /// <param name="pStatuses">Project statuses</param>
        /// <returns></returns>
        public async Task<List<ProjectListResponse>> GetAllAsync(List<ProjectStatus> pStatuses = null)
        {
            var lQuery = _Context.Project.AsQueryable();

            if (pStatuses != null && pStatuses.Count != 0)
                lQuery = lQuery.Where(p => pStatuses.Contains(p.Status));

            return await _Context.Project
                .Select(p => new ProjectListResponse
                {
                    ProjectId = p.Id,
                    CustomerName = p.Customer.Name,
                    ProjectName = p.Name,
                    Status = p.Status.ToString()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Method that obtains the details of a project based on its ID
        /// </summary>
        /// <param name="pProjectId">Project ID</param>
        /// <returns></returns>
        public async Task<Project> GetProjectByIdAsync(int pProjectId)
        {
            return await _Context.Project
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(p => p.Id == pProjectId);
        }

        /// <summary>
        /// Method that updates the status of a project in the database
        /// </summary>
        /// <param name="pProject">Project model to update</param>
        /// <returns></returns>
        public async Task UpdateAsync(Project pProject)
        {
            _Context.Project.Update(pProject);
            await _Context.SaveChangesAsync();
        }
    }
}

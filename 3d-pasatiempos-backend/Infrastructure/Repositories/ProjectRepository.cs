using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
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
        /// 
        /// </summary>
        /// <param name="pProject"></param>
        /// <returns></returns>
        public async Task AddAsync(Project pProject)
        {
            await _Context.AddAsync(pProject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pProjectId"></param>
        /// <returns></returns>
        public async Task<Project> GetProjectByIdAsync(int pProjectId)
        {
            return await _Context.Project
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == pProjectId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<(List<Project> Data, int TotalCount)> GetPagedProjectAsync(QueryParams pQuery)
        {
            var lDbQuery = _Context.Project.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Name))
                lDbQuery = lDbQuery.Where(x => EF.Functions.ILike(x.Name, $"%{pQuery.Name}%"));

            if (!string.IsNullOrWhiteSpace(pQuery.Status))
                lDbQuery = lDbQuery.Where(x => EF.Functions.ILike(x.Status, $"%{pQuery.Status}%"));

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Include(x => x.Customer)
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return (lData, lTotalCount);
        }
    }
}

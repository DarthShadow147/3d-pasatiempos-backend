using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.Utils;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class UtilsRepository : IUtilsRepository
    {
        private readonly AppDbContext _Context;

        public UtilsRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public IQueryable<T> GetFilteredQuery<T>(QueryParams pQuery) where T : class
        {
            var lDbSet = _Context.Set<T>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Name))
            {
                lDbSet = lDbSet.Where(x => 
                EF.Functions.ILike(
                    EF.Property<string>(x, "Name"), $"%{pQuery.Name}%"));
            }

            if (!string.IsNullOrWhiteSpace(pQuery.Status) && 
                int.TryParse(pQuery.Status, out int lCustomerId))
            {
                lDbSet = lDbSet.Where(x =>
                    EF.Property<int>(x, "CustomerId") == lCustomerId);
            }

            return lDbSet;
        }
    }
}

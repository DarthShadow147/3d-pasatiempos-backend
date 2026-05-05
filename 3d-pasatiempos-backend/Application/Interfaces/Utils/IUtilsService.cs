using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.UtilsDto;
using System.Linq.Expressions;

namespace _3d_pasatiempos_backend.Application.Interfaces.Utils
{
    public interface IUtilsService
    {
        Task<PagedResult<TDto>> PaginateAsync<TEntity, TDto>(IQueryable<TEntity> pQuery, QueryParams pQueryParams, Expression<Func<TEntity, TDto>> pSelector);
        Task<PagedResult<UtilDto>> GetCustomersAsync(QueryParams pQueryParams);
        Task<PagedResult<UtilDto>> GetMaterialAsync(QueryParams pQueryParams);
        Task<PagedResult<UtilDto>> GetPrinterAsync(QueryParams pQueryParams);
        Task<PagedResult<UtilDto>> GetProjectAsync(QueryParams pQueryParams);
    }
}

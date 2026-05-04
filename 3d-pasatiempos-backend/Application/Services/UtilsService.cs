using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.UtilsDto;
using _3d_pasatiempos_backend.Application.Interfaces.Utils;
using _3d_pasatiempos_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class UtilsService : IUtilsService
    {
        private readonly IUtilsRepository _UtilsRepository;

        public UtilsService(IUtilsRepository UtilsRepository)
        {
            _UtilsRepository = UtilsRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="pQuery"></param>
        /// <param name="pQueryParams"></param>
        /// <param name="pSelector"></param>
        /// <returns></returns>
        public async Task<PagedResult<TDto>> PaginateAsync<TEntity, TDto>(
            IQueryable<TEntity> pQuery, 
            QueryParams pQueryParams, 
            Expression<Func<TEntity, TDto>> pSelector)
        {
            var lTotal = await pQuery.CountAsync();

            var lData = await pQuery
                .Skip((pQueryParams.Page - 1) * pQueryParams.PageSize)
                .Take(pQueryParams.PageSize)
                .Select(pSelector)
                .ToListAsync();

            return new PagedResult<TDto>
            {
                Items = lData,
                TotalCount = lTotal,
                Page = pQueryParams.Page,
                PageSize = pQueryParams.PageSize
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQueryParams"></param>
        /// <returns></returns>
        public async Task<PagedResult<UtilDto>> GetCustomersAsync(QueryParams pQueryParams)
        {
            var lQuery = _UtilsRepository.GetFilteredQuery<Customer>(pQueryParams);

            return await PaginateAsync(
                lQuery, pQueryParams,
                    x => new UtilDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQueryParams"></param>
        /// <returns></returns>
        public async Task<PagedResult<UtilDto>> GetMaterialAsync(QueryParams pQueryParams)
        {
            var lQuery = _UtilsRepository.GetFilteredQuery<Material>(pQueryParams);

            return await PaginateAsync(
                lQuery, pQueryParams,
                    x => new UtilDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQueryParams"></param>
        /// <returns></returns>
        public async Task<PagedResult<UtilDto>> GetPrinterAsync(QueryParams pQueryParams)
        {
            var lQuery = _UtilsRepository.GetFilteredQuery<Printer>(pQueryParams);

            return await PaginateAsync(
                lQuery, pQueryParams,
                    x => new UtilDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    });
        }
    }
}

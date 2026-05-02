using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _Context;

        public CustomerRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCustomer"></param>
        /// <returns></returns>
        public async Task AddAsync(Customer pCustomer)
        {
            await _Context.Customer.AddAsync(pCustomer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCustomerId"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerByIdAsync(int pCustomerId)
        {
            return await _Context.Customer
                .FirstOrDefaultAsync(x => x.Id == pCustomerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<(List<Customer> Data, int TotalCount)> GetPagedAsync(QueryParams pQuery)
        {
            var lDbQuery = _Context.Customer.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Name))
                lDbQuery = lDbQuery.Where(x => EF.Functions.ILike(x.Name, $"%{pQuery.Name}%"));

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .ToListAsync();

            return (lData, lTotalCount);
        }
    }
}

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
        /// Method that uses data access to add clients to the database
        /// </summary>
        /// <param name="pCustomer">Customer model</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(Customer pCustomer)
        {
            _Context.Customer.Add(pCustomer);
            var lTxResult = await _Context.SaveChangesAsync();

            if (lTxResult > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Method that obtains the list of clients from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Customer>> GetAllAsync()
        {
            return await _Context.Customer.ToListAsync();
        }
    }
}

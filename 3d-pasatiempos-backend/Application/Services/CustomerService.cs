using _3d_pasatiempos_backend.Application.Dtos.Customer;
using _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _CustomerRepository;

        public CustomerService(ICustomerRepository CustomerRepository)
        {
            _CustomerRepository = CustomerRepository;
        }

        /// <summary>
        /// Method that uses the data access instance to create the client model and convert it into its DTO
        /// </summary>
        /// <param name="pRequest">Service request</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(CreateCustomerRequest pRequest)
        {
            if (String.IsNullOrWhiteSpace(pRequest.Name))
                throw new NotImplementedException();

            var lCustomer = new Customer
            {
                Name = pRequest.Name,
                Email = pRequest.Email,
                Phone = pRequest.Phone,
                CreatedAt = DateTime.UtcNow
            };

            return await _CustomerRepository.AddAsync(lCustomer);
        }

        /// <summary>
        /// Method that retrieves the customer list from the database and transforms it into the DTO
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerResponse>> GetAllAsync()
        {
            var lCustomers = await _CustomerRepository.GetAllAsync();

            return lCustomers.Select(c => new CustomerResponse
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
            }).ToList();
        }
    }
}

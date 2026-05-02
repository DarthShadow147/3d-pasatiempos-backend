using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using _3d_pasatiempos_backend.Application.Exceptions.Common;
using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public CustomerService(ICustomerRepository CustomerRepository, IUnitOfWork UnitOfWork)
        {
            _CustomerRepository = CustomerRepository;
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(CreateCustomerDto pRequest)
        {
            var lCustomerRequest = new Customer
            {
                Name = pRequest.Name,
                Email = pRequest.Email,
                Phone = pRequest.Phone,
                CreatedAt = DateTime.Now
            };

            await _CustomerRepository.AddAsync(lCustomerRequest);
            await _UnitOfWork.SaveChangesAsync();

            return lCustomerRequest.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task UpdateCustomerAsync(CreateCustomerDto pRequest)
        {
            var lCustomerDetail = await _CustomerRepository.GetCustomerByIdAsync(pRequest.CustomerId)
                ?? throw new NotFoundException("Customer not found");

            lCustomerDetail.Name = pRequest.Name;
            lCustomerDetail.Email = pRequest.Email;
            lCustomerDetail.Phone = pRequest.Phone;

            await _UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<DetailCustomerDto>> GetPagedAsync(QueryParams pQuery)
        {
            var (lData, lTotal) = await _CustomerRepository.GetPagedAsync(pQuery);

            var lQueryResult = lData.Select(x => new DetailCustomerDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone
            });

            return new PagedResult<DetailCustomerDto>
            {
                Items = lQueryResult,
                TotalCount = lTotal,
                Page = pQuery.Page,
                PageSize = pQuery.PageSize
            };
        }
    }
}

using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using _3d_pasatiempos_backend.Application.Dtos.OrderDto;
using _3d_pasatiempos_backend.Application.Dtos.PaymentDto;
using _3d_pasatiempos_backend.Application.Exceptions.Common;
using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.PaymentInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IPaymentRepository _PaymentRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public PaymentService(IOrderRepository OrderRepository, IPaymentRepository PaymentRepository, IUnitOfWork UnitOfWork)
        {
            _OrderRepository = OrderRepository;
            _PaymentRepository = PaymentRepository;
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<int> CreatePaymentAsync(CreatePaymentDto pRequest)
        {
            _ = await _OrderRepository.GetOrderByIdAsync(pRequest.OrderId) 
                ?? throw new NotFoundException("Order not found");

            var lPayment = new Payment
            {
                OrderId = pRequest.OrderId,
                Amount = pRequest.Amount,
                Method = pRequest.Method,
                Type = pRequest.Type.ToString()
            };

            await _PaymentRepository.AddAsync(lPayment);
            await _UnitOfWork.SaveChangesAsync();

            return lPayment.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<PaymentListDto>> GetPagedPaymentAsync(QueryParams pQuery)
        {
            var (lData, lTotal) = await _PaymentRepository.GetPagedPaymentAsync(pQuery);

            var lQueryResult = lData.Select(x => new PaymentListDto
            {
                PaymentId = x.Id,
                CustomerName = x.Order.Quote.Customer.Name,
                CreatedAt = x.CreatedAt,
                Method = x.Method,
                Type = x.Type,
                Amount = x.Amount
            });

            return new PagedResult<PaymentListDto>
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
        /// <param name="pPaymentId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<PaymentDetailDto> GetPaymentDetailAsync(int pPaymentId)
        {
            var lPayment = await _PaymentRepository.GetPaymentByIdAsync(pPaymentId)
                ?? throw new NotFoundException("Payment not found");

            return new PaymentDetailDto
            {
                PaymentId = lPayment.Id,
                OrderId = lPayment.OrderId,

                CustomerDetail = new DetailCustomerDto 
                { 
                    Id = lPayment.Order.Quote.Customer.Id,
                    Name = lPayment.Order.Quote.Customer.Name,
                    Email = lPayment.Order.Quote.Customer.Email,
                    Phone = lPayment.Order.Quote.Customer.Phone
                },
                OrderDetail = new OrderDetailDto
                {
                    OrderId = lPayment.Order.Id,
                    Status = lPayment.Order.Status,
                    StartDate = lPayment.Order.StartDate,
                    EndDate = lPayment.Order.EndDate,
                    Total = lPayment.Order.Quote.Total
                },
                CreatedAt = lPayment.CreatedAt,
                Amount = lPayment.Amount,
                Method = lPayment.Method,
                Type = lPayment.Type
            };
        }
    }
}

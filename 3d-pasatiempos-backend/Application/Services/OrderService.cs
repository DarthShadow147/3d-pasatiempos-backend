using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.OrderDto;
using _3d_pasatiempos_backend.Application.Exceptions.Common;
using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IProductionRepository _ProductionRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public OrderService(IOrderRepository OrderRepository, IProductionRepository ProductionRepository, IUnitOfWork UnitOfWork)
        {
            _OrderRepository = OrderRepository;
            _ProductionRepository = ProductionRepository;
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOrderId"></param>
        /// <returns></returns>
        public async Task StartOrderAsync(int pOrderId)
        {
            try
            {
                await _UnitOfWork.BeginTransactionAsync();

                var lOrderRecord = await _OrderRepository.GetOrderByIdAsync(pOrderId) 
                    ?? throw new NotFoundException("Order not found");

                if (lOrderRecord.Status != OrderStatus.PENDING.ToString())
                    throw new BadRequestException("Only pending orders can be started");

                decimal? lTotalEstimatedHours = lOrderRecord.Quote.QuoteItem
                    .Sum(i => i.EstimatedHours);

                var lNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Local);

                lOrderRecord.Status = OrderStatus.IN_PROGRESS.ToString();
                lOrderRecord.StartDate = lNow;

                var lProduction = new Production
                {
                    OrderId = pOrderId,
                    StartTime = lNow,
                    EstimatedEndTime = lNow.AddMinutes((double)lTotalEstimatedHours),
                    Status = ProductionStatus.IN_PROGRESS.ToString()
                };

                await _ProductionRepository.AddAsync(lProduction);
                await _UnitOfWork.SaveChangesAsync();
                await _UnitOfWork.CommitAsync();
            }
            catch
            {
                await _UnitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOrderId"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task<int> CreateStandAloneProduction(CreateProductionDto pRequest)
        {
            var lOrderRecord = await _OrderRepository.GetOrderByIdAsync(pRequest.OrderId) 
                ?? throw new NotFoundException("Order not found");

            if (lOrderRecord.Status != OrderStatus.PENDING.ToString() || lOrderRecord.Status != OrderStatus.IN_PROGRESS.ToString())
                throw new BadRequestException("Only pending or in progress orders can be started");

            var lNow = DateTime.UtcNow;
            var lTotalProductionMinutes = (pRequest.Hours * 60) + pRequest.Minutes;

            var lProduction = new Production
            {
                OrderId = pRequest.OrderId,
                StartTime = lNow,
                EstimatedEndTime = lNow.AddMinutes(lTotalProductionMinutes),
                Status = ProductionStatus.IN_PROGRESS.ToString()
            };

            await _ProductionRepository.AddAsync(lProduction);
            await _UnitOfWork.SaveChangesAsync();

            return lProduction.OrderId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<OrderDetailDto>> GetPagedOrderAsync(QueryParams pQuery)
        {
            var (lData, lTotal) = await _OrderRepository.GetPagedOrderAsync(pQuery);

            var lQueryResult = lData.Select(x => new OrderDetailDto
            {
                OrderId = x.Id,
                Status = x.Status,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Total = x.Quote.Total
            });

            return new PagedResult<OrderDetailDto>
            {
                Items = lQueryResult,
                TotalCount = lTotal,
                Page = pQuery.Page,
                PageSize = pQuery.PageSize
            };
        }
    }
}

using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _Context;
        private readonly IOrderRepository _OrderRepository;
        private readonly IProductionRepository _ProductionRepository;

        public OrderService(AppDbContext Context, IOrderRepository OrderRepository, IProductionRepository ProductionRepository)
        {
            _Context = Context;
            _OrderRepository = OrderRepository;
            _ProductionRepository = ProductionRepository;
        }

        /// <summary>
        /// Method that initiates production of an order.
        /// This involves validations such as:
        /// That the order is in "PENDING" status
        /// That there is NO record of the order in production.
        /// </summary>
        /// <param name="pOrderId">Order ID</param>
        /// <returns></returns>
        public async Task StartOrderAsync(int pOrderId)
        {
            using var lTransaction = await _Context.Database.BeginTransactionAsync();

            var lOrder = await _OrderRepository.GetOrderByQuoteDetail(pOrderId) ?? throw new Exception("Order not found");

            if (lOrder.Status != OrderStatus.PENDING)
                throw new Exception("Only pending orders can be started");

            var lExistingProduction = await _ProductionRepository.GetProductuonByOrderIdAsync(pOrderId);

            if (lExistingProduction != null)
                throw new Exception("Production already exists for this order");

            var lTotalEstimatedHours = lOrder.Quote.Items
                .Sum(i => i.EstimatedHours);

            if (lTotalEstimatedHours <= 0)
                throw new Exception("Invalid estimated hours");

            var lNow = DateTime.UtcNow;

            lOrder.Status = OrderStatus.IN_PROGRESS;
            lOrder.StartDate = lNow;

            var lProduction = new Production
            {
                OrderId = pOrderId,
                StartTime = lNow,
                EstimatedEndTime = lNow.AddMinutes((double)lTotalEstimatedHours),
                Status = ProductionStatus.IN_PROGRESS
            };

            await _OrderRepository.UpdateAsync(lOrder);
            await _ProductionRepository.AddAsync(lProduction);

            await lTransaction.CommitAsync();
        }
    }
}

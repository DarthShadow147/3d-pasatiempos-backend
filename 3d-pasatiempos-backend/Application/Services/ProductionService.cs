using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using _3d_pasatiempos_backend.Application.Dtos.OrderDto;
using _3d_pasatiempos_backend.Application.Dtos.ProductionDto;
using _3d_pasatiempos_backend.Application.Dtos.QuoteDto;
using _3d_pasatiempos_backend.Application.Exceptions.Common;
using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class ProductionService : IProductionService
    {
        private readonly IProductionRepository _ProductionRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public ProductionService(IProductionRepository ProductionRepository, IUnitOfWork UnitOfWork)
        {
            _ProductionRepository = ProductionRepository;
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pProductionId"></param>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public async Task CompleteProductionAsync(int pProductionId, CompleteProductionDto pRequest)
        {
            try
            {
                await _UnitOfWork.BeginTransactionAsync();

                var lProductionRecord = await _ProductionRepository.GetProductionDetailByIdAsync(pProductionId)
                    ?? throw new NotFoundException("Production not found");

                if (lProductionRecord.Status != ProductionStatus.IN_PROGRESS.ToString())
                    throw new BadRequestException("Production is not in progress");

                lProductionRecord.Status = ProductionStatus.COMPLETE.ToString();
                lProductionRecord.GramsUsed = pRequest.GramsUsed;
                lProductionRecord.TimeUsed = (pRequest.ExecutionHours * 60) + pRequest.ExecutionMinutes;

                if (pRequest.OrderFinish)
                {
                    var lOrder = lProductionRecord.Order;
                    lOrder.Status = OrderStatus.COMPLETE.ToString();
                    lOrder.EndDate = DateTime.UtcNow;
                }

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
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<ProductionListDto>> GetPagedProductionAsync(QueryParams pQuery)
        {
            var (lData, lTotal) = await _ProductionRepository.GetPagedProductionAsync(pQuery);

            var lQueryResult = lData.Select(x => new ProductionListDto
            {
                ProductionId = x.Id,
                StartTime = x.StartTime,
                EndTime = x.EstimatedEndTime,
                Status = x.Status
            });

            return new PagedResult<ProductionListDto>
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
        /// <param name="pProductionId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ProductionDetailDto> GetProductionDetailAsync(int pProductionId)
        {
            var lProduction = await _ProductionRepository.GetProductionDetailByIdAsync(pProductionId) 
                ?? throw new NotFoundException("Production not found");

            return new ProductionDetailDto
            {
                Status = lProduction.Status,
                StartDate = lProduction.StartTime,
                EndDate = lProduction.EstimatedEndTime,
                GramsUsed = lProduction.GramsUsed,
                TimeUsed = lProduction.TimeUsed,

                QuoteDetail = new QuoteDetailSimply
                {
                    QuoteId = lProduction.Order.QuoteId,
                    Customer = new DetailCustomerDto
                    {
                        Id = lProduction.Order.Quote.Customer.Id,
                        Name = lProduction.Order.Quote.Customer.Name,
                        Email = lProduction.Order.Quote.Customer.Email,
                        Phone = lProduction.Order.Quote.Customer.Phone
                    },

                    QuoteDate = lProduction.Order.Quote.CreatedAt,
                    QuoteStatus = lProduction.Order.Quote.Status,
                    QuoteTotal = lProduction.Order.Quote.Total
                },

                OrderDetail = new OrderDetailDto
                {
                    OrderId = lProduction.OrderId,
                    Status = lProduction.Order.Status,
                    StartDate = lProduction.Order.StartDate,
                    EndDate = lProduction.Order.EndDate
                }
            };
        }
    }
}

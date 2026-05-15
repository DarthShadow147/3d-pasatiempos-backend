using _3d_pasatiempos_backend.Application.Dtos.AggregateCostDto;
using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using _3d_pasatiempos_backend.Application.Dtos.ProjectDto;
using _3d_pasatiempos_backend.Application.Dtos.QuoteDto;
using _3d_pasatiempos_backend.Application.Exceptions.Common;
using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces;
using _3d_pasatiempos_backend.Domain.Entities;
using _3d_pasatiempos_backend.Domain.Enums;

namespace _3d_pasatiempos_backend.Application.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IQuoteRepository _QuoteRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public QuoteService(IOrderRepository OrderRepository, IQuoteRepository QuoteRepository, IUnitOfWork UnitOfWork)
        {
            _OrderRepository = OrderRepository;
            _QuoteRepository = QuoteRepository;
            _UnitOfWork = UnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<int> CreateQuoteAsync(CreateQuoteDto pRequest)
        {
            try
            {
                await _UnitOfWork.BeginTransactionAsync();

                var lMaterial = await _QuoteRepository.GetMaterialDetail(pRequest.Material);
                var lPrinter = await _QuoteRepository.GetPrinterDetail(pRequest.Printer);

                var lAggregateCost = await _QuoteRepository.GetAggregateCostDetailsAsync();
                var lExtraCost = GetAdditionalCosts(pRequest, lAggregateCost);

                if (lMaterial == null || lPrinter == null)
                    throw new NotFoundException("Printer or Material are missing");

                var lQuote = new Quote
                {
                    CustomerId = pRequest.CustomerId,
                    ProjectId = pRequest.ProjectId,
                    QuoteName = pRequest.QuoteName,
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Local),
                    Status = QuoteStatus.PENDING.ToString(),
                    QuoteItem = []
                };

                decimal lTotal = 0;
                foreach (var lItem in pRequest.Items)
                {
                    var lQuoteItem = new QuoteItem
                    {
                        ProductName = lItem.ProductName,
                        EstimatedGrams = lItem.Grams,
                        EstimatedHours = GetEstimatedHours(lItem.Hours, lItem.Minutes),
                        CalculatedPrice = GetEstimatedTotalCost(lMaterial, lPrinter, lItem.Grams, lItem.Hours, lItem.Minutes, pRequest.ProfitPercentage),
                        PricePerGramUsed = GetEstimatedPricePerGram(lMaterial.PricePerGram, lItem.Grams),
                        CostPerKwhUsed = GetEstimatedEnergyCost(lPrinter.PowerConsumptionKwh, lPrinter.PowerConsumptionWh, lItem.Hours, lItem.Minutes),
                        MachineWearCostUsed = GetEstimatedWearMachine(lPrinter.CostPerMinute, lItem.Hours, lItem.Minutes),
                        CostOverrunFailure = GetEstimatedOverrunFailture(lMaterial, lPrinter, lItem.Grams, lItem.Hours, lItem.Minutes),
                        ProfitPercentage = pRequest.ProfitPercentage,

                        ShippingCost = lExtraCost.ShippingCost,
                        ModelCost = lExtraCost.ModelCost,
                        PaintCost = lExtraCost.PaintCost,
                        HardwareCost = lExtraCost.HardwareCost,
                        PackingCost = lExtraCost.PackingCost
                    };

                    lTotal += lQuoteItem.CalculatedPrice
                            + lQuoteItem.ShippingCost
                            + lQuoteItem.ModelCost
                            + lQuoteItem.PaintCost
                            + lQuoteItem.HardwareCost
                            + lQuoteItem.PackingCost;

                    lQuote.QuoteItem.Add(lQuoteItem);
                }

                lQuote.Total = lTotal;

                await _QuoteRepository.AddAsync(lQuote);
                await _UnitOfWork.SaveChangesAsync();
                await _UnitOfWork.CommitAsync();

                return lQuote.Id;
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
        public async Task<PagedResult<QuoteListDto>> GetPagedQuoteAsync(QueryParams pQuery)
        {
            var (lData, lTotal) = await _QuoteRepository.GetPagedQuoteAsync(pQuery);

            var lQueryResult = lData.Select(x => new QuoteListDto
            {
                Id = x.Id,
                CustomerName = x.Customer.Name,
                ProjectName = x.Project?.Name,
                Status = x.Status,
                Total = x.Total,
                CreatedAt = x.CreatedAt
            });

            return new PagedResult<QuoteListDto>
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
        /// <param name="pQuoteId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<QuoteDetailDto> GetQuoteDetailAsync(int pQuoteId)
        {
            var lQuote = await _QuoteRepository.GetQuoteByIdAsync(pQuoteId) 
                ?? throw new NotFoundException("Quote not found");

            return new QuoteDetailDto
            {
                QuoteId = lQuote.Id,
                QuoteDate = lQuote.CreatedAt,
                QuoteStatus = lQuote.Status.ToString(),
                QuoteTotal = lQuote.Total,

                Customer = new DetailCustomerDto
                {
                    Id = lQuote.Customer.Id,
                    Name = lQuote.Customer.Name,
                    Phone = lQuote.Customer.Phone,
                    Email = lQuote.Customer.Email
                },

                ProjectDetail = lQuote.Project == null ? null : new ProjectListDto
                {
                    ProjectId = lQuote.Project.Id,
                    ProjectName = lQuote.Project.Name,
                    Status = lQuote.Project.Status.ToString()
                },

                Items = lQuote.QuoteItem.Select(i => new QuoteItemDetailDto
                {
                    QuoteItemId = i.Id,
                    ProductName = i.ProductName,
                    EstimatedGrams = i.EstimatedGrams,
                    EstimatedTime = i.EstimatedHours,
                    PricePerGram = i.PricePerGramUsed,
                    CostPerKwh = i.CostPerKwhUsed,
                    CostOverrunFail = i.CostOverrunFailure,
                    ProfitPercentage = i.ProfitPercentage
                }).ToList()
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuoteId"></param>
        /// <returns></returns>
        public async Task ApproveAsync(int pQuoteId)
        {
            try
            {
                await _UnitOfWork.BeginTransactionAsync();

                var lQuoteRecord = await _QuoteRepository.GetQuoteByIdAsync(pQuoteId) 
                    ?? throw new NotFoundException("Quote not found");

                if (lQuoteRecord.Status != QuoteStatus.PENDING.ToString())
                    throw new BadRequestException("Only pending quotes can be approved");

                var lExistsOrder = await _OrderRepository.GetOrderByQuoteIdAsync(pQuoteId);
                if (lExistsOrder != null)
                    throw new BadRequestException("Order already exists for this quote");

                lQuoteRecord.Status = QuoteStatus.APPROVED.ToString();

                var lOrder = new Order
                {
                    QuoteId = lQuoteRecord.Id,
                    Status = OrderStatus.PENDING.ToString(),
                    StartDate = null,
                    EndDate = null
                };

                await _OrderRepository.AddAsync(lOrder);
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
        /// <param name="pQuoteId"></param>
        /// <param name="pRejectReason"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task RejectAsync(int pQuoteId, string pRejectReason)
        {
            var lQuoteRecord = await _QuoteRepository.GetQuoteByIdAsync(pQuoteId) ?? throw new NotFoundException("Quote not found");

            if (lQuoteRecord.Status != QuoteStatus.PENDING.ToString())
                throw new BadRequestException("Only pending quotes can be rejected");

            lQuoteRecord.Status = QuoteStatus.REJECTED.ToString();
            lQuoteRecord.RejectReason = pRejectReason;

            await _UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <param name="pCostList"></param>
        /// <returns></returns>
        public AdditionalCosts GetAdditionalCosts(CreateQuoteDto pRequest, List<AggregateCost> pCostList)
        {
            var lShippingCost = pRequest.NeedShipping
                ? pCostList.FirstOrDefault(c => c.CostName == "ENVIOS")?.CostValue ?? 0
                : 0;

            var lModelCost = pRequest.NeedModel
                ? pCostList.FirstOrDefault(c => c.CostName == "MODELAJE")?.CostValue ?? 0
                : 0;

            var lPaintCost = pRequest.NeedPaint
                ? pCostList.FirstOrDefault(c => c.CostName == "PINTURA")?.CostValue ?? 0
                : 0;

            var lHardwareCost = pRequest.KeyChainQuantity.HasValue
                ? (pCostList.FirstOrDefault(c => c.CostName == "HERRAJES")?.UnitCost ?? 0) * pRequest.KeyChainQuantity.Value
                : 0;

            var lPackingCost = pCostList.FirstOrDefault(c => c.CostName == "CAJAS")?.CostValue ?? 0;

            return new AdditionalCosts
            {
                ShippingCost = lShippingCost,
                ModelCost = lModelCost,
                PaintCost = lPaintCost,
                HardwareCost = lHardwareCost,
                PackingCost = lPackingCost
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        public int GetEstimatedHours(int pHours, int pMinutes)
        {
            return (pHours * 60) + pMinutes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPricePerGram"></param>
        /// <param name="pGramUsed"></param>
        /// <returns></returns>
        public decimal GetEstimatedPricePerGram(decimal pPricePerGram, int pGramUsed)
        {
            return pPricePerGram * pGramUsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUseCost"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        public decimal GetEstimatedWearMachine(decimal pUseCost, int pHours, int pMinutes)
        {
            int lPrintTime = GetEstimatedHours(pHours, pMinutes);
            return pUseCost * lPrintTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPowerKWh"></param>
        /// <param name="pPowerWh"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        public decimal GetEstimatedEnergyCost(decimal pPowerKWh, decimal pPowerWh, int pHours, int pMinutes)
        {
            int lPrintTime = GetEstimatedHours(pHours, pMinutes);
            return (((pPowerWh / 1000) * pPowerKWh) / 60) * lPrintTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMaterial"></param>
        /// <param name="pPrinter"></param>
        /// <param name="pGramUsed"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <returns></returns>
        public decimal GetEstimatedOverrunFailture(Material pMaterial, Printer pPrinter, int pGramUsed, int pHours, int pMinutes)
        {
            decimal lWearMachine = GetEstimatedWearMachine(pPrinter.CostPerMinute, pHours, pMinutes);
            decimal lEnergyCost = GetEstimatedEnergyCost(pPrinter.PowerConsumptionKwh, pPrinter.PowerConsumptionWh, pHours, pMinutes);
            decimal lPrintCost = GetEstimatedPricePerGram(pMaterial.PricePerGram, pGramUsed);

            decimal lFailPercentage = pPrinter.FailPercentage / 100;
            return (lWearMachine + lEnergyCost + lPrintCost) * lFailPercentage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMaterial"></param>
        /// <param name="pPrinter"></param>
        /// <param name="pGramUsed"></param>
        /// <param name="pHours"></param>
        /// <param name="pMinutes"></param>
        /// <param name="pProfitPercentage"></param>
        /// <returns></returns>
        public decimal GetEstimatedTotalCost(Material pMaterial, Printer pPrinter, int pGramUsed, int pHours, int pMinutes, decimal pProfitPercentage)
        {
            decimal lWearMachine = GetEstimatedWearMachine(pPrinter.CostPerMinute, pHours, pMinutes);
            decimal lEnergyCost = GetEstimatedEnergyCost(pPrinter.PowerConsumptionKwh, pPrinter.PowerConsumptionWh, pHours, pMinutes);
            decimal lPrintCost = GetEstimatedPricePerGram(pMaterial.PricePerGram, pGramUsed);
            decimal lOverrunCost = GetEstimatedOverrunFailture(pMaterial, pPrinter, pGramUsed, pHours, pMinutes);

            decimal lBaseCost = lPrintCost + lEnergyCost + lWearMachine + lOverrunCost;
            decimal lProfitPercentage = pProfitPercentage / 100;

            return lBaseCost * (1 + lProfitPercentage);
        }
    }
}

namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class QuoteItem
{
    public int Id { get; set; }
    public int QuoteId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal? EstimatedGrams { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? CalculatedPrice { get; set; }
    public decimal? PricePerGramUsed { get; set; }
    public decimal? CostPerKwhUsed { get; set; }
    public decimal? MachineWearCostUsed { get; set; }
    public decimal? CostOverrunFailure { get; set; }
    public decimal? ProfitPercentage { get; set; }


    public virtual Quote Quote { get; set; } = null!;
}

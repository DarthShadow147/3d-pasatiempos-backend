namespace _3d_pasatiempos_backend.Domain.Entities;

public partial class Printer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PowerConsumptionKwh { get; set; }
    public decimal PowerConsumptionWh { get; set; }
    public decimal CostPerMinute { get; set; }
    public int UsefulLifeHours { get; set; }
    public decimal FailPercentage { get; set; }
}

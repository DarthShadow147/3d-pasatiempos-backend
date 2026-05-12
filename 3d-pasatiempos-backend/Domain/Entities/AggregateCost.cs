namespace _3d_pasatiempos_backend.Domain.Entities
{
    public class AggregateCost
    {
        public int Id { get; set; }
        public string CostName { get; set; } = string.Empty;
        public decimal CostValue { get; set; }
        public decimal UnitCost { get; set; }
    }
}

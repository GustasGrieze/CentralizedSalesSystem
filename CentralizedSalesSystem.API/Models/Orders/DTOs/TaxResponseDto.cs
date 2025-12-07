namespace CentralizedSalesSystem.API.Models.DTOs
{
    public class TaxResponseDto
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Activity { get; set; } = "inactive";
        public DateTimeOffset EffectiveFrom { get; set; }
        public DateTimeOffset EffectiveTo { get; set; }
    }
}

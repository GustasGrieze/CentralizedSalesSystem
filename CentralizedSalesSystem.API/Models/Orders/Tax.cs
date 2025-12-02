using System.Reflection;
using CentralizedSalesSystem.API.Models.Orders.enums;


namespace CentralizedSalesSystem.API.Models.Orders
{
    public class Tax
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public TaxStatus Status { get; set; }
        public DateTimeOffset EffectiveFrom { get; set; }
        public DateTimeOffset EffectiveTo { get; set; }


    }
}
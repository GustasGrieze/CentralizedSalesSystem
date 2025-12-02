using System.Reflection;

namespace CentralizedSalesSystem.API.Models.Orders
{
    public class ServiceCharge
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal rate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }



    }
}

namespace CentralizedSalesSystem.API.Models.Orders
{
    public class OrderItem
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public ICollection<Tax> Taxes { get; set; } = new List<Tax>();

        public required Item Item { get; set; }
        public required Order Order { get; set; }


    }
}
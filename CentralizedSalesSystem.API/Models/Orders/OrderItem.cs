
namespace CentralizedSalesSystem.API.Models.Orders
{
    using CentralizedSalesSystem.API.Models;

    public class OrderItem
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; }
        public long? DiscountId { get; set; }
        public string? Notes { get; set; }
        public long? ReservationId { get; set; }
        public ICollection<Tax> Taxes { get; set; } = new List<Tax>();
        public ICollection<ServiceCharge> ServiceCharges { get; set; } = new List<ServiceCharge>();

        public required Item Item { get; set; }
        public required Order Order { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
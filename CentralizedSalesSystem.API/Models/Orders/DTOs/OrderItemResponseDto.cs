namespace CentralizedSalesSystem.API.Models.DTOs
{
    public class OrderItemResponseDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public long? DiscountId { get; set; }
        public string? Notes { get; set; }
        public List<TaxResponseDto>? Taxes { get; set; }
        public List<ServiceChargeResponseDto>? ServiceCharges { get; set; }
    }
}

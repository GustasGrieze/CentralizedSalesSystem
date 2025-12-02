using CentralizedSalesSystem.API.Models.Orders.enums;
namespace CentralizedSalesSystem.API.Models.Orders.DTOs
{
    public class ItemReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ItemType Type { get; set; }
        public int Stock { get; set; }
        public long BusinessId { get; set; }
    }
}

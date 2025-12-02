using CentralizedSalesSystem.API.Models.Orders.enums;
namespace CentralizedSalesSystem.API.Models.Orders.DTOs
{
    public class ItemCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ItemType Type { get; set; }   // or ItemType if you prefer
        public int Stock { get; set; }
        public long BusinessId { get; set; }
    }
}

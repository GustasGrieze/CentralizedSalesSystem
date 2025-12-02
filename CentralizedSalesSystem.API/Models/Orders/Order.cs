using CentralizedSalesSystem.API.Models.Orders.enums;
using System.Reflection;

namespace CentralizedSalesSystem.API.Models.Orders
{
	public class Order
	{
		public long Id { get; set; }
		public long BusinessId { get; set; }
		public decimal Tip { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
		public long UserId { get; set; }
		public OrderStatus Status { get; set; }
		public long TableId { get; set; }

		public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

		public required User User { get; set; }
		public required Table Table { get; set; }


	}
}
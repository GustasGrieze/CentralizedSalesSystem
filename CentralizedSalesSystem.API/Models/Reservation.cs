using System.ComponentModel.DataAnnotations;
using CentralizedSalesSystem.API.Models.Orders;

namespace CentralizedSalesSystem.API.Models
{
    public class Reservation
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerNote { get; set; }
        public DateTimeOffset AppointmentTime { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Scheduled;
        public long? AssignedEmployee { get; set; }
        public int GuestNumber { get; set; }
        public long? TableId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Navigation properties, params are denoted by primitives to be compliant with documentation
        public User? CreatedByUser { get; set; }
        public User? AssignedEmployeeUser { get; set; }
        public Table? Table { get; set; }
    }
}

namespace CentralizedSalesSystem.API.Models.DTOs
{
    public class ReservationPatchDto
    {
        public long? BusinessId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerNote { get; set; }
        public DateTimeOffset? AppointmentTime { get; set; }
        public long? CreatedBy { get; set; }
        public string? Status { get; set; }
        public long? AssignedEmployee { get; set; }
        public int? GuestNumber { get; set; }
        public long? TableId { get; set; }
    }
}

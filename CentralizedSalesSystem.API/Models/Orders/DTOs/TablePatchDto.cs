namespace CentralizedSalesSystem.API.Models.DTOs
{
    public class TablePatchDto
    {
        public long? BusinessId { get; set; }
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public string? Status { get; set; }
    }
}

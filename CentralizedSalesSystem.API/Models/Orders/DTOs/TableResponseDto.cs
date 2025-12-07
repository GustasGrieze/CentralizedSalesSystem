namespace CentralizedSalesSystem.API.Models.DTOs
{
    public class TableResponseDto
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = "free";
    }
}

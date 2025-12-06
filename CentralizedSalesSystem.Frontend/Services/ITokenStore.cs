namespace CentralizedSalesSystem.Frontend.Services
{
    public interface ITokenStore
    {
        Task SetTokenAsync(string token);
        Task<string?> GetTokenAsync();
        Task ClearTokenAsync();
    }
}

using Microsoft.JSInterop;

namespace CentralizedSalesSystem.Frontend.Services
{
    public sealed class LocalStorageTokenStore : ITokenStore
    {
        private const string TokenKey = "accessToken";
        private readonly IJSRuntime _js;

        public LocalStorageTokenStore(IJSRuntime js)
        {
            _js = js;
        }

        public Task SetTokenAsync(string token) =>
            _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token).AsTask();

        public Task<string?> GetTokenAsync() =>
            _js.InvokeAsync<string?>("localStorage.getItem", TokenKey).AsTask();

        public Task ClearTokenAsync() =>
            _js.InvokeVoidAsync("localStorage.removeItem", TokenKey).AsTask();
    }
}

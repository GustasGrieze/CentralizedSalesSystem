using System.Text.Json.Serialization;

namespace CentralizedSalesSystem.API.Models.Auth
{
    public record TokenResponse([property: JsonPropertyName("accessToken")] string AccessToken);
}

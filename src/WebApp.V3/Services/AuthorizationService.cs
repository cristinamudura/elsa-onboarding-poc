using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using WebApp.V3.Models;

namespace WebApp.V3.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IHttpClientFactory httpClientFactory;

    public AuthorizationService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<LoginResponse> GetAccessToken()
    {
        var httpClient = httpClientFactory.CreateClient("AuthorizationServiceClient");
        
        var data = new LoginRequest
        {
            UserName = "admin",
            Password = "password"
        };

        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"elsa/api/identity/login", content);

        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }
}

public interface IAuthorizationService
{
    Task<LoginResponse> GetAccessToken();
}

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    [JsonPropertyName("isAuthenticated")]
    public bool IsAuthenticated { get; set; }

    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}
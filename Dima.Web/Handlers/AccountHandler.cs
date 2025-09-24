using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;
using System.Net.Http.Json;
using System.Text;

namespace Dima.Web.Handlers;

public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.BackendUrl);

    public async Task<Response<string>> LoginAsync(LoginRequest loginRequest)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/identity/login?useCookies=true", loginRequest);
        return result.IsSuccessStatusCode
            ? new Response<string>("Login Realizado Com Sucesso!", (int)result.StatusCode, "Login Realizado com sucesso")
            : new Response<string>(null, (int)result.StatusCode, "Não foi possível realizar o login");
    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", encoding: Encoding.UTF8, "application/json");
        await _httpClient.PostAsJsonAsync("v1/identity/logout", emptyContent);
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest registerRequest)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/identity/register", registerRequest);
        return result.IsSuccessStatusCode
            ? new Response<string>("Cadastro Realizado Com Sucesso!", (int)result.StatusCode, "Cadastro Realizado com sucesso")
            : new Response<string>(null, (int)result.StatusCode, "Não foi possível realizar o cadastro");
    }
}

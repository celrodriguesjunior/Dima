using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;

namespace Dima.Web.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory clientFactory) : AuthenticationStateProvider, ICookieAuthenticationStateProvider
{
    private readonly HttpClient _clientFactory = clientFactory.CreateClient(Configuration.BackendUrl);
    private bool _isAuthenticated = false;

    public async Task<bool> CheckAuthenticationStateProvider()
    {
        await GetAuthenticationStateAsync();
        return _isAuthenticated;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        
    }

    public void NotifyAuthenticationStateChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}

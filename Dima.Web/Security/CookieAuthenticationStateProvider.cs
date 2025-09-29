using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Security.Claims;

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

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _isAuthenticated = false;

        var user = new ClaimsPrincipal(new ClaimsIdentity());

        var userInfo = await GetUserAsync();
        if (userInfo == null || !userInfo.IsEmailConfirmed)
            return new AuthenticationState(user);

        var claims = await GetClaimsAsync(userInfo);

        var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));

        user = new ClaimsPrincipal(id);

        _isAuthenticated = true;
        return new AuthenticationState(user);
    }

    public void NotifyAuthenticationStateChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    private async Task<User?> GetUserAsync()
    {
        try
        {
            return await _clientFactory.GetFromJsonAsync<User>("v1/identity/manage/info");
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        if (user == null || !user.IsEmailConfirmed)
            return [];

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Email),
            new (ClaimTypes.Email, user.Email)
        };

        claims.AddRange(user.Claims.Where(x => x.Key != ClaimTypes.Name && x.Key != ClaimTypes.Email).Select(x => new Claim(x.Key, x.Value)));

        RoleClaim[]? roles;
        try
        {
            roles = await _clientFactory.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");
        }
        catch (Exception ex)
        {
            return claims;
        }

        foreach (var role in roles ?? [])
            if (!string.IsNullOrWhiteSpace(role.Value) || !string.IsNullOrEmpty(role.Type))
                claims.Add(new(role.Type, role.Value!, role.ValueType, role.Issuer, role.OriginalIssuer));

        return claims;
    }

}
